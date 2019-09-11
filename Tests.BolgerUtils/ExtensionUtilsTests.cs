using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using BolgerUtils;
using Xunit;

namespace Tests.BolgerUtils
{
    public class ExtensionUtilsTests
    {
        #region Boolean

        [Theory]
        [InlineData("checked", true)]
        [InlineData("", false)]
        public void CheckedTest(string expected, bool item) => Assert.Equal(expected, item.Checked());

        [Theory]
        [InlineData("disabled", true)]
        [InlineData("", false)]
        public void DisabledTest(string expected, bool item) => Assert.Equal(expected, item.Disabled());

        [Fact]
        public void DisplayFactTest()
        {
            Assert.Equal("World", true.Display("World"));
            Assert.Equal(string.Empty, false.Display("World"));
        }

        [Theory]
        [InlineData("Hello", true, "Hello", "World")]
        [InlineData("World", false, "Hello", "World")]
        public void DisplayTest(string expected, bool item, string trueDisplay, string falseDisplay) =>
            Assert.Equal(expected, item.Display(trueDisplay, falseDisplay));

        [Theory]
        [InlineData("has-error", true)]
        [InlineData("", false)]
        public void HasErrorTest(string expected, bool item) => Assert.Equal(expected, item.HasError());

        [Theory]
        [InlineData("<input type=checkbox disabled checked />", true)]
        [InlineData("<input type=checkbox disabled  />", false)]
        public void ReadOnlyCheckboxTest(string expected, bool item) =>
            Assert.Equal(expected, item.ReadOnlyCheckbox());

        [Theory]
        [InlineData("selected", true)]
        [InlineData("", false)]
        public void SelectedTest(string expected, bool item) => Assert.Equal(expected, item.Selected());

        [Theory]
        [InlineData("Yes", true)]
        [InlineData("No", false)]
        public void YesOrNoTest(string expected, bool item) => Assert.Equal(expected, item.YesOrNo());

        #endregion

        #region Byte
        
        [Theory]
        [InlineData("", new byte[0])]
        [InlineData("0123456789ABCDEF", new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF })]
        [InlineData("05", new byte[] { 0x5 })]
        [InlineData("0102300456", new byte[] { 0x1, 0x02, 0x30, 0x4, 0x56 })]
        public void HexStringTest(string expected, byte[] item) => Assert.Equal(expected, item.ToHexString());

        #endregion

        #region DateTime

        [Fact]
        public void AddWeeksTest()
        {
            var date = DateTime.Today;
            for(var i = -10; i < 10; i++)
            {
                Assert.Equal(date.AddDays(i * Utils.DaysPerWeek), date.AddWeeks(i));
                Assert.Equal(date.AddDays(i * 7), date.AddWeeks(i));
            }
        }

        [Fact]
        public void FindDayOfWeekBackwardTest()
        {
            // 2019 September Monday the 16th.
            var date = new DateTime(2019, 9, 16);
            Assert.Equal(date, date.FindDayOfWeekBackward(DayOfWeek.Monday));
            Assert.Equal(date.AddDays(-1), date.FindDayOfWeekBackward(DayOfWeek.Sunday));
            Assert.Equal(date.AddDays(-2), date.FindDayOfWeekBackward(DayOfWeek.Saturday));
            Assert.Equal(date.AddDays(-3), date.FindDayOfWeekBackward(DayOfWeek.Friday));
            Assert.Equal(date.AddDays(-4), date.FindDayOfWeekBackward(DayOfWeek.Thursday));
            Assert.Equal(date.AddDays(-5), date.FindDayOfWeekBackward(DayOfWeek.Wednesday));
            Assert.Equal(date.AddDays(-6), date.FindDayOfWeekBackward(DayOfWeek.Tuesday));
        }

