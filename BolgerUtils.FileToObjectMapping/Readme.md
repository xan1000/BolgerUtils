# BolgerUtils.FileToObjectMapping

BolgerUtils.FileToObjectMapping is a .NET Standard 2.1 library which provides a means to parse a files content and cache the result. If the file is modified the change will be detected with its updated content used. The **BolgerUtils.FileToObjectMapping.FileToObject** class implements this functionality and static methods from the **BolgerUtils.FileToObjectMapping.FileToObjectUtils** class are available to streamline its usage.

These classes are available when the **BolgerUtils.FileToObjectMapping** namespace is imported via:

######

```csharp
using BolgerUtils.FileToObjectMapping;
```

# FileToObject class

## Constructors

Name | Parameters
--- | ---
FileToObject | void

## Methods

Name | Parameters | Returns
--- | --- | ---
Clear | void | void
IsRegistered | string path | bool
Load<T\> | string path | T
Map<T\> | string path, Func<string, T\> fileContentToObject | T
Register<T\> where T : class | string path, Func<string, T\> fileContentToObject | void
UnMap | string path | bool
UnRegister | string path | bool

# FileToObjectUtils static class

## Methods

Name | Parameters | Returns
--- | --- | ---
IsRegistered | string path | bool
Load<T\> | string path | T
Map<T\> | string path, Func<string, T\> fileContentToObject | T
Register<T\> where T : class | string path, Func<string, T\> fileContentToObject | void
UnMap | string path | bool
UnRegister | string path | bool

# Remarks

The recommended entry point is to use the **FileToObjectUtils.Map** method and/or **FileToObjectUtils.Register** & **FileToObjectUtils.Load** methods.

# Examples

## Reading a text file

**test.txt** file:

######

```
Hello World
Line 1
Line 2
Line 3
```

### Using Map method

######

```csharp
// Note content type is string in this context (i.e., the file's content unchanged).
var content = FileToObjectUtils.Map("test.txt", x => x);

// Output is:
// Hello World
// Line 1
// Line 2
// Line 3
Console.WriteLine(content);
```

Loading the file again:

######

```csharp
var content = FileToObjectUtils.Map("test.txt", x => x);

// Output is the same as above.
Console.WriteLine(content);
```

Note the file is only opened and parsed if its content has changed, otherwise an in-memory cached object loaded from a previous run is returned.

### Using Register & Load methods

If you don't want to repeat the processing logic passed as the 2nd parameter to **.Map** this can be done by using the **.Register** & **.Load** methods.

######

```csharp
FileToObjectUtils.Register("test.txt", x => x);
```

######

```csharp
var content = FileToObjectUtils.Load<string>("test.txt");

// Output is:
// Hello World
// Line 1
// Line 2
// Line 3
Console.WriteLine(content);
```

Loading the file again:

######

```csharp
var content = FileToObjectUtils.Load<string>("test.txt");
```

## Reading a json file (using Newtonsoft.Json)

Model classes:

######

```csharp
public class Account
{
    public string Bsb { get; set; }
    public string AccountNumber { get; set; }
    public Person Owner { get; set; }
    public decimal Balance { get; set; }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

**test.json** file:

######

```json
[
  {
    "Bsb": "123456",
    "AccountNumber": "12345678",
    "Owner": {
      "FirstName": "Alice",
      "LastName": "Test"
    },
    "Balance": 100.00
  },
  {
    "Bsb": "654321",
    "AccountNumber": "87654321",
    "Owner": {
      "FirstName": "Bob",
      "LastName": "Test"
    },
    "Balance": 500.95
  }
]
```

### Using Map method

######

```csharp
// Note accounts type is List<Account> in this context.
var accounts = FileToObjectUtils.Map("test.json", JsonConvert.DeserializeObject<List<Account>>);

// Output is:
// Bsb: 123456, Account number: 12345678
// Owner: Alice Test
// Balance: $100.00
//
// Bsb: 654321, Account number: 87654321
// Owner: Bob Test
// Balance: $500.95
foreach(var x in accounts)
{
    Console.WriteLine($"Bsb: {x.Bsb}, Account number: {x.AccountNumber}");
    Console.WriteLine($"Owner: {x.Owner.FirstName} {x.Owner.LastName}");
    Console.WriteLine($"Balance: {x.Balance:C}");
    Console.WriteLine();
}
```

Loading the file again:

######

```csharp
var accounts = FileToObjectUtils.Map("test.json", JsonConvert.DeserializeObject<List<Account>>);

// Output is the same as above.
foreach(var x in accounts)
{
    Console.WriteLine($"Bsb: {x.Bsb}, Account number: {x.AccountNumber}");
    Console.WriteLine($"Owner: {x.Owner.FirstName} {x.Owner.LastName}");
    Console.WriteLine($"Balance: {x.Balance:C}");
    Console.WriteLine();
}
```

Again the file is only opened and parsed if its content has changed, otherwise an in-memory cached object loaded from a previous run is returned.

### Using Register & Load methods

Again if you don't want to repeat the processing logic passed as the 2nd parameter to **.Map** this can be done by using the **.Register** & **.Load** methods.

######

```csharp
FileToObjectUtils.Register("test.json", JsonConvert.DeserializeObject<List<Account>>);
```

######

```csharp
var accounts = FileToObjectUtils.Load<List<Account>>("test.json");

// Output is:
// Bsb: 123456, Account number: 12345678
// Owner: Alice Test
// Balance: $100.00
//
// Bsb: 654321, Account number: 87654321
// Owner: Bob Test
// Balance: $500.95
foreach(var x in accounts)
{
    Console.WriteLine($"Bsb: {x.Bsb}, Account number: {x.AccountNumber}");
    Console.WriteLine($"Owner: {x.Owner.FirstName} {x.Owner.LastName}");
    Console.WriteLine($"Balance: {x.Balance:C}");
    Console.WriteLine();
}
```

Loading the file again:

######

```csharp
var accounts = FileToObjectUtils.Load<List<Account>>("test.json");
```

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe:

https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
