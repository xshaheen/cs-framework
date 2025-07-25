// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Recaptcha.Contracts;

public enum ReCaptchaError
{
    /// <summary>The secret parameter is missing.</summary>
    MissingInputSecret = 0,

    /// <summary>The secret parameter is invalid or malformed.</summary>
    InvalidInputSecret = 1,

    /// <summary>The response parameter is missing.</summary>
    MissingInputResponse = 2,

    /// <summary>The response parameter is invalid or malformed.</summary>
    InvalidInputResponse = 3,

    /// <summary>The request is invalid or malformed.</summary>
    BadRequest = 4,

    /// <summary>The response is no longer valid: either is too old or has been used previously.</summary>
    TimeOutOrDuplicate = 5,
}
