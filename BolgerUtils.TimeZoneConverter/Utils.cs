using System;
using System.Collections.Generic;
using System.Globalization;
using BolgerUtils.EnumDisplay;

namespace BolgerUtils.TimeZoneConverter
{
    public enum SystemTimeZoneInfoID
    {
        // ReSharper disable IdentifierTypo
        DatelineStandardTime,
        [Add("UTC-11")]
        UTC11,
        AleutianStandardTime,
        HawaiianStandardTime,
        MarquesasStandardTime,
        AlaskanStandardTime,
        [Add("UTC-09")]
        UTC09,
        [Add("Pacific Standard Time (Mexico)")]
        PacificStandardTimeMexico,
        [Add("UTC-08")]
        UTC08,
        PacificStandardTime,
        UsMountainStandardTime,
        [Add("Mountain Standard Time (Mexico)")]
        MountainStandardTimeMexico,
        MountainStandardTime,
        CentralAmericaStandardTime,
        CentralStandardTime,
        EasterIslandStandardTime,
        [Add("Central Standard Time (Mexico)")]
        CentralStandardTimeMexico,
        CanadaCentralStandardTime,
        SaPacificStandardTime,
        [Add("Eastern Standard Time (Mexico)")]
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
        [Add("E. South America Standard Time")]
        ESouthAmericaStandardTime,
        SaEasternStandardTime,
        ArgentinaStandardTime,
        GreenlandStandardTime,
        MontevideoStandardTime,
        MagallanesStandardTime,
        SaintPierreStandardTime,
        BahiaStandardTime,
        [Add("UTC-02")]
        UTC02,
        [Add("Mid-Atlantic Standard Time")]
        MidAtlanticStandardTime,
        AzoresStandardTime,
        CapeVerdeStandardTime,
        [Add("UTC")]
        UTC,
        GmtStandardTime,
        GreenwichStandardTime,
        SaoTomeStandardTime,
        MoroccoStandardTime,
        [Add("W. Europe Standard Time")]
        WEuropeStandardTime,
        CentralEuropeStandardTime,
        RomanceStandardTime,
        CentralEuropeanStandardTime,
        [Add("W. Central Africa Standard Time")]
        WCentralAfricaStandardTime,
        JordanStandardTime,
        GtbStandardTime,
        MiddleEastStandardTime,
        EgyptStandardTime,
        [Add("E. Europe Standard Time")]
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
        [Add("E. Africa Standard Time")]
        EAfricaStandardTime,
        IranStandardTime,
        ArabianStandardTime,
        AstrakhanStandardTime,
        AzerbaijanStandardTime,
        [Add("Russia Time Zone 3")]
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
        [Add("W. Mongolia Standard Time")]
        WMongoliaStandardTime,
        NorthAsiaStandardTime,
        [Add("N. Central Asia Standard Time")]
        NCentralAsiaStandardTime,
        TomskStandardTime,
        ChinaStandardTime,
        NorthAsiaEastStandardTime,
        SingaporeStandardTime,
        [Add("W. Australia Standard Time")]
        WAustraliaStandardTime,
        TaipeiStandardTime,
        UlaanbaatarStandardTime,
        [Add("Aus Central W. Standard Time")]
        AusCentralWStandardTime,
        TransbaikalStandardTime,
        TokyoStandardTime,
        NorthKoreaStandardTime,
        KoreaStandardTime,
        YakutskStandardTime,
        [Add("Cen. Australia Standard Time")]
        CenAustraliaStandardTime,
        AusCentralStandardTime,
        [Add("E. Australia Standard Time")]
        EAustraliaStandardTime,
        AusEasternStandardTime,
        WestPacificStandardTime,
        TasmaniaStandardTime,
        VladivostokStandardTime,
        LordHoweStandardTime,
        BougainvilleStandardTime,
        [Add("Russia Time Zone 10")]
        RussiaTimeZone10,
        MagadanStandardTime,
        NorfolkStandardTime,
        SakhalinStandardTime,
        CentralPacificStandardTime,
        [Add("Russia Time Zone 11")]
        RussiaTimeZone11,
        NewZealandStandardTime,
        [Add("UTC+12")]
        UTC12,
        FijiStandardTime,
        KamchatkaStandardTime,
        ChathamIslandsStandardTime,
        [Add("UTC+13")]
        UTC13,
        TongaStandardTime,
        SamoaStandardTime,
        LineIslandsStandardTime
        // ReSharper restore IdentifierTypo
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
            ConvertTimeFromTimeZoneToUtc(dateTime, FindSystemTimeZoneById(sourceTimeZoneID));

