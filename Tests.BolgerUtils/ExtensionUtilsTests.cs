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
        public void CheckedTest(string expected, bool value) => Assert.Equal(expected, value.Checked());

        [Theory]
        [InlineData("disabled", true)]
        [InlineData("", false)]
        public void DisabledTest(string expected, bool value) => Assert.Equal(expected, value.Disabled());

        [Fact]
        public void DisplayFactTest()
        {
            Assert.Equal("World", true.Display("World"));
            Assert.Equal(string.Empty, false.Display("World"));
        }

        [Theory]
        [InlineData("Hello", true, "Hello", "World")]
        [InlineData("World", false, "Hello", "World")]
        public void DisplayTest(string expected, bool value, string trueDisplay, string falseDisplay) =>
            Assert.Equal(expected, value.Display(trueDisplay, falseDisplay));

        [Theory]
        [InlineData("has-error", true)]
        [InlineData("", false)]
        public void HasErrorTest(string expected, bool value) => Assert.Equal(expected, value.HasError());

        [Theory]
        [InlineData("selected", true)]
        [InlineData("", false)]
        public void SelectedTest(string expected, bool value) => Assert.Equal(expected, value.Selected());

        [Theory]
        [InlineData("Yes", true)]
        [InlineData("No", false)]
        public void YesOrNoTest(string expected, bool value) => Assert.Equal(expected, value.YesOrNo());

        #endregion

        #region Byte
        
        [Theory]
        [InlineData("", new byte[0])]
        [InlineData("0123456789ABCDEF", new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF })]
        [InlineData("05", new byte[] { 0x5 })]
        [InlineData("0102300456", new byte[] { 0x1, 0x02, 0x30, 0x4, 0x56 })]
        public void HexStringTest(string expected, byte[] bytes) => Assert.Equal(expected, bytes.ToHexString());

        #endregion

        #region DateTime
        
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

        private void ToEmailAndNameStringTestImplementation(string expected, EmailAddress emailAddress) =>
            Assert.Equal(expected, emailAddress.ToEmailAndNameString());

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

        #endregion

        #region TimeSpan

        //public static string ToTimeString(this TimeSpan value, string format = "h:mm tt") =>
        //    DateTime.MinValue.Add(value).ToString(format);

        #endregion
    }
}
