// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Framework.Checks.Internals;

namespace Framework.Checks;

public static partial class Argument
{
    /// <summary>Asserts that the input value is <see langword="default"/>.</summary>
    /// <typeparam name="T">The type of <see langword="struct"/> value type being tested.</typeparam>
    /// <param name="argument">The input value to test.</param>
    /// <param name="message">(Optional) Custom error message.</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> is not <see langword="default"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsDefault<T>(
        T argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
        where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(argument, default))
        {
            return;
        }

        message ??= $"The argument {paramName.ToAssertString()} must be default.";

        throw new ArgumentException(message, paramName);
    }

    /// <summary>Throws an <see cref="ArgumentException" /> if <paramref name="argument" /> is default(T).</summary>
    /// <param name="argument">The argument to check.</param>
    /// <param name="message">(Optional) Custom error message.</param>
    /// <param name="paramName">Parameter name (auto generated no need to pass it).</param>
    /// <returns><paramref name="argument" /> if the value is not default for that type.</returns>
    /// <exception cref="ArgumentException">if <paramref name="argument" /> is default for that type.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T IsNotDefault<T>(
        T argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
        where T : struct
    {
        if (!EqualityComparer<T>.Default.Equals(argument, default))
        {
            return argument;
        }

        message ??=
            $"The argument {paramName.ToAssertString()} can NOT be the default value of {typeof(T).ToAssertString()}.";

        throw new ArgumentException(message, paramName);
    }

    /// <inheritdoc cref="IsNotDefault{T}(T,string?,string?)"/>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? IsNotDefault<T>(
        T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
        where T : struct
    {
        if (argument is null)
        {
            return argument;
        }

        if (EqualityComparer<T>.Default.Equals(argument.Value, default))
        {
            throw new ArgumentException(
                message ?? $"{paramName.ToAssertString()} cannot be the default value of {typeof(T).Name}.",
                paramName
            );
        }

        return argument;
    }

    /// <inheritdoc cref="IsNotDefault{T}(T,string?,string?)"/>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? IsNotDefaultOrNull<T>(
        T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
        where T : struct
    {
        IsNotNull(argument, message, paramName);
        IsNotDefault(argument, message, paramName);

        return argument;
    }
}
