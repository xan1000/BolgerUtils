# BolgerUtils.Framework.EntityFramework

BolgerUtils.Framework.EntityFramework is a .NET Framework 4.8 library which provides various reusable (and sometimes useful) methods aimed for use in a EntityFramework project. This functionality comes in the form of extension methods available when the BolgerUtils.Framework.EntityFramework namespace is imported via:

######

```csharp
using BolgerUtils.Framework.EntityFramework;
```

# Extension methods

## DbContext

Name | Parameters | Returns
--- | --- | ---
LogSql | this DbContext item | void
PropertyMaximumLength | this DbContext item, string entitySet, string property | int

## String

Name | Parameters | Returns
--- | --- | ---
ToSqlConnectionStringBuilder | this string item | SqlConnectionStringBuilder

# Remarks

LogSql writes the SQL EntityFramework runs to System.Diagnostics.Debug.
