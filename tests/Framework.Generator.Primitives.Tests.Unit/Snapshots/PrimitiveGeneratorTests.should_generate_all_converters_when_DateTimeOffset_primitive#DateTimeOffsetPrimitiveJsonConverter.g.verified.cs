﻿//HintName: DateTimeOffsetPrimitiveJsonConverter.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Framework.Primitives;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json.Serialization.Metadata;
using Framework.Generator.Primitives;

namespace Framework.Primitives.Converters;

/// <summary>JsonConverter for <see cref = "DateTimeOffsetPrimitive"/></summary>
public sealed class DateTimeOffsetPrimitiveJsonConverter : JsonConverter<DateTimeOffsetPrimitive>
{
    /// <inheritdoc/>
    public override DateTimeOffsetPrimitive Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.DateTimeOffsetConverter.Read(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTimeOffsetPrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.DateTimeOffsetConverter.Write(writer, (DateTimeOffset)value, options);
    }

    /// <inheritdoc/>
    public override DateTimeOffsetPrimitive ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.DateTimeOffsetConverter.ReadAsPropertyName(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, DateTimeOffsetPrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.DateTimeOffsetConverter.WriteAsPropertyName(writer, (DateTimeOffset)value, options);
    }
}
