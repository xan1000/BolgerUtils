using System;
using System.Collections.Generic;
using System.Linq;
using BolgerUtils;
using SendGrid.Helpers.Mail;
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

        #region EmailAddress

        [Fact]
        public void ToEmailAndNameStringTest()
        {
            ToEmailAndNameStringTestImplementation(null, new EmailAddress());
            ToEmailAndNameStringTestImplementation("test@gmail.com", new EmailAddress("test@gmail.com"));
            ToEmailAndNameStringTestImplementation("Test <test@gmail.com>", new EmailAddress("test@gmail.com", "Test"));
        }

        private void ToEmailAndNameStringTestImplementation(string expected, EmailAddress item) =>
            Assert.Equal(expected, item.ToEmailAndNameString());

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

        [Theory]
        [InlineData("test", true)]
        [InlineData("test@gmail.com", false)]
        public void IsInvalidEmailTest(string item, bool isInvalidEmail) =>
            Assert.Equal(isInvalidEmail, item.IsInvalidEmail());

        [Theory]
        [InlineData("test", true)]
        [InlineData("5", false)]
        public void IsInvalidIntTest(string item, bool isInvalidInt) =>
            Assert.Equal(isInvalidInt, item.IsInvalidInt());
        
        [Theory]
        [InlineData("test", true)]
        [InlineData("-1", true)]
        [InlineData("1.951", true)]
        [InlineData(".5", true)]
        [InlineData("1", false)]
        [InlineData("0.5", false)]
        [InlineData("1.5", false)]
        [InlineData("1.95", false)]
        [InlineData("250", false)]
        [InlineData("250.95", false)]
        public void IsInvalidMoneyTest(string item, bool isInvalidMoney) =>
            Assert.Equal(isInvalidMoney, item.IsInvalidMoney());

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
