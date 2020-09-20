using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BolgerUtils;
using Microsoft.Data.SqlClient;
using Xunit;

namespace Tests.BolgerUtils
{
    public enum TestType
    {
        Test1,
        Test2,
        Test3
    }

    public enum TestAnotherType
    {
        Test1 = 10,
        Test2 = 20,
        Test3 = 30
    }

    public class UtilsTests
    {
        #region Constants

        [Fact]
        public void Test_BrElement() => Assert.Equal("<br />", Utils.BrElement);

        [Fact]
        public void Test_CarriageReturn() => Assert.Equal("\r", Utils.CarriageReturn);

        [Fact]
        public void Test_CarriageReturnChar() => Assert.Equal('\r', Utils.CarriageReturnChar);

        [Fact]
        public void Test_DaysPerWeek() => Assert.Equal(7, Utils.DaysPerWeek);

        [Fact]
        public void Test_DoubleQuote() => Assert.Equal(@"""", Utils.DoubleQuote);

        [Fact]
        public void Test_DoubleQuoteChar() => Assert.Equal('"', Utils.DoubleQuoteChar);

        [Fact]
        public void Test_Empty() => Assert.Equal(string.Empty, Utils.Empty);

        [Fact]
        public void Test_Localhost() => Assert.Equal("localhost", Utils.Localhost);

        [Fact]
        public void Test_MonthsPerYear() => Assert.Equal(12, Utils.MonthsPerYear);

        [Fact]
        public void Test_NewLine() => Assert.Equal("\n", Utils.NewLine);

        [Fact]
        public void Test_NewLineChar() => Assert.Equal('\n', Utils.NewLineChar);

        [Fact]
        public void Test_NonBreakingSpace() => Assert.Equal("&nbsp;", Utils.NonBreakingSpace);

        [Fact]
        public void Test_SingleQuote() => Assert.Equal("'", Utils.SingleQuote);

        [Fact]
        public void Test_SingleQuoteChar() => Assert.Equal('\'', Utils.SingleQuoteChar);

        [Fact]
        public void Test_Space() => Assert.Equal(" ", Utils.Space);

        [Fact]
        public void Test_SpaceChar() => Assert.Equal(' ', Utils.SpaceChar);

        [Fact]
        public void Test_WeekTimeSpan() => Assert.Equal(TimeSpan.FromDays(7), Utils.WeekTimeSpan);

        #region Numbers

        [Fact]
        public void Test_Zero() => Assert.Equal(0, Utils.Zero);

        [Fact]
        public void Test_Ten() => Assert.Equal(10, Utils.Ten);

        [Fact]
        public void Test_Hundred() => Assert.Equal(100, Utils.Hundred);

        [Fact]
        public void Test_Thousand() => Assert.Equal(1000, Utils.Thousand);

        [Fact]
        public void Test_Million() => Assert.Equal(1000000, Utils.Million);

        [Fact]
        public void Test_Billion() => Assert.Equal(1000000000, Utils.Billion);

        #endregion

        #region Characters

        [Fact]
        public void Test_UppercaseCharacters() =>
            // ReSharper disable once StringLiteralTypo
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Utils.UppercaseCharacters);

        [Fact]
        public void Test_LowercaseCharacters() =>
            // ReSharper disable once StringLiteralTypo
            Assert.Equal("abcdefghijklmnopqrstuvwxyz", Utils.LowercaseCharacters);

        [Fact]
        public void Test_DigitCharacters() =>
            Assert.Equal("0123456789", Utils.DigitCharacters);

        [Fact]
        public void Test_UppercaseLowercaseAndDigitCharacters() =>
            // ReSharper disable StringLiteralTypo
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",
                Utils.UppercaseLowercaseAndDigitCharacters);
            // ReSharper restore StringLiteralTypo

        #endregion

        #endregion

        #region Methods

        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Database=Test;Trusted_Connection=True";
        internal static readonly object CreateConnectionLock = new object();

        [Fact]
        public void Test_CreateAndOpenConnection()
        {
            lock(CreateConnectionLock)
            {
                try
                {
                    Assert.Throws<InvalidOperationException>(Utils.CreateAndOpenConnection);

                    Utils.ConnectionString = ConnectionString;
                    Assert.Throws<InvalidOperationException>(Utils.CreateAndOpenConnection);

                    Utils.CreateConnectionFunc = connectionString => new SqlConnection(connectionString);

                    // CreateAndOpenConnection block.
                    {
                        using var connection = Utils.CreateAndOpenConnection();
                        Assert.IsAssignableFrom<DbConnection>(connection);
                        Assert.Equal(ConnectionState.Open, connection.State);
                    }

                    // CreateAndOpenConnection<T> block.
                    {
                        using var sqlConnection = Utils.CreateAndOpenConnection<SqlConnection>();
                        Assert.IsAssignableFrom<DbConnection>(sqlConnection);
                        Assert.IsType<SqlConnection>(sqlConnection);
                        Assert.Equal(ConnectionState.Open, sqlConnection.State);
                    }
                }
                finally
                {
                    // Test cleanup.
                    Utils.ConnectionString = null;
                    Utils.CreateConnectionFunc = null;
                }
            }
        }

        [Fact]
        public void Test_CreateConnection()
        {
            lock(CreateConnectionLock)
            {
                try
                {
                    Assert.Throws<InvalidOperationException>(Utils.CreateConnection);

                    Utils.ConnectionString = ConnectionString;
                    Assert.Throws<InvalidOperationException>(Utils.CreateConnection);

                    Utils.CreateConnectionFunc = connectionString => new SqlConnection(connectionString);

                    // CreateConnection block.
                    {
                        using var connection = Utils.CreateConnection();
                        Assert.IsAssignableFrom<DbConnection>(connection);
                        Assert.Equal(ConnectionState.Closed, connection.State);
                    }

                    // CreateConnection<T> block.
                    {
                        using var sqlConnection = Utils.CreateConnection<SqlConnection>();
                        Assert.IsAssignableFrom<DbConnection>(sqlConnection);
                        Assert.IsType<SqlConnection>(sqlConnection);
                        Assert.Equal(ConnectionState.Closed, sqlConnection.State);
                    }
                }
                finally
                {
                    // Test cleanup.
                    Utils.ConnectionString = null;
                    Utils.CreateConnectionFunc = null;
                }
            }
        }

        [Fact]
        public void Test_EachDay()
        {
            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(30);
            var expectedDays = new List<DateTime>();
            for(var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.Tomorrow())
            {
                expectedDays.Add(currentDate);
            }

            var days = Utils.EachDay(startDate, endDate);
            Assert.Equal(expectedDays.Count, days.Count);
            Assert.Equal(days.Count, days.Capacity);
            Assert.Equal(expectedDays, days);

            days = Utils.EachDay(startDate, (int) endDate.Subtract(startDate).TotalDays);
            Assert.Equal(expectedDays.Count, days.Count);
            Assert.Equal(days.Count, days.Capacity);
            Assert.Equal(expectedDays, days);

            days = Utils.EachDay(startDate, startDate);
            Assert.Single(days);
            Assert.Equal(days.Count, days.Capacity);

            days = Utils.EachDay(startDate, 0);
            Assert.Single(days);
            Assert.Equal(days.Count, days.Capacity);

            Assert.Throws<ArgumentException>(() => Utils.EachDay(endDate, startDate));
            Assert.Throws<ArgumentException>(() => Utils.EachDay(startDate.Tomorrow(), startDate));
            Assert.Throws<ArgumentException>(() => Utils.EachDay(startDate, startDate.Yesterday()));
            Assert.Throws<ArgumentException>(() => Utils.EachDay(startDate, -1));
        }

        [Fact]
        public void Test_ExecuteWithTryCatch()
        {
            const string s = "Hello World";
            const string h = "Hello";

            // ExecuteWithTryCatch Action.
            {
                var i = 0;
                Assert.Null(Utils.ExecuteWithTryCatch(() => { i = 5; }));
                Assert.Equal(5, i);

                Assert.NotNull(Utils.ExecuteWithTryCatch(() => throw new Exception()));

                var throwException = new Exception();
                Assert.Same(throwException, Utils.ExecuteWithTryCatch(() => throw throwException));

                // ReSharper disable once UnusedVariable
                var e = Utils.ExecuteWithTryCatch(() => { var c = s[0]; });
                Assert.Null(e);

                // ReSharper disable once UnusedVariable
                e = Utils.ExecuteWithTryCatch(() => { var c = s[-1]; });
                Assert.NotNull(e);
                Assert.IsType<IndexOutOfRangeException>(e);

                // ReSharper disable once UnusedVariable
                e = Utils.ExecuteWithTryCatch(() => { var c = s[s.Length]; });
                Assert.NotNull(e);
                Assert.IsType<IndexOutOfRangeException>(e);
            }

            // ExecuteWithTryCatch Func<T>
            {
                var result = Utils.ExecuteWithTryCatch(() => s.Substring(0, h.Length));
                Assert.True(result.HasReturnValue);
                Assert.Equal(h, result.ReturnValue);
                Assert.Null(result.Exception);

                result = Utils.ExecuteWithTryCatch(() => s.Substring(-1));
                Assert.False(result.HasReturnValue);
                Assert.Throws<InvalidOperationException>(() => result.ReturnValue);
                Assert.NotNull(result.Exception);
                Assert.IsType<ArgumentOutOfRangeException>(result.Exception);

                result = Utils.ExecuteWithTryCatch(() => s.Substring(s.Length + 1));
                Assert.False(result.HasReturnValue);
                Assert.Throws<InvalidOperationException>(() => result.ReturnValue);
                Assert.NotNull(result.Exception);
                Assert.IsType<ArgumentOutOfRangeException>(result.Exception);
            }
        }

        [Fact]
        public async Task Test_ExecuteTryCatchAsync()
        {
            const string s = "Hello World";
            const string h = "Hello";

            // ExecuteWithTryCatchAsync Func<Task>.
            {
                var i = 0;
                Assert.Null(await Utils.ExecuteWithTryCatchAsync(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    i = 5;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }));
                Assert.Equal(5, i);

                // Disable async with no await warning.
                #pragma warning disable 1998

                i = 0;
                Assert.Null(await Utils.ExecuteWithTryCatchAsync(async () => { i = 5; }));
                Assert.Equal(5, i);

                Assert.NotNull(await Utils.ExecuteWithTryCatchAsync(async () => throw new Exception()));

                var throwException = new Exception();
                Assert.Same(throwException, await Utils.ExecuteWithTryCatchAsync(async () => throw throwException));

                // ReSharper disable once UnusedVariable
                var e = await Utils.ExecuteWithTryCatchAsync(async () => { var c = s[0]; });
                Assert.Null(e);

                // ReSharper disable once UnusedVariable
                e = await Utils.ExecuteWithTryCatchAsync(async () => { var c = s[-1]; });
                Assert.NotNull(e);
                Assert.IsType<IndexOutOfRangeException>(e);

                // ReSharper disable once UnusedVariable
                e = await Utils.ExecuteWithTryCatchAsync(async () => { var c = s[s.Length]; });
                Assert.NotNull(e);
                Assert.IsType<IndexOutOfRangeException>(e);

                #pragma warning restore 1998
            }

            // ExecuteWithTryCatchAsync Func<Task<T>>.
            {
                var result = await Utils.ExecuteWithTryCatchAsync(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    var substring = s.Substring(0, h.Length);
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    return substring;
                });
                Assert.True(result.HasReturnValue);
                Assert.Equal(h, result.ReturnValue);
                Assert.Null(result.Exception);

                // Disable async with no await warning.
                #pragma warning disable 1998

                result = await Utils.ExecuteWithTryCatchAsync(async () => s.Substring(-1));
                Assert.False(result.HasReturnValue);
                Assert.Throws<InvalidOperationException>(() => result.ReturnValue);
                Assert.NotNull(result.Exception);
                Assert.IsType<ArgumentOutOfRangeException>(result.Exception);

                result = await Utils.ExecuteWithTryCatchAsync(async () => s.Substring(s.Length + 1));
                Assert.False(result.HasReturnValue);
                Assert.Throws<InvalidOperationException>(() => result.ReturnValue);
                Assert.NotNull(result.Exception);
                Assert.IsType<ArgumentOutOfRangeException>(result.Exception);

                #pragma warning restore 1998
            }
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 10)]
        [InlineData(0, 10, 0)]
        [InlineData(10, 10, 1)]
        [InlineData(15, 10, 1.5)]
        [InlineData(500, 100, 5)]
        [InlineData(9000, 1000, 9)]
        [InlineData(15000000, 1000000, 15)]
        [InlineData(15250000, 1000000, 15.25)]
        [InlineData(2000000000, 1000000000, 2)]
        [InlineData(-2000000000, 1000000000, -2)]
        [InlineData(-2500000000, 1000000000, -2.5)]
        public void Test_From(double expected, int constant, double value) =>
            Assert.Equal(expected, Utils.From(constant, value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 1)]
        [InlineData(50, 5)]
        [InlineData(90, 9)]
        [InlineData(100, 10)]
        [InlineData(150, 15)]
        [InlineData(-150, -15)]
        public void Test_FromTens(int expected, int value) => Assert.Equal(expected, Utils.FromTens(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 1)]
        [InlineData(500, 5)]
        [InlineData(900, 9)]
        [InlineData(1000, 10)]
        [InlineData(1500, 15)]
        [InlineData(-1500, -15)]
        public void Test_FromHundreds(int expected, int value) => Assert.Equal(expected, Utils.FromHundreds(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 1)]
        [InlineData(5000, 5)]
        [InlineData(9000, 9)]
        [InlineData(10000, 10)]
        [InlineData(15000, 15)]
        [InlineData(-15000, -15)]
        public void Test_FromThousands(int expected, int value) => Assert.Equal(expected, Utils.FromThousands(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000000, 1)]
        [InlineData(5000000, 5)]
        [InlineData(9000000, 9)]
        [InlineData(10000000, 10)]
        [InlineData(15000000, 15)]
        [InlineData(-15000000, -15)]
        public void Test_FromMillions(int expected, int value) => Assert.Equal(expected, Utils.FromMillions(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000000000, 1)]
        [InlineData(2000000000, 2)]
        [InlineData(-2000000000, -2)]
        public void Test_FromBillions(int expected, int value) => Assert.Equal(expected, Utils.FromBillions(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(7, 1)]
        [InlineData(14, 2)]
        [InlineData(21, 3)]
        [InlineData(28, 4)]
        [InlineData(35, 5)]
        [InlineData(3.5, 0.5)]
        [InlineData(10.5, 1.5)]
        public void Test_FromWeeks(double expectedDays, double weeks) =>
            Assert.Equal(TimeSpan.FromDays(expectedDays), Utils.FromWeeks(weeks));

        [Fact]
        public void Test_GetEnumValue()
        {
            Assert.Equal(TestType.Test1, Utils.GetEnumValue<TestType>(TestType.Test1.ToInt()));
            Assert.Equal(TestType.Test2, Utils.GetEnumValue<TestType>(TestType.Test2.ToInt()));
            Assert.Equal(TestType.Test3, Utils.GetEnumValue<TestType>(TestType.Test3.ToInt()));
            Assert.NotEqual(TestType.Test1, Utils.GetEnumValue<TestType>(TestType.Test3.ToInt()));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestType>(-1));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestType>(TestType.Test1.ToInt() - 1));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestType>(TestType.Test3.ToInt() + 1));

            Assert.Equal(TestAnotherType.Test1, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test1.ToInt()));
            Assert.Equal(TestAnotherType.Test2, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test2.ToInt()));
            Assert.Equal(TestAnotherType.Test3, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test3.ToInt()));
            Assert.NotEqual(TestAnotherType.Test1, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test3.ToInt()));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestAnotherType>(0));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestType>(TestAnotherType.Test1.ToInt() - 1));
            Assert.Throws<ArgumentException>(() => Utils.GetEnumValue<TestType>(TestAnotherType.Test3.ToInt() + 1));
        }

        [Fact]
        public void Test_GetEnumValues()
        {
            var testTypeList = new[] { TestType.Test1, TestType.Test2, TestType.Test3 }.ToList();
            Assert.Equal(testTypeList, Utils.GetEnumValues<TestType>());

            testTypeList.Remove(TestType.Test1);
            Assert.NotEqual(testTypeList, Utils.GetEnumValues<TestType>());

            Assert.Equal(new[] { TestAnotherType.Test1, TestAnotherType.Test2, TestAnotherType.Test3 },
                Utils.GetEnumValues<TestAnotherType>());
        }

        [Fact]
        public void Test_DateTime_Max()
        {
            DateTime dateSmaller, dateBigger;

            void Test_DateTime_MaxDateSmallerAndDateBigger()
            {
                Assert.True(dateSmaller <= dateBigger);

                var maxDate = Utils.Max(dateSmaller, dateBigger);
                Assert.Equal(dateBigger, maxDate);

                maxDate = Utils.Max(dateBigger, dateSmaller);
                Assert.Equal(dateBigger, maxDate);
                Assert.True(dateSmaller <= maxDate);
            }

            dateSmaller = DateTime.MinValue;
            dateBigger = DateTime.MaxValue;
            Test_DateTime_MaxDateSmallerAndDateBigger();

            var date = DateTime.Now;
            dateSmaller = date;
            dateBigger = date.AddDays(1);
            Test_DateTime_MaxDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(-1);
            dateBigger = date;
            Test_DateTime_MaxDateSmallerAndDateBigger();

            dateSmaller = date;
            dateBigger = date.Add(TimeSpan.FromHours(12));
            Test_DateTime_MaxDateSmallerAndDateBigger();

            dateSmaller = date;
            dateBigger = date;
            Test_DateTime_MaxDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(1);
            dateBigger = date.AddHours(24);
            Test_DateTime_MaxDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(1);
            dateBigger = date.Add(TimeSpan.FromDays(1));
            Test_DateTime_MaxDateSmallerAndDateBigger();
        }

        [Theory]
        [InlineData(0f, 0f, 0f)]
        [InlineData(10f, 0f, 10f)]
        [InlineData(10f, 10f, 0f)]
        [InlineData(10f, 5f, 10f)]
        [InlineData(10f, 10f, 5f)]
        [InlineData(15f, 10f, 15f)]
        [InlineData(15f, 15f, 10f)]
        [InlineData(0f, -10f, 0f)]
        [InlineData(0f, 0f, -10f)]
        [InlineData(1f, 1f, -10f)]
        [InlineData(1f, -10f, 1f)]
        [InlineData(-10f, -10f, -20f)]
        [InlineData(-10f, -20f, -10f)]
        [InlineData(float.MaxValue, float.MaxValue, float.MinValue)]
        [InlineData(float.MaxValue, float.MinValue, float.MaxValue)]
        [InlineData(2.5f, 1.5f, 2.5f)]
        [InlineData(2.5f, 2.5f, 1.5f)]
        [InlineData(-1.5f, -1.5f, -2.5f)]
        [InlineData(-1.5f, -2.5f, -1.5f)]
        [InlineData(1.75f, 1.25f, 1.75f)]
        [InlineData(1.75f, 1.75f, 1.25f)]
        public void Test_Max(float expected, float a, float b) => Assert.Equal(expected, Utils.Max(a, b));

        [Fact]
        public void Test_DateTime_Min()
        {
            DateTime dateSmaller, dateBigger;

            void Test_DateTime_MinDateSmallerAndDateBigger()
            {
                Assert.True(dateSmaller <= dateBigger);

                var minDate = Utils.Min(dateSmaller, dateBigger);
                Assert.Equal(dateSmaller, minDate);

                minDate = Utils.Min(dateBigger, dateSmaller);
                Assert.Equal(dateSmaller, minDate);
                Assert.True(dateBigger >= minDate);
            }

            dateSmaller = DateTime.MinValue;
            dateBigger = DateTime.MaxValue;
            Test_DateTime_MinDateSmallerAndDateBigger();

            var date = DateTime.Now;
            dateSmaller = date;
            dateBigger = date.AddDays(1);
            Test_DateTime_MinDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(-1);
            dateBigger = date;
            Test_DateTime_MinDateSmallerAndDateBigger();

            dateSmaller = date;
            dateBigger = date.Add(TimeSpan.FromHours(12));
            Test_DateTime_MinDateSmallerAndDateBigger();

            dateSmaller = date;
            dateBigger = date;
            Test_DateTime_MinDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(1);
            dateBigger = date.AddHours(24);
            Test_DateTime_MinDateSmallerAndDateBigger();

            dateSmaller = date.AddDays(1);
            dateBigger = date.Add(TimeSpan.FromDays(1));
            Test_DateTime_MinDateSmallerAndDateBigger();
        }

        [Theory]
        [InlineData(0f, 0f, 0f)]
        [InlineData(0f, 0f, 10f)]
        [InlineData(0f, 10f, 0f)]
        [InlineData(5f, 5f, 10f)]
        [InlineData(5f, 10f, 5f)]
        [InlineData(10f, 10f, 15f)]
        [InlineData(10f, 15f, 10f)]
        [InlineData(-10f, -10f, 0f)]
        [InlineData(-10f, 0f, -10f)]
        [InlineData(-10f, 1f, -10f)]
        [InlineData(-10f, -10f, 1f)]
        [InlineData(-20f, -10f, -20f)]
        [InlineData(-20f, -20f, -10f)]
        [InlineData(float.MinValue, float.MaxValue, float.MinValue)]
        [InlineData(float.MinValue, float.MinValue, float.MaxValue)]
        [InlineData(1.5f, 1.5f, 2.5f)]
        [InlineData(1.5f, 2.5f, 1.5f)]
        [InlineData(-2.5f, -1.5f, -2.5f)]
        [InlineData(-2.5f, -2.5f, -1.5f)]
        [InlineData(1.25f, 1.25f, 1.75f)]
        [InlineData(1.25f, 1.75f, 1.25f)]
        public void Test_Min(float expected, float a, float b) => Assert.Equal(expected, Utils.Min(a, b));

        [Theory]
        [InlineData(Utils.UppercaseCharacters)]
        [InlineData(Utils.LowercaseCharacters)]
        [InlineData(Utils.DigitCharacters)]
        [InlineData(Utils.UppercaseLowercaseAndDigitCharacters)]
        [InlineData("test")]
        public void Test_RandomString(string characters)
        {
            const int baseLength = 10;

            for(var i = 1; i < 10; i++)
            {
                var length = baseLength * i;
                var randomString1 = Utils.RandomString(length, characters);
                var randomString2 = Utils.RandomString(length, characters);

                Test_RandomString_Implementation(characters, length, randomString1);
                Test_RandomString_Implementation(characters, length, randomString2);
                Assert.NotEqual(randomString1, randomString2);
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void Test_RandomString_Implementation(string characters, int length, string randomString)
        {
            Assert.Equal(length, randomString.Length);
            Assert.True(randomString.All(characters.Contains));
        }

        [Fact]
        public void Test_Self()
        {
            const string testString = "Hello World";
            Assert.Same(testString, Utils.Self(testString));

            const int testInt = 5;
            Assert.Equal(testInt, Utils.Self(testInt));

            const decimal testDecimal = 5m;
            Assert.Equal(testDecimal, Utils.Self(testDecimal));

            const decimal testDouble = 5m;
            Assert.Equal(testDouble, Utils.Self(testDouble));

            var testObject = new object();
            Assert.Same(testObject, Utils.Self(testObject));

            var testStringArray = new[] { "Hello", "World", "Test" };
            Assert.Equal(testStringArray.Select(x => x), testStringArray.Select(Utils.Self));

            var testIntArray = new[] { 1, 2, 3, 5, 10 };
            Assert.Equal(testIntArray.Select(x => x), testIntArray.Select(Utils.Self));
        }

        [Fact]
        public void Test_Swap()
        {
            Test_Swap_Implementation(5, 10);
            Test_Swap_Implementation(5.5, 10.10);
            Test_Swap_Implementation(5.5m, 10.10m);
            Test_Swap_Implementation("Hello", "World");
            Test_Swap_Implementation("Hello", null);
            Test_Swap_Implementation(null, "World");
            Test_Swap_Implementation("Hello", new string(new[] { 'H', 'e', 'l', 'l', 'o' } ));
            Test_Swap_Implementation(new UtilsTests(), new UtilsTests());
        }

        private void Test_Swap_Implementation<T>(T a, T b)
        {
            var isValueType = typeof(T).IsValueType;
            // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
            void AssertNotEqual(T x, T y)
            // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local
            {
                if(isValueType)
                    Assert.NotEqual(x, y);
                else
                {
                    // ReSharper disable RedundantCast
                    Assert.NotSame((object) x, (object) y);
                    // ReSharper restore RedundantCast
                }
            }
            // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
            void AssertEqual(T x, T y)
            // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local
            {
                if(isValueType)
                    Assert.Equal(x, y);
                else
                {
                    // ReSharper disable RedundantCast
                    Assert.Same((object) x, (object) y);
                    // ReSharper restore RedundantCast
                }
            }

            var originalA = a;
            var originalB = b;
            AssertNotEqual(a, b);
            AssertEqual(a, originalA);
            AssertNotEqual(a, originalB);
            AssertEqual(b, originalB);
            AssertNotEqual(b, originalA);

            Utils.Swap(ref a, ref b);

            AssertNotEqual(a, b);
            AssertNotEqual(a, originalA);
            AssertEqual(a, originalB);
            AssertNotEqual(b, originalB);
            AssertEqual(b, originalA);
        }

        #endregion
    }
}
