// Copyright (c) Mahmoud Shaheen. All rights reserved.

using System.Net.Http.Json;
using Flurl;
using Framework.Payments.Paymob.CashIn.Models;
using Framework.Payments.Paymob.CashIn.Models.Payment;

namespace Framework.Payments.Paymob.CashIn;

public partial class PaymobCashInBroker
{
    /// <summary>
    /// Get a payment key which is used to authenticate payment request and verifying transaction
    /// request metadata.
    /// </summary>
    public async Task<CashInPaymentKeyResponse> RequestPaymentKeyAsync(CashInPaymentKeyRequest request)
    {
        var authToken = await authenticator.GetAuthenticationTokenAsync();
        var requestUrl = Url.Combine(_options.ApiBaseUrl, "acceptance/payment_keys");
        var internalRequest = new CashInPaymentKeyInternalRequest(request, authToken, _options.ExpirationPeriod);

        using var response = await httpClient.PostAsJsonAsync(
            requestUrl,
            internalRequest,
            _options.SerializationOptions
        );

        if (!response.IsSuccessStatusCode)
        {
            await PaymobCashInException.ThrowAsync(response);
        }

        return (await response.Content.ReadFromJsonAsync<CashInPaymentKeyResponse>(_options.DeserializationOptions))!;
    }
}
