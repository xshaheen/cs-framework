// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Emails.Mailkit;

[PublicAPI]
public static class AddMailKitExtensions
{
    public static IServiceCollection AddMailKitEmailSender(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MailkitSmtpOptions, MailkitSmtpOptionsValidator>(config);

        return _AddCore(services);
    }

    public static IServiceCollection AddMailKitEmailSender(
        this IServiceCollection services,
        Action<MailkitSmtpOptions> configure
    )
    {
        services.Configure<MailkitSmtpOptions, MailkitSmtpOptionsValidator>(configure);

        return _AddCore(services);
    }

    public static IServiceCollection AddMailKitEmailSender(
        this IServiceCollection services,
        Action<MailkitSmtpOptions, IServiceProvider> configure
    )
    {
        services.Configure<MailkitSmtpOptions, MailkitSmtpOptionsValidator>(configure);

        return _AddCore(services);
    }

    private static IServiceCollection _AddCore(IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, MailkitEmailSender>();

        return services;
    }
}
