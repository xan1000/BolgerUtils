using System;
using System.Collections.Generic;
using System.Globalization;
using BolgerUtils.EnumDisplay;

namespace BolgerUtils.TimeZoneConverter
{
    public enum SystemTimeZoneInfoID
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable IdentifierTypo
        DatelineStandardTime,
        [EnumDisplay("UTC-11")]
        UTC11,
        AleutianStandardTime,
        HawaiianStandardTime,
        MarquesasStandardTime,
        AlaskanStandardTime,
        [EnumDisplay("UTC-09")]
        UTC09,
        [EnumDisplay("Pacific Standard Time (Mexico)")]
        PacificStandardTimeMexico,
        [EnumDisplay("UTC-08")]
        UTC08,
        PacificStandardTime,
        UsMountainStandardTime,
        [EnumDisplay("Mountain Standard Time (Mexico)")]
        MountainStandardTimeMexico,
        MountainStandardTime,
        CentralAmericaStandardTime,
        CentralStandardTime,
        EasterIslandStandardTime,
        [EnumDisplay("Central Standard Time (Mexico)")]
        CentralStandardTimeMexico,
        CanadaCentralStandardTime,
        SaPacificStandardTime,
        [EnumDisplay("Eastern Standard Time (Mexico)")]
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
        [EnumDisplay("E. South America Standard Time")]
        ESouthAmericaStandardTime,
        SaEasternStandardTime,
        ArgentinaStandardTime,
        GreenlandStandardTime,
        MontevideoStandardTime,
        MagallanesStandardTime,
        SaintPierreStandardTime,
        BahiaStandardTime,
        [EnumDisplay("UTC-02")]
        UTC02,
        [EnumDisplay("Mid-Atlantic Standard Time")]
        MidAtlanticStandardTime,
        AzoresStandardTime,
        CapeVerdeStandardTime,
        [EnumDisplay("UTC")]
        UTC,
        GmtStandardTime,
        GreenwichStandardTime,
        SaoTomeStandardTime,
        MoroccoStandardTime,
        [EnumDisplay("W. Europe Standard Time")]
        WEuropeStandardTime,
        CentralEuropeStandardTime,
        RomanceStandardTime,
        CentralEuropeanStandardTime,
        [EnumDisplay("W. Central Africa Standard Time")]
        WCentralAfricaStandardTime,
        JordanStandardTime,
        GtbStandardTime,
        MiddleEastStandardTime,
        EgyptStandardTime,
        [EnumDisplay("E. Europe Standard Time")]
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
        [EnumDisplay("E. Africa Standard Time")]
        EAfricaStandardTime,
        IranStandardTime,
        ArabianStandardTime,
        AstrakhanStandardTime,
        AzerbaijanStandardTime,
        [EnumDisplay("Russia Time Zone 3")]
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
        [EnumDisplay("W. Mongolia Standard Time")]
        WMongoliaStandardTime,
        NorthAsiaStandardTime,
        [EnumDisplay("N. Central Asia Standard Time")]
        NCentralAsiaStandardTime,
        TomskStandardTime,
        ChinaStandardTime,
        NorthAsiaEastStandardTime,
        SingaporeStandardTime,
        [EnumDisplay("W. Australia Standard Time")]
        WAustraliaStandardTime,
        TaipeiStandardTime,
        UlaanbaatarStandardTime,
        [EnumDisplay("Aus Central W. Standard Time")]
        AusCentralWStandardTime,
        TransbaikalStandardTime,
        TokyoStandardTime,
        NorthKoreaStandardTime,
        KoreaStandardTime,
        YakutskStandardTime,
        [EnumDisplay("Cen. Australia Standard Time")]
        CenAustraliaStandardTime,
        AusCentralStandardTime,
        [EnumDisplay("E. Australia Standard Time")]
        EAustraliaStandardTime,
        AusEasternStandardTime,
        WestPacificStandardTime,
        TasmaniaStandardTime,
        VladivostokStandardTime,
        LordHoweStandardTime,
        BougainvilleStandardTime,
        [EnumDisplay("Russia Time Zone 10")]
        RussiaTimeZone10,
        MagadanStandardTime,
        NorfolkStandardTime,
        SakhalinStandardTime,
        CentralPacificStandardTime,
        [EnumDisplay("Russia Time Zone 11")]
        RussiaTimeZone11,
        NewZealandStandardTime,
        [EnumDisplay("UTC+12")]
        UTC12,
        FijiStandardTime,
        KamchatkaStandardTime,
        ChathamIslandsStandardTime,
        [EnumDisplay("UTC+13")]
        UTC13,
        TongaStandardTime,
        SamoaStandardTime,
        LineIslandsStandardTime
        // ReSharper restore IdentifierTypo
        // ReSharper restore UnusedMember.Global
    }

    public static class Utils
    {
        private static readonly Dictionary<string, TimeZoneInfo> _timeZoneInfoDictionary =
            new Dictionary<string, TimeZoneInfo>();

        private static TimeZoneInfo _defaultTimeZone;
        public static TimeZoneInfo DefaultTimeZone
        {
            get
            {
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
            ConvertTimeFromTimeZoneToUtc(dateTime, FindSystemTimeZoneByID(sourceTimeZoneID));

        public static DateTime ConvertTimeFromTimeZoneToUtc(
            DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ConvertTimeFromTimeZoneToUtc(dateTime, sourceTimeZoneID.Display());

        public static DateTime ConvertTimeFromUtcToDefaultTimeZone(DateTime dateTimeUtc) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, DefaultTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, TimeZoneInfo targetTimeZone) =>
            TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, targetTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, string targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, FindSystemTimeZoneByID(targetTimeZoneID));

        public static DateTime ConvertTimeFromUtcToTimeZone(
            DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, targetTimeZoneID.Display());

        private static TimeZoneInfo FindSystemTimeZoneByID(string timeZoneID)
        {
            if(_timeZoneInfoDictionary.TryGetValue(timeZoneID, out var timeZoneInfo))
                return timeZoneInfo;

            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            _timeZoneInfoDictionary.Add(timeZoneID, timeZoneInfo);

            return timeZoneInfo;
        }

        public static DateTime GetTimeNowInTimeZone(TimeZoneInfo timeZone) =>
            ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, timeZone);

        public static DateTime GetTimeNowInTimeZone(string timeZoneID) =>
            GetTimeNowInTimeZone(FindSystemTimeZoneByID(timeZoneID));

        public static DateTime GetTimeNowInTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            GetTimeNowInTimeZone(timeZoneID.Display());

        public static DateTime GetTimeTodayInTimeZone(TimeZoneInfo timeZone) => GetTimeNowInTimeZone(timeZone).Date;

        public static DateTime GetTimeTodayInTimeZone(string timeZoneID) =>
            GetTimeTodayInTimeZone(FindSystemTimeZoneByID(timeZoneID));

        public static DateTime GetTimeTodayInTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            GetTimeTodayInTimeZone(timeZoneID.Display());

        public static TimeZoneInfo GetTimeZone(string timeZoneID) => FindSystemTimeZoneByID(timeZoneID);
        public static TimeZoneInfo GetTimeZone(SystemTimeZoneInfoID timeZoneID) => GetTimeZone(timeZoneID.Display());

        public static DateTime ParseExactTimeFromDefaultTimeZoneToUtc(string value, string format) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, DefaultTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, TimeZoneInfo sourceTimeZone) =>
            ConvertTimeFromTimeZoneToUtc(
                DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None), sourceTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(string value, string format, string sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, FindSystemTimeZoneByID(sourceTimeZoneID));

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, sourceTimeZoneID.Display());

        public static void SetDefaultTimeZone(TimeZoneInfo timeZone) => DefaultTimeZone = timeZone;

        public static void SetDefaultTimeZone(string timeZoneID) =>
            SetDefaultTimeZone(FindSystemTimeZoneByID(timeZoneID));

        public static void SetDefaultTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            SetDefaultTimeZone(timeZoneID.Display());

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
            TryParseExactTimeFromTimeZoneToUtc(value, format, FindSystemTimeZoneByID(sourceTimeZoneID), out resultUtc);

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID, out DateTime resultUtc) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, sourceTimeZoneID.Display(), out resultUtc);
    }
}
