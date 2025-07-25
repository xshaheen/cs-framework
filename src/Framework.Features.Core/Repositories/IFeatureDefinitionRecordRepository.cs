// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Features.Entities;

namespace Framework.Features.Repositories;

public interface IFeatureDefinitionRecordRepository
{
    Task<List<FeatureGroupDefinitionRecord>> GetGroupsListAsync(CancellationToken cancellationToken = default);

    Task<List<FeatureDefinitionRecord>> GetFeaturesListAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(
        List<FeatureGroupDefinitionRecord> newGroups,
        List<FeatureGroupDefinitionRecord> updatedGroups,
        List<FeatureGroupDefinitionRecord> deletedGroups,
        List<FeatureDefinitionRecord> newFeatures,
        List<FeatureDefinitionRecord> updatedFeatures,
        List<FeatureDefinitionRecord> deletedFeatures,
        CancellationToken cancellationToken = default
    );
}
