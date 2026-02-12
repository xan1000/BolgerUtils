using BolgerUtils;
using BolgerUtils.EnumDisplay;
using Xunit;

namespace Tests.BolgerUtils.EnumDisplay;

public enum TestType
{
    Test,
    TestWithManyWords,
    TestCustomValue,
    [EnumDisplay(EnumDisplayUtilsTests.CustomAttributeValue)]
    TestCustomValueWithAttribute
}

public enum TestAnotherType
{
    Test,
    TestAnother,
    TestAnotherCustomValue
}

public class EnumDisplayUtilsTests
{
    public const string CustomAttributeValue = "Custom Attribute";

    private readonly EnumDisplayUtils _enumDisplayUtils = new();

    [Fact]
    public void Test_AddAll()
    {
        Assert.True(Utils.GetEnumValues<TestType>().NotAll(x => _enumDisplayUtils.ContainsKey(x)));
        Assert.True(Utils.GetEnumValues<TestAnotherType>().NotAll(x => _enumDisplayUtils.ContainsKey(x)));

        _enumDisplayUtils.AddAll<TestType>();

        Assert.True(Utils.GetEnumValues<TestType>().All(x => _enumDisplayUtils.ContainsKey(x)));
        Assert.True(Utils.GetEnumValues<TestAnotherType>().NotAll(x => _enumDisplayUtils.ContainsKey(x)));

        _enumDisplayUtils.AddAll<TestAnotherType>();

        Assert.True(Utils.GetEnumValues<TestType>().All(x => _enumDisplayUtils.ContainsKey(x)));
        Assert.True(Utils.GetEnumValues<TestAnotherType>().All(x => _enumDisplayUtils.ContainsKey(x)));
    }

