// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Collections.Immutable;
using Framework.Generator.Primitives.Extensions;
using Framework.Generator.Primitives.Helpers;
using Framework.Generator.Primitives.Models;
using Framework.Generator.Primitives.Shared;
using Microsoft.CodeAnalysis;

namespace Framework.Generator.Primitives;

/// <summary>A static class responsible for executing the generation of code for primitive types.</summary>
internal static class Emitter
{
    /// <summary>Executes the generation of primitives based on the provided parameters.</summary>
    /// <param name="context">The source production context.</param>
    /// <param name="typesToGenerate">The list of primitives to generate.</param>
    /// <param name="assemblyName">The name of the assembly.</param>
    /// <param name="globalOptions">The global options for primitive generation.</param>
    internal static void Execute(
        in SourceProductionContext context,
        in ImmutableArray<INamedTypeSymbol?> typesToGenerate,
        in string assemblyName,
        in PrimitiveGlobalOptions globalOptions
    )
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        if (typesToGenerate.IsDefaultOrEmpty)
        {
            return;
        }

        var swaggerTypes = new List<GeneratorData>(typesToGenerate.Length);
        var efValueConverterTypes = new List<INamedTypeSymbol>(typesToGenerate.Length);
        var dapperConverterTypes = new List<INamedTypeSymbol>(typesToGenerate.Length);

        var cachedOperationsAttributes = new Dictionary<INamedTypeSymbol, SupportedOperationsAttributeData>(
            SymbolEqualityComparer.Default
        );

        try
        {
            foreach (var typeSymbol in typesToGenerate)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                if (typeSymbol is null) // Will never happen
                {
                    continue;
                }

                var generatorData = _CreateGeneratorData(
                    context,
                    typeSymbol,
                    globalOptions,
                    cachedOperationsAttributes
                );

                if (generatorData is null)
                {
                    continue;
                }

                switch (generatorData.PrimitiveTypeSymbol.IsValueType)
                {
                    case true when !generatorData.TypeSymbol.IsValueType:
                        context.ReportDiagnostic(
                            DiagnosticHelper.TypeShouldBeValueType(
                                generatorData.ClassName,
                                generatorData.PrimitiveTypeFriendlyName,
                                typeSymbol.Locations.FirstOrDefault()
                            )
                        );

                        break;

                    case false when generatorData.TypeSymbol.IsValueType:
                        context.ReportDiagnostic(
                            DiagnosticHelper.TypeShouldBeReferenceType(
                                generatorData.ClassName,
                                generatorData.PrimitiveTypeFriendlyName,
                                typeSymbol.Locations.FirstOrDefault()
                            )
                        );

                        break;
                }

                if (!_ProcessType(generatorData, globalOptions, context))
                {
                    continue;
                }

                if (globalOptions.GenerateJsonConverters)
                {
                    context.AddJsonConverter(generatorData);
                }

                if (globalOptions.GenerateTypeConverters)
                {
                    context.AddTypeConverter(generatorData);
                }

                if (globalOptions.GenerateEntityFrameworkValueConverters)
                {
                    efValueConverterTypes.Add(generatorData.TypeSymbol);

                    context.AddEntityFrameworkValueConverter(generatorData);
                }

                if (globalOptions.GenerateDapperConverters)
                {
                    dapperConverterTypes.Add(generatorData.TypeSymbol);

                    context.AddDapperTypeHandlerConverter(generatorData);
                }

                if (globalOptions.GenerateSwashbuckleSwaggerConverters || globalOptions.GenerateNswagSwaggerConverters)
                {
                    swaggerTypes.Add(generatorData);
                }
            }

            // Add helpers

            var addAssemblyAttribute = true;

            if (globalOptions.GenerateSwashbuckleSwaggerConverters && swaggerTypes.Count > 0)
            {
                context.AddSwashbuckleSwaggerMappingsHelper(assemblyName, swaggerTypes, addAssemblyAttribute);
                addAssemblyAttribute = false;
            }

            if (globalOptions.GenerateNswagSwaggerConverters && swaggerTypes.Count > 0)
            {
                context.AddNswagSwaggerMappingsHelper(assemblyName, swaggerTypes, addAssemblyAttribute);
                addAssemblyAttribute = false;
            }

            if (globalOptions.GenerateEntityFrameworkValueConverters && efValueConverterTypes.Count > 0)
            {
                context.AddEntityFrameworkValueConvertersHelper(
                    assemblyName,
                    efValueConverterTypes,
                    addAssemblyAttribute
                );
                addAssemblyAttribute = false;
            }

