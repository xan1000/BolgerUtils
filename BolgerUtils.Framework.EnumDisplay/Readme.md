# BolgerUtils.Framework.EnumDisplay

BolgerUtils.Framework.EnumDisplay is a .NET Framework 4.8 library which provides a means to display enums with defined names. A default behaviour of upper casing the enum name and inserting a space before every proceeding upper case letter is used when a specific enum name is not defined.

The **BolgerUtils.Framework.EnumDisplay.EnumDisplayUtils** class implements this functionality and extension methods on the enum type are available when the **BolgerUtils.Framework.EnumDisplay** namespace is imported via:

######

```csharp
using BolgerUtils.Framework.EnumDisplay;
```

Specific enum names can be defined by using the **.Add** & **.AddAll** methods or by using the **BolgerUtils.Framework.EnumDisplay.EnumDisplayAttribute** on an enum.

# EnumDisplayAttribute class inherits Attribute

## Constructors

Name | Parameters
--- | ---
EnumDisplayAttribute | string value

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
EnumDisplayUtils | bool cacheValueOnDisplay = true, Func<Enum, string\>  defaultDisplay = null

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

The **EnumDisplayUtils.DefaultDisplay** method is used if null is passed as the defaultDisplay parameter to the **EnumDisplayUtils** constructor.

# Examples

Consider the following enum:

######

```csharp
public enum EmailSentState
{
    PreparingToSend,
    Sent,
    Error
}
```

Calling **.ToString** on the enum will return the enum's name:

######

```csharp
// Output is:
// PreparingToSend
// PreparingToSend
Console.WriteLine(EmailSentState.PreparingToSend);
Console.WriteLine(EmailSentState.PreparingToSend.ToString());
```

Calling **.Display** on the enum:

######

```csharp
// Output is:
// Preparing To Send
// Sent
// Error
Console.WriteLine(EmailSentState.PreparingToSend.Display());
Console.WriteLine(EmailSentState.Sent.Display());
Console.WriteLine(EmailSentState.Error.Display());
```

Displaying a specific name for an enum using **.Add**:

######

```csharp
EmailSentState.PreparingToSend.Add("Preparing...");
```

######

```csharp
// Output is:
// Preparing...
Console.WriteLine(EmailSentState.PreparingToSend.Display());
```

Displaying a specific name for an enum using **EnumDisplayAttribute**:

######

```csharp
public enum EmailSentState
{
    PreparingToSend,
    [EnumDisplay("Email has been sent")]
    Sent,
    Error
}
```

######

```csharp
// Output is:
// Email has been sent
Console.WriteLine(EmailSentState.Sent.Display());
```

The **.Display** method also works with variables, for example:

######

```csharp
public enum EmailSentState
{
    PreparingToSend,
    [EnumDisplay("Email has been sent")]
    Sent,
    Error
}
```

######

```csharp
EmailSentState.PreparingToSend.Add("Preparing...");
```

######

```csharp
public void SomeMethod(EmailSentState state)
{
    Console.WriteLine(state.Display());
}
```

######

```csharp
// Output is:
// Preparing...
// Email has been sent
// Error
x.SomeMethod(EmailSentState.PreparingToSend);
x.SomeMethod(EmailSentState.Sent);
x.SomeMethod(EmailSentState.Error);
```

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe:

https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