    [Theory]
    [InlineData(TestType.Test)]
    [InlineData(TestType.TestWithManyWords)]
    [InlineData(TestAnotherType.Test)]
    public void Test_AddAndContainsKeyAndDisplay(Enum key)
    {
        Assert.False(_enumDisplayUtils.ContainsKey(key));

        _enumDisplayUtils.Add(key);

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(key), _enumDisplayUtils.Display(key));
    }

    [Theory]
    [InlineData(TestType.TestCustomValueWithAttribute, CustomAttributeValue)]
    public void Test_AddAndDisplayAttributeValue(TestType testType, string value)
    {
        _enumDisplayUtils.Add(testType);

        Assert.Equal(value, _enumDisplayUtils.Display(testType));
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(testType), _enumDisplayUtils.Display(testType));
    }

    [Theory]
    [InlineData(TestType.TestCustomValue, "Custom Value")]
    [InlineData(TestAnotherType.TestAnotherCustomValue, "Another Custom Value")]
    public void Test_AddAndDisplayCustomValue(Enum key, string value)
    {
        _enumDisplayUtils.Add(key, value);

        Assert.Equal(value, _enumDisplayUtils.Display(key));
    }

    [Theory]
    [InlineData(TestType.Test, TestType.TestWithManyWords)]
    [InlineData(TestType.TestWithManyWords, TestType.Test)]
    [InlineData(TestType.Test, TestAnotherType.Test)]
    [InlineData(TestAnotherType.TestAnother, TestType.TestWithManyWords)]
    public void Test_AddDuplicate(Enum key, Enum otherKey)
    {
        Assert.False(_enumDisplayUtils.ContainsKey(key));
        Assert.False(_enumDisplayUtils.ContainsKey(otherKey));

        _enumDisplayUtils.Add(key);
        Assert.Throws<ArgumentException>(() => _enumDisplayUtils.Add(key));

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.False(_enumDisplayUtils.ContainsKey(otherKey));

        _enumDisplayUtils.Add(otherKey);
        Assert.Throws<ArgumentException>(() => _enumDisplayUtils.Add(otherKey));

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.True(_enumDisplayUtils.ContainsKey(otherKey));
    }

    [Theory]
    [InlineData(TestType.Test, TestType.TestWithManyWords)]
    [InlineData(TestType.Test, TestAnotherType.Test)]
    [InlineData(TestAnotherType.Test, TestType.Test)]
    [InlineData(TestAnotherType.Test, TestAnotherType.TestAnother)]
    public void Test_AddMany(Enum key, Enum otherKey)
    {
        Assert.False(_enumDisplayUtils.ContainsKey(key));
        Assert.False(_enumDisplayUtils.ContainsKey(otherKey));

        _enumDisplayUtils.Add(key);

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.False(_enumDisplayUtils.ContainsKey(otherKey));

        _enumDisplayUtils.Add(otherKey);

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.True(_enumDisplayUtils.ContainsKey(otherKey));
    }

    [Theory]
    [InlineData(TestType.Test, TestType.TestWithManyWords)]
    [InlineData(TestType.Test, TestAnotherType.Test)]
    [InlineData(TestAnotherType.Test, TestType.Test)]
    [InlineData(TestAnotherType.Test, TestAnotherType.TestAnother)]
    public void Test_CacheValueOnDisplay(Enum key, Enum otherKey)
    {
        Test_CacheValueOnDisplay_Implementation(key, otherKey, true);
        Test_CacheValueOnDisplay_Implementation(key, otherKey, false);

        // Ensure CacheValueOnDisplay is true by default.
        Test_CacheValueOnDisplay_Implementation(key, otherKey, _enumDisplayUtils, true);
    }

    private void Test_CacheValueOnDisplay_Implementation(Enum key, Enum otherKey, bool cacheValueOnDisplay) =>
        Test_CacheValueOnDisplay_Implementation(key, otherKey, new EnumDisplayUtils(cacheValueOnDisplay),
            cacheValueOnDisplay);

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private void Test_CacheValueOnDisplay_Implementation(
        Enum key, Enum otherKey, EnumDisplayUtils enumDisplayUtils, bool cacheValueOnDisplay)
    {
        Assert.Equal(cacheValueOnDisplay, enumDisplayUtils.CacheValueOnDisplay);
        Assert.False(enumDisplayUtils.ContainsKey(key));
        Assert.False(enumDisplayUtils.ContainsKey(otherKey));

        enumDisplayUtils.Add(key);
        Assert.True(enumDisplayUtils.ContainsKey(key));
        Assert.False(enumDisplayUtils.ContainsKey(otherKey));

        Assert.Equal(EnumDisplayUtils.DefaultDisplay(key), enumDisplayUtils.Display(key));
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(otherKey), enumDisplayUtils.Display(otherKey));

        Assert.True(enumDisplayUtils.ContainsKey(key));
        Assert.Equal(enumDisplayUtils.CacheValueOnDisplay, enumDisplayUtils.ContainsKey(otherKey));
    }

    [Fact]
    public void Test_Clear()
    {
        Assert.True(Utils.GetEnumValues<TestType>().NotAll(x => _enumDisplayUtils.ContainsKey(x)));

        _enumDisplayUtils.AddAll<TestType>();

        Assert.True(Utils.GetEnumValues<TestType>().All(x => _enumDisplayUtils.ContainsKey(x)));

        _enumDisplayUtils.Clear();

        Assert.True(Utils.GetEnumValues<TestType>().NotAll(x => _enumDisplayUtils.ContainsKey(x)));
    }

    [Theory]
    [InlineData(TestType.TestCustomValueWithAttribute, CustomAttributeValue)]
    public void Test_DisplayAttributeValue(TestType testType, string value)
    {
        Assert.Equal(value, _enumDisplayUtils.Display(testType));
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(testType), _enumDisplayUtils.Display(testType));
    }

    [Theory]
    [InlineData(TestType.Test)]
    [InlineData(TestAnotherType.Test)]
    public void Test_DisplayCustomDefault(Enum key)
    {
        Test_DisplayCustomDefault_Implementation(key, x => x.ToString().ToLower());
        Test_DisplayCustomDefault_Implementation(key, x => x.ToString().ToUpper());
        Test_DisplayCustomDefault_Implementation(key, _ => "Constant");
        Test_DisplayCustomDefault_Implementation(key, _ => string.Empty);
        Test_DisplayCustomDefault_Implementation(key, _ => null!);
    }

    private void Test_DisplayCustomDefault_Implementation(Enum key, Func<Enum, string> defaultDisplay)
    {
        var enumDisplayUtils = new EnumDisplayUtils(defaultDisplay: defaultDisplay);
        enumDisplayUtils.Add(key);
        Assert.Equal(defaultDisplay(key), enumDisplayUtils.Display(key));

        enumDisplayUtils = new EnumDisplayUtils(defaultDisplay: defaultDisplay);
        Assert.Equal(defaultDisplay(key), enumDisplayUtils.Display(key));
    }

    [Theory]
    [InlineData(TestType.Test, TestType.TestWithManyWords)]
    [InlineData(TestType.Test, TestAnotherType.Test)]
    [InlineData(TestAnotherType.Test, TestType.Test)]
    [InlineData(TestAnotherType.Test, TestAnotherType.TestAnother)]
    public void Test_Remove(Enum key, Enum otherKey)
    {
        _enumDisplayUtils.Add(key);
        _enumDisplayUtils.Add(otherKey);

        Assert.True(_enumDisplayUtils.ContainsKey(key));
        Assert.True(_enumDisplayUtils.ContainsKey(otherKey));

        Assert.True(_enumDisplayUtils.Remove(key));
        Assert.False(_enumDisplayUtils.Remove(key));
        Assert.False(_enumDisplayUtils.ContainsKey(key));
        Assert.True(_enumDisplayUtils.ContainsKey(otherKey));

        Assert.True(_enumDisplayUtils.Remove(otherKey));
        Assert.False(_enumDisplayUtils.Remove(otherKey));
        Assert.False(_enumDisplayUtils.ContainsKey(key));
        Assert.False(_enumDisplayUtils.ContainsKey(otherKey));
    }
}

