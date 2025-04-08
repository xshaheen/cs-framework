// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Abstractions;
using Framework.Settings.Values;

namespace Framework.Settings.ValueProviders;

/// <summary>Current user setting value provider.</summary>
public sealed class UserSettingValueProvider(ISettingValueStore store, ICurrentUser user)
    : StoreSettingValueProvider(store)
{
    public const string ProviderName = SettingValueProviderNames.User;

    public override string Name => ProviderName;

    protected override string? NormalizeProviderKey(string? providerKey)
    {
        return providerKey ?? user.UserId?.ToString();
    }
}
