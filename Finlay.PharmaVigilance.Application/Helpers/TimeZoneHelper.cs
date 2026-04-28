namespace Finlay.PharmaVigilance.Application.Helpers;

/// <summary>
/// Helper class for managing timezones consistently across the application.
/// All date validations should use UTC-5 (Eastern Time) as the reference timezone.
/// </summary>
public static class TimeZoneHelper
{
    /// <summary>
    /// Eastern Time Zone (UTC-5 / EST, UTC-4 / EDT during daylight saving)
    /// </summary>
    private static readonly TimeZoneInfo EasternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    /// <summary>
    /// Gets the current date and time in Eastern Time Zone (UTC-5).
    /// </summary>
    /// <returns>Current DateTime in Eastern Time</returns>
    public static DateTime GetEasternNow()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, EasternTimeZone);
    }

    /// <summary>
    /// Gets the current date (without time component) in Eastern Time Zone (UTC-5).
    /// </summary>
    /// <returns>Current date in Eastern Time (time set to 00:00:00)</returns>
    public static DateTime GetEasternNowDate()
    {
        return GetEasternNow().Date;
    }

    /// <summary>
    /// Converts a UTC DateTime to Eastern Time Zone (UTC-5).
    /// </summary>
    /// <param name="utcDateTime">DateTime in UTC</param>
    /// <returns>DateTime converted to Eastern Time</returns>
    public static DateTime ConvertFromUtcToEastern(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTime(utcDateTime, TimeZoneInfo.Utc, EasternTimeZone);
    }

    /// <summary>
    /// Converts an Eastern Time DateTime to UTC.
    /// </summary>
    /// <param name="easternDateTime">DateTime in Eastern Time</param>
    /// <returns>DateTime converted to UTC</returns>
    public static DateTime ConvertFromEasternToUtc(DateTime easternDateTime)
    {
        // Assume the incoming datetime is in Eastern Time (no kind specified)
        var easternTime = new DateTime(easternDateTime.Year, easternDateTime.Month, easternDateTime.Day,
            easternDateTime.Hour, easternDateTime.Minute, easternDateTime.Second, DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc(easternTime, EasternTimeZone);
    }
}
