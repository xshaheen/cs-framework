// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Payments.Paymob.CashOut.Models;

[PublicAPI]
public sealed class CashOutGetTransactionsResponse
{
    private IReadOnlyList<CashOutTransaction>? _results;

    [JsonPropertyName("count")]
    public int Count { get; init; }

    [JsonPropertyName("next")]
    public string? Next { get; init; }

    [JsonPropertyName("previous")]
    public string? Previous { get; init; }

    [JsonPropertyName("results")]
    public IReadOnlyList<CashOutTransaction> Results
    {
        get => _results ?? [];
        init => _results = value;
    }
}
