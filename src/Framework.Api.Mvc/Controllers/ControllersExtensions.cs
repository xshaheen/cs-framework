// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Framework.Api.Mvc.Controllers;

[PublicAPI]
public static class ControllerBaseExtensions
{
    /// <summary>
    /// Returns the proper ActionResult for unauthorized or unauthenticated users.
    /// Will return a forbid when the user is authenticated.
    /// Will return a challenge when the user is not authenticated.
    /// </summary>
    /// <param name="controller"></param>
    /// <returns>The proper ActionResult based upon if the user is authenticated.</returns>
    public static ActionResult ChallengeOrForbid(this ControllerBase controller)
    {
        return controller.User.Identity?.IsAuthenticated ?? false ? controller.Forbid() : controller.Challenge();
    }

    /// <summary>
    /// Returns the proper ActionResult for unauthorized or unauthenticated users
    /// with the specified authenticationSchemes.
    /// Will return a forbid when the user is authenticated.
    /// Will return a challenge when the user is not authenticated.
    /// If authentication schemes are specified, will return a challenge to them.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The proper ActionResult based upon if the user is authenticated.</returns>
    public static ActionResult ChallengeOrForbid(this ControllerBase controller, params string[] authenticationSchemes)
    {
        return controller.User.Identity?.IsAuthenticated ?? false
            ? controller.Forbid(authenticationSchemes)
            : controller.Challenge(authenticationSchemes);
    }

    /// <summary>
    /// Creates a <see cref="LocalRedirectResult"/> object that redirects to the specified local localUrl.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <param name="escapeUrl">Whether to escape the url.</param>
    public static ActionResult LocalRedirect(this ControllerBase controller, string localUrl, bool escapeUrl)
    {
        if (!escapeUrl)
        {
            return controller.LocalRedirect(localUrl);
        }

        return controller.LocalRedirect(localUrl.ToUriComponents());
    }

    /// <summary>
    /// Creates a <see cref="RedirectResult"/> object that redirects to the specified url.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="url">The URL to redirect to.</param>
    /// <param name="escapeUrl">Whether to escape the url.</param>
    public static ActionResult Redirect(this ControllerBase controller, string url, bool escapeUrl)
    {
        if (!escapeUrl)
        {
            return controller.Redirect(url);
        }

        return controller.Redirect(url.ToUriComponents());
    }
}
