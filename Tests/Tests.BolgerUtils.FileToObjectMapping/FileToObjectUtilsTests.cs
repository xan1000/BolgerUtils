using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BolgerUtils;
using BolgerUtils.FileToObjectMapping;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests.BolgerUtils.FileToObjectMapping;

public class Account(string accountNumber, string ownerName, decimal balance) : IEquatable<Account>
{
    // ReSharper disable MemberCanBePrivate.Global
    public string AccountNumber { get; } = accountNumber;
    public string OwnerName { get; } = ownerName;
    public decimal Balance { get; } = balance;
    // ReSharper restore MemberCanBePrivate.Global

    public bool Equals(Account account)
    {
        return account != null &&
            AccountNumber == account.AccountNumber && OwnerName == account.OwnerName && Balance == account.Balance;
    }
}

public class FileToObjectTests
{
    public const string TextFilePath = "test.txt";
    public const string JsonFilePath = "accounts.json";

    private readonly FileToObject _fileToObject = new();

    public Func<string, string> TextFileToObject { get; } = x => x;
    public Func<string, List<Account>> JsonFileToObject { get; }= JsonConvert.DeserializeObject<List<Account>>;

    public string TextFileContent { get; private set; }
    public List<Account> JsonFileContent { get; private set; }

    private readonly ITestOutputHelper _output;

    public FileToObjectTests(ITestOutputHelper output = null)
    {
        _output = output;
        CreateTextFile();
        CreateJsonFile();
    }

    private void CreateJsonFile() =>
        UpdateJsonFileContent([
            new Account("1234 5678", "Test 1", 500.50m),
            new Account("9876 5432", "Test 2", 1000),
            new Account("4444 4444", "Test 3", 0)
        ]);

    private void CreateTextFile()
    {
        const string content =
            "Line 1\n" +
            "Line 2\n" +
            "Line 3\n\n" +
            "Hello   World";

        UpdateTextFileContent(content);
    }

