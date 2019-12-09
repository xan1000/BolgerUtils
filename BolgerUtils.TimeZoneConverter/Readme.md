# BolgerUtils.TimeZoneConverter

BolgerUtils.TimeZoneConverter is a .NET Standard 2.0 library which provides methods to convert a DateTime from UTC to a specified timezone and vice-versa. This functionality comes in two forms, as static methods found in the **BolgerUtils.TimeZoneConverter.Utils** class and via extension methods available when the **BolgerUtils.TimeZoneConverter** namespace is imported via:

######

```csharp
using BolgerUtils.TimeZoneConverter;
```

There is an enum **BolgerUtils.TimeZoneConverter.SystemTimeZoneInfoID** declared which can be used to identify a timezone over using its system name.

A default timezone can be set to streamline converting to & from the same timezone.

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
GetTimeZone | string timeZoneID | TimeZoneInfo
GetTimeZone | SystemTimeZoneInfoID timeZoneID | TimeZoneInfo
ParseExactTimeFromDefaultTimeZoneToUtc | string value, string format | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, TimeZoneInfo sourceTimeZone | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, string sourceTimeZoneID | DateTime
ParseExactTimeFromTimeZoneToUtc | string value, string format, SystemTimeZoneInfoID sourceTimeZoneID | DateTime
SetDefaultTimeZone | TimeZoneInfo timeZone | void
SetDefaultTimeZone | string timeZoneID | void
SetDefaultTimeZone | SystemTimeZoneInfoID timeZoneID | void
TryParseExactTimeFromDefaultTimeZoneToUtc | string value, string format, out DateTime resultUtc | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, TimeZoneInfo sourceTimeZone, out DateTime resultUtc | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, string sourceTimeZoneID, out DateTime resultUtc | bool
TryParseExactTimeFromTimeZoneToUtc | string value, string format, SystemTimeZoneInfoID sourceTimeZoneID, out DateTime resultUtc | bool

# Extension methods

Name | Parameters | Returns
--- | --- | ---
ConvertTimeFromDefaultTimeZoneToUtc | this DateTime dateTime | DateTime
ConvertTimeFromTimeZoneToUtc | this DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID | DateTime
ConvertTimeFromUtcToDefaultTimeZone | this DateTime dateTimeUtc | DateTime
ConvertTimeFromUtcToTimeZone | this DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID | DateTime

# Remarks

The static methods of the **Utils** class are all overloaded to use a timezone either by directly passing the timezone as a TimeZoneInfo instance, by a timezone's system name or via the **SystemTimeZoneInfoID** enum.

Note the default timezone is set globally and once set cannot be changed to another timezone - using the default timezone is intended for scenarios in which only a single target timezone is required. Attempting to perform an operation that involves the default timezone when the default timezone has not been set results in an InvalidOperationException being thrown.

# Examples

## Looking up a timezone

A TimeZoneInfo instance can be looked up by a timezone's system name or via the **SystemTimeZoneInfoID** enum.

Using timezone's system name:

######

```csharp
TimeZoneInfo timeZone = Utils.GetTimeZone("AUS Eastern Standard Time");
```

Using **SystemTimeZoneInfoID** enum:

######

```csharp
TimeZoneInfo timeZone = Utils.GetTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
```

## Converting time from timezone to UTC

This example shows how to use all the provided overloads, however all subsequent examples will only show using the **SystemTimeZoneInfoID** enum.

Using TimeZoneInfo:

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var timeZone = Utils.GetTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
var dateTimeUtc = Utils.ConvertTimeFromTimeZoneToUtc(dateTime, timeZone);
```

Using timezone's system name:

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var dateTimeUtc = Utils.ConvertTimeFromTimeZoneToUtc(dateTime, "AUS Eastern Standard Time");
```

Using **SystemTimeZoneInfoID** enum:

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var dateTimeUtc = Utils.ConvertTimeFromTimeZoneToUtc(dateTime, SystemTimeZoneInfoID.AusEasternStandardTime);
```

## Converting time from UTC to timezone

######

```csharp
var dateTimeUtc = DateTime.UtcNow;

var dateTime = Utils.ConvertTimeFromUtcToTimeZone(dateTimeUtc, SystemTimeZoneInfoID.AusEasternStandardTime);
```

## Getting the current time or day for timezone

######

```csharp
var dateTimeNow = Utils.GetTimeNowInTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
```

######

```csharp
var dateTimeToday = Utils.GetTimeTodayInTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
```

## Setting the default timezone

######

```csharp
Utils.SetDefaultTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
```

## Getting the default timezone after its been set

######

```csharp
var timeZone = Utils.DefaultTimeZone;
```

## Using the default timezone

Once set the default timezone methods can be used.

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var dateTimeUtc = Utils.ConvertTimeFromDefaultTimeZoneToUtc(dateTime);
```

######

```csharp
var dateTimeUtc = DateTime.UtcNow;

var dateTime = Utils.ConvertTimeFromUtcToDefaultTimeZone(dateTimeUtc);
```

######

```csharp
var dateTimeNow = Utils.TimeNowInDefaultTimeZone;
```

######

```csharp
var dateTimeToday = Utils.TimeTodayInDefaultTimeZone;
```

## Using the DateTime extension methods

Extension methods on DateTime can be used for conversions, however these methods only support using the **SystemTimeZoneInfoID** enum.

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var dateTimeUtc = dateTime.ConvertTimeFromTimeZoneToUtc(SystemTimeZoneInfoID.AusEasternStandardTime);
```

######

```csharp
var dateTime = new DateTime(2019, 12, 6, 9, 30, 0);

var dateTimeUtc = dateTime.ConvertTimeFromDefaultTimeZoneToUtc();
```

######

```csharp
var dateTimeUtc = DateTime.UtcNow;

var dateTime = dateTimeUtc.ConvertTimeFromUtcToTimeZone(SystemTimeZoneInfoID.AusEasternStandardTime);
```

######

```csharp
var dateTimeUtc = DateTime.UtcNow;

var dateTime = dateTimeUtc.ConvertTimeFromUtcToDefaultTimeZone();
```

## Parsing a string with specified format from timezone directly to UTC

######

```csharp
var value = "06/12/2019 09:30 AM";

var dateTimeUtc = Utils.ParseExactTimeFromTimeZoneToUtc(value, "dd/MM/yyyy hh:mm tt", SystemTimeZoneInfoID.AusEasternStandardTime);
```

Additionally the TryParseExactTimeFromTimeZoneToUtc method is also available:

######

```csharp
var value = "06/12/2019 09:30 AM";

if(Utils.TryParseExactTimeFromTimeZoneToUtc(value, "dd/MM/yyyy hh:mm tt", SystemTimeZoneInfoID.AusEasternStandardTime, out var resultUtc))
{
    // Parsing was successful.
}
```

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe: https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
