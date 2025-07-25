﻿//HintName: IntOfIntPrimitiveValueConverter.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Framework.Primitives;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Framework.Generator.Primitives;

namespace Framework.Primitives.EntityFrameworkCore.Converters;

/// <summary>ValueConverter for <see cref = "IntOfIntPrimitive"/></summary>
public sealed class IntOfIntPrimitiveValueConverter : ValueConverter<IntOfIntPrimitive, int>
{
    public IntOfIntPrimitiveValueConverter() : base(v => v, v => v) { }

    public IntOfIntPrimitiveValueConverter(ConverterMappingHints? mappingHints = null) : base(v => v, v => v, mappingHints) { }
}
