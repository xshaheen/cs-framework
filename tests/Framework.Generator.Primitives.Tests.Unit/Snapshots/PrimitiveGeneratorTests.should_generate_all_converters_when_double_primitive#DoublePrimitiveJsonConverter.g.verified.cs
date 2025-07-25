﻿//HintName: DoublePrimitiveJsonConverter.g.cs
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

/// <summary>JsonConverter for <see cref = "DoublePrimitive"/></summary>
public sealed class DoublePrimitiveJsonConverter : JsonConverter<DoublePrimitive>
{
    /// <inheritdoc/>
    public override DoublePrimitive Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.DoubleConverter.Read(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DoublePrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.DoubleConverter.Write(writer, (double)value, options);
    }

    /// <inheritdoc/>
    public override DoublePrimitive ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return JsonInternalConverters.DoubleConverter.ReadAsPropertyName(ref reader, typeToConvert, options);
        }
        catch (InvalidPrimitiveValueException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, DoublePrimitive value, JsonSerializerOptions options)
    {
        JsonInternalConverters.DoubleConverter.WriteAsPropertyName(writer, (double)value, options);
    }
}
