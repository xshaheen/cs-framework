// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Framework.Checks.Internals;

namespace Framework.Checks;

public static partial class Argument
{
    /// <summary>Asserts that the input value is of a specific type.</summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> is not of type <typeparamref name="T"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsOfType<T>(
        object argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        var type = typeof(T);

        if (argument.GetType() == type)
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultOfTypeMessage(paramName, argument, type), paramName);
    }

    /// <summary>Asserts that the input value is of a specific type.</summary>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if the type of <paramref name="argument"/> is not the same as <paramref name="type"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsOfType(
        object argument,
        Type type,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (argument.GetType() == type)
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultOfTypeMessage(paramName, argument, type), paramName);
    }

    /// <summary>Asserts that the input value is not of a specific type.</summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> is of type <typeparamref name="T"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotOfType<T>(
        object argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        var type = typeof(T);

        if (argument.GetType() != type)
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultNotOfTypeMessage(paramName, type), paramName);
    }

    /// <summary>Asserts that the input value is not of a specific type.</summary>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if the type of <paramref name="argument"/> is the same as <paramref name="type"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotOfType(
        object argument,
        Type type,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (argument.GetType() != type)
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultNotOfTypeMessage(paramName, type), paramName);
    }

    /// <summary>Asserts that the input value can be assigned to a specified type.</summary>
    /// <typeparam name="T">The type to check the input value against.</typeparam>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> can't be assigned to type <typeparamref name="T"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsAssignableToType<T>(
        object argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (argument is T)
        {
            return;
        }

        throw new ArgumentException(
            message ?? _GetDefaultAssignableToMessage(paramName, argument, typeof(T)),
            paramName
        );
    }

    /// <summary>Asserts that the input value can be assigned to a specified type.</summary>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> can't be assigned to <paramref name="type"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsAssignableToType(
        object argument,
        Type type,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (type.IsInstanceOfType(argument))
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultAssignableToMessage(paramName, argument, type), paramName);
    }

    /// <summary>Asserts that the input value can't be assigned to a specified type.</summary>
    /// <typeparam name="T">The type to check the input value against.</typeparam>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> can be assigned to type <typeparamref name="T"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotAssignableToType<T>(
        object argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (argument is not T)
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultNotAssignableToMessage(paramName, typeof(T)), paramName);
    }

    /// <summary>Asserts that the input value can't be assigned to a specified type.</summary>
    /// <param name="argument">The input <see cref="object"/> to test.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="message">(Optional) Custom error message</param>
    /// <param name="paramName">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="argument"/> can be assigned to <paramref name="type"/>.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotAssignableToType(
        object argument,
        Type type,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null
    )
    {
        if (!type.IsInstanceOfType(argument))
        {
            return;
        }

        throw new ArgumentException(message ?? _GetDefaultNotAssignableToMessage(paramName, type), paramName);
    }

    private static string _GetDefaultOfTypeMessage(string? paramName, object argument, Type expectedType)
    {
        return $"The argument {paramName.ToAssertString()} must be of type {expectedType.ToAssertString()}. (Actual type {argument.GetType().ToAssertString()})";
    }

    private static string _GetDefaultNotOfTypeMessage(string? paramName, Type type)
    {
        return $"The argument {paramName.ToAssertString()} must NOT be of type {type.ToAssertString()}.";
    }

    private static string _GetDefaultAssignableToMessage(string? paramName, object argument, Type expectedType)
    {
        return $"The argument {paramName.ToAssertString()} must be assignable to {expectedType.ToAssertString()}. (Actual type {argument.GetType().ToAssertString()})";
    }

    private static string _GetDefaultNotAssignableToMessage(string? paramName, Type expectedType)
    {
        return $"The argument {paramName.ToAssertString()} must NOT be assignable to {expectedType.ToAssertString()}.";
    }
}