        [Fact]
        public void FindDayOfWeekForwardTest()
        {
            // 2019 September Monday the 16th.
            var date = new DateTime(2019, 9, 16);
            Assert.Equal(date, date.FindDayOfWeekBackward(DayOfWeek.Monday));
            Assert.Equal(date.AddDays(1), date.FindDayOfWeekForward(DayOfWeek.Tuesday));
            Assert.Equal(date.AddDays(2), date.FindDayOfWeekForward(DayOfWeek.Wednesday));
            Assert.Equal(date.AddDays(3), date.FindDayOfWeekForward(DayOfWeek.Thursday));
            Assert.Equal(date.AddDays(4), date.FindDayOfWeekForward(DayOfWeek.Friday));
            Assert.Equal(date.AddDays(5), date.FindDayOfWeekForward(DayOfWeek.Saturday));
            Assert.Equal(date.AddDays(6), date.FindDayOfWeekForward(DayOfWeek.Sunday));
        }

        [Fact]
        public void YesterdayTest()
        {
            var date = DateTime.Today;
            for(var i = 0; i < 10; i++, date = date.AddDays(1))
            {
                Assert.Equal(date.AddDays(-1), date.Yesterday());
            }
        }

        #endregion

        #region DayOfWeek

        [Fact]
        public void OrderByDayOfWeekStartingOnMondayTest()
        {
            var daysOfWeek = Utils.GetEnumValues<DayOfWeek>().OrderByDayOfWeekStartingOnMonday().ToArray();

            Assert.Equal(DayOfWeek.Monday, daysOfWeek[0]);
            Assert.Equal(DayOfWeek.Sunday, daysOfWeek[daysOfWeek.Length - 1]);
            Assert.Equal(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, daysOfWeek);
        }

        #endregion

        #region Enum

        [Fact]
        public void ToIntTestFact()
        {
            foreach(var x in Utils.GetEnumValues<TestType>())
            {
                Assert.Equal((int) x, x.ToInt());
            }
            foreach(var x in Utils.GetEnumValues<TestAnotherType>())
            {
                Assert.Equal((int) x, x.ToInt());
            }
        }

        [Theory]
        [InlineData(0, TestType.Test1)]
        [InlineData(1, TestType.Test2)]
        [InlineData(2, TestType.Test3)]
        [InlineData(10, TestAnotherType.Test1)]
        [InlineData(20, TestAnotherType.Test2)]
        [InlineData(30, TestAnotherType.Test3)]
        public void ToIntTest(int expected, Enum item) => Assert.Equal(expected, item.ToInt());

        [Fact]
        public void ToValueStringTestFact()
        {
            foreach(var x in Utils.GetEnumValues<TestType>())
            {
                Assert.Equal(((int) x).ToString(), x.ToValueString());
                Assert.Equal(x.ToInt().ToString(), x.ToValueString());
            }
            foreach(var x in Utils.GetEnumValues<TestAnotherType>())
            {
                Assert.Equal(((int) x).ToString(), x.ToValueString());
                Assert.Equal(x.ToInt().ToString(), x.ToValueString());
            }
        }

        [Theory]
        [InlineData("0", TestType.Test1)]
        [InlineData("1", TestType.Test2)]
        [InlineData("2", TestType.Test3)]
        [InlineData("10", TestAnotherType.Test1)]
        [InlineData("20", TestAnotherType.Test2)]
        [InlineData("30", TestAnotherType.Test3)]
        public void ToValueStringTest(string expected, Enum item) => Assert.Equal(expected, item.ToValueString());

        #endregion

        #region Generic

