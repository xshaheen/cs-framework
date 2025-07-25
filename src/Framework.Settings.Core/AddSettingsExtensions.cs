// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Abstractions;
using Framework.Domains;
using Framework.Settings.Definitions;
using Framework.Settings.Entities;
using Framework.Settings.Helpers;
using Framework.Settings.Models;
using Framework.Settings.Resources;
using Framework.Settings.Seeders;
using Framework.Settings.ValueProviders;
using Framework.Settings.Values;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Settings;

[PublicAPI]
public static class AddSettingsExtensions
{
    /// <summary>
    /// Adds core setting management services to the host builder and registers default setting value providers.
    /// You should add TimeProvider, Cache, ResourceLock, GuidGenerator, IConfiguration, ICurrentUser,
    /// and ICurrentTenant implementations to be able to use this feature.
    /// </summary>
    public static IServiceCollection AddSettingsManagementCore(
        this IServiceCollection services,
        Action<SettingManagementOptions, IServiceProvider> setupAction
    )
    {
        services.Configure<SettingManagementOptions, SettingManagementOptionsValidator>(setupAction);

        return _AddCore(services);
    }

    /// <summary>
    /// Adds core setting management services to the host builder and registers default setting value providers.
    /// You should add TimeProvider, Cache, ResourceLock, GuidGenerator, IConfiguration, ICurrentUser,
    /// and ICurrentTenant implementations to be able to use this feature.
    /// </summary>
    public static IServiceCollection AddSettingsManagementCore(
        this IServiceCollection services,
        Action<SettingManagementOptions>? setupAction = null
    )
    {
        services.Configure<SettingManagementOptions, SettingManagementOptionsValidator>(setupAction);

        return _AddCore(services);
    }

    public static IServiceCollection AddSettingDefinitionProvider<T>(this IServiceCollection services)
        where T : class, ISettingDefinitionProvider
    {
        services.AddSingleton<T>();

        services.Configure<SettingManagementProvidersOptions>(options =>
        {
            options.DefinitionProviders.Add<T>();
        });

        return services;
    }

    public static IServiceCollection AddSettingValueProvider<T>(this IServiceCollection services) // Transient
        where T : class, ISettingValueReadProvider
    {
        services.AddSingleton<T>();

        services.Configure<SettingManagementProvidersOptions>(options =>
        {
            if (!options.ValueProviders.Contains<T>())
            {
                options.ValueProviders.Add<T>();
            }
        });

        return services;
    }

    private static void _AddCoreValueProvider(this IServiceCollection services)
    {
        services.Configure<SettingManagementProvidersOptions>(options =>
        {
            // Last added provider has the highest priority
            options.ValueProviders.Add<DefaultValueSettingValueProvider>();
            options.ValueProviders.Add<ConfigurationSettingValueProvider>();
            options.ValueProviders.Add<GlobalSettingValueProvider>();
            options.ValueProviders.Add<TenantSettingValueProvider>();
            options.ValueProviders.Add<UserSettingValueProvider>();
        });

        services.AddSingleton<DefaultValueSettingValueProvider>();
        services.AddSingleton<ConfigurationSettingValueProvider>();
        services.AddSingleton<GlobalSettingValueProvider>();
        services.AddSingleton<TenantSettingValueProvider>();
        services.AddSingleton<UserSettingValueProvider>();
    }

    private static void _AddSettingEncryption(this IServiceCollection services)
    {
        services.AddOptions<StringEncryptionOptions, StringEncryptionOptionsValidator>();
        services.AddSingletonOptionValue<StringEncryptionOptions>();
        services.TryAddSingleton<IStringEncryptionService, StringEncryptionService>();
        services.TryAddSingleton<ISettingEncryptionService, SettingEncryptionService>();
    }

    private static IServiceCollection _AddCore(IServiceCollection services)
    {
        services._AddSettingEncryption();
        services._AddCoreValueProvider();

        services.AddHostedService<SettingsInitializationBackgroundService>();

        services.TryAddTransient<
            ILocalMessageHandler<EntityChangedEventData<SettingValueRecord>>,
            SettingValueCacheItemInvalidator
        >();

        services.TryAddSingleton<ISettingsErrorsDescriptor, DefaultSettingsErrorsDescriptor>();

        // Definition Services
        /*
         * 1. You need to provide a storage implementation for `ISettingDefinitionRecordRepository`
         * 2. Implement `ISettingDefinitionProvider` to define your settings in code
         *    and use `AddSettingDefinitionProvider` to register it
         */
        services.TryAddSingleton<ISettingDefinitionSerializer, SettingDefinitionSerializer>(); // Transient
        services.TryAddSingleton<IStaticSettingDefinitionStore, StaticSettingDefinitionStore>(); // Singleton
        services.TryAddSingleton<IDynamicSettingDefinitionStore, DynamicSettingDefinitionStore>(); // Transient
        services.TryAddSingleton<ISettingDefinitionManager, SettingDefinitionManager>(); // Singleton

        // Value Services
        /*
         * You need to provide a storage implementation for `ISettingValueRecordRepository`
         */
        services.TryAddSingleton<ISettingValueStore, SettingValueStore>(); // Transient
        services.TryAddSingleton<ISettingValueProviderManager, SettingValueProviderManager>(); // Singleton
        services.TryAddSingleton<ISettingManager, SettingManager>(); // Singleton

        return services;
    }
}
