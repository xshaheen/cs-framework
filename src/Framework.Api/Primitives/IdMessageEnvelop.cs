// Copyright (c) Mahmoud Shaheen. All rights reserved.

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Framework.Primitives;

public sealed record IdMessageEnvelop(string Id, MessageDescriptor Message) : IIdEnvelop, IMessageEnvelop
{
    public IdMessageEnvelop(Guid id, MessageDescriptor message)
        : this(id.ToString(), message) { }

    public IdMessageEnvelop(long id, MessageDescriptor message)
        : this(id.ToString(CultureInfo.InvariantCulture), message) { }

    public IdMessageEnvelop(int id, MessageDescriptor message)
        : this(id.ToString(CultureInfo.InvariantCulture), message) { }
}
