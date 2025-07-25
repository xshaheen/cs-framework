﻿// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace Framework.Hosting.Seeders;

[PublicAPI]
public static class AddDbMigrationSeederExtensions
{
    public static void AddDbMigrationPreSeeder<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddPreSeeder<DbMigrationPreSeeder<TContext>>();
    }
}
