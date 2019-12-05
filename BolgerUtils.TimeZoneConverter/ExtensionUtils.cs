using System;

namespace BolgerUtils.TimeZoneConverter
{
    public static class ExtensionUtils
    {
        public static DateTime ConvertTimeFromDefaultTimeZoneToUtc(this DateTime dateTime) =>
            Utils.ConvertTimeFromDefaultTimeZoneToUtc(dateTime);

        public static DateTime ConvertTimeFromTimeZoneToUtc(
            this DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID) =>
            Utils.ConvertTimeFromTimeZoneToUtc(dateTime, sourceTimeZoneID);

        public static DateTime ConvertTimeFromUtcToDefaultTimeZone(this DateTime dateTimeUtc) =>
            Utils.ConvertTimeFromUtcToDefaultTimeZone(dateTimeUtc);

        public static DateTime ConvertTimeFromUtcToTimeZone(
            this DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID) =>
            Utils.ConvertTimeFromUtcToTimeZone(dateTimeUtc, targetTimeZoneID);
    }
}
