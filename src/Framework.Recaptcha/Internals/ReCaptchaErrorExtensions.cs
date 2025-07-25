// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Recaptcha.Contracts;

namespace Framework.Recaptcha.Internals;

internal static class ReCaptchaErrorExtensions
{
    public static ReCaptchaError ToReCaptchaError(this string error)
    {
        return error switch
        {
            "bad-request" => ReCaptchaError.BadRequest,
            "timeout-or-duplicate" => ReCaptchaError.TimeOutOrDuplicate,
            "invalid-input-response" => ReCaptchaError.InvalidInputResponse,
            "missing-input-response" => ReCaptchaError.MissingInputResponse,
            "invalid-input-secret" => ReCaptchaError.InvalidInputSecret,
            "missing-input-secret" => ReCaptchaError.MissingInputSecret,
            _ => throw new InvalidOperationException($"Unknown {nameof(ReCaptchaError)}={error}"),
        };
    }
}
