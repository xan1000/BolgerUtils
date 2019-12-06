using System;
using BolgerUtils.TimeZoneConverter;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests.BolgerUtils.TimeZoneConverter
{
    public class UtilsTests
    {
        // Note .NET automatically includes daylight savings in AEST (Australian Eastern Standard Time).
        private const string AustralianEasternStandardTime = "AUS Eastern Standard Time";
        internal const SystemTimeZoneInfoID AestTimeZoneID = SystemTimeZoneInfoID.AusEasternStandardTime;
        internal static readonly TimeZoneInfo AestTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(AustralianEasternStandardTime);

        internal static readonly DateTime DateTimeAest = new DateTime(2019, 12, 6, 9, 30, 0);
        internal static readonly DateTime DateTimeUtc = new DateTime(2019, 12, 5, 22, 30, 0);

        [Fact]
        public void Test_ConvertTimeFromDefaultTimeZoneToUtc()
        {
            Assert.Throws<InvalidOperationException>(() => Utils.ConvertTimeFromDefaultTimeZoneToUtc(DateTimeAest));
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromDefaultTimeZoneToUtc(DateTimeAest));
        }

        [Fact]
        public void Test_ConvertTimeFromTimeZoneToUtc()
        {
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AestTimeZone));
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AustralianEasternStandardTime));
            Assert.Equal(DateTimeUtc, Utils.ConvertTimeFromTimeZoneToUtc(DateTimeAest, AestTimeZoneID));
        }

        [Fact]
        public void Test_ConvertTimeFromUtcToDefaultTimeZone()
        {
            Assert.Throws<InvalidOperationException>(() => Utils.ConvertTimeFromUtcToDefaultTimeZone(DateTimeUtc));
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToDefaultTimeZone(DateTimeUtc));
        }

        [Fact]
        public void Test_ConvertTimeFromUtcToTimeZone()
        {
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AestTimeZone));
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AustralianEasternStandardTime));
            Assert.Equal(DateTimeAest, Utils.ConvertTimeFromUtcToTimeZone(DateTimeUtc, AestTimeZoneID));
        }

        [Fact]
        public void Test_GetTimeNowInTimeZone()
        {
            var dateTimeAest = Utils.ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, AestTimeZone);
            var precision = TimeSpan.FromSeconds(1);

            Assert.Equal(dateTimeAest, Utils.GetTimeNowInTimeZone(AestTimeZone), precision);
            Assert.Equal(dateTimeAest, Utils.GetTimeNowInTimeZone(AustralianEasternStandardTime), precision);
            Assert.Equal(dateTimeAest, Utils.GetTimeNowInTimeZone(AestTimeZoneID), precision);
        }

        [Fact]
        public void Test_GetTimeTodayInTimeZone()
        {
            var dateTimeAest = Utils.ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, AestTimeZone).Date;

            Assert.Equal(dateTimeAest, Utils.GetTimeTodayInTimeZone(AestTimeZone));
            Assert.Equal(dateTimeAest, Utils.GetTimeTodayInTimeZone(AustralianEasternStandardTime));
            Assert.Equal(dateTimeAest, Utils.GetTimeTodayInTimeZone(AestTimeZoneID));
        }

        [Fact]
        public void Test_GetTimeZone()
        {
            var timeZone = Utils.GetTimeZone(AustralianEasternStandardTime);

            Assert.IsType<TimeZoneInfo>(timeZone);
            Assert.Equal("AUS Eastern Standard Time", timeZone.StandardName);
            Assert.Equal(TimeSpan.FromHours(10), timeZone.BaseUtcOffset);
            Assert.True(timeZone.SupportsDaylightSavingTime);
            Assert.Equal(timeZone, Utils.GetTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime));
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

        //ParseExactTimeFromDefaultTimeZoneToUtc
        //ParseExactTimeFromTimeZoneToUtc

        [Fact]
        public void Test_SetDefaultTimeZoneByTimeZone() =>
            Test_SetDefaultTimeZone_Implementation(() => Utils.SetDefaultTimeZone(AestTimeZone));

        [Fact]
        public void Test_SetDefaultTimeZoneByTimeZoneID() => Test_SetDefaultTimeZone_Implementation(() =>
            Utils.SetDefaultTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime));

        [Fact]
        public void Test_SetDefaultTimeZoneByTimeZoneString() =>
            Test_SetDefaultTimeZone_Implementation(() => Utils.SetDefaultTimeZone(AustralianEasternStandardTime));

        private void Test_SetDefaultTimeZone_Implementation(Action setDefaultTimeZone)
        {
            Assert.Throws<InvalidOperationException>(() => Utils.DefaultTimeZone);
            Utils.SetDefaultTimeZone((TimeZoneInfo) null);
            Assert.Throws<InvalidOperationException>(() => Utils.DefaultTimeZone);

            setDefaultTimeZone();

            Assert.NotNull(Utils.DefaultTimeZone);
            Assert.Equal(AestTimeZone, Utils.DefaultTimeZone);
            Assert.Throws<InvalidOperationException>(() => Utils.SetDefaultTimeZone(AestTimeZoneID));
            Assert.Throws<InvalidOperationException>(() =>
                Utils.SetDefaultTimeZone(SystemTimeZoneInfoID.EasternStandardTime));
        }

        //TryParseExactTimeFromTimeZoneToUtc

        [Fact]
        public void Test_SystemTimeZoneInfoID()
        {
            var dateTimeUtc = DateTime.UtcNow;
            foreach(var timeZoneID in global::BolgerUtils.Utils.GetEnumValues<SystemTimeZoneInfoID>())
            {
                Assert.IsType<DateTime>(Utils.ConvertTimeFromUtcToTimeZone(dateTimeUtc, timeZoneID));
            }
        }

        [Fact]
        public void Test_TimeNowInDefaultTimeZone()
        {
            Assert.Throws<InvalidOperationException>(() => Utils.TimeNowInDefaultTimeZone);
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.Equal(Utils.ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, AestTimeZone),
                Utils.TimeNowInDefaultTimeZone, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Test_TimeTodayInDefaultTimeZone()
        {
            Assert.Throws<InvalidOperationException>(() => Utils.TimeTodayInDefaultTimeZone);
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.Equal(Utils.ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, AestTimeZone).Date,
                Utils.TimeTodayInDefaultTimeZone);
        }
    }
}
