﻿//HintName: ShortPrimitive.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by 'Primitives Generator'.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using System;
using System.Numerics;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Framework.Generator.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Framework.Primitives.Converters;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Framework.Primitives;

[UnderlyingPrimitiveType(typeof(short))]
[global::System.Diagnostics.DebuggerDisplay("{_value}")]
[global::System.Text.Json.Serialization.JsonConverter(typeof(ShortPrimitiveJsonConverter))]
[global::System.ComponentModel.TypeConverter(typeof(ShortPrimitiveTypeConverter))]
public readonly partial struct ShortPrimitive : global::System.IEquatable<ShortPrimitive>
        , global::System.IComparable
        , global::System.IComparable<ShortPrimitive>
        , global::System.Numerics.IComparisonOperators<ShortPrimitive, ShortPrimitive, bool>
        , global::System.ISpanParsable<ShortPrimitive>
        , global::System.IConvertible
        , global::System.Xml.Serialization.IXmlSerializable
#if NET8_0_OR_GREATER
        , global::System.IUtf8SpanFormattable
#endif
{
    /// <inheritdoc/>
    public Type GetUnderlyingPrimitiveType() => typeof(short);

    /// <inheritdoc/>
    public short GetUnderlyingPrimitiveValue() => this;

    private short _valueOrThrow => _isInitialized ? _value : throw new InvalidPrimitiveValueException("The domain value has not been initialized", this);

    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private readonly short _value;

    [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
    private readonly bool _isInitialized;

    /// <summary>Initializes a new instance of the <see cref="ShortPrimitive"/> class by validating the specified <see cref="short"/> value using <see cref="Validate"/> static method.</summary>
    /// <param name="value">The value to be validated.</param>
    public ShortPrimitive(short value) : this(value, true) { }

    private ShortPrimitive(short value, bool validate)
    {
        if (validate)
        {
            ValidateOrThrow(value);
        }
        _value = value;
        _isInitialized = true;
    }

#pragma warning disable AL1003 // Should not have non obsolete empty constructors.
    [Obsolete("Primitive cannot be created using empty Constructor", true)]
    public ShortPrimitive() { }
#pragma warning restore AL1003

    /// <summary>Tries to create an instance of AsciiString from the specified value.</summary>
    /// <param name="value">The value to create ShortPrimitive from</param>
    /// <param name="result">When this method returns, contains the created ShortPrimitive if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>true if the conversion succeeded; otherwise, false.</returns>
    public static bool TryCreate(short value, [NotNullWhen(true)] out ShortPrimitive? result)
    {
        return TryCreate(value, out result, out _);
    }

    /// <summary>Tries to create an instance of AsciiString from the specified value.</summary>
    /// <param name="value">The value to create ShortPrimitive from</param>
    /// <param name="result">When this method returns, contains the created ShortPrimitive if the conversion succeeded, or null if the conversion failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if the conversion failed; otherwise, null.</param>
    /// <returns>true if the conversion succeeded; otherwise, false.</returns>
    public static bool TryCreate(short value,[NotNullWhen(true)]  out ShortPrimitive? result, [NotNullWhen(false)]  out string? errorMessage)
    {
        var validationResult = Validate(value);

        if (!validationResult.IsValid)
        {
            result = null;
            errorMessage = validationResult.ErrorMessage;
            return false;
        }

        result = new (value, false);
        errorMessage = null;
        return true;
    }

    /// <summary>Validates the specified value and throws an exception if it is not valid.</summary>
    /// <param name="value">The value to validate</param>
    /// <exception cref="InvalidPrimitiveValueException">Thrown when the value is not valid.</exception>
    public void ValidateOrThrow(short value)
    {
        var result = Validate(value);

        if (!result.IsValid)
        {
            throw new InvalidPrimitiveValueException(result.ErrorMessage, this);
        }
    }

    #region IEquatable Implementation

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is ShortPrimitive other && Equals(other);

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(ShortPrimitive other)
    {
        if (!_isInitialized || !other._isInitialized)
        {
            return false;
        }

        return _value.Equals(other._value);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ShortPrimitive left, ShortPrimitive right) => left.Equals(right);

    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ShortPrimitive left, ShortPrimitive right) => !(left == right);

    #endregion

    #region IComparable Implementation

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        return obj switch
        {
            null => 1,
            ShortPrimitive c => CompareTo(c),
            _ => throw new ArgumentException("Object is not a ShortPrimitive", nameof(obj)),
        };
    }

    /// <inheritdoc/>
    public int CompareTo(ShortPrimitive other)
    {
        if (!other._isInitialized)
        {
            return 1;
        }

        if (!_isInitialized)
        {
            return -1;
        }

        return _value.CompareTo(other._value);
    }

    #endregion

    #region IParsable Implementation

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ShortPrimitive Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider) => short.Parse(s, provider);

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ShortPrimitive Parse(string s, global::System.IFormatProvider? provider) => Parse(s.AsSpan(), provider);

    /// <inheritdoc/>
    public static bool TryParse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider, [MaybeNullWhen(false)] out ShortPrimitive result)
    {
        if (!short.TryParse(s, provider, out var value))
        {
            result = default;
            return false;
        }

        if (TryCreate(value, out var created))
        {
            result = created.Value;
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc/>
    public static bool TryParse([NotNullWhen(true)] string? s, global::System.IFormatProvider? provider, [MaybeNullWhen(false)] out ShortPrimitive result) => TryParse(s is null ? [] : s.AsSpan(), provider, out result);

    #endregion

    #region IUtf8SpanFormattable Implementation

#if NET8_0_OR_GREATER
    /// <inheritdoc cref="global::System.IUtf8SpanFormattable.TryFormat"/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool TryFormat(global::System.Span<byte> utf8Destination, out int bytesWritten, [global::System.Diagnostics.CodeAnalysis.StringSyntax(global::System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)]global::System.ReadOnlySpan<char> format, global::System.IFormatProvider? provider)
    {
        return ((global::System.IUtf8SpanFormattable)_valueOrThrow).TryFormat(utf8Destination, out bytesWritten, format, provider);
    }
#endif

    #endregion

    #region IConvertible Implementation

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    global::System.TypeCode global::System.IConvertible.GetTypeCode() => ((global::System.IConvertible)(Int16)_valueOrThrow).GetTypeCode();

    /// <inheritdoc/>
    bool global::System.IConvertible.ToBoolean(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToBoolean(provider);

    /// <inheritdoc/>
    byte global::System.IConvertible.ToByte(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToByte(provider);

    /// <inheritdoc/>
    char global::System.IConvertible.ToChar(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToChar(provider);

    /// <inheritdoc/>
    global::System.DateTime global::System.IConvertible.ToDateTime(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToDateTime(provider);

    /// <inheritdoc/>
    decimal global::System.IConvertible.ToDecimal(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToDecimal(provider);

    /// <inheritdoc/>
    double global::System.IConvertible.ToDouble(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToDouble(provider);

    /// <inheritdoc/>
    short global::System.IConvertible.ToInt16(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToInt16(provider);

    /// <inheritdoc/>
    int global::System.IConvertible.ToInt32(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToInt32(provider);

    /// <inheritdoc/>
    long global::System.IConvertible.ToInt64(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToInt64(provider);

    /// <inheritdoc/>
    sbyte global::System.IConvertible.ToSByte(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToSByte(provider);

    /// <inheritdoc/>
    float global::System.IConvertible.ToSingle(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToSingle(provider);

    /// <inheritdoc/>
    string global::System.IConvertible.ToString(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToString(provider);

    /// <inheritdoc/>
    object global::System.IConvertible.ToType(Type conversionType, global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToType(conversionType, provider);

    /// <inheritdoc/>
    ushort global::System.IConvertible.ToUInt16(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToUInt16(provider);

    /// <inheritdoc/>
    uint global::System.IConvertible.ToUInt32(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToUInt32(provider);

    /// <inheritdoc/>
    ulong global::System.IConvertible.ToUInt64(global::System.IFormatProvider? provider) => ((global::System.IConvertible)(Int16)_valueOrThrow).ToUInt64(provider);

    #endregion

    #region IXmlSerializable Implementation

    /// <inheritdoc/>
    public XmlSchema? GetSchema() => null;

    /// <inheritdoc/>
    public void ReadXml(XmlReader reader)
    {
        var value = reader.ReadElementContentAs<short>();
        ValidateOrThrow(value);
        System.Runtime.CompilerServices.Unsafe.AsRef(in _value) = value;
        System.Runtime.CompilerServices.Unsafe.AsRef(in _isInitialized) = true;
    }

    /// <inheritdoc/>
    public void WriteXml(XmlWriter writer) => writer.WriteValue(((short)_valueOrThrow).ToXmlString());

    #endregion

    #region Implicit Operators

    /// <summary>Implicit conversion from <see cref = "short"/> to <see cref = "ShortPrimitive"/></summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator ShortPrimitive(short value) => new(value);

    /// <summary>Implicit conversion from <see cref = "short"/> (nullable) to <see cref = "ShortPrimitive"/> (nullable)</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator ShortPrimitive?(short? value) => value is null ? null : new(value.Value);

    /// <summary>Implicit conversion from <see cref = "ShortPrimitive"/> to <see cref = "short"/></summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator short(ShortPrimitive value) => (short)value._valueOrThrow;

    /// <summary>Implicit conversion from <see cref = "ShortPrimitive"/> (nullable) to <see cref = "short"/> (nullable)</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator short?(ShortPrimitive? value) => value is null ? null : (short?)value.Value._valueOrThrow;

    #endregion

    #region Comparison Operators

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator <(ShortPrimitive left, ShortPrimitive right) => left._valueOrThrow < right._valueOrThrow;

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(ShortPrimitive left, ShortPrimitive right) => left._valueOrThrow <= right._valueOrThrow;

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator >(ShortPrimitive left, ShortPrimitive right) => left._valueOrThrow > right._valueOrThrow;

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(ShortPrimitive left, ShortPrimitive right) => left._valueOrThrow >= right._valueOrThrow;

    #endregion

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _valueOrThrow.ToString();

    /// <inheritdoc/>
    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _valueOrThrow.GetHashCode();
}