        [Fact]
        public void IsContainedInTest()
        {
            var intValues = new[] { 2, 5, 6, 9 };
            foreach(var item in intValues)
            {
                IsContainedInTestImplementation(true, item, intValues);
            }
            IsContainedInTestImplementation(false, 1, intValues);
            IsContainedInTestImplementation(true, 2, intValues);
            IsContainedInTestImplementation(false, 3, intValues);
            IsContainedInTestImplementation(false, 4, intValues);
            IsContainedInTestImplementation(true, 5, intValues);
            IsContainedInTestImplementation(true, 6, intValues);
            IsContainedInTestImplementation(false, 7, intValues);
            IsContainedInTestImplementation(false, 8, intValues);
            IsContainedInTestImplementation(true, 9, intValues);
            IsContainedInTestImplementation(false, 10, intValues);

            Assert.True(4.IsContainedIn(1, 2, 4));
            Assert.False(3.IsContainedIn(1, 2, 4));

            var stringValues = new[] { "Test", "Hello", "World" };
            foreach(var item in stringValues)
            {
                IsContainedInTestImplementation(true, item, stringValues);
            }
            IsContainedInTestImplementation(true, "Test", stringValues);
            IsContainedInTestImplementation(true, "Hello", stringValues);
            IsContainedInTestImplementation(true, "World", stringValues);
            IsContainedInTestImplementation(false, "Bye", stringValues);

            Assert.True("World".IsContainedIn("Test", "Hello", "World"));
            Assert.False("Bye".IsContainedIn("Test", "Hello", "World"));
        }

        private void IsContainedInTestImplementation<T>(bool expected, T item, T[] values)
        {
            Assert.Equal(expected, item.IsContainedIn(values));
            if(expected)
                Assert.Contains(item, values);
            else
                Assert.DoesNotContain(item, values);
        }

        [Fact]
        public void ScalarToListTest()
        {
            Assert.IsType<List<int>>(10.ScalarToList());
            Assert.IsType<List<string>>("Test".ScalarToList());
            Assert.IsType<List<ExtensionUtilsTests>>(new ExtensionUtilsTests().ScalarToList());
        }

        #endregion

        #region ICollection

        #endregion

        #region IEnumerable

        #endregion

        #region List

        [Fact]
        public void ToHashSetTest()
        {
            Assert.IsType<HashSet<int>>(new List<int>().ToHashSet());
            Assert.IsType<HashSet<string>>(new List<string>().ToHashSet());
            Assert.IsType<HashSet<ExtensionUtilsTests>>(new List<ExtensionUtilsTests>().ToHashSet());
        }

        #endregion

        #region Object

        [Fact]
        public void ToDynamicTest()
        {
            var item = new { Name = "Test", X = 5, Y = 10 };
            var itemDynamic = item.ToDynamic();

            Assert.IsAssignableFrom<dynamic>(itemDynamic);
            Assert.Equal(item.Name, itemDynamic.Name);
            Assert.Equal(item.X, itemDynamic.X);
            Assert.Equal(item.Y, itemDynamic.Y);
        }

        #endregion

        #region String

        //public static string Abbreviate(this string item, int length)
        //public static string GetLast8Digits(this string item)

