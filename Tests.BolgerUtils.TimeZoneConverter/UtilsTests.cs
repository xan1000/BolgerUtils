using System;
using BolgerUtils.TimeZoneConverter;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests.BolgerUtils.TimeZoneConverter
{
    public class UtilsTests
    {
        // Note .NET treats AEST (Australian Eastern Standard Time) as AEDT (Australian Eastern Daylight Time).
        private const string AustralianEasternStandardTime = "AUS Eastern Standard Time";
        internal const SystemTimeZoneInfoID AestTimeZoneID = SystemTimeZoneInfoID.AusEasternStandardTime;
        private static readonly TimeZoneInfo AestTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(AustralianEasternStandardTime);

        internal static readonly DateTime DateTimeAest = new DateTime(2019, 12, 6, 9, 30, 0);
        internal static readonly DateTime DateTimeUtc = new DateTime(2019, 12, 5, 22, 30, 0);

        //public static TimeZoneInfo DefaultTimeZone
        //public static DateTime TimeNowInDefaultTimeZone
        //public static DateTime TimeTodayInDefaultTimeZone
        //public void Test_ConvertTimeFromDefaultTimeZoneToUtc()

        [Fact]
        public void Test_ConvertTimeFromTimeZoneToUtc()
        {
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AestTimeZone));
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AustralianEasternStandardTime));
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AestTimeZoneID));
        }

        //ConvertTimeFromUtcToDefaultTimeZone
        //GetTimeNowInTimeZone
        //GetTimeTodayInTimeZone
        //ParseExactTimeFromDefaultTimeZoneToUtc
        //ParseExactTimeFromTimeZoneToUtc
        //SetDefaultTimeZone
        //TryParseExactTimeFromTimeZoneToUtc

        [Fact]
        public void Test_ConvertTimeFromUtcToTimeZone()
        {
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AestTimeZone));
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AustralianEasternStandardTime));
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AestTimeZoneID));
        }

        [Fact]
        public void Test_MultipleTimeZones()
        {
            var dateTimeNewYork = new DateTime(2019, 12, 5, 17, 30, 0);

            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AestTimeZoneID));
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(dateTimeNewYork, "Eastern Standard Time"));
            Assert.Equal(DateTimeUtc,
                Utils.ConvertTimeFromTimeZoneToUtc(dateTimeNewYork, SystemTimeZoneInfoID.EasternStandardTime));
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(
                Utils.ConvertTimeFromTimeZoneToUtc(dateTimeNewYork, SystemTimeZoneInfoID.EasternStandardTime),
                AestTimeZoneID));
        }

        [Fact]
        public void Test_SystemTimeZoneInfoID()
        {
            var dateTimeUtc = DateTime.UtcNow;
            foreach(var timeZoneID in global::BolgerUtils.Utils.GetEnumValues<SystemTimeZoneInfoID>())
            {
                Assert.IsType<DateTime>(Utils.ConvertTimeFromUtcToTimeZone(dateTimeUtc, timeZoneID));
            }
        }
    }
}
