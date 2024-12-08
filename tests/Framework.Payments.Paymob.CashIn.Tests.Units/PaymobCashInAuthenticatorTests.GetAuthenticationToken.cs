// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.

using Framework.Payments.Paymob.CashIn;
using Framework.Payments.Paymob.CashIn.Models.Auth;
using Humanizer;
using Microsoft.Extensions.Time.Testing;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Tests;

public partial class PaymobCashInAuthenticatorTests
{
    [Fact]
    public async Task should_request_new_token_when_cache_token_expired()
    {
        // given
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        _SetupRandomResponse();

        // when
        var authenticator = new PaymobCashInAuthenticator(fixture.HttpClient, timeProvider, fixture.OptionsAccessor);
        var result1 = await authenticator.GetAuthenticationTokenAsync();
        timeProvider.Advance(61.Minutes());
        var result2 = await authenticator.GetAuthenticationTokenAsync();

        // then
        result1.Should().NotBe(result2);
    }

    [Fact]
    public async Task should_cache_token_fo_an_hour_when_success()
    {
        // given
        var timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
        _SetupRandomResponse();

        // when
        var authenticator = new PaymobCashInAuthenticator(fixture.HttpClient, timeProvider, fixture.OptionsAccessor);
        var result1 = await authenticator.GetAuthenticationTokenAsync();
        timeProvider.Advance(50.Minutes());
        var result2 = await authenticator.GetAuthenticationTokenAsync();

        // then
        result1.Should().Be(result2);
    }

    private void _SetupRandomResponse()
    {
        var apiKey = fixture.AutoFixture.Create<string>();
        var config = fixture.CashInOptions with { ApiKey = apiKey };
        fixture.OptionsAccessor.CurrentValue.Returns(config);
        var request = new CashInAuthenticationTokenRequest { ApiKey = apiKey };
        var requestJson = JsonSerializer.Serialize<CashInAuthenticationTokenRequest>(request);

        fixture
            .Server.Given(Request.Create().WithPath("/auth/tokens").UsingPost().WithBody(requestJson))
            .RespondWith(Response.Create().WithBody(_ => _GetTokenResponseJson()));
    }

    private string _GetTokenResponseJson()
    {
        // var fixture = new Fixture();
        var response = fixture.AutoFixture.Create<CashInAuthenticationTokenResponse>();
        return JsonSerializer.Serialize(response);
    }
}
