# BolgerUtils.EnumDisplay

BolgerUtils.EnumDisplay is a .NET Standard 2.0 library which provides a means to display enum values with defined names. Note a default behaviour of upper casing the enum name and inserting a space before every proceeding upper case letter is used when a specific name is not defined.

The EnumDisplayUtils class implements this functionality and extension methods on the enum type are available when the BolgerUtils.EnumDisplay namespace is imported via:

```C#
using BolgerUtils.EnumDisplay;
```

Specific names can be set by using the .Add & .AddAll methods or by using the BolgerUtils.EnumDisplay.AddAttribute on an enum.

# AddAttribute class inherits Attribute

## Constructors

Name | Parameters
--- | ---
AddAttribute | string value

## Properties

Name | Type | Operations
--- | --- | ---
Value | string | get

# EnumDisplayUtils class

## Static methods

Name | Parameters | Returns
--- | --- | ---
DefaultDisplay | Enum key | string

## Constructors

Name | Parameters
--- | ---
EnumDisplayUtils | bool cacheValueOnDisplay = true, Func\<Enum, string\>  defaultDisplay = null

## Properties

Name | Type | Operations
--- | --- | ---
CacheValueOnDisplay | bool | get

## Methods

Name | Parameters | Returns
--- | --- | ---
Add | Enum key, string value = null | void
AddAll | Enum key, string value = null | void
Clear | void | void
ContainsKey | Enum key | bool
Display | Enum key | string
Remove | Enum key | bool

# Extension methods

## Enum

Name | Parameters | Returns
--- | --- | ---
Add | this Enum key, string value = null | void
ContainsKey | this Enum key | bool
Display | this Enum key | string
Remove | this Enum key | bool

# Remarks

Note the EnumDisplayUtils.DefaultDisplay method is used if null is passed as the defaultDisplay parameter to the EnumDisplayUtils constructor.

# Examples
