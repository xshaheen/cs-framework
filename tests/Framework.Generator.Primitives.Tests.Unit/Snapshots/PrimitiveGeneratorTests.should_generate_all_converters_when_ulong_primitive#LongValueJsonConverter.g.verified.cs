﻿//HintName: LongValueJsonConverter.g.cs
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

/// <summary>JsonConverter for <see cref = "LongValue"/></summary>
public sealed class LongValueJsonConverter : JsonConverter<LongValue>
{
    /// <inheritdoc/>
    public override LongValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.UInt64Converter.Read(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, LongValue value, JsonSerializerOptions options)
    {
        JsonInternalConverters.UInt64Converter.Write(writer, (ulong)value, options);
    }

    /// <inheritdoc/>
    public override LongValue ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.UInt64Converter.ReadAsPropertyName(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, LongValue value, JsonSerializerOptions options)
    {
        JsonInternalConverters.UInt64Converter.WriteAsPropertyName(writer, (ulong)value, options);
    }
}
