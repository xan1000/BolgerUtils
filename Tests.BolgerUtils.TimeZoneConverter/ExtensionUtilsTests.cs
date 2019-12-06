using System;
using BolgerUtils.TimeZoneConverter;
using Xunit;

namespace Tests.BolgerUtils.TimeZoneConverter
{
    public class ExtensionUtilsTests
    {
        public ExtensionUtilsTests() => UtilsTests.ResetDefaultTimeZone();

        [Fact]
        public void Test_ConvertTimeFromDefaultTimeZoneToUtc()
        {
            Assert.Throws<InvalidOperationException>(() =>
                UtilsTests.DateTimeAest.ConvertTimeFromDefaultTimeZoneToUtc());
            Utils.SetDefaultTimeZone(UtilsTests.AestTimeZone);
            Assert.Equal(UtilsTests.DateTimeUtc, UtilsTests.DateTimeAest.ConvertTimeFromDefaultTimeZoneToUtc());
        }

        [Fact]
        public void Test_ConvertTimeFromTimeZoneToUtc() => Assert.Equal(UtilsTests.DateTimeUtc,
            UtilsTests.DateTimeAest.ConvertTimeFromTimeZoneToUtc(UtilsTests.AestTimeZoneID));

        [Fact]
        public void Test_ConvertTimeFromUtcToDefaultTimeZone()
        {
            Assert.Throws<InvalidOperationException>(() =>
                UtilsTests.DateTimeUtc.ConvertTimeFromUtcToDefaultTimeZone());
            Utils.SetDefaultTimeZone(UtilsTests.AestTimeZone);
            Assert.Equal(UtilsTests.DateTimeAest, UtilsTests.DateTimeUtc.ConvertTimeFromUtcToDefaultTimeZone());
        }

        [Fact]
        public void Test_ConvertTimeFromUtcToTimeZone() => Assert.Equal(UtilsTests.DateTimeAest,
            UtilsTests.DateTimeUtc.ConvertTimeFromUtcToTimeZone(UtilsTests.AestTimeZoneID));

        [Fact]
        public void Test_MultipleTimeZones()
        {
            var dateTimeNewYork = new DateTime(2019, 12, 5, 17, 30, 0);

            Assert.Equal(UtilsTests.DateTimeUtc,
                dateTimeNewYork.ConvertTimeFromTimeZoneToUtc(SystemTimeZoneInfoID.EasternStandardTime));
            Assert.Equal(UtilsTests.DateTimeAest, dateTimeNewYork.ConvertTimeFromTimeZoneToUtc(
                SystemTimeZoneInfoID.EasternStandardTime).ConvertTimeFromUtcToTimeZone(UtilsTests.AestTimeZoneID));
        }
    }
}
