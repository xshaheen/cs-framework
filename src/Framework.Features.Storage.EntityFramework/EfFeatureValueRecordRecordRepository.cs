// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Features.Entities;
using Framework.Features.Values;
using Microsoft.EntityFrameworkCore;

namespace Framework.Features.Storage.EntityFramework;

public sealed class EfFeatureValueRecordRecordRepository(IDbContextFactory<FeaturesDbContext> dbFactory)
    : IFeatureValueRecordRepository
{
    public async Task<FeatureValueRecord?> FindAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default
    )
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        return await db
            .FeatureValues.OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey,
                cancellationToken
            );
    }

    public async Task<List<FeatureValueRecord>> FindAllAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default
    )
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var query = db.FeatureValues.Where(s => s.Name == name);

        if (providerName != null)
        {
            query = query.Where(s => s.ProviderName == providerName);
        }

        if (providerKey != null)
        {
            query = query.Where(s => s.ProviderKey == providerKey);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<FeatureValueRecord>> GetListAsync(
        string providerName,
        string? providerKey,
        CancellationToken cancellationToken = default
    )
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        return await db
            .FeatureValues.Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
            .ToListAsync(cancellationToken);
    }

    public async Task InsertAsync(FeatureValueRecord featureValue, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        db.FeatureValues.Add(featureValue);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FeatureValueRecord featureValue, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        db.FeatureValues.Update(featureValue);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        IEnumerable<FeatureValueRecord> featureValues,
        CancellationToken cancellationToken = default
    )
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        db.FeatureValues.RemoveRange(featureValues);
        await db.SaveChangesAsync(cancellationToken);
    }
}
