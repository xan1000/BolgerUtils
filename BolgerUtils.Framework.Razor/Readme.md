# BolgerUtils.Framework.Razor

BolgerUtils.Framework.Razor is a .NET Framework 4.8 library which streamlines how to excute a Razor tempalte. If the template has been executed previously then its cached compiled version is used, additionally if the template is modified whilst the program is running the change will be detected and the updated file will be used. This functionality comes in two forms, as static methods found in the **BolgerUtils.Framework.Razor.Utils** class and by extension methods available when the **BolgerUtils.Framework.Razor** namespace is imported via:

######

```csharp
using BolgerUtils.Framework.Razor;
```

# Utils static class

## Methods

Name | Parameters | Returns
--- | --- | ---
Parse<T\> where T : class | string path, T model = null, DynamicViewBag viewBag = null | string
Parse<T\> where T : class | FileInfo fileInfo, T model = null, DynamicViewBag viewBag = null | string

# Extension methods

## IRazorEngineService

Name | Parameters | Returns
--- | --- | ---
Parse<T\> where T : class | this IRazorEngineService item, string path, T model = null, DynamicViewBag viewBag = null | string
Parse<T\> where T : class | this IRazorEngineService item, FileInfo fileInfo, T model = null, DynamicViewBag viewBag = null | string

## String

Name | Parameters | Returns
--- | --- | ---
ToRawString | this string item | IEncodedString

# Remarks

The recommended entry point is to use the **Utils.Parse** methods.

You can use **.ToRawString()** if content in the template should not be encoded and printed literally.

If the template file is used in a non ASP.NET project to get cshtml syntax highlighting and editor awareness in Visual Studio you can add the **@inherits** directive at the top of the file:

######

```csharp
@inherits TemplateBase<TestModel>
```

Replace **TestModel** with the model class you intend to pass to the template.

# Examples

Model class:

######

```csharp
namespace Tests.BolgerUtils.Framework.Razor.Models
{
    public class TestModel
    {
        public TestModel(int loopCount = 0, string text = null, bool shouldOutputRawString = false)
        {
            LoopCount = loopCount;
            Text = text;
            ShouldOutputRawString = shouldOutputRawString;
        }

        public bool IsDisplayed => LoopCount > 0;
        public int LoopCount { get; }
        public string Text { get; }
        public bool ShouldOutputRawString { get; }
    }
}
```

**_Test.cshtml** template file:

######

```csharp
@using BolgerUtils.Framework.Razor
@using Tests.BolgerUtils.Framework.Razor.Models
@using RazorEngine.Templating
@inherits TemplateBase<TestModel>
<!DOCTYPE html>
<html>
<head>
    <title>Test</title>
</head>
<body>
    <div>
        <header>
            <h1>Test</h1>
        </header>
        
        <main>
            @if(Model.IsDisplayed) {
                <ul>
                    @for(var i = 0; i < Model.LoopCount; i++) {
                        <li>@if(Model.ShouldOutputRawString) { @Model.Text.ToRawString() } else { @Model.Text }</li>
                    }
                </ul>
            }
        </main>
        
        <footer>
            &copy; Copyright 2019, Matthew Bolger
        </footer>
    </div>
</body>
</html>
```

Execute template:

######

```csharp
var model = new TestModel(10, "Hello World");
var output = Utils.Parse("_Test.cshtml", model);
```

# Attribution

The BolgerUtils icon is attributed to Bernadette O'Keefe: https://svg-clipart.com/sign/RsnsoX2-man-carrying-a-big-bag-of-money-clipart
