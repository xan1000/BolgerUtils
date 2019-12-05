# BolgerUtils.TimeZoneConverter

BolgerUtils.TimeZoneConverter is a .NET Standard 2.0 library which provides...

# SystemTimeZoneInfoID enum

* DatelineStandardTime
* UTC11
* AleutianStandardTime
* HawaiianStandardTime
* MarquesasStandardTime
* AlaskanStandardTime
* UTC09
* PacificStandardTimeMexico
* UTC08
* PacificStandardTime
* UsMountainStandardTime
* MountainStandardTimeMexico
* MountainStandardTime
* CentralAmericaStandardTime
* CentralStandardTime
* EasterIslandStandardTime
* CentralStandardTimeMexico
* CanadaCentralStandardTime
* SaPacificStandardTime
* EasternStandardTimeMexico
* EasternStandardTime
* HaitiStandardTime
* CubaStandardTime
* UsEasternStandardTime
* TurksAndCaicosStandardTime
* ParaguayStandardTime
* AtlanticStandardTime
* VenezuelaStandardTime
* CentralBrazilianStandardTime
* SaWesternStandardTime
* PacificSaStandardTime
* NewfoundlandStandardTime
* TocantinsStandardTime
* ESouthAmericaStandardTime
* SaEasternStandardTime
* ArgentinaStandardTime
* GreenlandStandardTime
* MontevideoStandardTime
* MagallanesStandardTime
* SaintPierreStandardTime
* BahiaStandardTime
* UTC02
* MidAtlanticStandardTime
* AzoresStandardTime
* CapeVerdeStandardTime
* UTC
* GmtStandardTime
* GreenwichStandardTime
* SaoTomeStandardTime
* MoroccoStandardTime
* WEuropeStandardTime
* CentralEuropeStandardTime
* RomanceStandardTime
* CentralEuropeanStandardTime
* WCentralAfricaStandardTime
* JordanStandardTime
* GtbStandardTime
* MiddleEastStandardTime
* EgyptStandardTime
* EEuropeStandardTime
* SyriaStandardTime
* WestBankStandardTime
* SouthAfricaStandardTime
* FleStandardTime
* IsraelStandardTime
* KaliningradStandardTime
* SudanStandardTime
* LibyaStandardTime
* NamibiaStandardTime
* ArabicStandardTime
* TurkeyStandardTime
* ArabStandardTime
* BelarusStandardTime
* RussianStandardTime
* EAfricaStandardTime
* IranStandardTime
* ArabianStandardTime
* AstrakhanStandardTime
* AzerbaijanStandardTime
* RussiaTimeZone3
* MauritiusStandardTime
* SaratovStandardTime
* GeorgianStandardTime
* VolgogradStandardTime
* CaucasusStandardTime
* AfghanistanStandardTime
* WestAsiaStandardTime
* EkaterinburgStandardTime
* PakistanStandardTime
* QyzylordaStandardTime
* IndiaStandardTime
* SriLankaStandardTime
* NepalStandardTime
* CentralAsiaStandardTime
* BangladeshStandardTime
* OmskStandardTime
* MyanmarStandardTime
* SeAsiaStandardTime
* AltaiStandardTime
* WMongoliaStandardTime
* NorthAsiaStandardTime
* NCentralAsiaStandardTime
* TomskStandardTime
* ChinaStandardTime
* NorthAsiaEastStandardTime
* SingaporeStandardTime
* WAustraliaStandardTime
* TaipeiStandardTime
* UlaanbaatarStandardTime
* AusCentralWStandardTime
* TransbaikalStandardTime
* TokyoStandardTime
* NorthKoreaStandardTime
* KoreaStandardTime
* YakutskStandardTime
* CenAustraliaStandardTime
* AusCentralStandardTime
* EAustraliaStandardTime
* AusEasternStandardTime
* WestPacificStandardTime
* TasmaniaStandardTime
* VladivostokStandardTime
* LordHoweStandardTime
* BougainvilleStandardTime
* RussiaTimeZone10
* MagadanStandardTime
* NorfolkStandardTime
* SakhalinStandardTime
* CentralPacificStandardTime
* RussiaTimeZone11
* NewZealandStandardTime
* UTC12
* FijiStandardTime
* KamchatkaStandardTime
* ChathamIslandsStandardTime
* UTC13
* TongaStandardTime
* SamoaStandardTime
* LineIslandsStandardTime

# Utils static class

## Properties

Name | Type | Operations
--- | --- | ---
DefaultTimeZone | TimeZoneInfo | get
TimeNowInDefaultTimeZone | DateTime | get
TimeTodayInDefaultTimeZone | DateTime | get

## Methods

Name | Parameters | Returns
--- | --- | ---
ConvertTimeFromDefaultTimeZoneToUtc | DateTime dateTime | DateTime
ConvertTimeFromTimeZoneToUtc | DateTime dateTime, TimeZoneInfo sourceTimeZone | DateTime
ConvertTimeFromTimeZoneToUtc | DateTime dateTime, string sourceTimeZoneID | DateTime
ConvertTimeFromTimeZoneToUtc | DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID | DateTime
ConvertTimeFromUtcToDefaultTimeZone | DateTime dateTimeUtc | DateTime
ConvertTimeFromUtcToTimeZone | DateTime dateTimeUtc, TimeZoneInfo targetTimeZone | DateTime
ConvertTimeFromUtcToTimeZone | DateTime dateTimeUtc, string targetTimeZoneID | DateTime
ConvertTimeFromUtcToTimeZone | DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID | DateTime
GetTimeNowInTimeZone | TimeZoneInfo timeZone | DateTime
GetTimeNowInTimeZone | string timeZoneID | DateTime
GetTimeNowInTimeZone | SystemTimeZoneInfoID timeZoneID | DateTime
GetTimeTodayInTimeZone | TimeZoneInfo timeZone | DateTime
GetTimeTodayInTimeZone | string timeZoneID | DateTime
GetTimeTodayInTimeZone | SystemTimeZoneInfoID timeZoneID | DateTime
ParseExactTimeFromDefaultTimeZoneToUtc | string value, string format | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, TimeZoneInfo sourceTimeZone | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, string sourceTimeZoneID | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, SystemTimeZoneInfoID sourceTimeZoneID | DateTime
SetDefaultTimeZone | TimeZoneInfo timeZone | void
SetDefaultTimeZone | string timeZoneID | void
SetDefaultTimeZone | SystemTimeZoneInfoID timeZoneID | void
TryParseExactTimeFromDefaultTimeZoneToUtc | string value, string format, out DateTime result | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, TimeZoneInfo sourceTimeZone, out DateTime result | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, string sourceTimeZoneID, out DateTime result | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, SystemTimeZoneInfoID sourceTimeZoneID, out DateTime result | bool

# Extension methods

Name | Parameters | Returns
--- | --- | ---
ConvertTimeFromDefaultTimeZoneToUtc | this DateTime item | DateTime
ConvertTimeFromTimeZoneToUtc | this DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID | DateTime
ConvertTimeFromUtcToDefaultTimeZone | this DateTime dateTimeUtc | DateTime
ConvertTimeFromUtcToTimeZone | this DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID | DateTime

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe: https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
