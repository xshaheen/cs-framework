// Copyright (c) Mahmoud Shaheen, 2021. All rights reserved.

using System.Text.Json.Serialization;
using Framework.Payments.Paymob.CashIn.Internal;

namespace Tests.Internal;

public sealed class AddEgyptZoneOffsetToUnspecifiedDateTimeJsonConverterTests
{
    public static readonly TheoryData<DateTimeOffset, string> WriteTestsData =
        new()
        {
            { new DateTimeOffset(2021, 8, 8, 6, 26, 21, 457, TimeSpan.Zero), "\"2021-08-08T06:26:21.457+00:00\"" },
            {
                new DateTimeOffset(2021, 8, 8, 6, 26, 21, 457, TimeSpan.FromHours(2)),
                "\"2021-08-08T04:26:21.457+00:00\""
            },
        };

    private static readonly JsonSerializerOptions _Options =
        new() { Converters = { new AddEgyptZoneOffsetToUnspecifiedDateTimeJsonConverter() } };

    [Theory]
    // without offset => consider it egypt time
    [InlineData("2021-08-08T06:26:21.4526814", 6, 2)]
    // with offset => Keep it as its
    [InlineData("2021-08-08T06:26:21.4526814+02:00", 6, 2)]
    [InlineData("2021-08-08T06:26:21.4526814Z", 6, 0)]
    [InlineData("2021-08-08T06:26:21.4526814+00:00", 6, 0)]
    [InlineData("2021-08-08T06:26:21.4526814+01:00", 6, 1)]
    public void read_tests(string timestamp, int hour, int offset)
    {
        var json = $$"""{ "Timestamp": "{{timestamp}}" }""";
        var result = JsonSerializer.Deserialize<Target>(json);
        result!.Timestamp.DateTime.Hour.Should().Be(hour);
        result.Timestamp.Offset.Should().Be(TimeSpan.FromHours(offset));
    }

    [Theory]
    [MemberData(nameof(WriteTestsData))]
    public void write_tests(DateTimeOffset dateTimeOffset, string expected)
    {
        var json = JsonSerializer.Serialize(dateTimeOffset, _Options);
        json.Should().Be(expected);
    }

    private sealed class Target
    {
        [JsonConverter(typeof(AddEgyptZoneOffsetToUnspecifiedDateTimeJsonConverter))]
        public DateTimeOffset Timestamp { get; init; }
    }
}
