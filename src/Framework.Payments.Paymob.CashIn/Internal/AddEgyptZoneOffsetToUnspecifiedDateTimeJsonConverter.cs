// Copyright (c) Mahmoud Shaheen. All rights reserved.

using TimeZoneConverter;

namespace Framework.Payments.Paymob.CashIn.Internal;

internal sealed class AddEgyptZoneOffsetToUnspecifiedDateTimeJsonConverter : JsonConverter<DateTimeOffset>
{
    public static readonly TimeZoneInfo EgyptTimeZone = TZConvert.GetTimeZoneInfo("Egypt Standard Time");

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTime = reader.GetDateTime();

        // If not have time zone offset, consider it cairo time.
        return dateTime.Kind is DateTimeKind.Unspecified
            ? new DateTimeOffset(dateTime, EgyptTimeZone.GetUtcOffset(dateTime))
            : reader.GetDateTimeOffset();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToOffset(TimeSpan.Zero));
    }
}