public class EnumDisplayExtensionUtilsTests
{
    public EnumDisplayExtensionUtilsTests() => EnumDisplayExtensionUtils.EnumDisplayUtils.Clear();

    [Theory]
    [InlineData(TestType.Test)]
    [InlineData(TestType.TestWithManyWords)]
    [InlineData(TestAnotherType.Test)]
    public void Test_AddAndContainsKeyAndDisplay(Enum key)
    {
        Assert.False(key.ContainsKey());

        key.Add();

        Assert.True(key.ContainsKey());
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(key), key.Display());
    }

    [Theory]
    [InlineData(TestType.TestCustomValueWithAttribute, EnumDisplayUtilsTests.CustomAttributeValue)]
    public void Test_AddAndDisplayAttributeValue(TestType testType, string value)
    {
        testType.Add();

        Assert.Equal(value, testType.Display());
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(testType), testType.Display());
    }

    [Theory]
    [InlineData(TestType.TestCustomValue, "Custom Value")]
    [InlineData(TestAnotherType.TestAnotherCustomValue, "Another Custom Value")]
    public void Test_AddAndDisplayCustomValue(Enum key, string value)
    {
        key.Add(value);

        Assert.Equal(value, key.Display());
    }

    [Fact]
    public void Test_CacheValueOnDisplayIsTrue() =>
        Assert.True(EnumDisplayExtensionUtils.EnumDisplayUtils.CacheValueOnDisplay);

    [Theory]
    [InlineData(TestType.TestCustomValueWithAttribute, EnumDisplayUtilsTests.CustomAttributeValue)]
    public void Test_DisplayAttributeValue(TestType testType, string value)
    {
        Assert.Equal(value, testType.Display());
        Assert.Equal(EnumDisplayUtils.DefaultDisplay(testType), testType.Display());
    }

    [Theory]
    [InlineData(TestType.Test, TestType.TestWithManyWords)]
    [InlineData(TestType.Test, TestAnotherType.Test)]
    [InlineData(TestAnotherType.Test, TestType.Test)]
    [InlineData(TestAnotherType.Test, TestAnotherType.TestAnother)]
    public void Test_Remove(Enum key, Enum otherKey)
    {
        key.Add();
        otherKey.Add();

        Assert.True(key.ContainsKey());
        Assert.True(otherKey.ContainsKey());

        Assert.True(key.Remove());
        Assert.False(key.Remove());
        Assert.False(key.ContainsKey());
        Assert.True(otherKey.ContainsKey());

        Assert.True(otherKey.Remove());
        Assert.False(otherKey.Remove());
        Assert.False(key.ContainsKey());
        Assert.False(otherKey.ContainsKey());
    }
}
