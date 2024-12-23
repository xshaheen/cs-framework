// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Text;
using System.Xml.Serialization;
using Framework.Generator.Primitives.Extensions;
using Framework.Generator.Primitives.Models;
using Framework.Generator.Primitives.Shared;
using Microsoft.CodeAnalysis;

namespace Framework.Generator.Primitives.Helpers;

internal static class PrimitiveSourceFilesGeneratorEmitter
{
    /// <summary>Processes the generator data and generates code for a specified class.</summary>
    /// <param name="context">The SourceProductionContext for reporting diagnostics.</param>
    /// <param name="data">The GeneratorData for the class.</param>
    /// <param name="ctorCode">The constructor code for the class.</param>
    /// <param name="options">The PrimitiveGlobalOptions for the generator.</param>
    internal static void AddPrimateImplementation(
        this SourceProductionContext context,
        GeneratorData data,
        string ctorCode,
        PrimitiveGlobalOptions options
    )
    {
        var modifiers = data.TypeSymbol.GetModifiers() ?? "public partial";

        if (!modifiers.Contains("partial"))
        {
            context.ReportDiagnostic(DiagnosticHelper.ClassMustBePartial(data.TypeSymbol.Locations.FirstOrDefault()));
        }

        var builder = new SourceCodeBuilder();

        var usings = new List<string>
        {
            "System",
            "System.Numerics",
            "System.Diagnostics",
            "System.Runtime.CompilerServices",
            AbstractionConstants.Namespace,
        };

        if (data.ParentSymbols.Count > 0)
        {
            usings.Add(data.ParentSymbols[0].ContainingNamespace.ToDisplayString());
        }

        if (data.GenerateImplicitOperators)
        {
            usings.Add("System.Diagnostics.CodeAnalysis");
        }

        if (options.GenerateJsonConverters)
        {
            usings.Add("System.Text.Json.Serialization");
            usings.Add($"{data.Namespace}.Converters");
        }

        if (options.GenerateTypeConverters)
        {
            usings.Add("System.ComponentModel");
        }

        if (options.GenerateXmlConverters)
        {
            usings.Add("System.Xml");
            usings.Add("System.Xml.Schema");
            usings.Add("System.Xml.Serialization");
        }

        var needsMathOperators = data.HasMathOperators();

        var isByteOrShort = data.ParentSymbols.Count == 0 && data.UnderlyingType.IsByteOrShort();

        builder.AppendSourceHeader("Primitives Generator");
        builder.AppendUsings(usings);
        builder.AppendNamespace(data.Namespace);

        if (options.GenerateJsonConverters)
        {
            builder.AppendLine($"[JsonConverter(typeof({data.ClassName + "JsonConverter"}))]");
        }

        if (options.GenerateTypeConverters)
        {
            builder.AppendLine(
                $"[TypeConverter(typeof({TypeConverterSourceFilesGeneratorEmitter.CreateClassName(data.ClassName)}))]"
            );
        }

        builder.Append("[UnderlyingPrimitiveType(typeof(").Append(data.PrimitiveTypeFriendlyName).AppendLine("))]");

        builder.AppendLine("[DebuggerDisplay(\"{\" + nameof(_value) + \"}\")]");

        if (!data.TypeSymbol.IsValueType)
        {
            builder.AppendClass(
                isRecord: false,
                modifiers,
                data.ClassName,
                createInheritedInterfaces(data, data.ClassName)
            );
        }
        else
        {
            builder.AppendStruct(modifiers, data.ClassName, createInheritedInterfaces(data, data.ClassName));
        }

        builder
            .AppendInheritDoc()
            .Append("public Type GetUnderlyingPrimitiveType() => typeof(")
            .Append(data.PrimitiveTypeFriendlyName)
            .AppendLine(");")
            .NewLine();

        builder
            .AppendInheritDoc()
            .AppendLine($"public {data.PrimitiveTypeFriendlyName} GetUnderlyingPrimitiveValue() => this;")
            .NewLine();

        builder.AppendLines(ctorCode);

        if (needsMathOperators && isByteOrShort)
        {
            // Add int constructor
            builder.AppendComment("Private constructor with 'int' value");

            builder
                .Append(
                    $"private {data.ClassName}(int value) : this(value is >= {data.PrimitiveTypeFriendlyName}.MinValue and <= {data.PrimitiveTypeFriendlyName}.MaxValue ? ({data.PrimitiveTypeFriendlyName})value : throw new InvalidPrimitiveValueException(\"The value has exceeded a {data.PrimitiveTypeFriendlyName} limit\"))"
                )
                .EmptyBracket()
                .NewLine()
                .NewLine();
        }

        builder.GenerateMandatoryMethods(data);

        if (string.Equals(data.PrimitiveTypeFriendlyName, "string", StringComparison.Ordinal))
        {
            builder.NewLine();
            builder.GenerateStringMethods();
        }

        builder.NewLine();
        builder.AppendRegion("IEquatable Implementation");
        builder.GenerateEquatableOperators(data.ClassName, data.TypeSymbol.IsValueType);
        builder.NewLine();
        builder.AppendEndRegion();

        builder.NewLine();
        builder.AppendRegion("IComparable Implementation");
        builder.GenerateComparableCode(data.ClassName, data.TypeSymbol.IsValueType);
        builder.NewLine();
        builder.AppendEndRegion();

        if (data.GenerateParsable)
        {
            builder.NewLine();
            builder.AppendRegion("IParsable Implementation");
            builder.GenerateParsable(data);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        if (data.GenerateSpanFormattable)
        {
            builder.NewLine();
            builder.AppendRegion("IFormattable Implementation");
            builder.GenerateSpanFormattable(data.FieldName);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        if (data.GenerateUtf8SpanFormattable)
        {
            builder.NewLine();
            builder.AppendRegion("IUtf8SpanFormattable Implementation");
            builder.GenerateUtf8Formattable(data.FieldName);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        if (data.GenerateConvertibles)
        {
            builder.NewLine();
            builder.AppendRegion("IConvertible Implementation");
            builder.GenerateConvertibles(data);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        if (data.GenerateXmlSerializableMethods)
        {
            builder.NewLine();
            builder.AppendRegion("IXmlSerializable Implementation");
            builder.GenerateIXmlSerializableMethods(data);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        if (data.GenerateImplicitOperators)
        {
            builder.NewLine();
            builder.AppendRegion("Implicit Operators");
            builder.GenerateImplicitOperators(data);
            builder.AppendEndRegion();
        }

        if (needsMathOperators)
        {
            builder.NewLine();
            builder.AppendRegion("Math Operators");

            if (data.GenerateAdditionOperators)
            {
                builder.GenerateAdditionCode(data.ClassName, data.FieldName);
                builder.NewLine();
            }

            if (data.GenerateSubtractionOperators)
            {
                builder.GenerateSubtractionCode(data.ClassName, data.FieldName);
                builder.NewLine();
            }

            if (data.GenerateMultiplyOperators)
            {
                builder.GenerateMultiplyCode(data.ClassName, data.FieldName);
                builder.NewLine();
            }

            if (data.GenerateDivisionOperators)
            {
                builder.GenerateDivisionCode(data.ClassName, data.FieldName);
                builder.NewLine();
            }

            if (data.GenerateModulusOperator)
            {
                builder.GenerateModulusCode(data.ClassName, data.FieldName);
                builder.NewLine();
            }

            builder.AppendEndRegion();
        }

        if (data.GenerateComparison)
        {
            builder.NewLine();
            builder.AppendRegion("Comparison Operators");
            builder.GenerateComparisonCode(data.ClassName, data.FieldName);
            builder.NewLine();
            builder.AppendEndRegion();
        }

        var baseType = data.ParentSymbols.Count == 0 ? data.PrimitiveTypeSymbol : data.ParentSymbols[0];

        var hasExplicitToStringMethod = data
            .TypeSymbol.GetMembersOfType<IMethodSymbol>()
            .Any(x =>
                string.Equals(x.Name, "ToString", StringComparison.Ordinal)
                && x is { IsStatic: true, Parameters.Length: 1 }
                && x.Parameters[0].Type.Equals(baseType, SymbolEqualityComparer.Default)
            );

        builder.NewLine();
        builder.AppendInheritDoc();
        builder.AppendMethodAggressiveInliningAttribute();

        builder.AppendLine(
            hasExplicitToStringMethod
                ? $"public override string ToString() => ToString({data.FieldName});"
                : $"public override string ToString() => {data.FieldName}.ToString();"
        );

        if (data.GenerateHashCode)
        {
            builder.NewLine();
            builder.AppendInheritDoc();
            builder.AppendMethodAggressiveInliningAttribute();

            builder.AppendLine(
                data.IsPrimitiveUnderlyingTypString()
                    ? $"public override int GetHashCode() => {data.FieldName}.GetHashCode(StringComparison.Ordinal);"
                    : $"public override int GetHashCode() => {data.FieldName}.GetHashCode();"
            );
        }

        builder.CloseBracket();

        context.AddSource(data.ClassName + ".g", builder.ToString());

        return;

        static string createInheritedInterfaces(GeneratorData data, string className)
        {
            var sb = new StringBuilder(8096);

            sb.Append("IEquatable<").Append(className).Append('>');

            appendInterface(sb, nameof(IComparable));
            appendInterface(sb, "IComparable<").Append(className).Append('>');

            if (data.GenerateAdditionOperators)
            {
                appendInterface(sb, "IAdditionOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append('>');
            }

            if (data.GenerateSubtractionOperators)
            {
                appendInterface(sb, "ISubtractionOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append('>');
            }

            if (data.GenerateMultiplyOperators)
            {
                appendInterface(sb, "IMultiplyOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append('>');
            }

            if (data.GenerateDivisionOperators)
            {
                appendInterface(sb, "IDivisionOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append('>');
            }

            if (data.GenerateModulusOperator)
            {
                appendInterface(sb, "IModulusOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append('>');
            }

            if (data.GenerateComparison)
            {
                appendInterface(sb, "IComparisonOperators<")
                    .Append(className)
                    .Append(", ")
                    .Append(className)
                    .Append(", ")
                    .Append("bool>");
            }

            if (data.GenerateSpanFormattable)
            {
                appendInterface(sb, "ISpanFormattable");
            }

            if (data.GenerateParsable)
            {
                appendInterface(sb, "ISpanParsable<").Append(className).Append('>');
            }

            if (data.GenerateConvertibles)
            {
                appendInterface(sb, nameof(IConvertible));
            }

            if (data.GenerateXmlSerializableMethods)
            {
                appendInterface(sb, nameof(IXmlSerializable));
            }

            if (data.GenerateUtf8SpanFormattable)
            {
                sb.AppendLine().Append("#if NET8_0_OR_GREATER");
                appendInterface(sb, "IUtf8SpanFormattable");
                sb.AppendLine().Append("#endif");
            }

            return sb.ToString();

            static StringBuilder appendInterface(StringBuilder sb, string interfaceName) =>
                sb.AppendLine().Append(SourceCodeBuilder.GetIndentation(2)).Append(", ").Append(interfaceName);
        }
    }

    /// <summary>Generates code for a JsonConverter for the specified type.</summary>
    /// <param name="context">The source production context.</param>
    /// <param name="data">The generator data containing type information.</param>
    internal static void AddJsonConverter(this SourceProductionContext context, GeneratorData data)
    {
        var builder = new SourceCodeBuilder();

        builder.AppendSourceHeader("Primitives Generator");

        builder.AppendUsings(
            [
                data.Namespace,
                "System",
                "System.Text.Json",
                "System.Text.Json.Serialization",
                "System.Globalization",
                "System.Text.Json.Serialization.Metadata",
                AbstractionConstants.Namespace,
            ]
        );

        var converterName = data.UnderlyingType.ToString();
        var primitiveTypeIsValueType = data.PrimitiveTypeSymbol.IsValueType;

        builder.AppendNamespace(data.Namespace + ".Converters");
        builder.AppendSummary($"JsonConverter for <see cref = \"{data.ClassName}\"/>");

        builder.AppendClass(
            isRecord: false,
            "public sealed",
            data.ClassName + "JsonConverter",
            $"JsonConverter<{data.ClassName}>"
        );

        builder
            .AppendInheritDoc()
            .Append("public override ")
            .Append(data.ClassName)
            .AppendLine(" Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)")
            .OpenBracket();

        if (data.SerializationFormat is null)
        {
            builder
                .AppendLine("try")
                .OpenBracket()
                .AppendLine(
                    $"return JsonInternalConverters.{converterName}Converter.Read(ref reader, typeToConvert, options){(primitiveTypeIsValueType ? "" : "!")};"
                )
                .CloseBracket();
        }
        else
        {
            builder
                .AppendLine("if (reader.TokenType != JsonTokenType.String)")
                .AppendIndentation()
                .Append("throw new JsonException(\"Expected a string value to deserialize ")
                .Append(data.ClassName)
                .AppendLine("\");")
                .NewLine()
                .Append(
                    "var str = reader.GetString() ?? throw new JsonException(\"Expected a non-null string value to deserialize "
                )
                .Append(data.ClassName)
                .AppendLine("\");")
                .AppendLine("try")
                .OpenBracket()
                .Append("return ")
                .Append(data.ClassName)
                .AppendLine(".Parse(str, CultureInfo.InvariantCulture);")
                .CloseBracket();
        }

        builder
            .AppendLine("catch (InvalidPrimitiveValueException ex)")
            .OpenBracket()
            .AppendLine("throw new JsonException(ex.Message);")
            .CloseBracket()
            .CloseBracket()
            .NewLine();

        builder
            .AppendInheritDoc()
            .AppendLine(
                $"public override void Write(Utf8JsonWriter writer, {data.ClassName} value, JsonSerializerOptions options)"
            )
            .OpenBracket()
            .AppendLineIf(
                data.SerializationFormat is null,
                $"JsonInternalConverters.{converterName}Converter.Write(writer, ({data.PrimitiveTypeFriendlyName})value, options);"
            )
            .AppendLineIf(
                data.SerializationFormat is not null,
                $"writer.WriteStringValue(value.ToString(\"{data.SerializationFormat}\", CultureInfo.InvariantCulture));"
            )
            .CloseBracket()
            .NewLine();

        builder
            .AppendInheritDoc()
            .Append("public override ")
            .Append(data.ClassName)
            .AppendLine(
                " ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)"
            )
            .OpenBracket();

        if (data.SerializationFormat is null)
        {
            builder
                .AppendLine("try")
                .OpenBracket()
                .AppendLine(
                    $"return JsonInternalConverters.{converterName}Converter.ReadAsPropertyName(ref reader, typeToConvert, options){(primitiveTypeIsValueType ? "" : "!")};"
                )
                .CloseBracket();
        }
        else
        {
            builder
                .AppendLine("if (reader.TokenType != JsonTokenType.String)")
                .AppendIndentation()
                .Append("throw new JsonException(\"Expected a string value to deserialize ")
                .Append(data.ClassName)
                .AppendLine("\");")
                .NewLine()
                .Append(
                    "var str = reader.GetString() ?? throw new JsonException(\"Expected a non-null string value to deserialize "
                )
                .Append(data.ClassName)
                .AppendLine("\");")
                .AppendLine("try")
                .OpenBracket()
                .Append("return ")
                .Append(data.ClassName)
                .AppendLine(".Parse(str, CultureInfo.InvariantCulture);")
                .CloseBracket();
        }

        builder
            .AppendLine("catch (InvalidPrimitiveValueException ex)")
            .OpenBracket()
            .AppendLine("throw new JsonException(ex.Message);")
            .CloseBracket()
            .CloseBracket()
            .NewLine();

        builder
            .AppendInheritDoc()
            .Append("public override void WriteAsPropertyName(Utf8JsonWriter writer, ")
            .Append(data.ClassName)
            .AppendLine(" value, JsonSerializerOptions options)")
            .OpenBracket()
            .AppendLineIf(
                data.SerializationFormat is null,
                $"JsonInternalConverters.{converterName}Converter.WriteAsPropertyName(writer, ({data.PrimitiveTypeFriendlyName})value, options);"
            )
            .AppendLineIf(
                data.SerializationFormat is not null,
                $"writer.WritePropertyName(value.ToString(\"{data.SerializationFormat}\", CultureInfo.InvariantCulture));"
            )
            .CloseBracket();

        builder.CloseBracket();

        context.AddSource($"{data.ClassName}JsonConverter.g.cs", builder.ToString());
    }
}
