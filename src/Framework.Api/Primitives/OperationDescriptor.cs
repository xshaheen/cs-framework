// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Framework.Primitives;

[PublicAPI]
public sealed record OperationDescriptor(string Code, string Href, string Method, string IdempotentKey);
