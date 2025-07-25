// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Settings.Definitions;
using Framework.Settings.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Framework.Settings.Seeders;

public sealed class SettingsInitializationBackgroundService(
    TimeProvider timeProvider,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<SettingManagementOptions> optionsAccessor,
    ILogger<SettingsInitializationBackgroundService> logger
) : IHostedService, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly SettingManagementOptions _options = optionsAccessor.Value;
    private Task? _initializeDynamicSettingsTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options is { SaveStaticSettingsToDatabase: false, IsDynamicSettingStoreEnabled: false })
        {
            return Task.CompletedTask;
        }

        _initializeDynamicSettingsTask = _InitializeDynamicSettingsAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _cancellationTokenSource.CancelAsync();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _initializeDynamicSettingsTask?.Dispose();
    }

    private async Task _InitializeDynamicSettingsAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();

        await _SaveStaticSettingsToDatabaseAsync(scope, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await _PreCacheDynamicSettingsAsync(scope, cancellationToken);
    }

    private async Task _SaveStaticSettingsToDatabaseAsync(AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        if (!_options.SaveStaticSettingsToDatabase)
        {
            return;
        }

        var options = new RetryStrategyOptions
        {
            Name = "SaveStaticSettingsToDatabaseRetry",
            Delay = TimeSpan.FromSeconds(2),
            MaxRetryAttempts = 10,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = false,
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        };

        var builder = new ResiliencePipelineBuilder { TimeProvider = timeProvider };
        var pipeline = builder.AddRetry(options).Build();

        await pipeline.ExecuteAsync(
            static async (state, cancellationToken) =>
            {
                var (scope, logger) = state;

                var store = scope.ServiceProvider.GetRequiredService<IDynamicSettingDefinitionStore>();

                try
                {
                    await store.SaveAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to save static settings to the database");

                    throw; // Polly will catch it
                }
            },
            (scope, logger),
            cancellationToken
        );
    }

    private async Task _PreCacheDynamicSettingsAsync(AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        if (!_options.IsDynamicSettingStoreEnabled)
        {
            return;
        }

        var store = scope.ServiceProvider.GetRequiredService<IDynamicSettingDefinitionStore>();

        try
        {
            await store.GetAllAsync(cancellationToken); // Pre-cache settings, so the first request doesn't wait
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to pre-cache dynamic settings");

            throw;
        }
    }
}
