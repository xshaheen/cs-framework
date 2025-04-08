// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Messaging.LocalServiceProvider;

[PublicAPI]
public static class AddLocalMessagePublisherExtensions
{
    public static IServiceCollection AddServiceProviderLocalMessagePublisher(this IServiceCollection services)
    {
        services.AddSingleton<ILocalMessagePublisher, ServiceProviderLocalMessagePublisher>();

        return services;
    }
}
