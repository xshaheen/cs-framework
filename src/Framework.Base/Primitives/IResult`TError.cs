// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Primitives;

[PublicAPI]
public interface IResult<out TError>
{
    /// <summary>Flag indicating whether if the operation succeeded or not.</summary>
    /// <value>True if the operation succeeded, otherwise false.</value>
    bool Succeeded { get; }

    /// <summary>Flag indicating whether if the operation failed or not.</summary>
    /// <value>True if the operation failed, otherwise true.</value>
    bool Failed { get; }

    TResult Match<TResult>(Func<TResult> success, Func<TError, TResult> failure);
}
