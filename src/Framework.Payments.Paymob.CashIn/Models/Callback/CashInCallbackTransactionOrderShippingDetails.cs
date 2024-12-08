// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Text.Json.Serialization;

namespace Framework.Payments.Paymob.CashIn.Models.Callback;

[PublicAPI]
public sealed class CashInCallbackTransactionOrderShippingDetails
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("cash_on_delivery_amount")]
    public int CashOnDeliveryAmount { get; init; }

    [JsonPropertyName("cash_on_delivery_type")]
    public string? CashOnDeliveryType { get; init; }

    [JsonPropertyName("is_same_day")]
    public int IsSameDay { get; init; }

    [JsonPropertyName("number_of_packages")]
    public int NumberOfPackages { get; init; }

    [JsonPropertyName("weight")]
    public int Weight { get; init; }

    [JsonPropertyName("weight_unit")]
    public required string WeightUnit { get; init; }

    [JsonPropertyName("length")]
    public int Length { get; init; }

    [JsonPropertyName("width")]
    public int Width { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }

    [JsonPropertyName("delivery_type")]
    public string? DeliveryType { get; init; }

    [JsonPropertyName("order_id")]
    public int OrderId { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }

    [JsonPropertyName("notes")]
    public string? Notes { get; init; }

    [JsonPropertyName("latitude")]
    public object? Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public object? Longitude { get; init; }

    [JsonPropertyName("return_type")]
    public object? ReturnType { get; init; }

    [JsonExtensionData]
    public IDictionary<string, object?>? ExtensionData { get; init; }
}
