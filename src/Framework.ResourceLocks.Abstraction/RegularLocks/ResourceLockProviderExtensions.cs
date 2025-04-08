// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace Framework.ResourceLocks;

[PublicAPI]
public static class ResourceLockProviderExtensions
{
    /// <summary>Releases a resource lock for <paramref name="resourceLock"/>.</summary>
    public static Task ReleaseAsync(this IResourceLockProvider provider, IResourceLock resourceLock)
    {
        return provider.ReleaseAsync(resourceLock.Resource, resourceLock.LockId);
    }

    /// <summary>
    /// Renews a resource lock for a specified <paramref name="resourceLock"/> by extending
    /// the expiration time of the lock if it is still held to the <see cref="IResourceLock.LockId"/>
    /// and return <see langword="true"/>, otherwise <see langword="false"/>.
    /// </summary>
    public static Task RenewAsync(
        this IResourceLockProvider provider,
        IResourceLock resourceLock,
        TimeSpan? timeUntilExpires = null
    )
    {
        return provider.RenewAsync(resourceLock.Resource, resourceLock.LockId, timeUntilExpires);
    }

    /// <summary>
    /// Tries to acquire a lock for a specified <paramref name="resource"/> and execute the <paramref name="work"/>
    /// </summary>
    /// <param name="timeUntilExpires">
    /// The amount of time until the lock expires. The allowed values are:
    /// <list type="bullet">
    /// <item><see langword="null"/>: means the default value (20 minutes).</item>
    /// <item><see cref="Timeout.InfiniteTimeSpan"/> (-1 milliseconds): means infinity no expiration set.</item>
    /// <item>Value greater than 0.</item>
    /// </list>
    /// </param>
    /// <param name="acquireTimeout">
    /// The amount of time to wait for the lock to be acquired. The allowed values are:
    /// <list type="bullet">
    /// <item><see langword="null"/>: means the default value (30 seconds).</item>
    /// <item><see cref="Timeout.InfiniteTimeSpan"/> (-1 millisecond): means infinity wait to acquire</item>
    /// <item>Value greater than or equal to 0.</item>
    /// </list>
    /// </param>
    public static async Task<bool> TryUsingAsync(
        this IResourceLockProvider provider,
        string resource,
        Func<Task> work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            await work().AnyContext();

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }

    /// <inheritdoc cref="TryUsingAsync(IResourceLockProvider,string,Func{Task},TimeSpan?,TimeSpan?,CancellationToken)"/>
    public static async Task<bool> TryUsingAsync<TState>(
        this IResourceLockProvider provider,
        string resource,
        TState state,
        Func<TState, Task> work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            await work(state).AnyContext();

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }

    /// <inheritdoc cref="TryUsingAsync(IResourceLockProvider,string,Func{Task},TimeSpan?,TimeSpan?,CancellationToken)"/>
    public static async Task<bool> TryUsingAsync(
        this IResourceLockProvider provider,
        string resource,
        Func<CancellationToken, Task> work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            await work(cancellationToken).AnyContext();

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }

    /// <inheritdoc cref="TryUsingAsync(IResourceLockProvider,string,Func{Task},TimeSpan?,TimeSpan?,CancellationToken)"/>
    public static async Task<bool> TryUsingAsync<TState>(
        this IResourceLockProvider provider,
        string resource,
        TState state,
        Func<TState, CancellationToken, Task> work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            await work(state, cancellationToken).AnyContext();

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }

    /// <inheritdoc cref="TryUsingAsync(IResourceLockProvider,string,Func{Task},TimeSpan?,TimeSpan?,CancellationToken)"/>
    public static async Task<bool> TryUsingAsync(
        this IResourceLockProvider provider,
        string resource,
        Action work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            work();

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }

    /// <inheritdoc cref="TryUsingAsync(IResourceLockProvider,string,Func{Task},TimeSpan?,TimeSpan?,CancellationToken)"/>
    public static async Task<bool> TryUsingAsync<TState>(
        this IResourceLockProvider provider,
        string resource,
        TState state,
        Action<TState> work,
        TimeSpan? timeUntilExpires = null,
        TimeSpan? acquireTimeout = null,
        CancellationToken cancellationToken = default
    )
    {
        var resourceLock = await provider
            .TryAcquireAsync(resource, timeUntilExpires, acquireTimeout, cancellationToken)
            .AnyContext();

        if (resourceLock is null)
        {
            return false;
        }

        try
        {
            work(state);

            return true;
        }
        finally
        {
            await resourceLock.ReleaseAsync();
        }
    }
}
