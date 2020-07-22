# BolgerUtils.Framework.WebForms

BolgerUtils.Framework.WebForms is a .NET Framework 4.8 library which provides various reusable (and sometimes useful) methods aimed for use in a WebForm project. This functionality comes in two forms, as static methods found in the **BolgerUtils.Framework.WebForms.Utils** class and by extension methods available when the **BolgerUtils.Framework.WebForms** namespace is imported via:

######

```csharp
using BolgerUtils.Framework.WebForms;
```

# Utils static class

## Methods

Name | Parameters | Returns
--- | --- | ---
TrimText | params TextBox[] textBoxes | void

# Extension methods

## DropDownList

Name | Parameters | Returns
--- | --- | ---
SelectedValueToDayOfWeekOrNull | this DropDownList item | DayOfWeek?
SelectedValueToIntOrNull | this DropDownList item | int?

## HttpRequest

Name | Parameters | Returns
--- | --- | ---
BeginUrl | this HttpRequest item, bool appendForwardSlash = true | string
GetFormIntOrNull | this HttpRequest item, string name | int?
GetFormValue | this HttpRequest item, string name | string
GetFormValueNotNull | this HttpRequest item, string name | string

## Label

Name | Parameters | Returns
--- | --- | ---
HasError | this Label item, out bool isFormInvalid, string text = null | void

## ListItemCollection

Name | Parameters | Returns
--- | --- | ---
AddRange | this ListItemCollection item, IEnumerable<ListItem> collection | void
ToEnumerable | this ListItemCollection item | IEnumerable<ListItem>

## Session

Name | Parameters | Returns
--- | --- | ---
Get<T\> | this HttpSessionState item, string key | T
Set<T\> | this HttpSessionState item, string key, T value | void

## String

Name | Parameters | Returns
--- | --- | ---
ConnectionString | this string item | string
ConnectionStringSettings | this string item | ConnectionStringSettings

## TextBox

Name | Parameters | Returns
--- | --- | ---
ToIntOrNull | this TextBox item | int?
ToTimeSpanOrNull | this TextBox item, string format = "h:mm tt" | TimeSpan?

# Remarks

The **.AddRange** and **.ToEnumerable** methods are intended to be used in conjunction with the Items property of a DropDownList, CheckBoxList, etc...

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe:

https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
