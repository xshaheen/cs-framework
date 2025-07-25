// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace Framework.Constants;

[PublicAPI]
public static class ProblemDetailTitles
{
    public const string EndpointNotFounded = "endpoint-not-found";
    public const string EntityNotFounded = "entity-not-found";
    public const string ValidationProblem = "validation-problem";
    public const string BadRequest = "bad-request";
    public const string ConflictRequest = "conflict-request";
    public const string ForbiddenRequest = "forbidden-request";
}
