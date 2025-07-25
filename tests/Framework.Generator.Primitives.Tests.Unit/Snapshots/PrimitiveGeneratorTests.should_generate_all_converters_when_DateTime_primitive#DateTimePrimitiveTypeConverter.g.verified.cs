﻿//HintName: DateTimePrimitiveTypeConverter.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Framework.Primitives;
using System;
using System.ComponentModel;
using System.Globalization;
using Framework.Generator.Primitives;

namespace Framework.Primitives.Converters;

/// <summary>TypeConverter for <see cref = "DateTimePrimitive"/></summary>
public sealed class DateTimePrimitiveTypeConverter : DateTimeConverter
{
    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var result = base.ConvertFrom(context, culture, value);

        if (result is null)
        {
            return null;
        }

        try
        {
            return new DateTimePrimitive((DateTime)result);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new FormatException("Cannot parse DateTimePrimitive", ex);
        }
    }
}
