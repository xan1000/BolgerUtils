using System;
using System.Collections.Generic;
using System.Globalization;
using TimeZoneConverter;

namespace BolgerUtils.TimeZoneConverter
{
    public enum SystemTimeZoneInfoID
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable IdentifierTypo
        DatelineStandardTime,
        UTC11,
        AleutianStandardTime,
        HawaiianStandardTime,
        MarquesasStandardTime,
        AlaskanStandardTime,
        UTC09,
        PacificStandardTimeMexico,
        UTC08,
        PacificStandardTime,
        UsMountainStandardTime,
        MountainStandardTimeMexico,
        MountainStandardTime,
        CentralAmericaStandardTime,
        CentralStandardTime,
        EasterIslandStandardTime,
        CentralStandardTimeMexico,
        CanadaCentralStandardTime,
        SaPacificStandardTime,
        EasternStandardTimeMexico,
        EasternStandardTime,
        HaitiStandardTime,
        CubaStandardTime,
        UsEasternStandardTime,
        TurksAndCaicosStandardTime,
        ParaguayStandardTime,
        AtlanticStandardTime,
        VenezuelaStandardTime,
        CentralBrazilianStandardTime,
        SaWesternStandardTime,
        PacificSaStandardTime,
        NewfoundlandStandardTime,
        TocantinsStandardTime,
        ESouthAmericaStandardTime,
        SaEasternStandardTime,
        ArgentinaStandardTime,
        GreenlandStandardTime,
        MontevideoStandardTime,
        MagallanesStandardTime,
        SaintPierreStandardTime,
        BahiaStandardTime,
        UTC02,
        MidAtlanticStandardTime,
        AzoresStandardTime,
        CapeVerdeStandardTime,
        UTC,
        GmtStandardTime,
        GreenwichStandardTime,
        SaoTomeStandardTime,
        MoroccoStandardTime,
        WEuropeStandardTime,
        CentralEuropeStandardTime,
        RomanceStandardTime,
        CentralEuropeanStandardTime,
        WCentralAfricaStandardTime,
        JordanStandardTime,
        GtbStandardTime,
        MiddleEastStandardTime,
        EgyptStandardTime,
        EEuropeStandardTime,
        SyriaStandardTime,
        WestBankStandardTime,
        SouthAfricaStandardTime,
        FleStandardTime,
        IsraelStandardTime,
        KaliningradStandardTime,
        SudanStandardTime,
        LibyaStandardTime,
        NamibiaStandardTime,
        ArabicStandardTime,
        TurkeyStandardTime,
        ArabStandardTime,
        BelarusStandardTime,
        RussianStandardTime,
        EAfricaStandardTime,
        IranStandardTime,
        ArabianStandardTime,
        AstrakhanStandardTime,
        AzerbaijanStandardTime,
        RussiaTimeZone3,
        MauritiusStandardTime,
        SaratovStandardTime,
        GeorgianStandardTime,
        VolgogradStandardTime,
        CaucasusStandardTime,
        AfghanistanStandardTime,
        WestAsiaStandardTime,
        EkaterinburgStandardTime,
        PakistanStandardTime,
        QyzylordaStandardTime,
        IndiaStandardTime,
        SriLankaStandardTime,
        NepalStandardTime,
        CentralAsiaStandardTime,
        BangladeshStandardTime,
        OmskStandardTime,
        MyanmarStandardTime,
        SeAsiaStandardTime,
        AltaiStandardTime,
        WMongoliaStandardTime,
        NorthAsiaStandardTime,
        NCentralAsiaStandardTime,
        TomskStandardTime,
        ChinaStandardTime,
        NorthAsiaEastStandardTime,
        SingaporeStandardTime,
        WAustraliaStandardTime,
        TaipeiStandardTime,
        UlaanbaatarStandardTime,
        AusCentralWStandardTime,
        TransbaikalStandardTime,
        TokyoStandardTime,
        NorthKoreaStandardTime,
        KoreaStandardTime,
        YakutskStandardTime,
        CenAustraliaStandardTime,
        AusCentralStandardTime,
        EAustraliaStandardTime,
        AusEasternStandardTime,
        WestPacificStandardTime,
        TasmaniaStandardTime,
        VladivostokStandardTime,
        LordHoweStandardTime,
        BougainvilleStandardTime,
        RussiaTimeZone10,
        MagadanStandardTime,
        NorfolkStandardTime,
        SakhalinStandardTime,
        CentralPacificStandardTime,
        RussiaTimeZone11,
        NewZealandStandardTime,
        UTC12,
        FijiStandardTime,
        KamchatkaStandardTime,
        ChathamIslandsStandardTime,
        UTC13,
        TongaStandardTime,
        SamoaStandardTime,
        LineIslandsStandardTime
        // ReSharper restore IdentifierTypo
        // ReSharper restore UnusedMember.Global
    }

    public static class Utils
    {
        private static readonly Dictionary<SystemTimeZoneInfoID, string> _systemTimeZoneInfoIDToTimeZoneID =
            new Dictionary<SystemTimeZoneInfoID, string>
            {
                { SystemTimeZoneInfoID.UTC11, "UTC-11" },
                { SystemTimeZoneInfoID.UTC09, "UTC-09" },
                { SystemTimeZoneInfoID.PacificStandardTimeMexico, "Pacific Standard Time (Mexico)" },
                { SystemTimeZoneInfoID.UTC08, "UTC-08" },
                { SystemTimeZoneInfoID.MountainStandardTimeMexico, "Mountain Standard Time (Mexico)" },
                { SystemTimeZoneInfoID.CentralStandardTimeMexico, "Central Standard Time (Mexico)" },
                { SystemTimeZoneInfoID.EasternStandardTimeMexico, "Eastern Standard Time (Mexico)" },
                { SystemTimeZoneInfoID.ESouthAmericaStandardTime, "E. South America Standard Time" },
                { SystemTimeZoneInfoID.UTC02, "UTC-02" },
                { SystemTimeZoneInfoID.MidAtlanticStandardTime, "Mid-Atlantic Standard Time" },
                { SystemTimeZoneInfoID.UTC, "UTC" },
                { SystemTimeZoneInfoID.WEuropeStandardTime, "W. Europe Standard Time" },
                { SystemTimeZoneInfoID.WCentralAfricaStandardTime, "W. Central Africa Standard Time" },
                { SystemTimeZoneInfoID.EEuropeStandardTime, "E. Europe Standard Time" },
                { SystemTimeZoneInfoID.EAfricaStandardTime, "E. Africa Standard Time" },
                { SystemTimeZoneInfoID.RussiaTimeZone3, "Russia Time Zone 3" },
                { SystemTimeZoneInfoID.WMongoliaStandardTime, "W. Mongolia Standard Time" },
                { SystemTimeZoneInfoID.NCentralAsiaStandardTime, "N. Central Asia Standard Time" },
                { SystemTimeZoneInfoID.WAustraliaStandardTime, "W. Australia Standard Time" },
                { SystemTimeZoneInfoID.AusCentralWStandardTime, "Aus Central W. Standard Time" },
                { SystemTimeZoneInfoID.CenAustraliaStandardTime, "Cen. Australia Standard Time" },
                { SystemTimeZoneInfoID.EAustraliaStandardTime, "E. Australia Standard Time" },
                { SystemTimeZoneInfoID.RussiaTimeZone10, "Russia Time Zone 10" },
                { SystemTimeZoneInfoID.RussiaTimeZone11, "Russia Time Zone 11" },
                { SystemTimeZoneInfoID.UTC12, "UTC+12" },
                { SystemTimeZoneInfoID.UTC13, "UTC+13" }
            };

        private static string GetTimeZoneID(SystemTimeZoneInfoID timeZoneInfoID) =>
            _systemTimeZoneInfoIDToTimeZoneID.TryGetValue(timeZoneInfoID, out var timeZoneID) ? timeZoneID :
                timeZoneInfoID.ToString().UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter();

        private static readonly Dictionary<string, TimeZoneInfo> _timeZoneInfoDictionaryCache =
            new Dictionary<string, TimeZoneInfo>();

        private static TimeZoneInfo? _defaultTimeZone;
        public static TimeZoneInfo DefaultTimeZone
        {
            get
            {
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if(_defaultTimeZone == null)
                    throw new InvalidOperationException("The DefaultTimeZone has not been set.");

                return _defaultTimeZone;
            }
            private set
            {
                if(_defaultTimeZone != null)
                    throw new InvalidOperationException("The DefaultTimeZone cannot be changed once set.");

                _defaultTimeZone = value;
            }
        }

        public static DateTime TimeNowInDefaultTimeZone => GetTimeNowInTimeZone(DefaultTimeZone);
        public static DateTime TimeTodayInDefaultTimeZone => GetTimeTodayInTimeZone(DefaultTimeZone);

        public static DateTime ConvertTimeFromDefaultTimeZoneToUtc(DateTime dateTime) =>
            ConvertTimeFromTimeZoneToUtc(dateTime, DefaultTimeZone);

        public static DateTime ConvertTimeFromTimeZoneToUtc(DateTime dateTime, TimeZoneInfo sourceTimeZone) =>
            TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);

        public static DateTime ConvertTimeFromTimeZoneToUtc(DateTime dateTime, string sourceTimeZoneID) =>
            ConvertTimeFromTimeZoneToUtc(dateTime, GetTimeZoneInfo(sourceTimeZoneID));

        public static DateTime ConvertTimeFromTimeZoneToUtc(
            DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ConvertTimeFromTimeZoneToUtc(dateTime, GetTimeZoneID(sourceTimeZoneID));

        public static DateTime ConvertTimeFromUtcToDefaultTimeZone(DateTime dateTimeUtc) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, DefaultTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, TimeZoneInfo targetTimeZone) =>
            TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, targetTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, string targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, GetTimeZoneInfo(targetTimeZoneID));

        public static DateTime ConvertTimeFromUtcToTimeZone(
            DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, GetTimeZoneID(targetTimeZoneID));

        public static DateTime GetTimeNowInTimeZone(TimeZoneInfo timeZone) =>
            ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, timeZone);

        public static DateTime GetTimeNowInTimeZone(string timeZoneID) =>
            GetTimeNowInTimeZone(GetTimeZoneInfo(timeZoneID));

        public static DateTime GetTimeNowInTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            GetTimeNowInTimeZone(GetTimeZoneID(timeZoneID));

        public static DateTime GetTimeTodayInTimeZone(TimeZoneInfo timeZone) => GetTimeNowInTimeZone(timeZone).Date;

        public static DateTime GetTimeTodayInTimeZone(string timeZoneID) =>
            GetTimeTodayInTimeZone(GetTimeZoneInfo(timeZoneID));

        public static DateTime GetTimeTodayInTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            GetTimeTodayInTimeZone(GetTimeZoneID(timeZoneID));

        public static TimeZoneInfo GetTimeZone(string timeZoneID) => GetTimeZoneInfo(timeZoneID);

        public static TimeZoneInfo GetTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            GetTimeZone(GetTimeZoneID(timeZoneID));

        private static TimeZoneInfo GetTimeZoneInfo(string timeZoneID)
        {
            if(_timeZoneInfoDictionaryCache.TryGetValue(timeZoneID, out var timeZoneInfo))
                return timeZoneInfo;

            timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZoneID);
            _timeZoneInfoDictionaryCache.Add(timeZoneID, timeZoneInfo);

            return timeZoneInfo;
        }

        public static DateTime ParseExactTimeFromDefaultTimeZoneToUtc(string value, string format) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, DefaultTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, TimeZoneInfo sourceTimeZone) =>
            ConvertTimeFromTimeZoneToUtc(
                DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None), sourceTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(string value, string format, string sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, GetTimeZoneInfo(sourceTimeZoneID));

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, GetTimeZoneID(sourceTimeZoneID));

        public static void SetDefaultTimeZone(TimeZoneInfo timeZone) => DefaultTimeZone = timeZone;

        public static void SetDefaultTimeZone(string timeZoneID) =>
            SetDefaultTimeZone(GetTimeZoneInfo(timeZoneID));

        public static void SetDefaultTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            SetDefaultTimeZone(GetTimeZoneID(timeZoneID));

        public static bool TryParseExactTimeFromDefaultTimeZoneToUtc(
            string value, string format, out DateTime resultUtc) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, DefaultTimeZone, out resultUtc);

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, TimeZoneInfo sourceTimeZone, out DateTime resultUtc)
        {
            if(!DateTime.TryParseExact(
                value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultUtc))
                return false;

            resultUtc = ConvertTimeFromTimeZoneToUtc(resultUtc, sourceTimeZone);

            return true;
        }

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, string sourceTimeZoneID, out DateTime resultUtc) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, GetTimeZoneInfo(sourceTimeZoneID), out resultUtc);

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID, out DateTime resultUtc) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, GetTimeZoneID(sourceTimeZoneID), out resultUtc);
    }
}
