﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using BolgerUtils;
using Microsoft.Data.SqlClient;
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
        public void Test_IsDayInFuture()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsDayInFuture_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsDayInFuture_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsDayInFuture(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsDayInFuture(date));
        }

        private void Test_IsDayInFuture_Implementation(DateTime date)
        {
            Assert.False(date.IsDayInFuture(date));
            Assert.False(date.IsDayInFuture(date.AddDays(0.1)));
            Assert.False(date.IsDayInFuture(date.AddDays(1)));
            Assert.False(date.IsDayInFuture(date.AddDays(5)));
            Assert.False(date.IsDayInFuture(date.AddDays(-0.1)));
            Assert.True(date.IsDayInFuture(date.AddDays(-1)));
            Assert.True(date.IsDayInFuture(date.AddDays(-5)));
            Assert.False(date.AddDays(0.1).IsDayInFuture(date));
            Assert.True(date.AddDays(1).IsDayInFuture(date));
            Assert.True(date.AddDays(5).IsDayInFuture(date));
            Assert.False(date.AddDays(-0.1).IsDayInFuture(date));
            Assert.False(date.AddDays(-1).IsDayInFuture(date));
            Assert.False(date.AddDays(-5).IsDayInFuture(date));
        }

        [Fact]
        public void Test_IsDayInFutureOrToday()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsDayInFutureOrToday_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsDayInFutureOrToday_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsDayInFutureOrToday(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsDayInFutureOrToday(date));
        }

        private void Test_IsDayInFutureOrToday_Implementation(DateTime date)
        {
            Assert.True(date.IsDayInFutureOrToday(date));
            Assert.True(date.IsDayInFutureOrToday(date.AddDays(0.1)));
            Assert.False(date.IsDayInFutureOrToday(date.AddDays(1)));
            Assert.False(date.IsDayInFutureOrToday(date.AddDays(5)));
            Assert.True(date.IsDayInFutureOrToday(date.AddDays(-0.1)));
            Assert.True(date.IsDayInFutureOrToday(date.AddDays(-1)));
            Assert.True(date.IsDayInFutureOrToday(date.AddDays(-5)));
            Assert.True(date.AddDays(0.1).IsDayInFutureOrToday(date));
            Assert.True(date.AddDays(1).IsDayInFutureOrToday(date));
            Assert.True(date.AddDays(5).IsDayInFutureOrToday(date));
            Assert.True(date.AddDays(-0.1).IsDayInFutureOrToday(date));
            Assert.False(date.AddDays(-1).IsDayInFutureOrToday(date));
            Assert.False(date.AddDays(-5).IsDayInFutureOrToday(date));
        }

        [Fact]
        public void Test_IsDayInPast()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsDayInPast_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsDayInPast_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsDayInPast(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsDayInPast(date));
        }

        private void Test_IsDayInPast_Implementation(DateTime date)
        {
            Assert.False(date.IsDayInPast(date));
            Assert.False(date.IsDayInPast(date.AddDays(0.1)));
            Assert.True(date.IsDayInPast(date.AddDays(1)));
            Assert.True(date.IsDayInPast(date.AddDays(5)));
            Assert.False(date.IsDayInPast(date.AddDays(-0.1)));
            Assert.False(date.IsDayInPast(date.AddDays(-1)));
            Assert.False(date.IsDayInPast(date.AddDays(-5)));
            Assert.False(date.AddDays(0.1).IsDayInPast(date));
            Assert.False(date.AddDays(1).IsDayInPast(date));
            Assert.False(date.AddDays(5).IsDayInPast(date));
            Assert.False(date.AddDays(-0.1).IsDayInPast(date));
            Assert.True(date.AddDays(-1).IsDayInPast(date));
            Assert.True(date.AddDays(-5).IsDayInPast(date));
        }

        [Fact]
        public void Test_IsDayInPastOrToday()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsDayInPastOrToday_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsDayInPastOrToday_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsDayInPastOrToday(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsDayInPastOrToday(date));
        }

        private void Test_IsDayInPastOrToday_Implementation(DateTime date)
        {
            Assert.True(date.IsDayInPastOrToday(date));
            Assert.True(date.IsDayInPastOrToday(date.AddDays(0.1)));
            Assert.True(date.IsDayInPastOrToday(date.AddDays(1)));
            Assert.True(date.IsDayInPastOrToday(date.AddDays(5)));
            Assert.True(date.IsDayInPastOrToday(date.AddDays(-0.1)));
            Assert.False(date.IsDayInPastOrToday(date.AddDays(-1)));
            Assert.False(date.IsDayInPastOrToday(date.AddDays(-5)));
            Assert.True(date.AddDays(0.1).IsDayInPastOrToday(date));
            Assert.False(date.AddDays(1).IsDayInPastOrToday(date));
            Assert.False(date.AddDays(5).IsDayInPastOrToday(date));
            Assert.True(date.AddDays(-0.1).IsDayInPastOrToday(date));
            Assert.True(date.AddDays(-1).IsDayInPastOrToday(date));
            Assert.True(date.AddDays(-5).IsDayInPastOrToday(date));
        }

        [Fact]
        public void Test_IsInFuture()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsInFuture_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsInFuture_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsInFuture(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsInFuture(date));
        }

        private void Test_IsInFuture_Implementation(DateTime date)
        {
            Assert.False(date.IsInFuture(date));
            Assert.False(date.IsInFuture(date.AddDays(0.1)));
            Assert.False(date.IsInFuture(date.AddDays(1)));
            Assert.False(date.IsInFuture(date.AddDays(5)));
            Assert.True(date.IsInFuture(date.AddDays(-0.1)));
            Assert.True(date.IsInFuture(date.AddDays(-1)));
            Assert.True(date.IsInFuture(date.AddDays(-5)));
            Assert.True(date.AddDays(0.1).IsInFuture(date));
            Assert.True(date.AddDays(1).IsInFuture(date));
            Assert.True(date.AddDays(5).IsInFuture(date));
            Assert.False(date.AddDays(-0.1).IsInFuture(date));
            Assert.False(date.AddDays(-1).IsInFuture(date));
            Assert.False(date.AddDays(-5).IsInFuture(date));
        }

        [Fact]
        public void Test_IsInFutureOrNow()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsInFutureOrNow_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsInFutureOrNow_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsInFutureOrNow(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsInFutureOrNow(date));
        }

        private void Test_IsInFutureOrNow_Implementation(DateTime date)
        {
            Assert.True(date.IsInFutureOrNow(date));
            Assert.False(date.IsInFutureOrNow(date.AddDays(0.1)));
            Assert.False(date.IsInFutureOrNow(date.AddDays(1)));
            Assert.False(date.IsInFutureOrNow(date.AddDays(5)));
            Assert.True(date.IsInFutureOrNow(date.AddDays(-0.1)));
            Assert.True(date.IsInFutureOrNow(date.AddDays(-1)));
            Assert.True(date.IsInFutureOrNow(date.AddDays(-5)));
            Assert.True(date.AddDays(0.1).IsInFutureOrNow(date));
            Assert.True(date.AddDays(1).IsInFutureOrNow(date));
            Assert.True(date.AddDays(5).IsInFutureOrNow(date));
            Assert.False(date.AddDays(-0.1).IsInFutureOrNow(date));
            Assert.False(date.AddDays(-1).IsInFutureOrNow(date));
            Assert.False(date.AddDays(-5).IsInFutureOrNow(date));
        }

        [Fact]
        public void Test_IsInPast()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsInPast_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsInPast_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsInPast(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsInPast(date));
        }

        private void Test_IsInPast_Implementation(DateTime date)
        {
            Assert.False(date.IsInPast(date));
            Assert.True(date.IsInPast(date.AddDays(0.1)));
            Assert.True(date.IsInPast(date.AddDays(1)));
            Assert.True(date.IsInPast(date.AddDays(5)));
            Assert.False(date.IsInPast(date.AddDays(-0.1)));
            Assert.False(date.IsInPast(date.AddDays(-1)));
            Assert.False(date.IsInPast(date.AddDays(-5)));
            Assert.False(date.AddDays(0.1).IsInPast(date));
            Assert.False(date.AddDays(1).IsInPast(date));
            Assert.False(date.AddDays(5).IsInPast(date));
            Assert.True(date.AddDays(-0.1).IsInPast(date));
            Assert.True(date.AddDays(-1).IsInPast(date));
            Assert.True(date.AddDays(-5).IsInPast(date));
        }

        [Fact]
        public void Test_IsInPastOrNow()
        {
            var date = new DateTime(2019, 10, 30, 9, 30, 30);
            Test_IsInPastOrNow_Implementation(date);

            var dateUtc = new DateTime(2019, 10, 30, 9, 30, 30, DateTimeKind.Utc);
            Test_IsInPastOrNow_Implementation(dateUtc);

            Assert.Throws<ArgumentException>(() => date.IsInPastOrNow(dateUtc));
            Assert.Throws<ArgumentException>(() => dateUtc.IsInPastOrNow(date));
        }

        private void Test_IsInPastOrNow_Implementation(DateTime date)
        {
            Assert.True(date.IsInPastOrNow(date));
            Assert.True(date.IsInPastOrNow(date.AddDays(0.1)));
            Assert.True(date.IsInPastOrNow(date.AddDays(1)));
            Assert.True(date.IsInPastOrNow(date.AddDays(5)));
            Assert.False(date.IsInPastOrNow(date.AddDays(-0.1)));
            Assert.False(date.IsInPastOrNow(date.AddDays(-1)));
            Assert.False(date.IsInPastOrNow(date.AddDays(-5)));
            Assert.False(date.AddDays(0.1).IsInPastOrNow(date));
            Assert.False(date.AddDays(1).IsInPastOrNow(date));
            Assert.False(date.AddDays(5).IsInPastOrNow(date));
            Assert.True(date.AddDays(-0.1).IsInPastOrNow(date));
            Assert.True(date.AddDays(-1).IsInPastOrNow(date));
            Assert.True(date.AddDays(-5).IsInPastOrNow(date));
        }

        [Fact]
        public void Test_Tomorrow()
        {
            var date = DateTime.Today;
            for(var i = 0; i < 10; i++, date = date.AddDays(1))
            {
                var tomorrow = date.Tomorrow();

                Assert.Equal(date.AddDays(1), tomorrow);
                Assert.Equal(1, tomorrow.Subtract(date).TotalDays);
            }
        }

        [Fact]
        public void Test_Yesterday()
        {
            var date = DateTime.Today;
            for(var i = 0; i < 10; i++, date = date.AddDays(1))
            {
                var yesterday = date.Yesterday();

                Assert.Equal(date.AddDays(-1), yesterday);
                Assert.Equal(-1, yesterday.Subtract(date).TotalDays);
            }
        }

        #endregion

        #region DayOfWeek

        [Fact]
        public void Test_OrderByDayOfWeekStartingOnMonday()
        {
            var daysOfWeek = Utils.GetEnumValues<DayOfWeek>().OrderByDayOfWeekStartingOnMonday().ToArray();

            Assert.Equal(DayOfWeek.Monday, daysOfWeek[0]);
            Assert.Equal(DayOfWeek.Sunday, daysOfWeek[^1]);
            Assert.Equal(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday }, daysOfWeek);
        }

        #endregion

        #region DbCommand

        [Fact]
        public void Test_ToDataTable()
        {
            lock(UtilsTests.s_createConnectionLock)
            {
                try
                {
                    Utils.ConnectionString = UtilsTests.ConnectionString;
                    Utils.CreateConnectionFunc = connectionString => new SqlConnection(connectionString);

                    using var connection = Utils.CreateConnection<SqlConnection>();
                    using var command = connection.CreateCommand();
                    command.CommandText = "select * from Animal";

                    Assert.Throws<InvalidOperationException>(command.ToDataTable);

                    // Set Utils.CreateDataAdapterFunc.
                    Utils.CreateDataAdapterFunc = dbCommand => new SqlDataAdapter((SqlCommand) dbCommand);

                    using var table = command.ToDataTable();
                    Assert.IsType<DataTable>(table);
                    Assert.Equal(3, table.Rows.Count);
                }
                finally
                {
                    // Test cleanup.
                    Utils.ConnectionString = null;
                    Utils.CreateConnectionFunc = null;
                    Utils.CreateDataAdapterFunc = null;
                }
            }
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
        public void Test_Enum_ToInt(int expected, Enum item) => Assert.Equal(expected, item.ToInt());

        [Fact]
        public void TestFact_Enum_ToInt()
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
                Test_IsContainedIn_Implementation(true, item, intValues);
            }
            Test_IsContainedIn_Implementation(false, 1, intValues);
            Test_IsContainedIn_Implementation(true, 2, intValues);
            Test_IsContainedIn_Implementation(false, 3, intValues);
            Test_IsContainedIn_Implementation(false, 4, intValues);
            Test_IsContainedIn_Implementation(true, 5, intValues);
            Test_IsContainedIn_Implementation(true, 6, intValues);
            Test_IsContainedIn_Implementation(false, 7, intValues);
            Test_IsContainedIn_Implementation(false, 8, intValues);
            Test_IsContainedIn_Implementation(true, 9, intValues);
            Test_IsContainedIn_Implementation(false, 10, intValues);

            Assert.True(4.IsContainedIn(1, 2, 4));
            Assert.False(3.IsContainedIn(1, 2, 4));

            var stringValues = new[] { "Test", "Hello", "World" };
            foreach(var item in stringValues)
            {
                Test_IsContainedIn_Implementation(true, item, stringValues);
            }
            Test_IsContainedIn_Implementation(true, "Test", stringValues);
            Test_IsContainedIn_Implementation(true, "Hello", stringValues);
            Test_IsContainedIn_Implementation(true, "World", stringValues);
            Test_IsContainedIn_Implementation(false, "Bye", stringValues);

            Assert.True("World".IsContainedIn("Test", "Hello", "World"));
            Assert.False("Bye".IsContainedIn("Test", "Hello", "World"));
        }

        private void Test_IsContainedIn_Implementation<T>(bool expected, T item, T[] values)
        {
            Assert.Equal(expected, item.IsContainedIn(values));
            if(expected)
                Assert.Contains(item, values);
            else
                Assert.DoesNotContain(item, values);
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, "Test")]
        [InlineData(false, 1)]
        [InlineData(false, 1.0)]
        public void Test_IsNull<T>(bool expected, T item) => Assert.Equal(expected, item.IsNull());

        [Fact]
        public void TestFact_IsNull()
        {
            // ReSharper disable ExpressionIsAlwaysNull
            int? i = null;
            Assert.True(i.IsNull());
            i = 1;
            Assert.False(i.IsNull());

            decimal? dec = null;
            Assert.True(dec.IsNull());
            dec = 1;
            Assert.False(dec.IsNull());
            
            double? d = null;
            Assert.True(d.IsNull());
            d = 1;
            Assert.False(d.IsNull());
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public void Test_ScalarToList()
        {
            Assert.IsType<List<int>>(10.ScalarToList());
            Assert.IsType<List<string>>("Test".ScalarToList());
            Assert.IsType<List<ExtensionUtilsTests>>(new ExtensionUtilsTests().ScalarToList());
        }

        [Fact]
        public void Test_ToStringIfNull()
        {
            const string stringIfNullDefaultValue = "(null)";

            // Test class.
            var s = "Hello World";
            Test_ToStringIfNull_Implementation(s, s);
            Test_ToStringIfNull_Implementation(s, s, "Test");
            Assert.Throws<ArgumentException>(() => s.ToStringIfNull(null));

            s = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Test_ToStringIfNull_Implementation(stringIfNullDefaultValue, s);
            Test_ToStringIfNull_Implementation("Test", s, "Test");
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => s.ToStringIfNull(null));

            var t = new ExtensionUtilsTests();
            Test_ToStringIfNull_Implementation(t.ToString(), t);
            Test_ToStringIfNull_Implementation(t.ToString(), t, "Test");
            Assert.Throws<ArgumentException>(() => t.ToStringIfNull(null));

            t = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Test_ToStringIfNull_Implementation(stringIfNullDefaultValue, t);
            Test_ToStringIfNull_Implementation("Test", t, "Test");
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => t.ToStringIfNull(null));

            // Test struct.
            int? i = 10;
            Test_ToStringIfNull_Implementation(i.ToString(), i);
            Test_ToStringIfNull_Implementation(i.ToString(), i, "Test");
            Assert.Throws<ArgumentException>(() => i.ToStringIfNull(null));

            i = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Test_ToStringIfNull_Implementation(stringIfNullDefaultValue, i);
            Test_ToStringIfNull_Implementation("Test", i, "Test");
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => i.ToStringIfNull(null));
        }

        private void Test_ToStringIfNull_Implementation<T>(string expected, T item, string stringIfNull = null) =>
            Assert.Equal(expected, stringIfNull == null ? item.ToStringIfNull() : item.ToStringIfNull(stringIfNull));

        #endregion

        #region ICollection

        [Fact]
        public void Test_AddAll()
        {
            Test_AddAll_Implementation(new[] { 4, 5, 6 }, new List<int>(), new[] { 4, 5, 6 });
            Test_AddAll_Implementation(new[] { 1, 2, 3, 7, 8, 9, 4, 5, 6 }, new List<int> { 1, 2, 3, 7, 8, 9 },
                new[] { 4, 5, 6 });
            Test_AddAll_Implementation(Array.Empty<int>(), new List<int>(), Array.Empty<int>());

            Test_AddAll_Implementation(new[] { "Hello" }, new List<string>(), new[] { "Hello" });
            Test_AddAll_Implementation(new[] { "Hello", "World" }, new List<string>(), new[] { "Hello", "World" });
            Test_AddAll_Implementation(new[] { "Test", "AnotherTest", "Hello", "World" },
                new List<string> { "Test", "AnotherTest" }, new[] { "Hello", "World" });

            var utilsTests1 = new UtilsTests();
            var utilsTests2 = new UtilsTests();
            Test_AddAll_Implementation(new[] { utilsTests1, utilsTests2 }, new List<UtilsTests>(),
                new[] { utilsTests1, utilsTests2 });

            var utilsTests3 = new UtilsTests();
            Test_AddAll_Implementation(new[] { utilsTests3, utilsTests1, utilsTests2 },
                new List<UtilsTests> { utilsTests3 }, new[] { utilsTests1, utilsTests2 });
        }

        private void Test_AddAll_Implementation<T>(
            IEnumerable<T> expected, ICollection<T> item, IEnumerable<T> objects)
        {
            var array = objects.ToArray();
            foreach(var x in array)
            {
                Assert.DoesNotContain(x, item);
            }

            var expectedCount = item.Count + array.Length;
            item.AddAll(array);

            Assert.Equal(expectedCount, item.Count);
            foreach(var x in array)
            {
                Assert.Contains(x, item);
            }

            Assert.Equal(expected, item);
        }

        [Fact]
        public void Test_RemoveAll()
        {
            Test_RemoveAll_Implementation(Array.Empty<int>(), new List<int> { 4, 5, 6 }, new[] { 4, 5, 6 });
            Test_RemoveAll_Implementation(new[] { 1, 2, 3 }, new List<int> { 1, 2, 3, 4, 5, 6 }, new[] { 4, 5, 6 });
            Test_RemoveAll_Implementation(new[] { 1, 3, 6 }, new List<int> { 1, 2, 3, 4, 5, 6 }, new[] { 2, 4, 5 });
            Test_RemoveAll_Implementation(new[] { 2 }, new List<int> { 1, 2, 3 }, new[] { 1, 3 });
            Test_RemoveAll_Implementation(Array.Empty<int>(), new List<int>(), Array.Empty<int>());

            Test_RemoveAll_Implementation(Array.Empty<string>(), new List<string> { "Hello" }, new[] { "Hello" });
            Test_RemoveAll_Implementation(new[] { "World" }, new List<string> { "Hello", "World" }, new[] { "Hello" });
            Test_RemoveAll_Implementation(Array.Empty<string>(), new List<string> { "Hello", "World" },
                new[] { "Hello", "World" });
            Test_RemoveAll_Implementation(new[] { "Test" }, new List<string> { "Hello", "Test", "World" },
                new[] { "Hello", "World" });
            Test_RemoveAll_Implementation(Array.Empty<string>(), new List<string>(), Array.Empty<string>());

            var utilsTests1 = new UtilsTests();
            var utilsTests2 = new UtilsTests();
            Test_RemoveAll_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests> { utilsTests1 },
                new[] { utilsTests1 });
            Test_RemoveAll_Implementation(new[] { utilsTests2 }, new List<UtilsTests> { utilsTests1, utilsTests2 },
                new[] { utilsTests1 });
            Test_RemoveAll_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests> { utilsTests1, utilsTests2 },
                new[] { utilsTests1, utilsTests2 });

            var utilsTests3 = new UtilsTests();
            Test_RemoveAll_Implementation(new[] { utilsTests2 },
                new List<UtilsTests> { utilsTests1, utilsTests2, utilsTests3 }, new[] { utilsTests1, utilsTests3 });
            Test_RemoveAll_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests>(), Array.Empty<UtilsTests>());

            Test_RemoveAll_Implementation(new[] { 1, 3 }, new List<int> { 1, 2, 3 }, new[] { 2, 4 }, false);
            Test_RemoveAll_Implementation(new[] { 1, 2, 3 }, new List<int> { 1, 2, 3 }, new[] { 4, 5, 6 }, false);
        }

        private void Test_RemoveAll_Implementation<T>(
            IEnumerable<T> expected, ICollection<T> item, IEnumerable<T> objects, bool containsAll = true)
        {
            var array = objects.ToArray();
            if(containsAll)
            {
                foreach(var x in array)
                {
                    Assert.Contains(x, item);
                }
            }

            var expectedCount = item.Count - array.Length;
            Assert.Equal(containsAll, item.RemoveAll(array));
            if(containsAll)
                Assert.Equal(expectedCount, item.Count);

            foreach(var x in array)
            {
                Assert.DoesNotContain(x, item);
            }

            Assert.Equal(expected, item);
        }

        [Fact]
        public void Test_RemoveAllPredicate()
        {
            Test_RemoveAllPredicate_Implementation(Array.Empty<int>(), new List<int> { 4, 5, 6 }, _ => true);
            Test_RemoveAllPredicate_Implementation(new[] { 4, 5, 6 }, new List<int> { 4, 5, 6 }, _ => false);
            Test_RemoveAllPredicate_Implementation(new[] { 4, 6 }, new List<int> { 4, 5, 6 }, x => x == 5);
            Test_RemoveAllPredicate_Implementation(new[] { 4, 6 }, new List<int> { 4, 5, 6 }, x => x == 5);
            Test_RemoveAllPredicate_Implementation(new[] { 1, 3, 5 }, new List<int> { 1, 2, 3, 4, 5, 6 },
                x => x % 2 == 0);
            Test_RemoveAllPredicate_Implementation(new[] { 2, 4, 6 }, new List<int> { 1, 2, 3, 4, 5, 6 },
                x => x % 2 == 1);
            Test_RemoveAllPredicate_Implementation(new[] { 2 }, new List<int> { 1, 2, 3 }, x => x is 1 or 3);
            Test_RemoveAllPredicate_Implementation(Array.Empty<int>(), new List<int>(), x => x == 0);

            Test_RemoveAllPredicate_Implementation(Array.Empty<string>(), new List<string> { "Hello" }, _ => true);
            Test_RemoveAllPredicate_Implementation(new[] { "Hello" }, new List<string> { "Hello" }, _ => false);
            Test_RemoveAllPredicate_Implementation(new[] { "World" }, new List<string> { "Hello", "World" },
                x => x == "Hello");
            Test_RemoveAllPredicate_Implementation(new[] { "Test" }, new List<string> { "Hello", "Test", "World" },
                x => x.Length == 5);
            Test_RemoveAllPredicate_Implementation(new[] { "Hello", "World" },
                new List<string> { "", "Hello", "", "", "World", "" }, x => x.IsEmpty());
            Test_RemoveAllPredicate_Implementation(new[] { "Test" }, new List<string> { "Hello", "Test", "World" },
                x => x is "Hello" or "World");
            Test_RemoveAllPredicate_Implementation(Array.Empty<string>(), new List<string>(), x => x.IsEmpty());

            var utilsTests1 = new UtilsTests();
            var utilsTests2 = new UtilsTests();
            Test_RemoveAllPredicate_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests> { utilsTests1 },
                _ => true);
            Test_RemoveAllPredicate_Implementation(new[] { utilsTests1 }, new List<UtilsTests> { utilsTests1 },
                _ => false);
            Test_RemoveAllPredicate_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests> { utilsTests1 },
                // ReSharper disable once ImplicitlyCapturedClosure
                x => x == utilsTests1);
            Test_RemoveAllPredicate_Implementation(new[] { utilsTests2 },
                // ReSharper disable once ImplicitlyCapturedClosure
                new List<UtilsTests> { utilsTests1, utilsTests2 }, x => x == utilsTests1);
            Test_RemoveAllPredicate_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests> { utilsTests1, utilsTests2 },
                x => x == utilsTests1 || x == utilsTests2);

            var utilsTests3 = new UtilsTests();
            Test_RemoveAllPredicate_Implementation(new[] { utilsTests2 },
                new List<UtilsTests> { utilsTests1, utilsTests2, utilsTests3 },
                // ReSharper disable once ImplicitlyCapturedClosure
                x => x == utilsTests1 || x == utilsTests3);
            // ReSharper disable once ImplicitlyCapturedClosure
            Test_RemoveAllPredicate_Implementation(Array.Empty<UtilsTests>(), new List<UtilsTests>(), x => x == utilsTests1);

            Test_RemoveAllPredicate_Implementation(new[] { 1, 3 }, new List<int> { 1, 2, 3 }, x => x is 2 or 4);
            Test_RemoveAllPredicate_Implementation(new[] { 1, 2, 3 }, new List<int> { 1, 2, 3 }, x => x > 3);
        }

        private void Test_RemoveAllPredicate_Implementation<T>(
            IEnumerable<T> expected, ICollection<T> item,  Func<T, bool> predicate)
        {
            var list = item.Where(predicate).ToList();
            foreach(var x in list)
            {
                Assert.Contains(x, item);
            }

            var expectedCount = item.Count - list.Count;
            Assert.True(item.RemoveAll(predicate));
            Assert.Equal(expectedCount, item.Count);

            foreach(var x in list)
            {
                Assert.DoesNotContain(x, item);
            }

            Assert.Equal(expected, item);
        }

        #endregion

        #region IEnumerable

        [Fact]
        public void Test_IEnumerable_IsEmpty()
        {
            Test_IEnumerable_IsEmpty_Implementation(false, new[] { 4, 5, 6 });
            Test_IEnumerable_IsEmpty_Implementation(true, Array.Empty<int>());

            var list = new List<string> { "Hello", "World", "Test" };
            Test_IEnumerable_IsEmpty_Implementation(false, list);

            list.RemoveAt(0);
            Test_IEnumerable_IsEmpty_Implementation(false, list);

            list.Clear();
            Test_IEnumerable_IsEmpty_Implementation(true, list);

            Test_IEnumerable_IsEmpty_Implementation(false, new ExtensionUtilsTests().ScalarToList());
            Test_IEnumerable_IsEmpty_Implementation(true, Array.Empty<ExtensionUtilsTests>());
        }

        private void Test_IEnumerable_IsEmpty_Implementation<T>(bool expected, IEnumerable<T> source)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.IsEmpty());
            Assert.Equal(!array.Any(), array.IsEmpty());
        }

        [Fact]
        public void Test_NotAll()
        {
            Test_NotAll_Implementation(true, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotAll_Implementation(true, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotAll_Implementation(false, new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotAll_Implementation(false, Array.Empty<int>(), x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAll_Implementation(true, list, x => x == "Hello");
            Test_NotAll_Implementation(true, list, x => x != "Hello");
            Test_NotAll_Implementation(false, list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotAll_Implementation(true, list, x => x == "Hello");
            Test_NotAll_Implementation(false, list, x => x != "Hello");
            Test_NotAll_Implementation(false, list, x => x != "Hello World");

            list.Clear();
            Test_NotAll_Implementation(false, list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotAll_Implementation(false, list, x => x == "Hello");
            Test_NotAll_Implementation(true, list, x => x != "Hello");
        }

        private void Test_NotAll_Implementation<T>(bool expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAll(predicate));
            Assert.Equal(!array.All(predicate), array.NotAll(predicate));
        }

        [Fact]
        public void Test_NotAny()
        {
            Test_NotAny_Implementation(false, new[] { 4, 5, 6 });
            Test_NotAny_Implementation(true, Array.Empty<int>());

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAny_Implementation(false, list);

            list.RemoveAt(0);
            Test_NotAny_Implementation(false, list);

            list.Clear();
            Test_NotAny_Implementation(true, list);

            Test_NotAny_Implementation(false, new ExtensionUtilsTests().ScalarToList());
            Test_NotAny_Implementation(true, Array.Empty<ExtensionUtilsTests>());
        }

        private void Test_NotAny_Implementation<T>(bool expected, IEnumerable<T> source)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAny());
            Assert.Equal(!array.Any(), array.NotAny());
        }

        [Fact]
        public void Test_NotAnyPredicate()
        {
            Test_NotAnyPredicate_Implementation(false, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotAnyPredicate_Implementation(true, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotAnyPredicate_Implementation(false, new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotAnyPredicate_Implementation(true, Array.Empty<int>(), x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotAnyPredicate_Implementation(false, list, x => x == "Hello");
            Test_NotAnyPredicate_Implementation(false, list, x => x != "Hello");
            Test_NotAnyPredicate_Implementation(false, list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotAnyPredicate_Implementation(true, list, x => x == "Hello");
            Test_NotAnyPredicate_Implementation(false, list, x => x != "Hello");
            Test_NotAnyPredicate_Implementation(false, list, x => x != "Hello World");

            list.Clear();
            Test_NotAnyPredicate_Implementation(true, list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotAnyPredicate_Implementation(false, list, x => x == "Hello");
            Test_NotAnyPredicate_Implementation(true, list, x => x != "Hello");
        }

        private void Test_NotAnyPredicate_Implementation<T>(
            bool expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotAny(predicate));
            Assert.Equal(!array.Any(predicate), array.NotAny(predicate));
        }

        [Fact]
        public void Test_NotNull()
        {
            Test_NotNull_Implementation(new[] { "Hello", null, "World", null });
            Test_NotNull_Implementation(new[] { "Hello", "World" });
            Test_NotNull_Implementation(new[] { 1, 2, 3 });
            Test_NotNull_Implementation(new int?[] { 1, null, 2, null, 3, null });
            Test_NotNull_Implementation(new object[] { 1, "Hello", 2, null, 3, "World", null });
        }

        private void Test_NotNull_Implementation<T>(IEnumerable<T> source) =>
            Assert.DoesNotContain(source.NotNull(), x => x.IsNull());

        [Fact]
        public void Test_NotWhere()
        {
            Test_NotWhere_Implementation(new[] { 5 }, new[] { 4, 5, 6 }, x => x % 2 == 0);
            Test_NotWhere_Implementation(new[] { 3, 5, 7 }, new[] { 3, 5, 7 }, x => x % 2 == 0);
            Test_NotWhere_Implementation(Array.Empty<int>(), new[] { 4, 6, 8 }, x => x % 2 == 0);
            Test_NotWhere_Implementation(Array.Empty<int>(), Array.Empty<int>(), x => x % 2 == 0);

            var list = new List<string> { "Hello", "World", "Test" };
            Test_NotWhere_Implementation(new[] { "World", "Test" }, list, x => x == "Hello");
            Test_NotWhere_Implementation(new[] { "Hello" }, list, x => x != "Hello");
            Test_NotWhere_Implementation(Array.Empty<string>(), list, x => x != "Hello World");

            list.RemoveAt(0);
            Test_NotWhere_Implementation(new[] { "World", "Test" }, list, x => x == "Hello");
            Test_NotWhere_Implementation(Array.Empty<string>(), list, x => x != "Hello");
            Test_NotWhere_Implementation(Array.Empty<string>(), list, x => x != "Hello World");

            list.Clear();
            Test_NotWhere_Implementation(Array.Empty<string>(), list, x => x == "Hello");

            list.AddRange(new[] { "Hello", "Hello", "Hello" });
            Test_NotWhere_Implementation(Array.Empty<string>(), list, x => x == "Hello");
            Test_NotWhere_Implementation(new[] { "Hello", "Hello", "Hello" }, list, x => x != "Hello");
        }

        private void Test_NotWhere_Implementation<T>(
            IEnumerable<T> expected, IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            Assert.Equal(expected, array.NotWhere(predicate));
            Assert.Equal(array.Where(x => !predicate(x)), array.NotWhere(predicate));
        }

        [Fact]
        public void Test_ToEnumerableEmptyIfNull()
        {
            // ReSharper disable ExpressionIsAlwaysNull
            var intArray = new[] { 1, 2, 3, 5, 10 };
            Assert.Equal(intArray, intArray.ToEnumerableEmptyIfNull());

            intArray = null;
            Assert.NotNull(intArray.ToEnumerableEmptyIfNull());
            Assert.Equal(Array.Empty<int>(), intArray.ToEnumerableEmptyIfNull());
            Assert.Equal(Enumerable.Empty<int>(), intArray.ToEnumerableEmptyIfNull());

            var stringArray = new[] { "Hello", "World", "Test" };
            Assert.Equal(stringArray, stringArray.ToEnumerableEmptyIfNull());

            stringArray = null;
            Assert.NotNull(stringArray.ToEnumerableEmptyIfNull());
            Assert.Equal(Array.Empty<string>(), stringArray.ToEnumerableEmptyIfNull());
            Assert.Equal(Enumerable.Empty<string>(), stringArray.ToEnumerableEmptyIfNull());
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public void Test_Yield()
        {
            const int i = 10;
            var enumerable = Assert.IsAssignableFrom<IEnumerable<int>>(i.Yield());
            var array = enumerable.ToArray();
            Assert.Single(array);
            Assert.Equal(i, array.First());
        }

        #endregion

        #region Numbers

        [Fact]
        public void Test_Decimal_HasMoreThanNDecimalPlaces()
        {
            // 0 decimal place.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 0m, 0);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 0m, 1);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 0m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1m, 0);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(true, 1.1m, 0);

            // 1 decimal place.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1m, 1);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1.1m, 1);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(true, 1.15m, 1);

            // 2 decimal places.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1.1m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1.15m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(true, 1.155m, 2);

            // 3 decimal places.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, 1.155m, 3);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(true, 1.1551m, 3);

            // Negative values, 2 decimal places.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, -1m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, -1.1m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(false, -1.15m, 2);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(true, -1.155m, 2);

            // Negative n.
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(null, 1m, -1);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(null, 1.1m, -10);
            Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(null, 1.55m, -20);
        }

        private void Test_Decimal_HasMoreThanNDecimalPlaces_Implementation(bool? expected, decimal item, int n)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentOutOfRangeException>(() => item.HasMoreThanNDecimalPlaces(n));
            else
                Assert.Equal(expected.Value, item.HasMoreThanNDecimalPlaces(n));
        }

        [Theory]
        // 0 decimal place.
        [InlineData(false, 0d, 0)]
        [InlineData(false, 0d, 1)]
        [InlineData(false, 0d, 2)]
        [InlineData(false, 1d, 0)]
        [InlineData(true, 1.1d, 0)]
        // 1 decimal place.
        [InlineData(false, 1d, 1)]
        [InlineData(false, 1.1d, 1)]
        [InlineData(true, 1.15d, 1)]
        // 2 decimal places.
        [InlineData(false, 1d, 2)]
        [InlineData(false, 1.1d, 2)]
        [InlineData(false, 1.15d, 2)]
        [InlineData(true, 1.155d, 2)]
        // 3 decimal places.
        [InlineData(false, 1.155d, 3)]
        [InlineData(true, 1.1551d, 3)]
        // Negative values, 2 decimal places.
        [InlineData(false, -1d, 2)]
        [InlineData(false, -1.1d, 2)]
        [InlineData(false, -1.15d, 2)]
        [InlineData(true, -1.155d, 2)]
        // Negative n.
        [InlineData(null, 1d, -1)]
        [InlineData(null, 1.1d, -10)]
        [InlineData(null, 1.55d, -20)]
        public void Test_Double_HasMoreThanNDecimalPlaces(bool? expected, double item, int n)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentOutOfRangeException>(() => item.HasMoreThanNDecimalPlaces(n));
            else
                Assert.Equal(expected.Value, item.HasMoreThanNDecimalPlaces(n));
        }

        [Fact]
        public void Test_Decimal_HasMoreThanTwoDecimalPlaces()
        {
            // 0 decimal place.
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, 0m);

            // 2 decimal places.
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, 1m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, 1.1m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, 1.15m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(true, 1.155m);

            // Negative values, 2 decimal places.
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, -1m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, -1.1m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(false, -1.15m);
            Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(true, -1.155m);
        }

        private void Test_Decimal_HasMoreThanTwoDecimalPlaces_Implementation(bool? expected, decimal item)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentOutOfRangeException>(() => item.HasMoreThanTwoDecimalPlaces());
            else
                Assert.Equal(expected.Value, item.HasMoreThanTwoDecimalPlaces());
        }

        [Theory]
        // 0 decimal place.
        [InlineData(false, 0d)]
        // 2 decimal places.
        [InlineData(false, 1d)]
        [InlineData(false, 1.1d)]
        [InlineData(false, 1.15d)]
        [InlineData(true, 1.155d)]
        // Negative values, 2 decimal places.
        [InlineData(false, -1d)]
        [InlineData(false, -1.1d)]
        [InlineData(false, -1.15d)]
        [InlineData(true, -1.155d)]
        public void Test_Double_HasMoreThanTwoDecimalPlaces(bool? expected, double item)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentOutOfRangeException>(() => item.HasMoreThanTwoDecimalPlaces());
            else
                Assert.Equal(expected.Value, item.HasMoreThanTwoDecimalPlaces());
        }

        [Theory]
        // Zero min & max.
        [InlineData(true, 0, 0, 0)]
        [InlineData(false, 1, 0, 0)]
        [InlineData(false, -1, 0, 0)]
        // Positive max.
        [InlineData(true, 0, 0, 10)]
        [InlineData(true, 1, 0, 10)]
        [InlineData(true, 5, 0, 10)]
        [InlineData(true, 10, 0, 10)]
        [InlineData(false, 11, 0, 10)]
        [InlineData(false, -1, 0, 10)]
        // Negative min.
        [InlineData(true, 0, -10, 0)]
        [InlineData(true, -1, -10, 0)]
        [InlineData(true, -5, -10, 0)]
        [InlineData(true, -10, -10, 0)]
        [InlineData(false, -11, -10, 0)]
        [InlineData(false, 1, -10, 0)]
        // Negative min & max.
        [InlineData(true, -5, -10, -5)]
        [InlineData(true, -10, -10, -5)]
        [InlineData(false, -11, -10, -5)]
        [InlineData(false, -4, -10, -5)]
        // Negative min & positive max.
        [InlineData(true, -10, -10, 10)]
        [InlineData(true, 0, -10, 10)]
        [InlineData(true, 10, -10, 10)]
        [InlineData(false, 11, -10, 10)]
        [InlineData(false, -11, -10, 10)]
        // Min is greater than max.
        [InlineData(null, 0, 10, 0)]
        [InlineData(null, 0, 0, -10)]
        [InlineData(null, 0, 10, -10)]
        public void Test_Int_IsInRange(bool? expected, int item, int min, int max)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentException>(() => item.IsInRange(min, max));
            else
                Assert.Equal(expected.Value, item.IsInRange(min, max));
        }

        [Fact]
        public void Test_Decimal_IsInRange()
        {
            // Zero min & max.
            var min = 0m;
            var max = 0m;
            Test_Decimal_IsInRange_Implementation(true, 0, min, max);
            Test_Decimal_IsInRange_Implementation(false, 1, min, max);
            Test_Decimal_IsInRange_Implementation(false, -1, min, max);

            // Positive max.
            min = 0;
            max = 10;
            Test_Decimal_IsInRange_Implementation(true, 0, min, max);
            Test_Decimal_IsInRange_Implementation(true, 1, min, max);
            Test_Decimal_IsInRange_Implementation(true, 5, min, max);
            Test_Decimal_IsInRange_Implementation(true, 10, min, max);
            Test_Decimal_IsInRange_Implementation(false, 11, min, max);
            Test_Decimal_IsInRange_Implementation(false, -1, min, max);

            // Negative min.
            min = -10;
            max = 0;
            Test_Decimal_IsInRange_Implementation(true, 0, min, max);
            Test_Decimal_IsInRange_Implementation(true, -1, min, max);
            Test_Decimal_IsInRange_Implementation(true, -5, min, max);
            Test_Decimal_IsInRange_Implementation(true, -10, min, max);
            Test_Decimal_IsInRange_Implementation(false, -11, min, max);
            Test_Decimal_IsInRange_Implementation(false, 1, min, max);

            // Negative min & max.
            min = -10;
            max = -5;
            Test_Decimal_IsInRange_Implementation(true, -5, min, max);
            Test_Decimal_IsInRange_Implementation(true, -10, min, max);
            Test_Decimal_IsInRange_Implementation(false, -11, min, max);
            Test_Decimal_IsInRange_Implementation(false, -4, min, max);

            // Negative min & positive max.
            min = -10;
            max = 10;
            Test_Decimal_IsInRange_Implementation(true, -10, min, max);
            Test_Decimal_IsInRange_Implementation(true, 0, min, max);
            Test_Decimal_IsInRange_Implementation(true, 10, min, max);
            Test_Decimal_IsInRange_Implementation(false, 11, min, max);
            Test_Decimal_IsInRange_Implementation(false, -11, min, max);

            // Min is greater than max.
            Test_Decimal_IsInRange_Implementation(null, 0, 10, 0);
            Test_Decimal_IsInRange_Implementation(null, 0, 0, -10);
            Test_Decimal_IsInRange_Implementation(null, 0, 10, -10);

            // Decimal places.
            min = -10.5m;
            max = 10.5m;
            Test_Decimal_IsInRange_Implementation(true, 0, min, max);
            Test_Decimal_IsInRange_Implementation(true, 0.1m, min, max);
            Test_Decimal_IsInRange_Implementation(true, -0.1m, min, max);
            Test_Decimal_IsInRange_Implementation(true, 10, min, max);
            Test_Decimal_IsInRange_Implementation(true, 10.1m, min, max);
            Test_Decimal_IsInRange_Implementation(true, 10.5m, min, max);
            Test_Decimal_IsInRange_Implementation(false, 10.6m, min, max);
            Test_Decimal_IsInRange_Implementation(false, 11, min, max);
            Test_Decimal_IsInRange_Implementation(true, -10, min, max);
            Test_Decimal_IsInRange_Implementation(true, -10.1m, min, max);
            Test_Decimal_IsInRange_Implementation(true, -10.5m, min, max);
            Test_Decimal_IsInRange_Implementation(false, -10.6m, min, max);
            Test_Decimal_IsInRange_Implementation(false, -11, min, max);
        }

        private void Test_Decimal_IsInRange_Implementation(bool? expected, decimal item, decimal min, decimal max)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentException>(() => item.IsInRange(min, max));
            else
                Assert.Equal(expected.Value, item.IsInRange(min, max));
        }

        [Theory]
        // Zero min & max.
        [InlineData(true, 0d, 0d, 0d)]
        [InlineData(false, 1d, 0d, 0d)]
        [InlineData(false, -1d, 0d, 0d)]
        // Positive max.
        [InlineData(true, 0d, 0d, 10d)]
        [InlineData(true, 1d, 0d, 10d)]
        [InlineData(true, 5d, 0d, 10d)]
        [InlineData(true, 10d, 0d, 10d)]
        [InlineData(false, 11d, 0d, 10d)]
        [InlineData(false, -1d, 0d, 10d)]
        // Negative min.
        [InlineData(true, 0d, -10d, 0d)]
        [InlineData(true, -1d, -10d, 0d)]
        [InlineData(true, -5d, -10d, 0d)]
        [InlineData(true, -10d, -10d, 0d)]
        [InlineData(false, -11d, -10d, 0d)]
        [InlineData(false, 1d, -10d, 0d)]
        // Negative min & max.
        [InlineData(true, -5d, -10d, -5d)]
        [InlineData(true, -10d, -10d, -5d)]
        [InlineData(false, -11d, -10d, -5d)]
        [InlineData(false, -4d, -10d, -5d)]
        // Negative min & positive max.
        [InlineData(true, -10d, -10d, 10d)]
        [InlineData(true, 0d, -10d, 10d)]
        [InlineData(true, 10d, -10d, 10d)]
        [InlineData(false, 11d, -10d, 10d)]
        [InlineData(false, -11d, -10d, 10d)]
        // Min is greater than max.
        [InlineData(null, 0d, 10d, 0d)]
        [InlineData(null, 0d, 0d, -10d)]
        [InlineData(null, 0d, 10d, -10d)]
        // Decimal places.
        [InlineData(true, 0d, -10.5, 10.5)]
        [InlineData(true, 0.1, -10.5, 10.5)]
        [InlineData(true, -0.1, -10.5, 10.5)]
        [InlineData(true, 10d, -10.5, 10.5)]
        [InlineData(true, 10.1, -10.5, 10.5)]
        [InlineData(true, 10.5, -10.5, 10.5)]
        [InlineData(false, 10.6, -10.5, 10.5)]
        [InlineData(false, 11d, -10.5, 10.5)]
        [InlineData(true, -10d, -10.5, 10.5)]
        [InlineData(true, -10.1, -10.5, 10.5)]
        [InlineData(true, -10.5, -10.5, 10.5)]
        [InlineData(false, -10.6, -10.5, 10.5)]
        [InlineData(false, -11d, -10.5, 10.5)]
        public void Test_Double_IsInRange(bool? expected, double item, double min, double max)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentException>(() => item.IsInRange(min, max));
            else
                Assert.Equal(expected.Value, item.IsInRange(min, max));
        }

        [Fact]
        public void Test_DateTime_InInRange()
        {
            var today = DateTime.Today;
            var min = today.AddDays(-10);
            var max = today.AddDays(20);
            Assert.True(today.IsInRange(min, max));
            Assert.True(today.AddDays(1).IsInRange(min, max));
            Assert.True(today.AddDays(5).IsInRange(min, max));
            Assert.True(today.AddDays(10).IsInRange(min, max));
            Assert.True(today.AddDays(15).IsInRange(min, max));
            Assert.True(today.AddDays(20).IsInRange(min, max));
            Assert.False(today.AddDays(21).IsInRange(min, max));
            Assert.True(today.AddDays(-1).IsInRange(min, max));
            Assert.True(today.AddDays(-5).IsInRange(min, max));
            Assert.True(today.AddDays(-10).IsInRange(min, max));
            Assert.False(today.AddDays(-11).IsInRange(min, max));

            Assert.True(today.IsInRange(today, today));
            Assert.False(today.AddDays(1).IsInRange(today, today));
            Assert.False(today.AddDays(-1).IsInRange(today, today));

            Assert.Throws<ArgumentException>(() => today.IsInRange(max, min));
        }

        [Theory]
        // Zero min & max.
        [InlineData(true, 0f, 0f, 0f)]
        [InlineData(false, 1f, 0f, 0f)]
        [InlineData(false, -1f, 0f, 0f)]
        // Positive max.
        [InlineData(true, 0f, 0f, 10f)]
        [InlineData(true, 1f, 0f, 10f)]
        [InlineData(true, 5f, 0f, 10f)]
        [InlineData(true, 10f, 0f, 10f)]
        [InlineData(false, 11f, 0f, 10f)]
        [InlineData(false, -1f, 0f, 10f)]
        // Negative min.
        [InlineData(true, 0f, -10f, 0f)]
        [InlineData(true, -1f, -10f, 0f)]
        [InlineData(true, -5f, -10f, 0f)]
        [InlineData(true, -10f, -10f, 0f)]
        [InlineData(false, -11f, -10f, 0f)]
        [InlineData(false, 1f, -10f, 0f)]
        // Negative min & max.
        [InlineData(true, -5f, -10f, -5f)]
        [InlineData(true, -10f, -10f, -5f)]
        [InlineData(false, -11f, -10f, -5f)]
        [InlineData(false, -4f, -10f, -5f)]
        // Negative min & positive max.
        [InlineData(true, -10f, -10f, 10f)]
        [InlineData(true, 0f, -10f, 10f)]
        [InlineData(true, 10f, -10f, 10f)]
        [InlineData(false, 11f, -10f, 10f)]
        [InlineData(false, -11f, -10f, 10f)]
        // Min is greater than max.
        [InlineData(null, 0f, 10f, 0f)]
        [InlineData(null, 0f, 0f, -10f)]
        [InlineData(null, 0f, 10f, -10f)]
        // Decimal places.
        [InlineData(true, 0f, -10.5f, 10.5f)]
        [InlineData(true, 0.1f, -10.5f, 10.5f)]
        [InlineData(true, -0.1f, -10.5f, 10.5f)]
        [InlineData(true, 10f, -10.5f, 10.5f)]
        [InlineData(true, 10.1f, -10.5f, 10.5f)]
        [InlineData(true, 10.5f, -10.5f, 10.5f)]
        [InlineData(false, 10.6f, -10.5f, 10.5f)]
        [InlineData(false, 11f, -10.5f, 10.5f)]
        [InlineData(true, -10f, -10.5f, 10.5f)]
        [InlineData(true, -10.1f, -10.5f, 10.5f)]
        [InlineData(true, -10.5f, -10.5f, 10.5f)]
        [InlineData(false, -10.6f, -10.5f, 10.5f)]
        [InlineData(false, -11f, -10.5f, 10.5f)]
        public void Test_IsInRange(bool? expected, float item, float min, float max)
        {
            if(!expected.HasValue)
                Assert.Throws<ArgumentException>(() => item.IsInRange(min, max));
            else
                Assert.Equal(expected.Value, item.IsInRange(min, max));
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(false, 1)]
        [InlineData(true, -1)]
        [InlineData(false, int.MaxValue)]
        [InlineData(true, int.MinValue)]
        public void Test_Int_IsNegative(bool expected, int item) => Assert.Equal(expected, item.IsNegative());

        [Fact]
        public void Test_Decimal_IsNegative()
        {
            Assert.False(0m.IsNegative());
            Assert.False(0.1m.IsNegative());
            Assert.True((-0.1m).IsNegative());
            Assert.False(decimal.Zero.IsNegative());
            Assert.False(decimal.MaxValue.IsNegative());
            Assert.True(decimal.MinValue.IsNegative());
        }

        [Theory]
        [InlineData(false, 0d)]
        [InlineData(false, 0.1)]
        [InlineData(true, -0.1)]
        [InlineData(false, double.MaxValue)]
        [InlineData(true, double.MinValue)]
        public void Test_Double_IsNegative(bool expected, double item) => Assert.Equal(expected, item.IsNegative());

        [Theory]
        [InlineData(false, 0f)]
        [InlineData(false, 0.1f)]
        [InlineData(true, -0.1f)]
        [InlineData(false , float.MaxValue)]
        [InlineData(true, float.MinValue)]
        public void Test_IsNegative(bool expected, float item) => Assert.Equal(expected, item.IsNegative());

        [Fact]
        public void TestFact_IsNegative()
        {
            Assert.False(new DateTime().IsNegative());
            Assert.False(DateTime.MaxValue.IsNegative());
            Assert.False(DateTime.MinValue.IsNegative());
            Assert.False(DateTime.Today.IsNegative());
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        [InlineData(false, -1)]
        [InlineData(true, int.MaxValue)]
        [InlineData(false, int.MinValue)]
        public void Test_Int_IsPositive(bool expected, int item) => Assert.Equal(expected, item.IsPositive());

        [Fact]
        public void Test_Decimal_IsPositive()
        {
            Assert.False(0m.IsPositive());
            Assert.True(0.1m.IsPositive());
            Assert.False((-0.1m).IsPositive());
            Assert.False(decimal.Zero.IsPositive());
            Assert.True(decimal.MaxValue.IsPositive());
            Assert.False(decimal.MinValue.IsPositive());
        }

        [Theory]
        [InlineData(false, 0d)]
        [InlineData(true, 0.1)]
        [InlineData(false, -0.1)]
        [InlineData(true, double.MaxValue)]
        [InlineData(false, double.MinValue)]
        public void Test_Double_IsPositive(bool expected, double item) => Assert.Equal(expected, item.IsPositive());

        [Theory]
        [InlineData(false, 0f)]
        [InlineData(true, 0.1f)]
        [InlineData(false, -0.1f)]
        [InlineData(true, float.MaxValue)]
        [InlineData(false, float.MinValue)]
        public void Test_IsPositive(bool expected, float item) => Assert.Equal(expected, item.IsPositive());

        [Fact]
        public void TestFact_IsPositive()
        {
            Assert.False(new DateTime().IsPositive());
            Assert.True(DateTime.MaxValue.IsPositive());
            Assert.False(DateTime.MinValue.IsPositive());
            Assert.True(DateTime.Today.IsPositive());
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        [InlineData(false, -1)]
        [InlineData(false, int.MaxValue)]
        [InlineData(false, int.MinValue)]
        public void Test_Int_IsZero(bool expected, int item) => Assert.Equal(expected, item.IsZero());

        [Fact]
        public void Test_Decimal_IsZero()
        {
            Assert.True(0m.IsZero());
            Assert.False(0.1m.IsZero());
            Assert.False((-0.1m).IsZero());
            Assert.True(decimal.Zero.IsZero());
            Assert.False(decimal.MaxValue.IsZero());
            Assert.False(decimal.MinValue.IsZero());
        }

        [Theory]
        [InlineData(true, 0d)]
        [InlineData(false, 0.1)]
        [InlineData(false, -0.1)]
        [InlineData(false, double.MaxValue)]
        [InlineData(false, double.MinValue)]
        public void Test_Double_IsZero(bool expected, double item) => Assert.Equal(expected, item.IsZero());

        [Theory]
        [InlineData(true, 0f)]
        [InlineData(false, 0.1f)]
        [InlineData(false, -0.1f)]
        [InlineData(false, float.MaxValue)]
        [InlineData(false, float.MinValue)]
        public void Test_IsZero(bool expected, float item) => Assert.Equal(expected, item.IsZero());

        [Fact]
        public void TestFact_IsZero()
        {
            Assert.True(new DateTime().IsZero());
            Assert.False(DateTime.MaxValue.IsZero());
            Assert.True(DateTime.MinValue.IsZero());
            Assert.False(DateTime.Today.IsZero());
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        [InlineData(true, -1)]
        [InlineData(false, int.MaxValue)]
        [InlineData(true, int.MinValue)]
        public void Test_Int_IsZeroOrNegative(bool expected, int item) =>
            Assert.Equal(expected, item.IsZeroOrNegative());

        [Fact]
        public void Test_Decimal_IsZeroOrNegative()
        {
            Assert.True(0m.IsZeroOrNegative());
            Assert.False(0.1m.IsZeroOrNegative());
            Assert.True((-0.1m).IsZeroOrNegative());
            Assert.True(decimal.Zero.IsZeroOrNegative());
            Assert.False(decimal.MaxValue.IsZeroOrNegative());
            Assert.True(decimal.MinValue.IsZeroOrNegative());
        }

        [Theory]
        [InlineData(true, 0d)]
        [InlineData(false, 0.1)]
        [InlineData(true, -0.1)]
        [InlineData(false, double.MaxValue)]
        [InlineData(true, double.MinValue)]
        public void Test_Double_IsZeroOrNegative(bool expected, double item) =>
            Assert.Equal(expected, item.IsZeroOrNegative());

        [Theory]
        [InlineData(true, 0f)]
        [InlineData(false, 0.1f)]
        [InlineData(true, -0.1f)]
        [InlineData(false, float.MaxValue)]
        [InlineData(true, float.MinValue)]
        public void Test_IsZeroOrNegative(bool expected, float item) =>
            Assert.Equal(expected, item.IsZeroOrNegative());

        [Fact]
        public void TestFact_IsZeroOrNegative()
        {
            Assert.True(new DateTime().IsZeroOrNegative());
            Assert.False(DateTime.MaxValue.IsZeroOrNegative());
            Assert.True(DateTime.MinValue.IsZeroOrNegative());
            Assert.False(DateTime.Today.IsZeroOrNegative());
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(true, 1)]
        [InlineData(false, -1)]
        [InlineData(true, int.MaxValue)]
        [InlineData(false, int.MinValue)]
        public void Test_Int_IsZeroOrPositive(bool expected, int item) =>
            Assert.Equal(expected, item.IsZeroOrPositive());

        [Fact]
        public void Test_Decimal_IsZeroOrPositive()
        {
            Assert.True(0m.IsZeroOrPositive());
            Assert.True(0.1m.IsZeroOrPositive());
            Assert.False((-0.1m).IsZeroOrPositive());
            Assert.True(decimal.Zero.IsZeroOrPositive());
            Assert.True(decimal.MaxValue.IsZeroOrPositive());
            Assert.False(decimal.MinValue.IsZeroOrPositive());
        }

        [Theory]
        [InlineData(true, 0d)]
        [InlineData(true, 0.1)]
        [InlineData(false, -0.1)]
        [InlineData(true, double.MaxValue)]
        [InlineData(false, double.MinValue)]
        public void Test_Double_IsZeroOrPositive(bool expected, double item) =>
            Assert.Equal(expected, item.IsZeroOrPositive());

        [Theory]
        [InlineData(true, 0f)]
        [InlineData(true, 0.1f)]
        [InlineData(false, -0.1f)]
        [InlineData(true, float.MaxValue)]
        [InlineData(false, float.MinValue)]
        public void Test_IsZeroOrPositive(bool expected, float item) =>
            Assert.Equal(expected, item.IsZeroOrPositive());

        [Fact]
        public void TestFact_IsZeroOrPositive()
        {
            Assert.True(new DateTime().IsZeroOrPositive());
            Assert.True(DateTime.MaxValue.IsZeroOrPositive());
            Assert.True(DateTime.MinValue.IsZeroOrPositive());
            Assert.True(DateTime.Today.IsZeroOrPositive());
        }

        #endregion

        #region Object

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

        [Theory]
        [InlineData("Test")]
        [InlineData(10)]
        [InlineData(null)]
        public void Test_ToDbNullIfNull(object item)
        {
            var result = item.ToDbNullIfNull();
            if(item != null)
                Assert.Same(item, result);
            else
                Assert.Equal(DBNull.Value, result);
        }

        #endregion

        #region String

        [Theory]
        [InlineData("", "", 0)]
        [InlineData("Hello", "Hello", 10)]
        [InlineData("Hello W...", "Hello World", 10)]
        [InlineData("HelloWo...", "HelloWorldTest", 10)]
        [InlineData("Hello World", "Hello World", 11)]
        public void Test_Abbreviate(string expected, string item, int length)
        {
            Assert.Equal(expected, item.Abbreviate(length));
        }

        [Fact]
        public void TestFact_Abbreviate()
        {
            const string item = "Hello";
            Assert.Equal(item, item.Abbreviate(5));
            Assert.Equal("H...", item.Abbreviate(4));
            Assert.Throws<ArgumentException>(() => item.Abbreviate(3));
            Assert.Throws<ArgumentException>(() => item.Abbreviate(2));
            Assert.Throws<ArgumentException>(() => item.Abbreviate(1));
            Assert.Throws<ArgumentException>(() => item.Abbreviate(0));
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
        [InlineData(false, "-5")]
        [InlineData(true, "5.5")]
        [InlineData(true, "-5.5")]
        [InlineData(true, "0.95")]
        [InlineData(true, ".95")]
        [InlineData(false, "100")]
        [InlineData(true, "100.1")]
        [InlineData(false, "0")]
        [InlineData(true, "0.0")]
        [InlineData(true, "a5")]
        [InlineData(true, "5a")]
        [InlineData(false, " 5")]
        [InlineData(false, "5 ")]
        public void Test_IsInvalidInt(bool expected, string item) => Assert.Equal(expected, item.IsInvalidInt());

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "5")]
        [InlineData(false, "-5")]
        [InlineData(false, "5.5")]
        [InlineData(false, "-5.5")]
        [InlineData(false, "0.95")]
        [InlineData(false, ".95")]
        [InlineData(false, "100")]
        [InlineData(false, "100.1")]
        [InlineData(false, "0")]
        [InlineData(false, "0.0")]
        [InlineData(true, "a5")]
        [InlineData(true, "5a")]
        [InlineData(false, " 5")]
        [InlineData(false, "5 ")]
        public void Test_IsInvalidDecimal(bool expected, string item)
        {
            Assert.Equal(expected, item.IsInvalidDecimal());
        }

        [Theory]
        [InlineData(true, "test")]
        [InlineData(false, "5")]
        [InlineData(false, "-5")]
        [InlineData(false, "5.5")]
        [InlineData(false, "-5.5")]
        [InlineData(false, "0.95")]
        [InlineData(false, ".95")]
        [InlineData(false, "100")]
        [InlineData(false, "100.1")]
        [InlineData(false, "0")]
        [InlineData(false, "0.0")]
        [InlineData(true, "a5")]
        [InlineData(true, "5a")]
        [InlineData(false, " 5")]
        [InlineData(false, "5 ")]
        public void Test_IsInvalidDouble(bool expected, string item) => Assert.Equal(expected, item.IsInvalidDouble());

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
        [InlineData(true, "-250.95")]
        [InlineData(true, "250.95 ")]
        [InlineData(true, " 250.95")]
        [InlineData(true, "a5")]
        [InlineData(true, "5a")]
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
        [InlineData(false, "test")]
        [InlineData(true, "test@gmail.com")]
        public void Test_IsValidEmail(bool expected, string item) => Assert.Equal(expected, item.IsValidEmail());

        [Theory]
        [InlineData(false, "test")]
        [InlineData(true, "5")]
        [InlineData(true, "-5")]
        [InlineData(false, "5.5")]
        [InlineData(false, "-5.5")]
        [InlineData(false, "0.95")]
        [InlineData(false, ".95")]
        [InlineData(true, "100")]
        [InlineData(false, "100.1")]
        [InlineData(true, "0")]
        [InlineData(false, "0.0")]
        [InlineData(false, "a5")]
        [InlineData(false, "5a")]
        [InlineData(true, " 5")]
        [InlineData(true, "5 ")]
        public void Test_IsValidInt(bool expected, string item) => Assert.Equal(expected, item.IsValidInt());

        [Theory]
        [InlineData(false , "test")]
        [InlineData(true, "5")]
        [InlineData(true, "-5")]
        [InlineData(true, "5.5")]
        [InlineData(true, "-5.5")]
        [InlineData(true, "0.95")]
        [InlineData(true, ".95")]
        [InlineData(true, "100")]
        [InlineData(true, "100.1")]
        [InlineData(true, "0")]
        [InlineData(true, "0.0")]
        [InlineData(false, "a5")]
        [InlineData(false, "5a")]
        [InlineData(true, " 5")]
        [InlineData(true, "5 ")]
        public void Test_IsValidDecimal(bool expected, string item) => Assert.Equal(expected, item.IsValidDecimal());

        [Theory]
        [InlineData(false, "test")]
        [InlineData(true, "5")]
        [InlineData(true, "-5")]
        [InlineData(true, "5.5")]
        [InlineData(true, "-5.5")]
        [InlineData(true, "0.95")]
        [InlineData(true, ".95")]
        [InlineData(true, "100")]
        [InlineData(true, "100.1")]
        [InlineData(true, "0")]
        [InlineData(true, "0.0")]
        [InlineData(false, "a5")]
        [InlineData(false, "5a")]
        [InlineData(true, " 5")]
        [InlineData(true, "5 ")]
        public void Test_IsValidDouble(bool expected, string item) => Assert.Equal(expected, item.IsValidDouble());

        [Theory]
        [InlineData(false, "test")]
        [InlineData(false, "-1")]
        [InlineData(false, "1.951")]
        [InlineData(false, ".5")]
        [InlineData(true, "1")]
        [InlineData(true, "0.5")]
        [InlineData(true, "1.5")]
        [InlineData(true, "1.95")]
        [InlineData(true, "250")]
        [InlineData(true, "250.95")]
        [InlineData(false, "-250.95")]
        [InlineData(false, "250.95 ")]
        [InlineData(false, " 250.95")]
        [InlineData(false, "a5")]
        [InlineData(false, "5a")]
        public void Test_IsValidMoney(bool expected, string item) => Assert.Equal(expected, item.IsValidMoney());

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
        [InlineData(true, "true")]
        [InlineData(true, "True")]
        [InlineData(true, "TRUE")]
        [InlineData(false, "false")]
        [InlineData(false, "False")]
        [InlineData(false, "FALSE")]
        public void Test_ToBoolean(bool expected, string item) => Assert.Equal(expected, item.ToBoolean());

        [Fact]
        public void TestFact_ToBoolean()
        {
            Assert.Throws<FormatException>(() => "abc".ToBoolean());
            Assert.Throws<ArgumentNullException>(() => ((string) null).ToBoolean());
        }

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void Test_ToDbConnectionStringBuilder(string item) =>
            Assert.IsType<DbConnectionStringBuilder>(item.ToDbConnectionStringBuilder());

        [Fact]
        public void Test_ToDecimal()
        {
            Test_ToDecimal_Implementation(5, "5");
            Test_ToDecimal_Implementation(-5, "-5");
            Test_ToDecimal_Implementation(5.5m, "5.5");
            Test_ToDecimal_Implementation(-5.5m, "-5.5");
            Test_ToDecimal_Implementation(0.95m, "0.95");
            Test_ToDecimal_Implementation(.95m, ".95");
            Test_ToDecimal_Implementation(100, "100");
            Test_ToDecimal_Implementation(100.1m, "100.1");
            Test_ToDecimal_Implementation(0, "0");
            Test_ToDecimal_Implementation(0.0m, "0.0");
            Test_ToDecimal_Implementation(null, "a5");
            Test_ToDecimal_Implementation(null, "5a");
            Test_ToDecimal_Implementation(5, " 5");
            Test_ToDecimal_Implementation(5, "5 ");
        }

        private void Test_ToDecimal_Implementation(decimal? expected, string item)
        {
            if(expected.HasValue)
                Assert.Equal(expected.Value, item.ToDecimal());
            else if(item.IsNull())
                Assert.Throws<ArgumentNullException>(() => item.ToDecimal());
            else
                Assert.Throws<FormatException>(() => item.ToDecimal());
        }

        [Fact]
        public void Test_ToDecimalOrNull()
        {
            Test_ToDecimalOrNull_Implementation(5, "5");
            Test_ToDecimalOrNull_Implementation(-5, "-5");
            Test_ToDecimalOrNull_Implementation(5.5m, "5.5");
            Test_ToDecimalOrNull_Implementation(-5.5m, "-5.5");
            Test_ToDecimalOrNull_Implementation(0.95m, "0.95");
            Test_ToDecimalOrNull_Implementation(.95m, ".95");
            Test_ToDecimalOrNull_Implementation(100, "100");
            Test_ToDecimalOrNull_Implementation(100.1m, "100.1");
            Test_ToDecimalOrNull_Implementation(0, "0");
            Test_ToDecimalOrNull_Implementation(0.0m, "0.0");
            Test_ToDecimalOrNull_Implementation(null, "a5");
            Test_ToDecimalOrNull_Implementation(null, "5a");
            Test_ToDecimalOrNull_Implementation(5, " 5");
            Test_ToDecimalOrNull_Implementation(5, "5 ");
        }

        private void Test_ToDecimalOrNull_Implementation(decimal? expected, string item) =>
            Assert.Equal(expected, item.ToDecimalOrNull());

        [Theory]
        [InlineData("Directory")]
        [InlineData("Directory/SubDirectory")]
        public void Test_ToDirectoryInfo(string item) => Assert.IsType<DirectoryInfo>(item.ToDirectoryInfo());

        [Theory]
        [InlineData(5d, "5")]
        [InlineData(-5d, "-5")]
        [InlineData(5.5, "5.5")]
        [InlineData(-5.5, "-5.5")]
        [InlineData(0.95, "0.95")]
        [InlineData(.95, ".95")]
        [InlineData(100d, "100")]
        [InlineData(100.1, "100.1")]
        [InlineData(0d, "0")]
        [InlineData(0.0, "0.0")]
        [InlineData(null, "a5")]
        [InlineData(null, "5a")]
        [InlineData(5d, " 5")]
        [InlineData(5d, "5 ")]
        public void Test_ToDouble(double? expected, string item)
        {
            if(expected.HasValue)
                Assert.Equal(expected.Value, item.ToDouble());
            else if(item.IsNull())
                Assert.Throws<ArgumentNullException>(() => item.ToDouble());
            else
                Assert.Throws<FormatException>(() => item.ToDouble());
        }

        [Theory]
        [InlineData(5d, "5")]
        [InlineData(-5d, "-5")]
        [InlineData(5.5, "5.5")]
        [InlineData(-5.5, "-5.5")]
        [InlineData(0.95, "0.95")]
        [InlineData(.95, ".95")]
        [InlineData(100d, "100")]
        [InlineData(100.1, "100.1")]
        [InlineData(0d, "0")]
        [InlineData(0.0, "0.0")]
        [InlineData(null, "a5")]
        [InlineData(null, "5a")]
        [InlineData(5d, " 5")]
        [InlineData(5d, "5 ")]
        public void Test_ToDoubleOrNull(double? expected, string item)
        {
            Assert.Equal(expected, item.ToDoubleOrNull());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "   ")]
        [InlineData("", "\n")]
        [InlineData("", null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void Test_ToEmptyIfNullOrWhiteSpace(string expected, string item)
        {
            Assert.Equal(expected, item.ToEmptyIfNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("Test.txt")]
        [InlineData("Directory/Test.txt")]
        public void Test_ToFileInfo(string item) => Assert.IsType<FileInfo>(item.ToFileInfo());

        [Theory]
        [InlineData(5, "5")]
        [InlineData(-5, "-5")]
        [InlineData(null, "5.5")]
        [InlineData(null, "-5.5")]
        [InlineData(null, "0.95")]
        [InlineData(null, ".95")]
        [InlineData(100, "100")]
        [InlineData(null, "100.1")]
        [InlineData(0, "0")]
        [InlineData(null, "0.0")]
        [InlineData(null, "a5")]
        [InlineData(null, "5a")]
        [InlineData(5, " 5")]
        [InlineData(5, "5 ")]
        public void Test_ToInt(int? expected, string item)
        {
            if(expected.HasValue)
                Assert.Equal(expected.Value, item.ToInt());
            else if(item.IsNull())
                Assert.Throws<ArgumentNullException>(() => item.ToInt());
            else
                Assert.Throws<FormatException>(() => item.ToInt());
        }

        [Theory]
        [InlineData(5, "5")]
        [InlineData(-5, "-5")]
        [InlineData(null, "5.5")]
        [InlineData(null, "-5.5")]
        [InlineData(null, "0.95")]
        [InlineData(null, ".95")]
        [InlineData(100, "100")]
        [InlineData(null, "100.1")]
        [InlineData(0, "0")]
        [InlineData(null, "0.0")]
        [InlineData(null, "a5")]
        [InlineData(null, "5a")]
        [InlineData(5, " 5")]
        [InlineData(5, "5 ")]
        public void Test_ToIntOrNull(int? expected, string item) => Assert.Equal(expected, item.ToIntOrNull());

        [Theory]
        [InlineData(null, "")]
        [InlineData(null, "   ")]
        [InlineData(null, "\n")]
        [InlineData(null, null)]
        [InlineData("Test", "Test")]
        [InlineData(" Test ", " Test ")]
        public void Test_ToNullIfNullOrWhiteSpace(string expected, string item)
        {
            Assert.Equal(expected, item.ToNullIfNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("http://example.com")]
        [InlineData("https://example.com")]
        [InlineData("https://example.com/page")]
        [InlineData("https://example.com/page?variable=value")]
        [InlineData("https://example.com/directory/page?a=1&b=2")]
        public void Test_ToUri(string item) => Assert.IsType<Uri>(item.ToUri());

        [Fact]
        public void Test_TrimStartOnAllLines()
        {
            const string input =
                @"Web Service and Database Example
                ================================
                1. Display People
                2. Quit

                Enter an option: ";

            var expected =
                $"Web Service and Database Example{Environment.NewLine}" +
                $"================================{Environment.NewLine}" +
                $"1. Display People{Environment.NewLine}" +
                $"2. Quit{Environment.NewLine}" +
                "\n" +
                "Enter an option: ";

            Assert.Equal(expected, input.TrimStartOnAllLines());
        }

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
        public void Test_Truncate(string expected, string item, int length)
        {
            Assert.Equal(expected, item.Truncate(length));
        }

        [Theory]
        [InlineData("Hello", "hello")]
        [InlineData("Hello", "Hello")]
        [InlineData("Hello World", "helloWorld")]
        [InlineData("Hello World Test", "helloWorldTest")]
        [InlineData("Hello World Test", "HelloWorldTest")]
        [InlineData(" hello ", " hello ")]
        public void Test_UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter(
            string expected, string item)
        {
            Assert.Equal(expected, item.UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter());
        }

        #endregion

        #region TimeSpan

        [Fact]
        public void Test_To24HourAndMinutesString()
        {
            Test_To24HourAndMinutesString_Implementation("00:00", TimeSpan.Zero);
            Test_To24HourAndMinutesString_Implementation("13:00", TimeSpan.FromHours(13));
            Test_To24HourAndMinutesString_Implementation("00:00", TimeSpan.FromDays(1));
            Test_To24HourAndMinutesString_Implementation("23:59", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)));
            Test_To24HourAndMinutesString_Implementation("10:30", new TimeSpan(10, 30, 0));
            Test_To24HourAndMinutesString_Implementation("10:30", new TimeSpan(10, 30, 30));
            Test_To24HourAndMinutesString_Implementation("20:15", new TimeSpan(20, 15, 0));
            Test_To24HourAndMinutesString_Implementation("20:15", new TimeSpan(20, 15, 50));
            Test_To24HourAndMinutesString_Implementation("12:30", new TimeSpan(1, 12, 30, 0));
        }

        private void Test_To24HourAndMinutesString_Implementation(string expected, TimeSpan item) =>
            Assert.Equal(expected, item.To24HourAndMinutesString());

        [Fact]
        public void Test_ToDateTimeString()
        {
            Test_ToDateTimeString_Implementation("12:00 AM", TimeSpan.Zero);
            Test_ToDateTimeString_Implementation("00:00", TimeSpan.Zero, "HH:mm");
            Test_ToDateTimeString_Implementation("1:00 PM", TimeSpan.FromHours(13));
            Test_ToDateTimeString_Implementation("13", TimeSpan.FromHours(13), "HH");
            Test_ToDateTimeString_Implementation("12:00 AM", TimeSpan.FromDays(1));
            Test_ToDateTimeString_Implementation("00:00", TimeSpan.FromDays(1), "HH:mm");
            Test_ToDateTimeString_Implementation("11:59 PM", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)));
            Test_ToDateTimeString_Implementation("23:59", TimeSpan.FromDays(1).Subtract(TimeSpan.FromMinutes(1)), "HH:mm");
            Test_ToDateTimeString_Implementation("10:30 AM", new TimeSpan(10, 30, 0));
            Test_ToDateTimeString_Implementation("10:30 AM", new TimeSpan(10, 30, 30));
            Test_ToDateTimeString_Implementation("8:15 PM", new TimeSpan(20, 15, 0));
            Test_ToDateTimeString_Implementation("8:15 PM", new TimeSpan(20, 15, 50));
            Test_ToDateTimeString_Implementation("12:30 PM", new TimeSpan(1, 12, 30, 0));
            Test_ToDateTimeString_Implementation("12:30", new TimeSpan(1, 12, 30, 0), "HH:mm");
        }

        private void Test_ToDateTimeString_Implementation(string expected, TimeSpan item) =>
            Assert.Equal(expected, item.ToDateTimeString());

        private void Test_ToDateTimeString_Implementation(string expected, TimeSpan item, string format) =>
            Assert.Equal(expected, item.ToDateTimeString(format));

        #endregion
    }
}
