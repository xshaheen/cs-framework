// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Microsoft.CodeAnalysis;

namespace Framework.Generator.Primitives.Models;

/// <summary>Represents data used by the code generator for generating Primitive types.</summary>
internal sealed class GeneratorData
{
    /// <summary>
    /// Gets or sets the field name.
    /// </summary>
    public string FieldName { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether to generate GetHashCode method.
    /// </summary>
    public bool GenerateHashCode { get; set; }

    /// <summary>
    /// Gets or sets the friendly name of the primitive type.
    /// </summary>
    public string PrimitiveTypeFriendlyName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type symbol.
    /// </summary>
    public INamedTypeSymbol TypeSymbol { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Primitive type.
    /// </summary>
    public PrimitiveUnderlyingType UnderlyingType { get; set; }

    /// <summary>
    /// Gets or sets the named type symbol of the primitive type.
    /// </summary>
    public INamedTypeSymbol PrimitiveTypeSymbol { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of parent symbols.
    /// </summary>
    public List<INamedTypeSymbol> ParentSymbols { get; set; } = null!;

    /// <summary>
    /// Gets or sets the namespace.
    /// </summary>
    public string Namespace { get; set; } = null!;

    /// <summary>
    /// Gets the class name.
    /// </summary>
    public string ClassName => TypeSymbol.Name;

    /// <summary>
    /// Gets or sets a value indicating whether to generate subtraction operators.
    /// </summary>
    public bool GenerateSubtractionOperators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate addition operators.
    /// </summary>
    public bool GenerateAdditionOperators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate division operators.
    /// </summary>
    public bool GenerateDivisionOperators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate multiplication operators.
    /// </summary>
    public bool GenerateMultiplyOperators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate modulus operator.
    /// </summary>
    public bool GenerateModulusOperator { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate comparison methods.
    /// </summary>
    public bool GenerateComparison { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate IParsable methods.
    /// </summary>
    public bool GenerateParsable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate implicit operators.
    /// </summary>
    public bool GenerateImplicitOperators { get; set; }

    /// <summary>
    /// Gets or sets the serialization format (if applicable).
    /// </summary>
    public string? SerializationFormat { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate ISpanFormattable methods.
    /// </summary>
    public bool GenerateSpanFormattable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate IConvertible methods.
    /// </summary>
    public bool GenerateConvertibles { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate IUtf8SpanFormattable methods.
    /// </summary>
    public bool GenerateUtf8SpanFormattable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the generate IXmlSerializable methods.
    /// </summary>
    public bool GenerateXmlSerializableMethods { get; set; }

    /// <summary>
    /// if StringLengthAttribute validation is applied to a Primitive this will be used to determine the values and use them before calling validation method.
    /// </summary>
    public (int minLength, int maxLength)? StringLengthAttributeValidation { get; set; }

    public bool HasMathOperators()
    {
        return GenerateAdditionOperators
            || GenerateSubtractionOperators
            || GenerateMultiplyOperators
            || GenerateDivisionOperators
            || GenerateModulusOperator;
    }

    public bool IsPrimitiveUnderlyingTypString()
    {
        return ParentSymbols.Count == 0 && UnderlyingType is PrimitiveUnderlyingType.String;
    }

    public bool IsPrimitiveUnderlyingTypeChar()
    {
        return ParentSymbols.Count == 0 && UnderlyingType is PrimitiveUnderlyingType.Char;
    }

    public bool IsPrimitiveUnderlyingTypeBool()
    {
        return ParentSymbols.Count == 0 && UnderlyingType is PrimitiveUnderlyingType.Boolean;
    }
}
