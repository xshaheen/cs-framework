﻿//HintName: DateTimePrimitiveValueConverter.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Framework.Primitives;
using System;
using Framework.Generator.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Framework.Primitives.Converters;

/// <summary>ValueConverter for <see cref = "DateTimePrimitive"/></summary>
public sealed class DateTimePrimitiveValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimePrimitive, DateTime>
{
    public DateTimePrimitiveValueConverter() : base(v => v, v => v) { }

    public DateTimePrimitiveValueConverter(global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null) : base(v => v, v => v, mappingHints) { }
}
