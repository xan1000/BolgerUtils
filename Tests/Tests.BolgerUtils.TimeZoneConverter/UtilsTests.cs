using System;
using System.Reflection;
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

        // Hack to reset the remembered value of a purposely static variable to a clean state for each test.
        internal static void ResetDefaultTimeZone() => typeof(Utils).
            GetField("_defaultTimeZone", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, null);

        public UtilsTests() => ResetDefaultTimeZone();

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

        [Fact]
        public void Test_ParseExactTimeFromDefaultTimeZoneToUtc()
        {
            const string format = "dd-MM-yyyy HH:mm:ss";
            var value = DateTimeAest.ToString(format);

            Assert.Throws<InvalidOperationException>(() =>
                Utils.ParseExactTimeFromDefaultTimeZoneToUtc(value, format));
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.Equal(DateTimeUtc, Utils.ParseExactTimeFromDefaultTimeZoneToUtc(value, format));
            Assert.NotEqual(DateTimeAest, Utils.ParseExactTimeFromDefaultTimeZoneToUtc(value, format));
            Assert.Throws<FormatException>(() => Utils.ParseExactTimeFromDefaultTimeZoneToUtc("test", format));
        }

        [Fact]
        public void Test_ParseExactTimeFromTimeZoneToUtc()
        {
            const string format = "dd-MM-yyyy HH:mm:ss";
            var value = DateTimeAest.ToString(format);

            Assert.Equal(DateTimeUtc, Utils.ParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZone));
            Assert.Equal(DateTimeUtc,
                Utils.ParseExactTimeFromTimeZoneToUtc(value, format, AustralianEasternStandardTime));
            Assert.Equal(DateTimeUtc, Utils.ParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZoneID));
            Assert.NotEqual(DateTimeAest, Utils.ParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZone));
            Assert.Throws<FormatException>(() => Utils.ParseExactTimeFromTimeZoneToUtc("test", format, AestTimeZone));
        }

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

        [Fact]
        public void Test_TryParseExactTimeFromDefaultTimeZoneToUtc()
        {
            const string format = "dd-MM-yyyy HH:mm:ss";
            var value = DateTimeAest.ToString(format);

            Assert.Throws<InvalidOperationException>(() =>
                Utils.TryParseExactTimeFromDefaultTimeZoneToUtc(value, format, out _));
            Utils.SetDefaultTimeZone(AestTimeZone);
            Assert.True(Utils.TryParseExactTimeFromDefaultTimeZoneToUtc(value, format, out var resultUtc));
            Assert.Equal(DateTimeUtc, resultUtc);
            Assert.True(Utils.TryParseExactTimeFromDefaultTimeZoneToUtc(value, format, out resultUtc));
            Assert.NotEqual(DateTimeAest, resultUtc);
            Assert.False(Utils.TryParseExactTimeFromDefaultTimeZoneToUtc("test", format, out _));
        }

        [Fact]
        public void Test_TryParseExactTimeFromTimeZoneToUtc()
        {
            const string format = "dd-MM-yyyy HH:mm:ss";
            var value = DateTimeAest.ToString(format);

            Assert.True(Utils.TryParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZone, out var resultUtc));
            Assert.Equal(DateTimeUtc, resultUtc);
            Assert.True(
                Utils.TryParseExactTimeFromTimeZoneToUtc(value, format, AustralianEasternStandardTime, out resultUtc));
            Assert.Equal(DateTimeUtc, resultUtc);
            Assert.True(Utils.TryParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZoneID, out resultUtc));
            Assert.Equal(DateTimeUtc, resultUtc);
            Assert.True(Utils.TryParseExactTimeFromTimeZoneToUtc(value, format, AestTimeZone, out resultUtc));
            Assert.NotEqual(DateTimeAest, resultUtc);
            Assert.False(Utils.TryParseExactTimeFromTimeZoneToUtc("test", format, AestTimeZone, out _));
        }
    }
}
