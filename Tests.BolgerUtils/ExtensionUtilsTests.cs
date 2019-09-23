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
        public void Test_Checked(string expected, bool item) => Assert.Equal(expected, item.Checked());

        [Theory]
        [InlineData("disabled", true)]
        [InlineData("", false)]
        public void Test_Disabled(string expected, bool item) => Assert.Equal(expected, item.Disabled());

        [Theory]
        [InlineData("Hello", true, "Hello", "World")]
        [InlineData("World", false, "Hello", "World")]
        public void Test_Display(string expected, bool item, string trueDisplay, string falseDisplay) =>
            Assert.Equal(expected, item.Display(trueDisplay, falseDisplay));

        [Fact]
        public void TestFact_Display()
        {
            Assert.Equal("World", true.Display("World"));
            Assert.Equal(string.Empty, false.Display("World"));
        }

        [Theory]
        [InlineData("has-error", true)]
        [InlineData("", false)]
        public void Test_HasError(string expected, bool item) => Assert.Equal(expected, item.HasError());

        [Theory]
        [InlineData("<input type=checkbox disabled checked />", true)]
        [InlineData("<input type=checkbox disabled  />", false)]
        public void Test_ReadOnlyCheckbox(string expected, bool item) =>
            Assert.Equal(expected, item.ReadOnlyCheckbox());

        [Theory]
        [InlineData("selected", true)]
        [InlineData("", false)]
        public void Test_Selected(string expected, bool item) => Assert.Equal(expected, item.Selected());

        [Theory]
        [InlineData("Yes", true)]
        [InlineData("No", false)]
        public void Test_YesOrNo(string expected, bool item) => Assert.Equal(expected, item.YesOrNo());

        #endregion

        #region Byte
        
        [Theory]
        [InlineData("", new byte[0])]
        [InlineData("0123456789ABCDEF", new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF })]
        [InlineData("05", new byte[] { 0x5 })]
        [InlineData("0102300456", new byte[] { 0x1, 0x02, 0x30, 0x4, 0x56 })]
        public void Test_HexString(string expected, byte[] item) => Assert.Equal(expected, item.ToHexString());

        #endregion

        #region DateTime

        [Fact]
        public void Test_AddWeeks()
        {
            var date = DateTime.Today;
            for(var i = -10; i < 10; i++)
            {
                Assert.Equal(date.AddDays(i * Utils.DaysPerWeek), date.AddWeeks(i));
                Assert.Equal(date.AddDays(i * 7), date.AddWeeks(i));
            }
        }

        [Fact]
        public void Test_FindDayOfWeekBackward()
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
        public void Test_FindDayOfWeekForward()
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
        public void Test_Yesterday()
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
        public void Test_OrderByDayOfWeekStartingOnMonday()
        {
            var daysOfWeek = Utils.GetEnumValues<DayOfWeek>().OrderByDayOfWeekStartingOnMonday().ToArray();

            Assert.Equal(DayOfWeek.Monday, daysOfWeek[0]);
            Assert.Equal(DayOfWeek.Sunday, daysOfWeek[daysOfWeek.Length - 1]);
            Assert.Equal(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, daysOfWeek);
        }

        #endregion

        #region Enum

        [Theory]
        [InlineData(0, TestType.Test1)]
        [InlineData(1, TestType.Test2)]
        [InlineData(2, TestType.Test3)]
        [InlineData(10, TestAnotherType.Test1)]
        [InlineData(20, TestAnotherType.Test2)]
        [InlineData(30, TestAnotherType.Test3)]
        public void Test_ToInt(int expected, Enum item) => Assert.Equal(expected, item.ToInt());

        [Fact]
        public void TestFact_ToInt()
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
        [InlineData("0", TestType.Test1)]
        [InlineData("1", TestType.Test2)]
        [InlineData("2", TestType.Test3)]
        [InlineData("10", TestAnotherType.Test1)]
        [InlineData("20", TestAnotherType.Test2)]
        [InlineData("30", TestAnotherType.Test3)]
        public void Test_ToValueString(string expected, Enum item) => Assert.Equal(expected, item.ToValueString());

        [Fact]
        public void TestFact_ToValueString()
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

        #endregion

        #region Generic

        [Fact]
        public void Test_IsContainedIn()
        {
            var intValues = new[] { 2, 5, 6, 9 };
            foreach(var item in intValues)
            {
                Test_IsContainedInImplementation(true, item, intValues);
            }
            Test_IsContainedInImplementation(false, 1, intValues);
            Test_IsContainedInImplementation(true, 2, intValues);
            Test_IsContainedInImplementation(false, 3, intValues);
            Test_IsContainedInImplementation(false, 4, intValues);
            Test_IsContainedInImplementation(true, 5, intValues);
            Test_IsContainedInImplementation(true, 6, intValues);
            Test_IsContainedInImplementation(false, 7, intValues);
            Test_IsContainedInImplementation(false, 8, intValues);
            Test_IsContainedInImplementation(true, 9, intValues);
            Test_IsContainedInImplementation(false, 10, intValues);

            Assert.True(4.IsContainedIn(1, 2, 4));
            Assert.False(3.IsContainedIn(1, 2, 4));

            var stringValues = new[] { "Test", "Hello", "World" };
            foreach(var item in stringValues)
            {
                Test_IsContainedInImplementation(true, item, stringValues);
            }
            Test_IsContainedInImplementation(true, "Test", stringValues);
            Test_IsContainedInImplementation(true, "Hello", stringValues);
            Test_IsContainedInImplementation(true, "World", stringValues);
            Test_IsContainedInImplementation(false, "Bye", stringValues);

            Assert.True("World".IsContainedIn("Test", "Hello", "World"));
            Assert.False("Bye".IsContainedIn("Test", "Hello", "World"));
        }

        private void Test_IsContainedInImplementation<T>(bool expected, T item, T[] values)
        {
            Assert.Equal(expected, item.IsContainedIn(values));
            if(expected)
                Assert.Contains(item, values);
            else
                Assert.DoesNotContain(item, values);
        }

        [Fact]
        public void Test_ScalarToList()
        {
            Assert.IsType<List<int>>(10.ScalarToList());
            Assert.IsType<List<string>>("Test".ScalarToList());
            Assert.IsType<List<ExtensionUtilsTests>>(new ExtensionUtilsTests().ScalarToList());
        }

        #endregion

        #region ICollection

        //public static void AddAll<T>(this ICollection<T> item, IEnumerable<T> objects)
        //public static bool RemoveAll<T>(this ICollection<T> item, IEnumerable<T> objects)
        //public static bool RemoveAll<T>(this ICollection<T> item, Func<T, bool> predicate)

        #endregion

        #region IEnumerable

        [Fact]
        public void Test_IEnumerable_IsEmpty()
        {
            Test_IEnumerable_IsEmptyImplementation(false, new[] { 4, 5, 6 });
            Test_IEnumerable_IsEmptyImplementation(true, new int[0]);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_IEnumerable_IsEmptyImplementation(false, list);

            list.RemoveAt(0);
            Test_IEnumerable_IsEmptyImplementation(false, list);

            list.Clear();
            Test_IEnumerable_IsEmptyImplementation(true, list);

            Test_IEnumerable_IsEmptyImplementation(false, new ExtensionUtilsTests().ScalarToList());
            Test_IEnumerable_IsEmptyImplementation(true, new ExtensionUtilsTests[0]);
        }

        private void Test_IEnumerable_IsEmptyImplementation<T>(bool expected, IEnumerable<T> source)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.IsEmpty());
            Assert.Equal(!array.Any(), array.IsEmpty());
        }

        [Fact]
        public void Test_NotAll()
        {
            Test_NotAllImplementation(true, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotAllImplementation(true, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotAllImplementation(false, new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotAllImplementation(false, new int[0], x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAllImplementation(true, list, x => x == "Hello");
            Test_NotAllImplementation(true, list, x => x != "Hello");
            Test_NotAllImplementation(false, list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotAllImplementation(true, list, x => x == "Hello");
            Test_NotAllImplementation(false, list, x => x != "Hello");
            Test_NotAllImplementation(false, list, x => x != "Hello World");

            list.Clear();
            Test_NotAllImplementation(false, list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotAllImplementation(false, list, x => x == "Hello");
            Test_NotAllImplementation(true, list, x => x != "Hello");
        }

        private void Test_NotAllImplementation<T>(bool expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAll(predicate));
            Assert.Equal(!array.All(predicate), array.NotAll(predicate));
        }

        [Fact]
        public void Test_NotAny()
        {
            Test_NotAnyImplementation(false, new[] { 4, 5, 6 });
            Test_NotAnyImplementation(true, new int[0]);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAnyImplementation(false, list);

            list.RemoveAt(0);
            Test_NotAnyImplementation(false, list);

            list.Clear();
            Test_NotAnyImplementation(true, list);

            Test_NotAnyImplementation(false, new ExtensionUtilsTests().ScalarToList());
            Test_NotAnyImplementation(true, new ExtensionUtilsTests[0]);
        }

        private void Test_NotAnyImplementation<T>(bool expected, IEnumerable<T> source)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAny());
            Assert.Equal(!array.Any(), array.NotAny());
        }

        [Fact]
        public void Test_NotAnyPredicate()
        {
            Test_NotAnyPredicateImplementation(false, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotAnyPredicateImplementation(true, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotAnyPredicateImplementation(false, new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotAnyPredicateImplementation(true, new int[0], x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAnyPredicateImplementation(false, list, x => x == "Hello");
            Test_NotAnyPredicateImplementation(false, list, x => x != "Hello");
            Test_NotAnyPredicateImplementation(false, list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotAnyPredicateImplementation(true, list, x => x == "Hello");
            Test_NotAnyPredicateImplementation(false, list, x => x != "Hello");
            Test_NotAnyPredicateImplementation(false, list, x => x != "Hello World");

            list.Clear();
            Test_NotAnyPredicateImplementation(true, list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotAnyPredicateImplementation(false, list, x => x == "Hello");
            Test_NotAnyPredicateImplementation(true, list, x => x != "Hello");
        }

        private void Test_NotAnyPredicateImplementation<T>(
            bool expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAny(predicate));
            Assert.Equal(!array.Any(predicate), array.NotAny(predicate));
        }

        [Fact]
        public void Test_NotWhere()
        {
            Test_NotWhereImplementation(new[] { 5 }, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotWhereImplementation(new[] { 3, 5, 7 }, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotWhereImplementation(new int[0], new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotWhereImplementation(new int[0], new int[0], x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotWhereImplementation(new[] { "World", "Test" }, list, x => x == "Hello");
            Test_NotWhereImplementation(new[] { "Hello" }, list, x => x != "Hello");
            Test_NotWhereImplementation(new string[0], list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotWhereImplementation(new[] { "World", "Test" }, list, x => x == "Hello");
            Test_NotWhereImplementation(new string[0], list, x => x != "Hello");
            Test_NotWhereImplementation(new string[0], list, x => x != "Hello World");

            list.Clear();
            Test_NotWhereImplementation(new string[0], list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotWhereImplementation(new string[0], list, x => x == "Hello");
            Test_NotWhereImplementation(new[] { "Hello", "Hello", "Hello" }, list, x => x != "Hello");
        }

        private void Test_NotWhereImplementation<T>(
            IEnumerable<T> expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotWhere(predicate));
            Assert.Equal(array.Where(x => !predicate(x)), array.NotWhere(predicate));
        }

        [Fact]
        public void Test_ToListToHashSet()
        {
            Assert.IsType<HashSet<int>>(new[] { 4, 5, 6 }.ToListToHashSet());
            Assert.IsType<HashSet<string>>(new[] { "Hello", "World", "Test" }.ToListToHashSet());
            Assert.IsType<HashSet<ExtensionUtilsTests>>(new ExtensionUtilsTests().ScalarToList().ToListToHashSet());
        }

        #endregion

        #region List

        [Fact]
        public void Test_ToHashSet()
        {
            Assert.IsType<HashSet<int>>(new List<int>().ToHashSet());
            Assert.IsType<HashSet<string>>(new List<string>().ToHashSet());
            Assert.IsType<HashSet<ExtensionUtilsTests>>(new List<ExtensionUtilsTests>().ToHashSet());
        }

        #endregion

        #region Object

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, "Test")]
        public void Test_IsNull(bool expected, object item) => Assert.Equal(expected, item.IsNull());

        [Fact]
        public void Test_ToDynamic()
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
        [InlineData("", "", 0)]
        [InlineData("Hello", "Hello", 10)]
        [InlineData("Hello W...", "Hello World", 10)]
        [InlineData("HelloWo...", "HelloWorldTest", 10)]
        [InlineData("Hello World", "Hello World", 11)]
        public void Test_Abbreviate(string expected, string item, int length) =>
            Assert.Equal(expected, item.Abbreviate(length));

        [Fact]
        public void TestFact_Abbreviate()
        {
            const string item = "Hello";
            Assert.Equal(item, item.Abbreviate(5));
            Assert.Equal("H...", item.Abbreviate(4));
            Assert.Throws<Exception>(() => item.Abbreviate(3));
            Assert.Throws<Exception>(() => item.Abbreviate(2));
            Assert.Throws<Exception>(() => item.Abbreviate(1));
            Assert.Throws<Exception>(() => item.Abbreviate(0));
        }

        [Theory]
        [InlineData(null, "Test")]
        [InlineData(null, "1234")]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678", "0412345678")]
        [InlineData("12345678", "A 1 B 2 C 3 D 4 E 5 F 6 G 7 H 8")]
        [InlineData("12345678", "A 12 B 34 C 56 D 78 E")]
        [InlineData("12345678", "A 0 B 4 C 1 D 2 E 3 F 4 G 5 H 6 I 7 J 8")]
        [InlineData(null, "A 1 B 2 C 3 D 4 E 5 F 6 G 7 H")]
        public void Test_GetLast8Digits(string expected, string item) => Assert.Equal(expected, item.GetLast8Digits());

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "Test")]
        public void Test_IsEmpty(bool expected, string item) => Assert.Equal(expected, item.IsEmpty());

        [Fact]
        public void TestFact_IsEmpty() => Assert.Throws<NullReferenceException>(() => ((string) null).IsEmpty());

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "test@gmail.com")]
        public void Test_IsInvalidEmail(bool expected, string item) => Assert.Equal(expected, item.IsInvalidEmail());

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "5")]
        public void Test_IsInvalidInt(bool expected, string item) => Assert.Equal(expected, item.IsInvalidInt());
        
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
        public void Test_IsInvalidMoney(bool expected, string item) => Assert.Equal(expected, item.IsInvalidMoney());

        [Theory]
        [InlineData(true, "")]
        [InlineData(false, "   ")]
        [InlineData(false, "\n")]
        [InlineData(true, null)]
        [InlineData(false, "Test")]
        [InlineData(false, " Test ")]
        public void Test_IsNullOrEmpty(bool expected, string item)
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
        public void Test_IsNullOrWhiteSpace(bool expected, string item)
        {
            Assert.Equal(expected, item.IsNullOrWhiteSpace());
            Assert.Equal(string.IsNullOrWhiteSpace(item), item.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("", new string[0], " ")]
        [InlineData("Hello", new[] { "Hello" }, " ")]
        [InlineData("Hello World", new[] { "Hello", "World" }, " ")]
        [InlineData("Hello World Test", new[] { "Hello", "World", "Test" }, " ")]
        [InlineData("Hello,World,Test", new[] { "Hello", "World", "Test" }, ",")]
        [InlineData("Hello, World, Test", new[] { "Hello", "World", "Test" }, ", ")]
        [InlineData("Hello-World-Test", new[] { "Hello", "World", "Test" }, "-")]
        [InlineData("Hello\nWorld\nTest", new[] { "Hello", "World", "Test" }, "\n")]
        [InlineData("HelloTestWorld", new[] { "Hello", "World" }, "Test")]
        [InlineData("HelloWorld", new[] { "Hello", "World" }, "")]
        [InlineData("HelloWorldTest", new[] { "Hello", "World", "Test" }, "")]
        public void Test_Join(string expected, IEnumerable<string> source, string separator) =>
            Assert.Equal(expected, source.Join(separator));

        [Theory]
        [InlineData("", new string[0], ' ')]
        [InlineData("Hello", new[] { "Hello" }, ' ')]
        [InlineData("Hello World", new[] { "Hello", "World" }, ' ')]
        [InlineData("Hello World Test", new[] { "Hello", "World", "Test" }, ' ')]
        [InlineData("Hello,World,Test", new[] { "Hello", "World", "Test" }, ',')]
        [InlineData("Hello-World-Test", new[] { "Hello", "World", "Test" }, '-')]
        [InlineData("Hello\nWorld\nTest", new[] { "Hello", "World", "Test" }, '\n')]
        public void Test_JoinChar(string expected, IEnumerable<string> source, char separator) =>
            Assert.Equal(expected, source.Join(separator));

        [Theory]
        [InlineData("", "")]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello World", "Hello World")]
        [InlineData("Hello World<br />", "Hello World\n")]
        [InlineData("Hello World <br />", "Hello World \n")]
        [InlineData("Hello<br />World<br />", "Hello\nWorld\n")]
        [InlineData("Hello <br /> World <br />", "Hello \n World \n")]
        [InlineData("<br />Hello<br />World<br />", "\nHello\nWorld\n")]
        [InlineData("<br /> Hello <br /> World <br />", "\n Hello \n World \n")]
        [InlineData("Hello World <br /><br /><br />", "Hello World \n\n\n")]
        [InlineData("Hello <br /> World <br />", "Hello \n World <br />")]
        public void Test_NewLineToBr(string expected, string item) => Assert.Equal(expected, item.NewLineToBr());

        [Theory]
        [InlineData("", "Hello", "Hello")]
        [InlineData(" World", "Hello World", "Hello")]
        [InlineData("Hello ", "Hello World", "World")]
        [InlineData("HelloTest", "Hello World Test", " World ")]
        [InlineData("Test", "Hello World Test", "Hello World ")]
        [InlineData("Hello World Test", "Hello World Test", "Not Found")]
        public void Test_Remove(string expected, string item, string value) =>
            Assert.Equal(expected, item.Remove(value));

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData("\nHello\n", "\nHello\n")]
        [InlineData("Hello", "\rHello\r")]
        [InlineData("\nHello\n", "\n\rHello\r\n")]
        [InlineData("Hello World\n", "Hello World\r\n")]
        [InlineData("", "")]
        [InlineData(@"""", @"""")]
        [InlineData("'", "'")]
        [InlineData(" ", " ")]
        public void Test_RemoveCarriageReturn(string expected, string item) =>
            Assert.Equal(expected, item.RemoveCarriageReturn());

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello", @"""Hello""")]
        [InlineData("'Hello'", "'Hello'")]
        [InlineData("'Hello'", @"""'Hello'""")]
        [InlineData("Hello World", "Hello World")]
        [InlineData("Hello World", @"""Hello"" ""World""")]
        [InlineData(" Hello  World ", @""" ""Hello"" "" ""World"" """)]
        [InlineData("", "")]
        [InlineData("", @"""")]
        [InlineData("'", "'")]
        [InlineData(" ", " ")]
        public void Test_RemoveDoubleQuotation(string expected, string item) =>
            Assert.Equal(expected, item.RemoveDoubleQuotation());

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello", "\nHello\n")]
        [InlineData("\rHello\r", "\rHello\r")]
        [InlineData("\rHello\r", "\n\rHello\r\n")]
        [InlineData("Hello World\r", "Hello World\r\n")]
        [InlineData("", "")]
        [InlineData(@"""", @"""")]
        [InlineData("'", "'")]
        [InlineData(" ", " ")]
        public void Test_RemoveNewLine(string expected, string item) => Assert.Equal(expected, item.RemoveNewLine());

        [Fact]
        public void Test_RemoveRedundantWhitespace()
        {
            // ReSharper disable StringLiteralTypo
            const string item =
@"<!DOCTYPE html>
<html>
    <head>
        <title>My Favourite Books</title>
    </head>
    <body>
        <header id='header'
            class='header'
            style='background-color: lightgrey; text-align: right;
            font-size: 125%;'>
            <h1>My Favourite Books</h1>
            <h2><em>A list of fictional books.</em></h2>
            <strong>   by   John   Smith   </strong>
        </header>
        
        <div id='content'>
            <img src='books.gif' alt='Book'   />
            
            <p>
                List of book series:
            </p>
            
            <ol>
                <li>Lord of the Rings</li>
                <li>Harry Potter</li>
                <li>
                    Twilight
                </li>
            </ol>
        </div>
        
        <footer>
            <p>
                Find these books on
                <strong>
                    <em>
                        <a href='http://www.amazon.com' title='Search Amazon'>
                            Amazon
                        </a>.
                    </em>
                </strong>
            </p>
        </footer>
    </body>
</html>";

            var expected =
@"<!DOCTYPE html>
<html>
<head>
<title>My Favourite Books</title>
</head>
<body>
<header id='header'
class='header'
style='background-color: lightgrey; text-align: right;
font-size: 125%;'>
<h1>My Favourite Books</h1>
<h2><em>A list of fictional books.</em></h2>
<strong> by John Smith </strong>
</header>
<div id='content'>
<img src='books.gif' alt='Book' />
<p>
List of book series:
</p>
<ol>
<li>Lord of the Rings</li>
<li>Harry Potter</li>
<li>
Twilight
</li>
</ol>
</div>
<footer>
<p>
Find these books on
<strong>
<em>
<a href='http://www.amazon.com' title='Search Amazon'>
Amazon
</a>.
</em>
</strong>
</p>
</footer>
</body>
</html>".RemoveCarriageReturn();
            // ReSharper restore StringLiteralTypo

            Assert.Equal(expected, item.RemoveRedundantWhitespace());
        }
        
        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData(@"""Hello""", @"""Hello""")]
        [InlineData("Hello", "'Hello'")]
        [InlineData(@"""Hello""", @"""'Hello'""")]
        [InlineData("Hello World", "Hello World")]
        [InlineData("Hello World", "'Hello' 'World'")]
        [InlineData(" Hello  World ", "' 'Hello' ' 'World' '")]
        [InlineData("", "")]
        [InlineData(@"""", @"""")]
        [InlineData("", "'")]
        [InlineData(" ", " ")]
        public void Test_RemoveSingleQuotation(string expected, string item) =>
            Assert.Equal(expected, item.RemoveSingleQuotation());

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData(@"""Hello""", @"""Hello""")]
        [InlineData("'Hello'", "'Hello'")]
        [InlineData(@"""'Hello'""", @"""'Hello'""")]
        [InlineData("HelloWorld", "Hello World")]
        [InlineData("HelloWorld", "  Hello   World   ")]
        [InlineData("", "")]
        [InlineData(@"""", @"""")]
        [InlineData("'", "'")]
        [InlineData("", " ")]
        public void Test_RemoveSpace(string expected, string item) => Assert.Equal(expected, item.RemoveSpace());

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData(@"""Hello""", @"""Hello""")]
        [InlineData("Hello", "'Hello'")]
        [InlineData(@"""Hello""", @"""'Hello'""")]
        [InlineData("HelloWorld", "Hello World")]
        [InlineData("HelloWorld", "'Hello' 'World'")]
        [InlineData("HelloWorld", "' 'Hello' ' 'World' '")]
        [InlineData("", "")]
        [InlineData(@"""", @"""")]
        [InlineData("", "'")]
        [InlineData("", " ")]
        public void Test_RemoveSpaceAndSingleQuotation(string expected, string item) =>
            Assert.Equal(expected, item.RemoveSpaceAndSingleQuotation());

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello&nbsp;World", "Hello World")]
        [InlineData("Hello&nbsp;World&nbsp;Test", "Hello World Test")]
        [InlineData("&nbsp;&nbsp;&nbsp;Hello&nbsp;&nbsp;&nbsp;World&nbsp;Test&nbsp;", "   Hello   World Test ")]
        public void Test_SpaceToNbsp(string expected, string item) => Assert.Equal(expected, item.SpaceToNbsp());

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void Test_ToDbConnectionStringBuilder(string item) =>
            Assert.IsType<DbConnectionStringBuilder>(item.ToDbConnectionStringBuilder());

        [Theory]
        [InlineData("", "")]
        [InlineData("", "   ")]
        [InlineData("", "\n")]
        [InlineData("", null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void Test_ToEmptyIfNullOrWhiteSpace(string expected, string item) =>
            Assert.Equal(expected, item.ToEmptyIfNullOrWhiteSpace());

        [Theory]
        [InlineData("Test.txt")]
        [InlineData("Directory/Test.txt")]
        public void Test_ToFileInfo(string item) => Assert.IsType<FileInfo>(item.ToFileInfo());

        [Theory]
        [InlineData(null, "")]
        [InlineData(null, "   ")]
        [InlineData(null, "\n")]
        [InlineData(null, null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void Test_ToNullIfNullOrWhiteSpace(string expected, string item) =>
            Assert.Equal(expected, item.ToNullIfNullOrWhiteSpace());

        [Theory]
        [InlineData("", "", 0)]
        [InlineData("Hello", "Hello", 5)]
        [InlineData("Hell", "Hello", 4)]
        [InlineData("Hel", "Hello", 3)]
        [InlineData("He", "Hello", 2)]
        [InlineData("H", "Hello", 1)]
        [InlineData("", "Hello", 0)]
        [InlineData("Hello World", "Hello World", 12)]
        [InlineData("Hello World", "Hello World", 11)]
        // ReSharper disable once StringLiteralTypo
        [InlineData("Hello Worl", "Hello World", 10)]
        [InlineData("Hello Wor", "Hello World", 9)]
        [InlineData("Hello Wo", "Hello World", 8)]
        [InlineData("Hello W", "Hello World", 7)]
        [InlineData("Hello ", "Hello World", 6)]
        [InlineData("Hello", "Hello World", 5)]
        [InlineData("Hell", "Hello World", 4)]
        [InlineData("Hel", "Hello World", 3)]
        [InlineData("He", "Hello World", 2)]
        [InlineData("H", "Hello World", 1)]
        [InlineData("", "Hello World", 0)]
        public void Test_Truncate(string expected, string item, int length) =>
            Assert.Equal(expected, item.Truncate(length));

        [Theory]
        [InlineData("Hello", "hello")]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello World", "helloWorld")]
        [InlineData("Hello World Test", "helloWorldTest")]
        [InlineData("Hello World Test", "HelloWorldTest")]
        [InlineData(" hello ", " hello ")]
        public void Test_UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter(
            string expected, string item) =>
            Assert.Equal(expected, item.UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter());

        #endregion

        #region TimeSpan

        [Fact]
        public void Test_ToTimeString()
        {
            Test_ToTimeStringImplementation("12:00 AM", TimeSpan.Zero);
            Test_ToTimeStringImplementation("00:00", TimeSpan.Zero, "HH:mm");
            Test_ToTimeStringImplementation("1:00 PM", TimeSpan.FromHours(13));
            Test_ToTimeStringImplementation("13", TimeSpan.FromHours(13), "HH");
            Test_ToTimeStringImplementation("12:00 AM", TimeSpan.FromDays(1));
            Test_ToTimeStringImplementation("00:00", TimeSpan.FromDays(1), "HH:mm");
            Test_ToTimeStringImplementation("11:59 PM", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)));
            Test_ToTimeStringImplementation("23:59", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)), "HH:mm");
            Test_ToTimeStringImplementation("10:30 AM", new TimeSpan(10, 30, 0));
            Test_ToTimeStringImplementation("8:15 PM", new TimeSpan(20, 15, 0));
        }

        private void Test_ToTimeStringImplementation(string expected, TimeSpan item) =>
            Assert.Equal(expected, item.ToTimeString());

        private void Test_ToTimeStringImplementation(string expected, TimeSpan item, string format) =>
            Assert.Equal(expected, item.ToTimeString(format));

        #endregion
    }
}