    private void UpdateJsonFileContent(List<Account> content)
    {
        JsonFileContent = content;
        File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(JsonFileContent));
    }

    private void UpdateTextFileContent(string content)
    {
        TextFileContent = content;
        File.WriteAllText(TextFilePath, TextFileContent);
    }

    [Fact]
    public void Test_Clear()
    {
        _fileToObject.Clear();

        _fileToObject.Register(TextFilePath, TextFileToObject);
        Assert.True(_fileToObject.IsRegistered(TextFilePath));
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        _fileToObject.Clear();
        Assert.False(_fileToObject.IsRegistered(TextFilePath));
        Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(TextFilePath));
        Assert.False(_fileToObject.UnMap(JsonFilePath));

        _fileToObject.Register(TextFilePath, TextFileToObject);
        Assert.True(_fileToObject.IsRegistered(TextFilePath));
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));
    }

    [Fact]
    public void Test_IsRegisteredAndRegisterAndUnRegister()
    {
        Assert.False(_fileToObject.IsRegistered(TextFilePath));
        Assert.False(_fileToObject.IsRegistered(JsonFilePath));

        _fileToObject.Register(TextFilePath, TextFileToObject);
        Assert.True(_fileToObject.IsRegistered(TextFilePath));
        Assert.False(_fileToObject.IsRegistered(JsonFilePath));

        Assert.Throws<ArgumentException>(() => _fileToObject.Register(TextFilePath, TextFileToObject));

        _fileToObject.Register(JsonFilePath, JsonFileToObject);
        Assert.True(_fileToObject.IsRegistered(TextFilePath));
        Assert.True(_fileToObject.IsRegistered(JsonFilePath));

        Assert.Throws<ArgumentException>(() => _fileToObject.Register(JsonFilePath, JsonFileToObject));

        Assert.True(_fileToObject.UnRegister(TextFilePath));
        Assert.False(_fileToObject.UnRegister(TextFilePath));
        Assert.False(_fileToObject.IsRegistered(TextFilePath));
        Assert.True(_fileToObject.IsRegistered(JsonFilePath));

        _fileToObject.Register(TextFilePath, TextFileToObject);
        Assert.True(_fileToObject.IsRegistered(TextFilePath));
        Assert.True(_fileToObject.IsRegistered(JsonFilePath));

        Assert.True(_fileToObject.UnRegister(TextFilePath));
        Assert.True(_fileToObject.UnRegister(JsonFilePath));
        Assert.False(_fileToObject.IsRegistered(TextFilePath));
        Assert.False(_fileToObject.IsRegistered(JsonFilePath));
    }

    [Fact]
    public void Test_Map()
    {
        Assert.Equal(TextFileContent, _fileToObject.Map(TextFilePath, TextFileToObject));
        Assert.IsType<string>(_fileToObject.Map(TextFilePath, TextFileToObject));

        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));
        Assert.IsType<List<Account>>(_fileToObject.Map(JsonFilePath, JsonFileToObject));

        Assert.Equal(TextFileContent, _fileToObject.Map(TextFilePath, TextFileToObject));
        Assert.IsType<string>(_fileToObject.Map(TextFilePath, TextFileToObject));

        var content = TextFileContent[TextFileContent.IndexOf(Utils.NewLineChar)..];
        Assert.NotEqual(content, _fileToObject.Map(TextFilePath, TextFileToObject));

        var list = JsonFileContent.ToList();
        list.RemoveAt(0);
        Assert.NotEqual(list, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        Assert.Throws<InvalidCastException>(() => _fileToObject.Map(TextFilePath, JsonFileToObject));
        Assert.Throws<InvalidCastException>(() => _fileToObject.Map(JsonFilePath, TextFileToObject));
    }

    [Fact]
    public void Test_ModifyingFiles()
    {
        _fileToObject.Register(TextFilePath, TextFileToObject);
        _fileToObject.Register(JsonFilePath, JsonFileToObject);
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        var content = TextFileContent + "\nTest\n";
        Assert.NotEqual(content, _fileToObject.Load<string>(TextFilePath));
        Assert.NotEqual(content, TextFileContent);
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        UpdateTextFileContent(content);
        Assert.Equal(content, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(content, TextFileContent);
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        var list = JsonFileContent.ToList();
        list.Add(new Account("5555 5555", "Test 4", -500.95m));
        Assert.NotEqual(list, _fileToObject.Map(JsonFilePath, JsonFileToObject));
        Assert.NotEqual(list, JsonFileContent);
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));

        UpdateJsonFileContent(list);
        Assert.Equal(list, _fileToObject.Map(JsonFilePath, JsonFileToObject));
        Assert.Equal(list, JsonFileContent);
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));

        content = TextFileContent.Split(Utils.NewLineChar)[0];
        Assert.NotEqual(content, _fileToObject.Load<string>(TextFilePath));
        Assert.NotEqual(content, TextFileContent);
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        UpdateTextFileContent(content);
        Assert.Equal(content, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(content, TextFileContent);
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        list = JsonFileContent.ToList();
        list.RemoveAll(list.Skip(1));
        Assert.NotEqual(list, _fileToObject.Map(JsonFilePath, JsonFileToObject));
        Assert.NotEqual(list, JsonFileContent);
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));

        UpdateJsonFileContent(list);
        Assert.Equal(list, _fileToObject.Map(JsonFilePath, JsonFileToObject));
        Assert.Equal(list, JsonFileContent);
        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));

        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    public void Test_Performance(int fileContentMultiplier)
    {
        const int loopCount = 10 * Utils.Thousand;

        var content = TextFileContent;
        var list = JsonFileContent.ToList();
        for(var i = 0; i < fileContentMultiplier; i++)
        {
            content += TextFileContent;
            list.AddRange(JsonFileContent);
        }
        UpdateTextFileContent(content);
        UpdateJsonFileContent(list);

        var stopwatch = new Stopwatch();
        for(var i = 0; i < loopCount; i++)
        {
            stopwatch.Start();
            content = File.ReadAllText(TextFilePath);
            list = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(JsonFilePath));
            stopwatch.Stop();

            Assert.Equal(TextFileContent, content);
            Assert.Equal(JsonFileContent, list);
        }

        var rawTimeSpan = stopwatch.Elapsed;

        _fileToObject.Register(TextFilePath, TextFileToObject);
        _fileToObject.Register(JsonFilePath, JsonFileToObject);
        stopwatch.Reset();
        for(var i = 0; i < loopCount; i++)
        {
            stopwatch.Start();
            content = _fileToObject.Load<string>(TextFilePath);
            list = _fileToObject.Load<List<Account>>(JsonFilePath);
            stopwatch.Stop();

            Assert.Equal(TextFileContent, content);
            Assert.Equal(JsonFileContent, list);
        }

        var fileToObjectTimeSpan = stopwatch.Elapsed;


        Display("Raw Time", rawTimeSpan);
        Display("File to Object Time", fileToObjectTimeSpan);

        Assert.True(fileToObjectTimeSpan < rawTimeSpan);

        Display("Difference", rawTimeSpan.Subtract(fileToObjectTimeSpan));

        const int columnLength = -19;

        var percentage = fileToObjectTimeSpan.TotalSeconds / rawTimeSpan.TotalSeconds * 100;
        _output.WriteLine($"{"Percentage Runtime",columnLength}: {ToPercentageString(percentage)}%");
        _output.WriteLine($"{"Percentage Faster",columnLength}: {ToPercentageString(100 / percentage * 100)}%");

        return;

        void Display(string title, TimeSpan timeSpan) =>
            _output.WriteLine($"{title,columnLength}: {ToPercentageString(timeSpan.TotalSeconds)}s");

        static string ToPercentageString(double value) => $"{Math.Round(value, 2):N}";
    }

    [Fact]
    public void Test_RegisterAndLoad()
    {
        Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(TextFilePath));
        Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(JsonFilePath));

        _fileToObject.Register(TextFilePath, TextFileToObject);

        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.IsType<string>(_fileToObject.Load<string>(TextFilePath));

        Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(JsonFilePath));

        _fileToObject.Register(JsonFilePath, JsonFileToObject);

        Assert.Equal(JsonFileContent, _fileToObject.Load<List<Account>>(JsonFilePath));
        Assert.IsType<List<Account>>(_fileToObject.Load<List<Account>>(JsonFilePath));

        Assert.Equal(TextFileContent, _fileToObject.Load<string>(TextFilePath));
        Assert.IsType<string>(_fileToObject.Load<string>(TextFilePath));

        var content = TextFileContent[TextFileContent.IndexOf(Utils.NewLineChar)..];
        Assert.NotEqual(content, _fileToObject.Load<string>(TextFilePath));

        var list = JsonFileContent.ToList();
        list.RemoveAt(0);
        Assert.NotEqual(list, _fileToObject.Load<List<Account>>(JsonFilePath));

        Assert.Throws<InvalidCastException>(() => _fileToObject.Load<List<Account>>(TextFilePath));
        Assert.Throws<InvalidCastException>(() => _fileToObject.Load<string>(JsonFilePath));
    }

    [Fact]
    public void Test_UnMap()
    {
        Assert.False(_fileToObject.UnMap(TextFilePath));
        Assert.False(_fileToObject.UnMap(JsonFilePath));

        Assert.Equal(TextFileContent, _fileToObject.Map(TextFilePath, TextFileToObject));
        Assert.False(_fileToObject.UnMap(JsonFilePath));

        Assert.Equal(JsonFileContent, _fileToObject.Map(JsonFilePath, JsonFileToObject));

        Assert.True(_fileToObject.UnMap(TextFilePath));
        Assert.False(_fileToObject.UnMap(TextFilePath));
        Assert.True(_fileToObject.UnMap(JsonFilePath));
        Assert.False(_fileToObject.UnMap(JsonFilePath));
    }
}