        public static DateTime ConvertTimeFromTimeZoneToUtc(
            DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ConvertTimeFromTimeZoneToUtc(dateTime, sourceTimeZoneID.Display());

        public static DateTime ConvertTimeFromUtcToDefaultTimeZone(DateTime dateTimeUtc) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, DefaultTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, TimeZoneInfo targetTimeZone) =>
            TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, targetTimeZone);

        public static DateTime ConvertTimeFromUtcToTimeZone(DateTime dateTimeUtc, string targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, FindSystemTimeZoneById(targetTimeZoneID));

        public static DateTime ConvertTimeFromUtcToTimeZone(
            DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID) =>
            ConvertTimeFromUtcToTimeZone(dateTimeUtc, targetTimeZoneID.Display());

        private static TimeZoneInfo FindSystemTimeZoneById(string timeZoneID)
        {
            if(_timeZoneInfoDictionary.TryGetValue(timeZoneID, out var timeZoneInfo))
                return timeZoneInfo;

            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            _timeZoneInfoDictionary.Add(timeZoneID, timeZoneInfo);

            return timeZoneInfo;
        }

        public static DateTime GetTimeNowInTimeZone(TimeZoneInfo timeZone) =>
            ConvertTimeFromUtcToTimeZone(DateTime.UtcNow, timeZone);

        public static DateTime GetTimeTodayInTimeZone(TimeZoneInfo timeZone) => GetTimeNowInTimeZone(timeZone).Date;

        public static DateTime ParseExactTimeFromDefaultTimeZoneToUtc(string value, string format) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, DefaultTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, TimeZoneInfo sourceTimeZone) =>
            ConvertTimeFromTimeZoneToUtc(
                DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None), sourceTimeZone);

        public static DateTime ParseExactTimeFromTimeZoneToUtc(string value, string format, string sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, FindSystemTimeZoneById(sourceTimeZoneID));

        public static DateTime ParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID) =>
            ParseExactTimeFromTimeZoneToUtc(value, format, sourceTimeZoneID.Display());

        public static void SetDefaultTimeZone(TimeZoneInfo timeZone) => DefaultTimeZone = timeZone;

        public static void SetDefaultTimeZone(string timeZoneID) =>
            SetDefaultTimeZone(FindSystemTimeZoneById(timeZoneID));

        public static void SetDefaultTimeZone(SystemTimeZoneInfoID timeZoneID) =>
            SetDefaultTimeZone(timeZoneID.Display());

        public static bool TryParseExactTimeFromDefaultTimeZoneToUtc(
            string value, string format, out DateTime result) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, DefaultTimeZone, out result);

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, TimeZoneInfo sourceTimeZone, out DateTime result)
        {
            if(!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return false;
            result = ConvertTimeFromTimeZoneToUtc(result, sourceTimeZone);
            return true;
        }

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, string sourceTimeZoneID, out DateTime result) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, FindSystemTimeZoneById(sourceTimeZoneID), out result);

        public static bool TryParseExactTimeFromTimeZoneToUtc(
            string value, string format, SystemTimeZoneInfoID sourceTimeZoneID, out DateTime result) =>
            TryParseExactTimeFromTimeZoneToUtc(value, format, sourceTimeZoneID.Display(), out result);
    }
}
