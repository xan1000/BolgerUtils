# BolgerUtils.EnumDisplay

BolgerUtils.EnumDisplay is a .NET Standard 2.0 library which provides a means to display enum values with defined names. Note a default behaviour of upper casing the enum name and inserting a space before every proceeding upper case letter is used when a specific name is not specified.

The EnumDisplayUtils class implements this functionality and extension methods on the enum type are available when the BolgerUtils.EnumDisplay namespace is imported via:

`using BolgerUtils.EnumDisplay;`

Specific names can be set by using the .Add & .AddAll methods or by using the BolgerUtils.EnumDisplay.AddAttribute on an enum.

# EnumDisplayUtils class

## Methods

Name | Parameters | Returns
--- | --- | ---
Add | Enum key, string value = null | void
AddAll | Enum key, string value = null | void
Clear | void | void
ContainsKey | Enum key | bool
Display | Enum key | string
Display | Enum key | bool

# Extension methods

## Enum

Name | Parameters | Returns
--- | --- | ---
Add | this Enum key, string value = null | void
ContainsKey | this Enum key | bool
Display | this Enum key | string
Display | this Enum key | bool