public class FileToObjectUtilsTests
{
    private const string TextFilePath = FileToObjectTests.TextFilePath;
    private const string JsonFilePath = FileToObjectTests.JsonFilePath;

    private readonly FileToObjectTests _fileToObjectTests = new();

    private Func<string, string> TextFileToObject { get; }
    private Func<string, List<Account>> JsonFileToObject { get; }

    private string TextFileContent => _fileToObjectTests.TextFileContent;
    private List<Account> JsonFileContent => _fileToObjectTests.JsonFileContent;

    public FileToObjectUtilsTests()
    {
        TextFileToObject = _fileToObjectTests.TextFileToObject;
        JsonFileToObject = _fileToObjectTests.JsonFileToObject;
        FileToObjectUtils.FileToObject.Clear();
    }

    [Fact]
    public void Test_IsRegisteredAndRegisterAndUnRegister()
    {
        Assert.False(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.False(FileToObjectUtils.IsRegistered(JsonFilePath));

        FileToObjectUtils.Register(TextFilePath, TextFileToObject);
        Assert.True(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.False(FileToObjectUtils.IsRegistered(JsonFilePath));

        Assert.Throws<ArgumentException>(() => FileToObjectUtils.Register(TextFilePath, TextFileToObject));

        FileToObjectUtils.Register(JsonFilePath, JsonFileToObject);
        Assert.True(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.True(FileToObjectUtils.IsRegistered(JsonFilePath));

        Assert.Throws<ArgumentException>(() => FileToObjectUtils.Register(JsonFilePath, JsonFileToObject));

        Assert.True(FileToObjectUtils.UnRegister(TextFilePath));
        Assert.False(FileToObjectUtils.UnRegister(TextFilePath));
        Assert.False(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.True(FileToObjectUtils.IsRegistered(JsonFilePath));

        FileToObjectUtils.Register(TextFilePath, TextFileToObject);
        Assert.True(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.True(FileToObjectUtils.IsRegistered(JsonFilePath));

        Assert.True(FileToObjectUtils.UnRegister(TextFilePath));
        Assert.True(FileToObjectUtils.UnRegister(JsonFilePath));
        Assert.False(FileToObjectUtils.IsRegistered(TextFilePath));
        Assert.False(FileToObjectUtils.IsRegistered(JsonFilePath));
    }

    [Fact]
    public void Test_Map()
    {
        Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, TextFileToObject));
        Assert.IsType<string>(FileToObjectUtils.Map(TextFilePath, TextFileToObject));

        Assert.Equal(JsonFileContent, FileToObjectUtils.Map(JsonFilePath, JsonFileToObject));
        Assert.IsType<List<Account>>(FileToObjectUtils.Map(JsonFilePath, JsonFileToObject));

        Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, TextFileToObject));
        Assert.IsType<string>(FileToObjectUtils.Map(TextFilePath, TextFileToObject));

        var content = TextFileContent[TextFileContent.IndexOf(Utils.NewLineChar)..];
        Assert.NotEqual(content, FileToObjectUtils.Map(TextFilePath, TextFileToObject));

        var list = JsonFileContent.ToList();
        list.RemoveAt(0);
        Assert.NotEqual(list, FileToObjectUtils.Map(JsonFilePath, JsonFileToObject));

        Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Map(TextFilePath, JsonFileToObject));
        Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Map(JsonFilePath, TextFileToObject));
    }

    [Fact]
    public void Test_RegisterAndLoad()
    {
        Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(TextFilePath));
        Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(JsonFilePath));

        FileToObjectUtils.Register(TextFilePath, TextFileToObject);

        Assert.Equal(TextFileContent, FileToObjectUtils.Load<string>(TextFilePath));
        Assert.IsType<string>(FileToObjectUtils.Load<string>(TextFilePath));

        Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(JsonFilePath));

        FileToObjectUtils.Register(JsonFilePath, JsonFileToObject);

        Assert.Equal(JsonFileContent, FileToObjectUtils.Load<List<Account>>(JsonFilePath));
        Assert.IsType<List<Account>>(FileToObjectUtils.Load<List<Account>>(JsonFilePath));

        Assert.Equal(TextFileContent, FileToObjectUtils.Load<string>(TextFilePath));
        Assert.IsType<string>(FileToObjectUtils.Load<string>(TextFilePath));

        var content = TextFileContent[TextFileContent.IndexOf(Utils.NewLineChar)..];
        Assert.NotEqual(content, FileToObjectUtils.Load<string>(TextFilePath));

        var list = JsonFileContent.ToList();
        list.RemoveAt(0);
        Assert.NotEqual(list, FileToObjectUtils.Load<List<Account>>(JsonFilePath));

        Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Load<List<Account>>(TextFilePath));
        Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Load<string>(JsonFilePath));
    }

    [Fact]
    public void Test_UnMap()
    {
        Assert.False(FileToObjectUtils.UnMap(TextFilePath));
        Assert.False(FileToObjectUtils.UnMap(JsonFilePath));

        Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, TextFileToObject));
        Assert.False(FileToObjectUtils.UnMap(JsonFilePath));

        Assert.Equal(JsonFileContent, FileToObjectUtils.Map(JsonFilePath, JsonFileToObject));

        Assert.True(FileToObjectUtils.UnMap(TextFilePath));
        Assert.False(FileToObjectUtils.UnMap(TextFilePath));
        Assert.True(FileToObjectUtils.UnMap(JsonFilePath));
        Assert.False(FileToObjectUtils.UnMap(JsonFilePath));
    }
}