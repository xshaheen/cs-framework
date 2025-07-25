// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Api.Logging.Serilog;

[PublicAPI]
public static class AddSerilogExtensions
{
    /// <summary>Adds the serilog enrichers middleware.</summary>
    public static IServiceCollection AddSerilogEnrichers(this IServiceCollection services)
    {
        return services.AddScoped<SerilogEnrichersMiddleware>();
    }

    public static IApplicationBuilder UseCustomSerilogEnrichers(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SerilogEnrichersMiddleware>();
    }
}
