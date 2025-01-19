// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Checks;
using Framework.Exceptions;
using Framework.Features.Definitions;
using Framework.Features.Models;
using Framework.Features.Resources;
using Framework.Features.ValueProviders;

namespace Framework.Features.Values;

public interface IFeatureManager
{
    /// <summary>Get feature value by name.</summary>
    /// <param name="name">The feature name.</param>
    /// <param name="providerName">
    /// If the providerName isn't provided, it will get the value from the first provider that has the value
    /// by the order of the registered providers.
    /// </param>
    /// <param name="providerKey">
    /// If the providerKey isn't provided, it will get the value according to each value provider's logic.
    /// </param>
    /// <param name="fallback">Force the value finds fallback to other providers based on the order of the registered providers.</param>
    /// <param name="cancellationToken">The abort token.</param>
    Task<FeatureValue> GetAsync(
        string name,
        string? providerName = null,
        string? providerKey = null,
        bool fallback = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>Get all feature values by providerName and providerKey.</summary>
    /// <param name="providerName">
    /// If the providerName isn't provided, it will get the value from the first provider that has the value
    /// by the order of the registered providers.
    /// </param>
    /// <param name="providerKey">
    /// If the providerKey isn't provided, it will get the value according to each value provider's logic.
    /// </param>
    /// <param name="fallback">Force the value finds fallback to other providers based on the order of the registered providers.</param>
    /// <param name="cancellationToken">The abort token.</param>
    Task<List<FeatureValue>> GetAllAsync(
        string providerName,
        string? providerKey,
        bool fallback = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>Set feature value by name.</summary>
    /// <param name="forceToSet">
    /// When <see langword="true"/> and the value is same as the fallback value, it will not set the value
    /// otherwise it will set the value even if it is same as the fallback value.
    /// </param>
    Task SetAsync(
        string name,
        string? value,
        string providerName,
        string? providerKey,
        bool forceToSet = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>Delete feature value from a specific providerName and providerKey.</summary>
    Task DeleteAsync(string providerName, string providerKey, CancellationToken cancellationToken = default);
}

public sealed class FeatureManager(
    IFeatureDefinitionManager definitionManager,
    IFeatureValueProviderManager valueProviderManager,
    IFeatureErrorsDescriptor errorsDescriptor
) : IFeatureManager
{
    public async Task<FeatureValue> GetAsync(
        string name,
        string? providerName = null,
        string? providerKey = null,
        bool fallback = true,
        CancellationToken cancellationToken = default
    )
    {
        if (!fallback)
        {
            Argument.IsNotNull(providerName);
        }

        return await _CoreGetOrDefaultAsync(name, providerName, providerKey, fallback, cancellationToken);
    }

    public async Task<List<FeatureValue>> GetAllAsync(
        string providerName,
        string? providerKey,
        bool fallback = true,
        CancellationToken cancellationToken = default
    )
    {
        Argument.IsNotNull(providerName);

        var definitions = await definitionManager.GetFeaturesAsync(cancellationToken);

        var providers = valueProviderManager.ValueProviders.SkipWhile(c =>
            !string.Equals(c.Name, providerName, StringComparison.Ordinal)
        );

        if (!fallback)
        {
            providers = providers.TakeWhile(c => string.Equals(c.Name, providerName, StringComparison.Ordinal));
        }

        var providerList = providers.ToList();

        if (providerList.Count == 0)
        {
            return [];
        }

        var featureValues = new Dictionary<string, FeatureValue>(StringComparer.Ordinal);

        foreach (var definition in definitions)
        {
            foreach (var provider in providerList)
            {
                var pk = string.Equals(provider.Name, providerName, StringComparison.Ordinal) ? providerKey : null;
                var value = await provider.GetOrDefaultAsync(definition, pk, cancellationToken);

                if (value is not null)
                {
                    featureValues[definition.Name] = new(definition.Name, value, new(provider.Name, pk));

                    break;
                }
            }
        }

        return [.. featureValues.Values];
    }

    public async Task SetAsync(
        string name,
        string? value,
        string providerName,
        string? providerKey,
        bool forceToSet = false,
        CancellationToken cancellationToken = default
    )
    {
        Argument.IsNotNull(name);
        Argument.IsNotNull(providerName);

        var feature =
            await definitionManager.GetOrDefaultAsync(name, cancellationToken)
            ?? throw new ConflictException(await errorsDescriptor.FeatureIsNotDefined(name));

        var providers = valueProviderManager
            .ValueProviders.SkipWhile(p => !string.Equals(p.Name, providerName, StringComparison.Ordinal))
            .ToList();

        if (providers.Count == 0)
        {
            throw new ConflictException(await errorsDescriptor.FeatureProviderNotDefined(name, providerName));
        }

        if (providers.Count > 1 && !forceToSet && value is not null)
        {
            await using (await providers[0].HandleContextAsync(providerName, providerKey, cancellationToken))
            {
                var fallbackValue = await _CoreGetOrDefaultAsync(
                    name,
                    providers[1].Name,
                    providerKey: null,
                    cancellationToken: cancellationToken
                );

                if (string.Equals(fallbackValue.Value, value, StringComparison.Ordinal))
                {
                    // Clear the value if it is same as it's fallback value
                    value = null;
                }
            }
        }

        // Getting list for case of there are more than one provider with the same providerName
        providers = providers.TakeWhile(p => string.Equals(p.Name, providerName, StringComparison.Ordinal)).ToList();

        foreach (var provider in providers)
        {
            if (provider is not IFeatureValueProvider p)
            {
                throw new ConflictException(await errorsDescriptor.ProviderIsReadonly(providerName));
            }

            if (value is null)
            {
                await p.ClearAsync(feature, providerKey, cancellationToken);
            }
            else
            {
                await p.SetAsync(feature, value, providerKey, cancellationToken);
            }
        }
    }

    public async Task DeleteAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    )
    {
        var featureNameValues = await GetAllAsync(providerName, providerKey, cancellationToken: cancellationToken);

        var providers = valueProviderManager.ValueProviders.SkipWhile(p =>
            !string.Equals(p.Name, providerName, StringComparison.Ordinal)
        );

        // Getting list for case of there are more than one provider with the same providerName
        providers = providers.TakeWhile(p => string.Equals(p.Name, providerName, StringComparison.Ordinal));

        var writableProviders = providers.OfType<IFeatureValueProvider>().ToList();

        if (writableProviders.Count == 0)
        {
            return;
        }

        foreach (var featureNameValue in featureNameValues)
        {
            var feature =
                await definitionManager.GetOrDefaultAsync(featureNameValue.Name, cancellationToken)
                ?? throw new ConflictException(await errorsDescriptor.FeatureIsNotDefined(featureNameValue.Name));

            foreach (var provider in writableProviders)
            {
                await provider.ClearAsync(feature, providerKey, cancellationToken);
            }
        }
    }

    private async Task<FeatureValue> _CoreGetOrDefaultAsync(
        string name,
        string? providerName,
        string? providerKey,
        bool fallback = true,
        CancellationToken cancellationToken = default
    )
    {
        Argument.IsNotNull(name);

        if (!fallback)
        {
            Argument.IsNotNull(providerName);
        }

        var definition =
            await definitionManager.GetOrDefaultAsync(name, cancellationToken)
            ?? throw new ConflictException(await errorsDescriptor.FeatureIsNotDefined(name));

        IEnumerable<IFeatureValueReadProvider> providers = valueProviderManager.ValueProviders;

        if (providerName is not null)
        {
            providers = providers.SkipWhile(c => !string.Equals(c.Name, providerName, StringComparison.Ordinal));
        }

        if (!fallback)
        {
            providers = providers.TakeWhile(c => string.Equals(c.Name, providerName, StringComparison.Ordinal));
        }

        foreach (var provider in providers)
        {
            var pk = string.Equals(provider.Name, providerName, StringComparison.Ordinal) ? providerKey : null;
            var value = await provider.GetOrDefaultAsync(definition, pk, cancellationToken);

            if (value is not null)
            {
                return new(name, value, new(provider.Name, pk));
            }
        }

        return new(name, Value: null, Provider: null);
    }
}
