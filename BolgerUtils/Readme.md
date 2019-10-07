# BolgerUtils

BolgerUtils is a .NET Standard 2.0 library which provides various re-useable (and sometimes useful) methods. This functionality comes in two forms, as static constants & methods found in the BolgerUtils.Utils class and via extension methods available when the BolgerUtils namespace is imported via:

`using BolgerUtils;`

# Utils class

## Constants

Name | Type | Value
--- | --- | ---
BrElement | string | `<br />`
CarriageReturn | string | `\r`
CarriageReturnChar | char | `\r`
DaysPerWeek | int | `7`
DoubleQuote | string | `"`
DoubleQuoteChar | char | `"`
Empty | string | `""`
NewLine | string | `\n`
NewLineChar | char | `\n`
NonBreakingSpace | string | `&nbsp;`
SingleQuote | string | `'`
SingleQuoteChar | char | `'`
Space | string | `" "`
SpaceChar | char | `' '`
Zero | int | `0`
Ten | int | `10`
Hundred | int | `100`
Thousand | int | `1000`
Million | int | `1000000`
Billion | int | `1000000000`

## Methods

- **public static double From(int constant, double value)**
- **public static int FromTens(int value)**
- **public static int FromHundreds(int value)**
- **public static int FromThousands(int value)**
- **public static int FromMillions(int value)**
- **public static int FromBillions(int value)**
- **public static T GetEnumValue<T>(int value) where T : Enum**
- **public static DateTime Max(DateTime a, DateTime b)**
- **public static T Max<T>(T a, T b) where T : struct, IComparable<T>**
- **public static DateTime Min(DateTime a, DateTime b)**
- **public static T Min<T>(T a, T b) where T : struct, IComparable<T>**
- **public static string RandomString(int length)**
- **public static void Swap<T>(ref T a, ref T b)**

# Extension methods 

## Boolean

- **public static string Checked(this bool item)**
- **public static string Disabled(this bool item)**
- **public static string Display(this bool item, string trueDisplay, string falseDisplay = Utils.Empty)**
- **public static string HasError(this bool item)**
- **public static string ReadOnlyCheckbox(this bool item)**
- **public static string Selected(this bool item)**
- **public static string YesOrNo(this bool item)**

...
