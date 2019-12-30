# BolgerUtils

BolgerUtils is a .NET Standard 2.0 library which provides various reusable (and sometimes useful) methods. This functionality comes in two forms, as static constants & methods found in the **BolgerUtils.Utils** class and by extension methods available when the **BolgerUtils** namespace is imported via:

######

```csharp
using BolgerUtils;
```

# Utils static class

## Constants

Name | Type | Value
--- | --- | ---
BrElement | string | <br /\>
CarriageReturn | string | \r
CarriageReturnChar | char | \r
DaysPerWeek | int | 7
DoubleQuote | string | "
DoubleQuoteChar | char | "
Empty | string |
NewLine | string | \n
NewLineChar | char | \n
NonBreakingSpace | string | &nbsp;
SingleQuote | string | '
SingleQuoteChar | char | '
Space | string | " "
SpaceChar | char | ' '
Zero | int | 0
Ten | int | 10
Hundred | int | 100
Thousand | int | 1,000
Million | int | 1,000,000
Billion | int | 1,000,000,000

## Methods

Name | Parameters | Returns
--- | --- | ---
EachDay | DateTime startDate, int daysForward | List<DateTime>
EachDay | DateTime startDate, DateTime endDate | List<DateTime>
From | int constant, double value | double
FromTens | int value | int
FromHundreds | int value | int
FromThousands | int value | int
FromMillions | int value | int
FromBillions | int value | int
GetEnumValue<T\> where T : Enum | int value | T
GetEnumValues<T\> where T : Enum | void | List<T\>
Max | DateTime a, DateTime b | DateTime
Max<T\> where T : struct, IComparable<T\> | T a, T b | T
Min | DateTime a, DateTime b | DateTime
Min<T\> where T : struct, IComparable<T\> | T a, T b | T
RandomString | int length | string
Swap<T\> | ref T a, ref T b | void

# Extension methods

## Boolean

Name | Parameters | Returns
--- | --- | ---
Checked | this bool item | string
Disabled | this bool item | string
Display | this bool item, string trueDisplay, string falseDisplay = Utils.Empty | string
HasError | this bool item | string
ReadOnlyCheckbox | this bool item | string
Selected | this bool item | string
YesOrNo | this bool item | string

## Byte

Name | Parameters | Returns
--- | --- | ---
ToHexString | this byte[] item | string

## DateTime

Name | Parameters | Returns
--- | --- | ---
AddWeeks | this DateTime item, double weeks | DateTime
FindDayOfWeekBackward | this DateTime item, DayOfWeek dayOfWeek | DateTime
FindDayOfWeekForward | this DateTime item, DayOfWeek dayOfWeek | DateTime
IsDayInFuture | this DateTime a, DateTime b | bool
IsDayInFutureOrToday | this DateTime a, DateTime b | bool
IsDayInPast | this DateTime a, DateTime b | bool
IsDayInPastOrToday | this DateTime a, DateTime b | bool
IsInFuture | this DateTime a, DateTime b | bool
IsInFutureOrNow | this DateTime a, DateTime b | bool
IsInPast | this DateTime a, DateTime b | bool
IsInPastOrNow | this DateTime a, DateTime b | bool
Tomorrow | this DateTime item | DateTime
Yesterday | this DateTime item | DateTime

## DayOfWeek

Name | Parameters | Returns
--- | --- | ---
OrderByDayOfWeekStartingOnMonday | this IEnumerable<DayOfWeek\> source | IOrderedEnumerable<DayOfWeek\>

## Enum

Name | Parameters | Returns
--- | --- | ---
ToInt | this Enum item | int
ToValueString | this Enum item | string

## Generic

Name | Parameters | Returns
--- | --- | ---
IsContainedIn<T\> | this T item, params T[] values | bool
IsNull<T\> where T : class | this T item | bool
ScalarToList<T\> | this T item | List<T\>

## ICollection

Name | Parameters | Returns
--- | --- | ---
AddAll<T\> | this ICollection<T\> item, IEnumerable<T\> objects | void
RemoveAll<T\> | this ICollection<T\> item, IEnumerable<T\> objects | bool
RemoveAll<T\> | this ICollection<T\> item, Func<T, bool\> predicate | bool

## IEnumerable

