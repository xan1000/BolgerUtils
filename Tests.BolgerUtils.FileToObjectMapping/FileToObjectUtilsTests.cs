using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BolgerUtils;
using BolgerUtils.FileToObjectMapping;
using Newtonsoft.Json;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests.BolgerUtils.FileToObjectMapping
{
    public class Account : IEquatable<Account>
    {
        public Account(string accountNumber, string ownerName, decimal balance)
        {
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            Balance = balance;
        }

        // ReSharper disable MemberCanBePrivate.Global
        public string AccountNumber { get; }
        public string OwnerName { get; }
        public decimal Balance { get; }
        // ReSharper restore MemberCanBePrivate.Global

        public bool Equals(Account account) => account != null && AccountNumber == account.AccountNumber &&
            OwnerName == account.OwnerName && Balance == account.Balance;
    }

    public class FileToObjectTests
    {
        public const string TextFilePath = "test.txt";
        public const string JsonFilePath = "accounts.json";

        private readonly FileToObject _fileToObject = new FileToObject();
        public string _textFileContent { get; private set; }
        public List<Account> _jsonFileContent { get; private set; }

        public FileToObjectTests()
        {
            CreateTextFile();
            CreateJsonFile();
        }

        private void CreateJsonFile() => UpdateJsonFileContent(new List<Account>
            {
                new Account("1234 5678", "Test 1", 500.50m),
                new Account("9876 5432", "Test 2", 1000),
                new Account("4444 4444", "Test 3", 0)
            });

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
            _jsonFileContent = content;
            File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(_jsonFileContent));
        }

        private void UpdateTextFileContent(string content)
        {
            _textFileContent = content;
            File.WriteAllText(TextFilePath, _textFileContent);
        }

        [Fact]
        public void Test_Clear()
        {
            _fileToObject.Clear();

            _fileToObject.Register(TextFilePath, x => x);
            Assert.True(_fileToObject.IsRegistered(TextFilePath));
            Assert.Equal(_textFileContent, _fileToObject.Load<string>(TextFilePath));
            Assert.Equal(_jsonFileContent,
                _fileToObject.Map(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));

            _fileToObject.Clear();
            Assert.False(_fileToObject.IsRegistered(TextFilePath));
            Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(TextFilePath));
            Assert.False(_fileToObject.UnMap(JsonFilePath));

            _fileToObject.Register(TextFilePath, x => x);
            Assert.True(_fileToObject.IsRegistered(TextFilePath));
            Assert.Equal(_textFileContent, _fileToObject.Load<string>(TextFilePath));
            Assert.Equal(_jsonFileContent,
                _fileToObject.Map(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));
        }

        [Fact]
        public void Test_IsRegisteredAndRegisterAndUnRegister()
        {
            Assert.False(_fileToObject.IsRegistered(TextFilePath));
            Assert.False(_fileToObject.IsRegistered(JsonFilePath));

            _fileToObject.Register(TextFilePath, x => x);
            Assert.True(_fileToObject.IsRegistered(TextFilePath));
            Assert.False(_fileToObject.IsRegistered(JsonFilePath));

            Assert.Throws<ArgumentException>(() => _fileToObject.Register(TextFilePath, x => x));

            _fileToObject.Register(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>);
            Assert.True(_fileToObject.IsRegistered(TextFilePath));
            Assert.True(_fileToObject.IsRegistered(JsonFilePath));

            Assert.Throws<ArgumentException>(() =>
                _fileToObject.Register(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));

            Assert.True(_fileToObject.UnRegister(TextFilePath));
            Assert.False(_fileToObject.UnRegister(TextFilePath));
            Assert.False(_fileToObject.IsRegistered(TextFilePath));
            Assert.True(_fileToObject.IsRegistered(JsonFilePath));

            _fileToObject.Register(TextFilePath, x => x);
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
            // ReSharper disable once ConvertToLocalFunction
            Func<string, string> textFileToObject = x => x;
            Func<string, List<Account>> jsonFileToObject = JsonConvert.DeserializeObject<List<Account>>;

            Assert.Equal(_textFileContent, _fileToObject.Map(TextFilePath, textFileToObject));
            Assert.IsType<string>(_fileToObject.Map(TextFilePath, textFileToObject));

            Assert.Equal(_jsonFileContent, _fileToObject.Map(JsonFilePath, jsonFileToObject));
            Assert.IsType<List<Account>>(_fileToObject.Map(JsonFilePath, jsonFileToObject));

            Assert.Equal(_textFileContent, _fileToObject.Map(TextFilePath, textFileToObject));
            Assert.IsType<string>(_fileToObject.Map(TextFilePath, textFileToObject));

            var content = _textFileContent.Substring(_textFileContent.IndexOf(Utils.NewLineChar));
            Assert.NotEqual(content, _fileToObject.Map(TextFilePath, textFileToObject));

            var list = _jsonFileContent.ToList();
            list.RemoveAt(0);
            Assert.NotEqual(list, _fileToObject.Map(JsonFilePath, jsonFileToObject));

            Assert.Throws<InvalidCastException>(() => _fileToObject.Map(TextFilePath, jsonFileToObject));
            Assert.Throws<InvalidCastException>(() => _fileToObject.Map(JsonFilePath, textFileToObject));
        }

        [Fact]
        public void Test_RegisterAndLoad()
        {
            Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(TextFilePath));
            Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(JsonFilePath));

            _fileToObject.Register(TextFilePath, x => x);

            Assert.Equal(_textFileContent, _fileToObject.Load<string>(TextFilePath));
            Assert.IsType<string>(_fileToObject.Load<string>(TextFilePath));

            Assert.Throws<KeyNotFoundException>(() => _fileToObject.Load<string>(JsonFilePath));

            _fileToObject.Register(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>);

            Assert.Equal(_jsonFileContent, _fileToObject.Load<List<Account>>(JsonFilePath));
            Assert.IsType<List<Account>>(_fileToObject.Load<List<Account>>(JsonFilePath));

            Assert.Equal(_textFileContent, _fileToObject.Load<string>(TextFilePath));
            Assert.IsType<string>(_fileToObject.Load<string>(TextFilePath));

            var content = _textFileContent.Substring(_textFileContent.IndexOf(Utils.NewLineChar));
            Assert.NotEqual(content, _fileToObject.Load<string>(TextFilePath));

            var list = _jsonFileContent.ToList();
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

            Assert.Equal(_textFileContent, _fileToObject.Map(TextFilePath, x => x));
            Assert.False(_fileToObject.UnMap(JsonFilePath));

            Assert.Equal(_jsonFileContent,
                _fileToObject.Map(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));

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

        private readonly FileToObjectTests _fileToObjectTests = new FileToObjectTests();
        private string TextFileContent => _fileToObjectTests._textFileContent;
        private List<Account> JsonFileContent => _fileToObjectTests._jsonFileContent;

        public FileToObjectUtilsTests() => FileToObjectUtils.FileToObject.Clear();

        [Fact]
        public void Test_IsRegisteredAndRegisterAndUnRegister()
        {
            Assert.False(FileToObjectUtils.IsRegistered(TextFilePath));
            Assert.False(FileToObjectUtils.IsRegistered(JsonFilePath));

            FileToObjectUtils.Register(TextFilePath, x => x);
            Assert.True(FileToObjectUtils.IsRegistered(TextFilePath));
            Assert.False(FileToObjectUtils.IsRegistered(JsonFilePath));

            Assert.Throws<ArgumentException>(() => FileToObjectUtils.Register(TextFilePath, x => x));

            FileToObjectUtils.Register(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>);
            Assert.True(FileToObjectUtils.IsRegistered(TextFilePath));
            Assert.True(FileToObjectUtils.IsRegistered(JsonFilePath));

            Assert.Throws<ArgumentException>(() => FileToObjectUtils.Register(
                JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));

            Assert.True(FileToObjectUtils.UnRegister(TextFilePath));
            Assert.False(FileToObjectUtils.UnRegister(TextFilePath));
            Assert.False(FileToObjectUtils.IsRegistered(TextFilePath));
            Assert.True(FileToObjectUtils.IsRegistered(JsonFilePath));

            FileToObjectUtils.Register(TextFilePath, x => x);
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
            // ReSharper disable once ConvertToLocalFunction
            Func<string, string> textFileToObject = x => x;
            Func<string, List<Account>> jsonFileToObject = JsonConvert.DeserializeObject<List<Account>>;

            Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, textFileToObject));
            Assert.IsType<string>(FileToObjectUtils.Map(TextFilePath, textFileToObject));

            Assert.Equal(JsonFileContent, FileToObjectUtils.Map(JsonFilePath, jsonFileToObject));
            Assert.IsType<List<Account>>(FileToObjectUtils.Map(JsonFilePath, jsonFileToObject));

            Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, textFileToObject));
            Assert.IsType<string>(FileToObjectUtils.Map(TextFilePath, textFileToObject));

            var content = TextFileContent.Substring(TextFileContent.IndexOf(Utils.NewLineChar));
            Assert.NotEqual(content, FileToObjectUtils.Map(TextFilePath, textFileToObject));

            var list = JsonFileContent.ToList();
            list.RemoveAt(0);
            Assert.NotEqual(list, FileToObjectUtils.Map(JsonFilePath, jsonFileToObject));

            Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Map(TextFilePath, jsonFileToObject));
            Assert.Throws<InvalidCastException>(() => FileToObjectUtils.Map(JsonFilePath, textFileToObject));
        }

        [Fact]
        public void Test_RegisterAndLoad()
        {
            Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(TextFilePath));
            Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(JsonFilePath));

            FileToObjectUtils.Register(TextFilePath, x => x);

            Assert.Equal(TextFileContent, FileToObjectUtils.Load<string>(TextFilePath));
            Assert.IsType<string>(FileToObjectUtils.Load<string>(TextFilePath));

            Assert.Throws<KeyNotFoundException>(() => FileToObjectUtils.Load<string>(JsonFilePath));

            FileToObjectUtils.Register(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>);

            Assert.Equal(JsonFileContent, FileToObjectUtils.Load<List<Account>>(JsonFilePath));
            Assert.IsType<List<Account>>(FileToObjectUtils.Load<List<Account>>(JsonFilePath));

            Assert.Equal(TextFileContent, FileToObjectUtils.Load<string>(TextFilePath));
            Assert.IsType<string>(FileToObjectUtils.Load<string>(TextFilePath));

            var content = TextFileContent.Substring(TextFileContent.IndexOf(Utils.NewLineChar));
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

            Assert.Equal(TextFileContent, FileToObjectUtils.Map(TextFilePath, x => x));
            Assert.False(FileToObjectUtils.UnMap(JsonFilePath));

            Assert.Equal(JsonFileContent,
                FileToObjectUtils.Map(JsonFilePath, JsonConvert.DeserializeObject<List<Account>>));

            Assert.True(FileToObjectUtils.UnMap(TextFilePath));
            Assert.False(FileToObjectUtils.UnMap(TextFilePath));
            Assert.True(FileToObjectUtils.UnMap(JsonFilePath));
            Assert.False(FileToObjectUtils.UnMap(JsonFilePath));
        }
    }
}
