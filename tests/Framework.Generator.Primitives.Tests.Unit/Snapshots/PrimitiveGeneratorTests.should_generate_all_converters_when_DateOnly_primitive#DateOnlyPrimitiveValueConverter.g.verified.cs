﻿//HintName: DateOnlyPrimitiveValueConverter.g.cs
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

/// <summary>ValueConverter for <see cref = "DateOnlyPrimitive"/></summary>
public sealed class DateOnlyPrimitiveValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnlyPrimitive, DateOnly>
{
    public DateOnlyPrimitiveValueConverter() : base(v => v, v => v) { }

    public DateOnlyPrimitiveValueConverter(global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null) : base(v => v, v => v, mappingHints) { }
}
