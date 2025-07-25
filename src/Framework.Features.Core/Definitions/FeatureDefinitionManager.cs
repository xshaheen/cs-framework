// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Checks;
using Framework.Features.Models;

namespace Framework.Features.Definitions;

public sealed class FeatureDefinitionManager(
    IStaticFeatureDefinitionStore staticStore,
    IDynamicFeatureDefinitionStore dynamicStore
) : IFeatureDefinitionManager
{
    public async Task<FeatureDefinition?> FindAsync(string name, CancellationToken cancellationToken = default)
    {
        Argument.IsNotNull(name);

        return await staticStore.GetOrDefaultAsync(name, cancellationToken)
            ?? await dynamicStore.GetOrDefaultAsync(name, cancellationToken);
    }

    public async Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync(CancellationToken cancellationToken = default)
    {
        var staticFeatures = await staticStore.GetFeaturesAsync(cancellationToken);
        var staticFeatureNames = staticFeatures.Select(p => p.Name).ToImmutableHashSet();

        // Prefer static features over dynamics
        var dynamicFeatures = await dynamicStore.GetFeaturesAsync(cancellationToken);
        var uniqueDynamicFeatures = dynamicFeatures.Where(d => !staticFeatureNames.Contains(d.Name));

        return staticFeatures.Concat(uniqueDynamicFeatures).ToImmutableList();
    }

    public async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var staticGroups = await staticStore.GetGroupsAsync(cancellationToken);
        var staticGroupNames = staticGroups.Select(p => p.Name).ToImmutableHashSet();
        // Prefer static features over dynamics
        var dynamicFeatures = await dynamicStore.GetGroupsAsync(cancellationToken);
        var uniqueDynamicFeatures = dynamicFeatures.Where(d => !staticGroupNames.Contains(d.Name));

        return staticGroups.Concat(uniqueDynamicFeatures).ToImmutableList();
    }
}
