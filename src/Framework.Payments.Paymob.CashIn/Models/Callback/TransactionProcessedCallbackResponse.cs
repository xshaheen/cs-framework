// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Payments.Paymob.CashIn.Internal;

namespace Framework.Payments.Paymob.CashIn.Models.Callback;

[PublicAPI]
public sealed class TransactionProcessedCallbackResponse
{
    [JsonPropertyName("response_received_at")]
    [JsonConverter(typeof(AddEgyptZoneOffsetToUnspecifiedDateTimeJsonConverter))]
    public DateTimeOffset ResponseReceivedAt { get; init; }

    [JsonPropertyName("callback_url")]
    public required string CallbackUrl { get; init; }

    [JsonPropertyName("response")]
    public required TransactionProcessedCallbackResponseObj Response { get; init; }
}
