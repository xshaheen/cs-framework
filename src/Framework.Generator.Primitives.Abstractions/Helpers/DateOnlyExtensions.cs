// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Framework.Generator.Primitives;

/// <summary>Utility methods for working with <see cref="DateOnly"/> and <see cref="TimeOnly"/> values.</summary>
public static class DateOnlyExtensions
{
    /// <summary>Converts a DateOnly value to a DateTime, with the time component set to the minimum value and kind set to local.</summary>
    /// <param name="value">The DateOnly value to convert.</param>
    /// <returns>A DateTime representation of the given DateOnly value.</returns>
    public static DateTime ToDateTime(this DateOnly value) => value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local);

    /// <summary>Converts a TimeOnly value to a DateTime, with the date component set to the minimum value and kind set to local.</summary>
    /// <param name="value">The TimeOnly value to convert.</param>
    /// <returns>A DateTime representation of the given TimeOnly value.</returns>
    public static DateTime ToDateTime(this TimeOnly value) => new(value.Ticks, DateTimeKind.Local);
}
