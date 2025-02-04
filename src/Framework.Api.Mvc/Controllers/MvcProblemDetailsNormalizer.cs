﻿// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Framework.Api.Mvc.Controllers;

public sealed class MvcProblemDetailsNormalizer(
    IOptions<ApiBehaviorOptions> apiOptionsAccessor,
    IOptions<ProblemDetailsOptions>? problemOptionsAccessor = null
)
{
    private readonly ApiBehaviorOptions _apiOptions = apiOptionsAccessor.Value;
    private readonly Action<ProblemDetailsContext>? _configure = problemOptionsAccessor?.Value.CustomizeProblemDetails;

    public void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails)
    {
        Debug.Assert(problemDetails.Status is not null);

        if (_apiOptions.ClientErrorMapping.TryGetValue(problemDetails.Status.Value, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _configure?.Invoke(new() { HttpContext = httpContext, ProblemDetails = problemDetails });
    }
}
