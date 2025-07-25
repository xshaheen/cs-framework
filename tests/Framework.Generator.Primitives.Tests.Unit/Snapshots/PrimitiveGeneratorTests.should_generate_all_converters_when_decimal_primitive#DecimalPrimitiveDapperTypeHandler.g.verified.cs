﻿//HintName: DecimalPrimitiveDapperTypeHandler.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using System;
using System.Globalization;
using Framework.Primitives;
using Framework.Generator.Primitives;

namespace Framework.Primitives.Converters;

/// <summary>Dapper TypeHandler for <see cref = "DecimalPrimitive"/></summary>
public sealed class DecimalPrimitiveDapperTypeHandler : global::Dapper.SqlMapper.TypeHandler<DecimalPrimitive>
{
    public override void SetValue(global::System.Data.IDbDataParameter parameter, DecimalPrimitive value)
    {
        parameter.Value = value.GetUnderlyingPrimitiveType();
    }

    public override DecimalPrimitive Parse(object value)
    {
        return value switch
        {
            decimal primitiveValue => new DecimalPrimitive(primitiveValue),
            _ => throw new global::System.InvalidCastException($"Unable to cast object of type {value.GetType()} to DecimalPrimitive"),
        };
    }
}