Name | Parameters | Returns
--- | --- | ---
IsEmpty<T\> | this IEnumerable<T\> source | bool
NotAll<T\> | this IEnumerable<T\> source, Func<T, bool\> predicate | bool
NotAny<T\> | this IEnumerable<T\> source | bool
NotAny<T\> | this IEnumerable<T\> source, Func<T, bool\> predicate | bool
NotWhere<T\> | this IEnumerable<T\> source, Func<T, bool\> predicate | IEnumerable<T\>
ToListToHashSet<T\> | this IEnumerable<T\> source | HashSet<T\>

## List

Name | Parameters | Returns
--- | --- | ---
ToHashSet<T\> | this List<T\> item | HashSet<T\>

## Numbers

Name | Parameters | Returns
--- | --- | ---
IsInRange | this int item, int min, int max | bool
IsInRange | this decimal item, decimal min, decimal max | bool
IsInRange | this double item, double min, double max | bool
IsInRange | this DateTime item, DateTime min, DateTime max | bool
IsInRange<T\> where T : struct, IComparable<T\> | this T item, T min, T max | bool
IsNegative | this int item | bool
IsNegative | this decimal item | bool
IsNegative | this double item | bool
IsNegative<T\> where T : struct, IComparable<T\> | this T item | bool
IsPositive | this int item | bool
IsPositive | this decimal item | bool
IsPositive | this double item | bool
IsPositive<T\> where T : struct, IComparable<T\> | this T item | bool
IsZero | this int item | bool
IsZero | this decimal item | bool
IsZero | this double item | bool
IsZero<T\> where T : struct, IComparable<T\> | this T item | bool

## Object

Name | Parameters | Returns
--- | --- | ---
ToDynamic | this object item | dynamic

## String

Name | Parameters | Returns
--- | --- | ---
Abbreviate | this string item, int length | string
GetLast8Digits | this string item | string
IsEmpty | this string item | bool
IsInvalidDecimal | this string item | bool
IsInvalidDouble | this string item | bool
IsInvalidEmail | this string item | bool
IsInvalidInt | this string item | bool
IsInvalidMoney | this string item | bool
IsNullOrEmpty | this string item | bool
IsNullOrWhiteSpace | this string item | bool
IsValidDecimal | this string item | bool
IsValidDouble | this string item | bool
IsValidEmail | this string item | bool
IsValidInt | this string item | bool
IsValidMoney | this string item | bool
Join | this IEnumerable<string\> source, string separator | string
Join | this IEnumerable<string\> source, char separator | string
NewLineToBr | this string item | string
Remove | this string item, string value | string
RemoveCarriageReturn | this string item | string
RemoveDoubleQuotation | this string item | string
RemoveNewLine | this string item | string
RemoveRedundantWhitespace | this string item | string
RemoveSingleQuotation | this string item | string
RemoveSpaceAndSingleQuotation | this string item | string
SpaceToNbsp | this string item | string
ToBoolean | this string item | bool
ToDbConnectionStringBuilder | this string item | DbConnectionStringBuilder
ToDecimal | this string item | decimal
ToDecimalOrNull | this string item | decimal?
ToDouble | this string item | double
ToDoubleOrNull | this string item | double?
ToEmptyIfNullOrWhiteSpace | this string item | string
ToFileInfo | this string item | FileInfo
ToInt | this string item | int
ToIntOrNull | this string item | int?
ToNullIfNullOrWhiteSpace | this string item | string
Truncate | this string item, int length | string
UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter | this string item | string

## TimeSpan

Name | Parameters | Returns
--- | --- | ---
To24HourAndMinutesString | this TimeSpan item | string
ToDateTimeString | this TimeSpan item, string format = "h:mm tt" | string

# Remarks

**Utils.Empty** exists so it can be used in a context that requires a compile time constant instead of using **""** since **string.Empty** will not work in such a case, e.g., as a default value for a parameter.

**.RandomString** returns a string consisting of the following characters: ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789

The **.AddAll** and **.RemoveAll** methods can be used on regular collections but are intended to be used in conjunction with EntityFramework.

The **.NotAll**, **.NotAny** and **.NotWhere** methods can be used on any **IEnumerable<T\>** source but are intended to be used in conjunction with EntityFramework.

**.ToListToHashSet()** can be used on any **IEnumerable<T\>** source but is intended to be used in conjunction with EntityFramework, i.e., **.ToList()** is used to execute the query which is then immediately converted to a **HashSet** via **.ToHashSet()**.

**.ToDateTimeString** uses **DateTime** format strings, see: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe: https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
