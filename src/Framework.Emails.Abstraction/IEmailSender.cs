// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Emails;

/// <summary>Email sender abstraction.</summary>
public interface IEmailSender
{
    ValueTask<SendSingleEmailResponse> SendAsync(
        SendSingleEmailRequest request,
        CancellationToken cancellationToken = default
    );
}