            if (globalOptions.GenerateDapperConverters && dapperConverterTypes.Count > 0)
            {
                context.AddDapperTypeHandlersHelper(assemblyName, dapperConverterTypes, addAssemblyAttribute);
            }
        }
        catch (Exception ex)
        {
            context.ReportDiagnostic(DiagnosticHelper.GeneralError(Location.None, ex));
        }
    }

    /// <summary>Creates generator data for a specified class symbol.</summary>
    /// <returns>The GeneratorData for the class or null if not applicable.</returns>
    private static GeneratorData? _CreateGeneratorData(
        in SourceProductionContext context,
        in INamedTypeSymbol typeSymbol,
        in PrimitiveGlobalOptions globalOptions,
        in Dictionary<INamedTypeSymbol, SupportedOperationsAttributeData> cachedOperationsAttributes
    )
    {
        var interfaceType = typeSymbol.AllInterfaces.First(x => x.IsImplementIPrimitive());

        if (interfaceType.TypeArguments[0] is not INamedTypeSymbol primitiveType)
        {
            context.ReportDiagnostic(DiagnosticHelper.InvalidBaseTypeSpecified(typeSymbol.Locations.FirstOrDefault()));

            return null;
        }

        var parentSymbols = new List<INamedTypeSymbol>(4);

        var (underlyingType, underlyingTypeSymbol) = primitiveType.GetUnderlyingPrimitiveType(parentSymbols);

        if (underlyingType == PrimitiveUnderlyingType.Other)
        {
            context.ReportDiagnostic(DiagnosticHelper.InvalidBaseTypeSpecified(typeSymbol.Locations.FirstOrDefault()));

            return null;
        }

        var hasOverridenHashCode = typeSymbol
            .GetMembersOfType<IMethodSymbol>()
            .Any(x => string.Equals(x.OverriddenMethod?.Name, "GetHashCode", StringComparison.Ordinal));

        var generatorData = new GeneratorData
        {
            FieldName = "_valueOrThrow",
            GenerateHashCode = !hasOverridenHashCode,
            UnderlyingType = underlyingType,
            TypeSymbol = typeSymbol,
            PrimitiveTypeSymbol = underlyingTypeSymbol,
            PrimitiveTypeFriendlyName = underlyingTypeSymbol.GetFriendlyName(),
            Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
            GenerateImplicitOperators = true,
            ParentSymbols = parentSymbols,
            GenerateConvertibles = underlyingType != PrimitiveUnderlyingType.Guid,
        };

        var attributes = typeSymbol.GetAttributes();

        var attributeData = attributes.FirstOrDefault(x =>
            string.Equals(
                x.AttributeClass?.ToDisplayString(),
                AbstractionConstants.SupportedOperationsAttributeFullName,
                StringComparison.Ordinal
            )
        );

        var serializationAttribute = attributes.FirstOrDefault(x =>
            string.Equals(
                x.AttributeClass?.ToDisplayString(),
                AbstractionConstants.SerializationFormatAttributeFullName,
                StringComparison.Ordinal
            )
        );

        var isNumeric = underlyingType.IsNumeric();
        var isDateOrTime = underlyingType.IsDateOrTime();

        if (!isNumeric && attributeData is not null)
        {
            context.ReportDiagnostic(
                DiagnosticHelper.TypeMustBeNumericType(attributeData.GetAttributeLocation(), typeSymbol.Name)
            );

            return null;
        }

        if (!isDateOrTime && serializationAttribute is not null)
        {
            context.ReportDiagnostic(
                DiagnosticHelper.TypeMustBeDateType(serializationAttribute.GetAttributeLocation(), typeSymbol.Name)
            );

            return null;
        }

        if (serializationAttribute is not null && serializationAttribute.ConstructorArguments.Length != 0)
        {
            var value = serializationAttribute.ConstructorArguments[0];
            generatorData.SerializationFormat = value.Value?.ToString();
        }

        if (isNumeric)
        {
            var supportedOperations = _GetSupportedOperationsAttributeData(
                typeSymbol,
                underlyingType,
                parentSymbols,
                cachedOperationsAttributes
            );

            generatorData.GenerateAdditionOperators =
                supportedOperations.Addition && !typeSymbol.ImplementsInterface(TypeNames.IAdditionOperators);

            generatorData.GenerateSubtractionOperators =
                supportedOperations.Subtraction && !typeSymbol.ImplementsInterface(TypeNames.ISubtractionOperators);

            generatorData.GenerateDivisionOperators =
                supportedOperations.Division && !typeSymbol.ImplementsInterface(TypeNames.IDivisionOperators);

            generatorData.GenerateMultiplyOperators =
                supportedOperations.Multiplication && !typeSymbol.ImplementsInterface(TypeNames.IMultiplyOperators);

            generatorData.GenerateModulusOperator =
                supportedOperations.Modulus && !typeSymbol.ImplementsInterface(TypeNames.IModulusOperators);
        }

        generatorData.GenerateParsable = !typeSymbol.ImplementsInterface(TypeNames.IParsable);

        generatorData.GenerateComparison =
            (isNumeric || underlyingType == PrimitiveUnderlyingType.Char || underlyingType.IsDateOrTime())
            && !typeSymbol.ImplementsInterface(TypeNames.IComparisonOperators);

        generatorData.GenerateSpanFormattable =
            (underlyingType == PrimitiveUnderlyingType.Guid || underlyingType.IsDateOrTime())
            && !typeSymbol.ImplementsInterface(TypeNames.ISpanFormattable);

        generatorData.GenerateUtf8SpanFormattable =
            primitiveType.ImplementsInterface(TypeNames.IUtf8SpanFormattable)
            && !typeSymbol.ImplementsInterface(TypeNames.IUtf8SpanFormattable);

        generatorData.GenerateXmlSerializableMethods = globalOptions.GenerateXmlConverters;

        return generatorData;
    }

    /// <summary>
    /// Retrieves the SupportedOperationsAttributeData for a specified class, considering inheritance.
    /// </summary>
    /// <returns>The combined SupportedOperationsAttributeData for the class and its inherited types.</returns>
    private static SupportedOperationsAttributeData _GetSupportedOperationsAttributeData(
        in INamedTypeSymbol @class,
        in PrimitiveUnderlyingType underlyingType,
        in List<INamedTypeSymbol> parentSymbols,
        in Dictionary<INamedTypeSymbol, SupportedOperationsAttributeData> cachedOperationsAttributes
    )
    {
        return createCombinedAttribute(@class, underlyingType, parentSymbols.Count, cachedOperationsAttributes);

        static SupportedOperationsAttributeData createCombinedAttribute(
            INamedTypeSymbol @class,
            PrimitiveUnderlyingType underlyingType,
            int parentCount,
            Dictionary<INamedTypeSymbol, SupportedOperationsAttributeData> cachedOperationsAttributes
        )
        {
            if (cachedOperationsAttributes.TryGetValue(@class, out var parentAttribute))
            {
                return parentAttribute;
            }

            var attributeData = @class
                .GetAttributes()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.AttributeClass?.ToDisplayString(),
                        AbstractionConstants.SupportedOperationsAttributeFullName,
                        StringComparison.Ordinal
                    )
                );

            var attribute = attributeData is null ? null : _GetAttributeFromData(attributeData);

            if (parentCount == 0)
            {
                attribute ??= _GetDefaultAttributeData(underlyingType);
                cachedOperationsAttributes[@class] = attribute;

                return attribute;
            }

            var parentType = @class.Interfaces.First(x => x.IsImplementIPrimitive());

            var attr = combineAttribute(
                attribute,
                createCombinedAttribute(
                    (parentType.TypeArguments[0] as INamedTypeSymbol)!,
                    underlyingType,
                    parentCount - 1,
                    cachedOperationsAttributes
                )
            );

            cachedOperationsAttributes[@class] = attr;

            return attr;
        }

        static SupportedOperationsAttributeData combineAttribute(
            SupportedOperationsAttributeData? attribute,
            SupportedOperationsAttributeData parentAttribute
        )
        {
            if (attribute is null)
            {
                return parentAttribute;
            }

            return new SupportedOperationsAttributeData
            {
                Addition = attribute.Addition && parentAttribute.Addition,
                Subtraction = attribute.Subtraction && parentAttribute.Subtraction,
                Multiplication = attribute.Multiplication && parentAttribute.Multiplication,
                Division = attribute.Division && parentAttribute.Division,
                Modulus = attribute.Modulus && parentAttribute.Modulus,
            };
        }
    }

    /// <summary>
    /// Gets the default SupportedOperationsAttributeData based on the given NumericType.
    /// </summary>
    /// <param name="underlyingType">The NumericType for which to determine default attribute values.</param>
    /// <returns>The default SupportedOperationsAttributeData with attributes set based on the NumericType.</returns>
    private static SupportedOperationsAttributeData _GetDefaultAttributeData(in PrimitiveUnderlyingType underlyingType)
    {
        var @default = defaultAttributeValue(underlyingType);

        return new SupportedOperationsAttributeData
        {
            Addition = @default,
            Subtraction = @default,
            Multiplication = @default,
            Division = @default,
            Modulus = @default,
        };

        static bool defaultAttributeValue(PrimitiveUnderlyingType underlyingType)
        {
            return underlyingType switch
            {
                PrimitiveUnderlyingType.Byte => false,
                PrimitiveUnderlyingType.SByte => false,
                PrimitiveUnderlyingType.Int16 => false,
                PrimitiveUnderlyingType.UInt16 => false,
                PrimitiveUnderlyingType.Int32 => true,
                PrimitiveUnderlyingType.UInt32 => true,
                PrimitiveUnderlyingType.Int64 => true,
                PrimitiveUnderlyingType.UInt64 => true,
                PrimitiveUnderlyingType.Decimal => true,
                PrimitiveUnderlyingType.Double => true,
                PrimitiveUnderlyingType.Single => true,
                _ => true,
            };
        }
    }

    /// <summary>
    /// Creates a SupportedOperationsAttributeData from the provided AttributeData.
    /// </summary>
    /// <param name="attributeData">The AttributeData from which to create the SupportedOperationsAttributeData.</param>
    /// <returns>The SupportedOperationsAttributeData with attributes based on the provided AttributeData.</returns>
    private static SupportedOperationsAttributeData _GetAttributeFromData(in AttributeData attributeData)
    {
        return new SupportedOperationsAttributeData
        {
            Addition = createAttributeValue(attributeData, nameof(SupportedOperationsAttributeData.Addition)),
            Subtraction = createAttributeValue(attributeData, nameof(SupportedOperationsAttributeData.Subtraction)),
            Multiplication = createAttributeValue(
                attributeData,
                nameof(SupportedOperationsAttributeData.Multiplication)
            ),
            Division = createAttributeValue(attributeData, nameof(SupportedOperationsAttributeData.Division)),
            Modulus = createAttributeValue(attributeData, nameof(SupportedOperationsAttributeData.Modulus)),
        };

        static bool createAttributeValue(AttributeData? parentAttributeData, string property)
        {
            return parentAttributeData!
                .NamedArguments.FirstOrDefault(x => string.Equals(x.Key, property, StringComparison.Ordinal))
                .Value.Value
                is true;
        }
    }

    /// <summary>
    /// Processes the generation of code for a specific data type.
    /// </summary>
    /// <param name="data">The GeneratorData containing information about the data type.</param>
    /// <param name="options">The PrimitiveGlobalOptions for code generation.</param>
    /// <param name="context">The SourceProductionContext for reporting diagnostics.</param>
    /// <returns>True if the code generation process was successful, otherwise, false.</returns>
    private static bool _ProcessType(
        in GeneratorData data,
        in PrimitiveGlobalOptions options,
        in SourceProductionContext context
    )
    {
        var builder = new SourceCodeBuilder();
        var isSuccess = _ProcessConstructor(data, builder, context);

        if (!isSuccess)
        {
            return false;
        }

        context.AddPrimateImplementation(data, builder.ToString(), options);

        return true;
    }

    /// <summary>Processes the constructor for a specified class.</summary>
    /// <param name="data">The GeneratorData containing information about the data type.</param>
    /// <param name="builder">The SourceCodeBuilder for generating source code.</param>
    /// <param name="context">The SourceProductionContext for reporting diagnostics.</param>
    /// <returns>A boolean indicating whether the constructor processing was successful.</returns>
    private static bool _ProcessConstructor(
        in GeneratorData data,
        in SourceCodeBuilder builder,
        in SourceProductionContext context
    )
    {
        var type = data.TypeSymbol;

        if (type.HasDefaultConstructor(out _))
        {
            var emptyCtor = type.Constructors.First(x => x.IsPublic() && x.Parameters.Length == 0);

            context.ReportDiagnostic(
                DiagnosticHelper.ClassHasDefaultConstructor(type.Name, emptyCtor.Locations.FirstOrDefault())
            );

            return false;
        }

        var interfaceGenericType = (INamedTypeSymbol)
            type.Interfaces.First(x => x.IsImplementIPrimitive()).TypeArguments[0];

        var ctorWithParam = type.Constructors.FirstOrDefault(x =>
            x.IsPublic()
            && x.Parameters.Length == 1
            && (x.Parameters[0].Type as INamedTypeSymbol)!.Equals(interfaceGenericType, SymbolEqualityComparer.Default)
        );

        if (ctorWithParam is not null)
        {
            context.ReportDiagnostic(
                DiagnosticHelper.ClassMustNotHaveConstructorWithParam(type.Name, type.Locations.FirstOrDefault())
            );

            return false;
        }

        var underlyingTypeName = interfaceGenericType.GetFriendlyName();

        builder.AppendLine(
            $"private {underlyingTypeName} _valueOrThrow => _isInitialized ? _value : throw new InvalidPrimitiveValueException(\"The domain value has not been initialized\", this);"
        );

        builder.NewLine();

        builder.AppendDebuggerBrowsableNeverAttribute();
        builder.AppendLine($"private readonly {underlyingTypeName} _value;");
        builder.NewLine();

        builder.AppendDebuggerBrowsableNeverAttribute();
        builder.AppendLine("private readonly bool _isInitialized;");
        builder.NewLine();

        builder.AppendSummary(
            $"Initializes a new instance of the <see cref=\"{type.Name}\"/> class by validating the specified <see cref=\"{underlyingTypeName}\"/> value using <see cref=\"Validate\"/> static method."
        );

        builder.AppendParamDescription("value", "The value to be validated.");

        builder.Append($"public {type.Name}({underlyingTypeName} value) : this(value, true) {{ }}").NewLine(2);

        builder.AppendLine($"private {type.Name}({underlyingTypeName} value, bool validate)").OpenBracket();
        builder.AppendLine("if (validate)").OpenBracket();

        if (data.UnderlyingType == PrimitiveUnderlyingType.String)
        {
            _AddStringLengthAttributeValidation(type, data, builder);
        }

        builder.AppendLine("ValidateOrThrow(value);");
        builder.CloseBracket().AppendLine("_value = value;").AppendLine("_isInitialized = true;").CloseBracket();

        builder.NewLine();

        var primitiveTypeIsValueType = data.PrimitiveTypeSymbol.IsValueType;

        if (!primitiveTypeIsValueType)
        {
            builder.AppendNullableDisable();
        }

        builder.AppendLine("#pragma warning disable AL1003 // Should not have non obsolete empty constructors.");

        builder
            .AppendLine("[Obsolete(\"Primitive cannot be created using empty Constructor\", true)]")
            .Append("public ")
            .Append(type.Name)
            .AppendLine("() { }");

        builder.AppendLine("#pragma warning restore AL1003");

        if (!primitiveTypeIsValueType)
        {
            builder.AppendNullableEnable();
        }

        return true;
    }

    private static void _AddStringLengthAttributeValidation(
        in ISymbol domainPrimitiveType,
        in GeneratorData data,
        in SourceCodeBuilder sb
    )
    {
        var attr = domainPrimitiveType
            .GetAttributes()
            .FirstOrDefault(x =>
                string.Equals(
                    x.AttributeClass?.ToDisplayString(),
                    AbstractionConstants.StringLengthAttributeFullName,
                    StringComparison.Ordinal
                )
            );

        if (attr is null)
        {
            return;
        }

        var minValue = (int)attr.ConstructorArguments[0].Value!;
        var maxValue = (int)attr.ConstructorArguments[1].Value!;
        var validate = (bool)attr.ConstructorArguments[2].Value!;

        if (!validate)
        {
            return;
        }

        var hasMinValue = minValue >= 0;
        var hasMaxValue = maxValue != int.MaxValue;

        if (!hasMinValue && !hasMaxValue)
        {
            return;
        }

        data.StringLengthAttributeValidation = (minValue, maxValue);

        var minValueText = minValue.ToString(CultureInfo.InvariantCulture);
        var maxValueText = maxValue.ToString(CultureInfo.InvariantCulture);

        sb.Append("if (value.Length is ")
            .AppendIf(hasMinValue, $"< {minValueText}")
            .AppendIf(hasMinValue && hasMaxValue, " or ")
            .AppendIf(hasMaxValue, $"> {maxValueText}")
            .AppendLine(")")
            .AppendLine(
                $"\tthrow new InvalidPrimitiveValueException(\"String length is out of range {minValueText}..{maxValueText}\", this);"
            )
            .NewLine();
    }
}
