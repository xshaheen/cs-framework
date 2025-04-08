// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Abstractions;
using Framework.Permissions.Definitions;
using Framework.Permissions.Grants;
using Framework.Permissions.Models;

namespace Framework.Permissions.Testing;

/// <summary>Always allows for any permission.</summary>
public sealed class AlwaysAllowPermissionManager(IPermissionDefinitionManager definitionManager) : IPermissionManager
{
    public Task<GrantedPermissionResult> GetAsync(
        string permissionName,
        ICurrentUser currentUser,
        string? providerName = null,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(new GrantedPermissionResult(permissionName, isGranted: true));
    }

    public async Task<List<GrantedPermissionResult>> GetAllAsync(
        ICurrentUser currentUser,
        string? providerName = null,
        CancellationToken cancellationToken = default
    )
    {
        var definitions = await definitionManager.GetPermissionsAsync(cancellationToken);

        return definitions.Select(x => new GrantedPermissionResult(x.Name, isGranted: true)).ToList();
    }

    public Task<List<GrantedPermissionResult>> GetAllAsync(
        IReadOnlyCollection<string> permissionNames,
        ICurrentUser currentUser,
        string? providerName = null,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(permissionNames.Select(x => new GrantedPermissionResult(x, isGranted: true)).ToList());
    }

    public Task SetAsync(
        string permissionName,
        string providerName,
        string providerKey,
        bool isGranted,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task SetAsync(
        IReadOnlyCollection<string> permissionNames,
        string providerName,
        string providerKey,
        bool isGranted,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