        [Fact]
        public void IsEmptyTestFact() => Assert.Throws<NullReferenceException>(() => ((string) null).IsEmpty());

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "Test")]
        public void IsEmptyTest(bool expected, string item) => Assert.Equal(expected, item.IsEmpty());

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "test@gmail.com")]
        public void IsInvalidEmailTest(bool expected, string item) => Assert.Equal(expected, item.IsInvalidEmail());

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "5")]
        public void IsInvalidIntTest(bool expected, string item) => Assert.Equal(expected, item.IsInvalidInt());
        
        [Theory]
        [InlineData(true, "test")]
        [InlineData(true, "-1")]
        [InlineData(true, "1.951")]
        [InlineData(true, ".5")]
        [InlineData(false, "1")]
        [InlineData(false, "0.5")]
        [InlineData(false, "1.5")]
        [InlineData(false, "1.95")]
        [InlineData(false, "250")]
        [InlineData(false, "250.95")]
        public void IsInvalidMoneyTest(bool expected, string item) => Assert.Equal(expected, item.IsInvalidMoney());

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "   ")]
        [InlineData(false, "\n")]
        [InlineData(true, null)]
        [InlineData(false, "Test")]
        [InlineData(false, " Test ")]
        public void IsNullOrEmptyTest(bool expected, string item)
        {
            Assert.Equal(expected, item.IsNullOrEmpty());
            Assert.Equal(string.IsNullOrEmpty(item), item.IsNullOrEmpty());
        }

        [Theory]
        [InlineData(true, "")]
        [InlineData(true, "   ")]
        [InlineData(true, "\n")]
        [InlineData(true, null)]
        [InlineData(false, "Test")]
        [InlineData(false, " Test ")]
        public void IsNullOrWhiteSpaceTest(bool expected, string item)
        {
            Assert.Equal(expected, item.IsNullOrWhiteSpace());
            Assert.Equal(string.IsNullOrWhiteSpace(item), item.IsNullOrWhiteSpace());
        }

        //public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);
        //public static string NewLineToBr(this string item) => item.Replace("\n", "<br />");
        //public static string RemoveDoubleQuotation(this string item) => item.Replace(@"\", string.Empty);
        //public static string RemoveRedundantWhitespace(this string item)
        //public static string RemoveSpaceAndApostrophe(this string item) =>
        //public static string SpaceToNbsp(this string item) => item.Replace(" ", "&nbsp;");
        //public static string ToAustralianMobileNumber(this string item)

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void ToDbConnectionStringBuilderTest(string item) =>
            Assert.IsType<DbConnectionStringBuilder>(item.ToDbConnectionStringBuilder());

        [Theory]
        [InlineData("", "")]
        [InlineData("", "   ")]
        [InlineData("", "\n")]
        [InlineData("", null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void ToEmptyIfNullOrWhiteSpaceTest(string expected, string item) =>
            Assert.Equal(expected, item.ToEmptyIfNullOrWhiteSpace());

        [Theory]
        [InlineData("Test.txt")]
        [InlineData("Directory/Test.txt")]
        public void ToFileInfoTest(string item) => Assert.IsType<FileInfo>(item.ToFileInfo());

        [Theory]
        [InlineData(null, "")]
        [InlineData(null, "   ")]
        [InlineData(null, "\n")]
        [InlineData(null, null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void ToNullIfNullOrWhiteSpaceTest(string expected, string item) =>
            Assert.Equal(expected, item.ToNullIfNullOrWhiteSpace());

        [Theory]
        [InlineData("Hello", "hello")]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello World", "helloWorld")]
        [InlineData("Hello World Test", "helloWorldTest")]
        [InlineData("Hello World Test", "HelloWorldTest")]
        [InlineData(" hello ", " hello ")]
        public void UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetterTest(
            string expected, string item) =>
            Assert.Equal(expected, item.UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter());

        #endregion

        #region TimeSpan

        [Fact]
        public void ToTimeStringTest()
        {
            ToTimeStringTestImplementation("12:00 AM", TimeSpan.Zero);
            ToTimeStringTestImplementation("00:00", TimeSpan.Zero, "HH:mm");
            ToTimeStringTestImplementation("1:00 PM", TimeSpan.FromHours(13));
            ToTimeStringTestImplementation("13", TimeSpan.FromHours(13), "HH");
            ToTimeStringTestImplementation("12:00 AM", TimeSpan.FromDays(1));
            ToTimeStringTestImplementation("00:00", TimeSpan.FromDays(1), "HH:mm");
            ToTimeStringTestImplementation("11:59 PM", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)));
            ToTimeStringTestImplementation("23:59", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)), "HH:mm");
            ToTimeStringTestImplementation("10:30 AM", new TimeSpan(10, 30, 0));
            ToTimeStringTestImplementation("8:15 PM", new TimeSpan(20, 15, 0));
        }

        private void ToTimeStringTestImplementation(string expected, TimeSpan item) =>
            Assert.Equal(expected, item.ToTimeString());

        private void ToTimeStringTestImplementation(string expected, TimeSpan item, string format) =>
            Assert.Equal(expected, item.ToTimeString(format));

        #endregion
    }
}
