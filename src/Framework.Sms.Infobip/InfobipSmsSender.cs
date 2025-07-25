// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Sms.Infobip;

public sealed class InfobipSmsSender : ISmsSender, IDisposable
{
    private readonly string _sender;
    private readonly SmsApi _smsApi;
    private readonly ILogger<InfobipSmsSender> _logger;

    public InfobipSmsSender(
        HttpClient httpClient,
        IOptions<InfobipOptions> optionsAccessor,
        ILogger<InfobipSmsSender> logger
    )
    {
        var value = optionsAccessor.Value;
        _sender = value.Sender;
        _smsApi = new SmsApi(httpClient, new Configuration { BasePath = value.BasePath, ApiKey = value.ApiKey });
        _logger = logger;
    }

    public async ValueTask<SendSingleSmsResponse> SendAsync(
        SendSingleSmsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var destinations = request.IsBatch
            ? request
                .Destinations.Select(
                    (item, index) =>
                    {
                        var messageId = request.MessageId is null
                            ? null
                            : request.MessageId + (index + 1).ToString(CultureInfo.InvariantCulture);

                        return new SmsDestination(to: item.ToString(hasPlusPrefix: false), messageId: messageId);
                    }
                )
                .ToList()
            :
            [
                new SmsDestination(
                    to: request.Destinations[0].ToString(hasPlusPrefix: false),
                    messageId: request.MessageId
                ),
            ];

        var smsMessage = new SmsMessage(_sender, destinations, new SmsMessageContent(new SmsTextContent(request.Text)));
        var smsRequest = new SmsRequest([smsMessage]);

        try
        {
            var smsResponse = await _smsApi.SendSmsMessagesAsync(smsRequest, cancellationToken);
            _logger.LogTrace("Infobip SMS request {@Request} success {@Response}", smsRequest, smsResponse);

            return SendSingleSmsResponse.Succeeded();
        }
        catch (ApiException e)
        {
            _logger.LogError(e, "Infobip SMS request {@Request} failed {@Error}", smsRequest, e.ErrorContent);
            FormattableString error = $"ErrorCode: {e.ErrorCode} {e.ErrorContent ?? e.Message}";

            return SendSingleSmsResponse.Failed(error.ToInvariantString());
        }
    }

    public void Dispose()
    {
        _smsApi.Dispose();
    }
}
