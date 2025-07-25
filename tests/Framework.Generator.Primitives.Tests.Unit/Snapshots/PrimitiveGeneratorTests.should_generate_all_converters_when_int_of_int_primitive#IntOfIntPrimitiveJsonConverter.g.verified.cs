﻿//HintName: IntOfIntPrimitiveJsonConverter.g.cs
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

/// <summary>JsonConverter for <see cref = "IntOfIntPrimitive"/></summary>
public sealed class IntOfIntPrimitiveJsonConverter : JsonConverter<IntOfIntPrimitive>
{
    /// <inheritdoc/>
    public override IntOfIntPrimitive Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.Int32Converter.Read(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, IntOfIntPrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.Int32Converter.Write(writer, (int)value, options);
    }

    /// <inheritdoc/>
    public override IntOfIntPrimitive ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.Int32Converter.ReadAsPropertyName(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, IntOfIntPrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.Int32Converter.WriteAsPropertyName(writer, (int)value, options);
    }
}
