// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using Framework.Checks;

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace
namespace Framework.PushNotifications;

public sealed record PushNotificationResponse
{
    private PushNotificationResponse() { }

    public string Token { get; private init; } = null!;

    public string? MessageId { get; private init; }

    public string? FailureError { get; private init; }

    public PushNotificationResponseStatus Status { get; private init; }

    [MemberNotNullWhen(true, nameof(MessageId))]
    public bool IsSucceeded()
    {
        return Status is PushNotificationResponseStatus.Success;
    }

    [MemberNotNullWhen(true, nameof(FailureError))]
    public bool IsFailed()
    {
        return Status is PushNotificationResponseStatus.Failure;
    }

    public static PushNotificationResponse Succeeded(string token, string messageId)
    {
        return new PushNotificationResponse
        {
            Status = PushNotificationResponseStatus.Success,
            Token = Argument.IsNotNullOrWhiteSpace(token),
            MessageId = Argument.IsNotNullOrWhiteSpace(messageId),
        };
    }

    public static PushNotificationResponse Failed(string token, string failureError)
    {
        return new PushNotificationResponse
        {
            Status = PushNotificationResponseStatus.Failure,
            Token = Argument.IsNotNullOrWhiteSpace(token),
            FailureError = Argument.IsNotNullOrWhiteSpace(failureError),
            MessageId = null,
        };
    }

    public static PushNotificationResponse Unregistered(string token)
    {
        return new PushNotificationResponse
        {
            Status = PushNotificationResponseStatus.Unregister,
            Token = Argument.IsNotNullOrWhiteSpace(token),
            MessageId = null,
        };
    }
}

public enum PushNotificationResponseStatus
{
    Unregister,
    Success,
    Failure,
}
