using System;
using System.Linq;
using BolgerUtils;
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

        #endregion

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
        public void FromHundreds(int expected, int value) => Assert.Equal(expected, Utils.FromHundreds(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 1)]
        [InlineData(5000, 5)]
        [InlineData(9000, 9)]
        [InlineData(10000, 10)]
        [InlineData(15000, 15)]
        [InlineData(-15000, -15)]
        public void FromThousands(int expected, int value) => Assert.Equal(expected, Utils.FromThousands(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000000, 1)]
        [InlineData(5000000, 5)]
        [InlineData(9000000, 9)]
        [InlineData(10000000, 10)]
        [InlineData(15000000, 15)]
        [InlineData(-15000000, -15)]
        public void FromMillions(int expected, int value) => Assert.Equal(expected, Utils.FromMillions(value));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000000000, 1)]
        [InlineData(2000000000, 2)]
        [InlineData(-2000000000, -2)]
        public void FromBillions(int expected, int value) => Assert.Equal(expected, Utils.FromBillions(value));

        [Fact]
        public void Test_GetEnumValue()
        {
            Assert.Equal(TestType.Test1, Utils.GetEnumValue<TestType>(TestType.Test1.ToInt()));
            Assert.Equal(TestType.Test2, Utils.GetEnumValue<TestType>(TestType.Test2.ToInt()));
            Assert.Equal(TestType.Test3, Utils.GetEnumValue<TestType>(TestType.Test3.ToInt()));
            Assert.NotEqual(TestType.Test1, Utils.GetEnumValue<TestType>(TestType.Test3.ToInt()));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestType>(-1));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestType>(TestType.Test1.ToInt() - 1));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestType>(TestType.Test3.ToInt() + 1));

            Assert.Equal(TestAnotherType.Test1, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test1.ToInt()));
            Assert.Equal(TestAnotherType.Test2, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test2.ToInt()));
            Assert.Equal(TestAnotherType.Test3, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test3.ToInt()));
            Assert.NotEqual(TestAnotherType.Test1, Utils.GetEnumValue<TestAnotherType>(TestAnotherType.Test3.ToInt()));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestAnotherType>(0));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestType>(TestAnotherType.Test1.ToInt() - 1));
            Assert.Throws<Exception>(() => Utils.GetEnumValue<TestType>(TestAnotherType.Test3.ToInt() + 1));
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

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(10, 10, 0)]
        [InlineData(10, 5, 10)]
        [InlineData(10, 10, 5)]
        [InlineData(15, 10, 15)]
        [InlineData(15, 15, 10)]
        [InlineData(0, -10, 0)]
        [InlineData(0, 0, -10)]
        [InlineData(1, 1, -10)]
        [InlineData(1, -10, 1)]
        [InlineData(-10, -10, -20)]
        [InlineData(-10, -20, -10)]
        [InlineData(int.MaxValue, int.MaxValue, int.MinValue)]
        [InlineData(int.MaxValue, int.MinValue, int.MaxValue)]
        public void Test_Int_Max(int expected, int a, int b) => Assert.Equal(expected, Utils.Max(a, b));

        [Fact]
        public void Test_Decimal_Max()
        {
            Test_Decimal_MaxImplementation(0m, 0m, 0m);
            Test_Decimal_MaxImplementation(10m, 0m, 10m);
            Test_Decimal_MaxImplementation(10m, 10m, 0m);
            Test_Decimal_MaxImplementation(10m, 5m, 10m);
            Test_Decimal_MaxImplementation(10m, 10m, 5m);
            Test_Decimal_MaxImplementation(15m, 10m, 15m);
            Test_Decimal_MaxImplementation(15m, 15m, 10m);
            Test_Decimal_MaxImplementation(0m, -10m, 0m);
            Test_Decimal_MaxImplementation(0m, 0m, -10m);
            Test_Decimal_MaxImplementation(1m, 1m, -10m);
            Test_Decimal_MaxImplementation(1m, -10m, 1m);
            Test_Decimal_MaxImplementation(-10m, -10m, -20m);
            Test_Decimal_MaxImplementation(-10m, -20m, -10m);
            Test_Decimal_MaxImplementation(decimal.MaxValue, decimal.MaxValue, decimal.MinValue);
            Test_Decimal_MaxImplementation(decimal.MaxValue, decimal.MinValue, decimal.MaxValue);
            Test_Decimal_MaxImplementation(2.5m, 1.5m, 2.5m);
            Test_Decimal_MaxImplementation(2.5m, 2.5m, 1.5m);
            Test_Decimal_MaxImplementation(-1.5m, -1.5m, -2.5m);
            Test_Decimal_MaxImplementation(-1.5m, -2.5m, -1.5m);
            Test_Decimal_MaxImplementation(1.75m, 1.25m, 1.75m);
            Test_Decimal_MaxImplementation(1.75m, 1.75m, 1.25m);
        }

        private void Test_Decimal_MaxImplementation(decimal expected, decimal a, decimal b) =>
            Assert.Equal(expected, Utils.Max(a, b));

        [Theory]
        [InlineData(0d, 0d, 0d)]
        [InlineData(10d, 0d, 10d)]
        [InlineData(10d, 10d, 0d)]
        [InlineData(10d, 5d, 10d)]
        [InlineData(10d, 10d, 5d)]
        [InlineData(15d, 10d, 15d)]
        [InlineData(15d, 15d, 10d)]
        [InlineData(0d, -10d, 0d)]
        [InlineData(0d, 0d, -10d)]
        [InlineData(1d, 1d, -10d)]
        [InlineData(1d, -10d, 1d)]
        [InlineData(-10d, -10d, -20d)]
        [InlineData(-10d, -20d, -10d)]
        [InlineData(double.MaxValue, double.MaxValue, double.MinValue)]
        [InlineData(double.MaxValue, double.MinValue, double.MaxValue)]
        [InlineData(2.5d, 1.5d, 2.5d)]
        [InlineData(2.5d, 2.5d, 1.5d)]
        [InlineData(-1.5d, -1.5d, -2.5d)]
        [InlineData(-1.5d, -2.5d, -1.5d)]
        [InlineData(1.75d, 1.25d, 1.75d)]
        [InlineData(1.75d, 1.75d, 1.25d)]
        public void Test_Double_Max(double expected, double a, double b) => Assert.Equal(expected, Utils.Max(a, b));

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

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 10)]
        [InlineData(0, 10, 0)]
        [InlineData(5, 5, 10)]
        [InlineData(5, 10, 5)]
        [InlineData(10, 10, 15)]
        [InlineData(10, 15, 10)]
        [InlineData(-10, -10, 0)]
        [InlineData(-10, 0, -10)]
        [InlineData(-10, 1, -10)]
        [InlineData(-10, -10, 1)]
        [InlineData(-20, -10, -20)]
        [InlineData(-20, -20, -10)]
        [InlineData(int.MinValue, int.MaxValue, int.MinValue)]
        [InlineData(int.MinValue, int.MinValue, int.MaxValue)]
        public void Test_Int_Min(int expected, int a, int b) => Assert.Equal(expected, Utils.Min(a, b));

        [Fact]
        public void Test_Decimal_Min()
        {
            Test_Decimal_MinImplementation(0m, 0m, 0m);
            Test_Decimal_MinImplementation(0m, 0m, 10m);
            Test_Decimal_MinImplementation(0m, 10m, 0m);
            Test_Decimal_MinImplementation(5m, 5m, 10m);
            Test_Decimal_MinImplementation(5m, 10m, 5m);
            Test_Decimal_MinImplementation(10m, 10m, 15m);
            Test_Decimal_MinImplementation(10m, 15m, 10m);
            Test_Decimal_MinImplementation(-10m, -10m, 0m);
            Test_Decimal_MinImplementation(-10m, 0m, -10m);
            Test_Decimal_MinImplementation(-10m, 1m, -10m);
            Test_Decimal_MinImplementation(-10m, -10m, 1m);
            Test_Decimal_MinImplementation(-20m, -10m, -20m);
            Test_Decimal_MinImplementation(-20m, -20m, -10m);
            Test_Decimal_MinImplementation(decimal.MinValue, decimal.MaxValue, decimal.MinValue);
            Test_Decimal_MinImplementation(decimal.MinValue, decimal.MinValue, decimal.MaxValue);
            Test_Decimal_MinImplementation(1.5m, 1.5m, 2.5m);
            Test_Decimal_MinImplementation(1.5m, 2.5m, 1.5m);
            Test_Decimal_MinImplementation(-2.5m, -1.5m, -2.5m);
            Test_Decimal_MinImplementation(-2.5m, -2.5m, -1.5m);
            Test_Decimal_MinImplementation(1.25m, 1.25m, 1.75m);
            Test_Decimal_MinImplementation(1.25m, 1.75m, 1.25m);
        }

        private void Test_Decimal_MinImplementation(decimal expected, decimal a, decimal b) =>
            Assert.Equal(expected, Utils.Min(a, b));

        [Theory]
        [InlineData(0d, 0d, 0d)]
        [InlineData(0d, 0d, 10d)]
        [InlineData(0d, 10d, 0d)]
        [InlineData(5d, 5d, 10d)]
        [InlineData(5d, 10d, 5d)]
        [InlineData(10d, 10d, 15d)]
        [InlineData(10d, 15d, 10d)]
        [InlineData(-10d, -10d, 0d)]
        [InlineData(-10d, 0d, -10d)]
        [InlineData(-10d, 1d, -10d)]
        [InlineData(-10d, -10d, 1d)]
        [InlineData(-20d, -10d, -20d)]
        [InlineData(-20d, -20d, -10d)]
        [InlineData(double.MinValue, double.MaxValue, double.MinValue)]
        [InlineData(double.MinValue, double.MinValue, double.MaxValue)]
        [InlineData(1.5d, 1.5d, 2.5d)]
        [InlineData(1.5d, 2.5d, 1.5d)]
        [InlineData(-2.5d, -1.5d, -2.5d)]
        [InlineData(-2.5d, -2.5d, -1.5d)]
        [InlineData(1.25d, 1.25d, 1.75d)]
        [InlineData(1.25d, 1.75d, 1.25d)]
        public void Test_Double_Min(double expected, double a, double b) => Assert.Equal(expected, Utils.Min(a, b));

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
        [InlineData(5)]
        [InlineData(10)]
        public void Test_RandomString(int length)
        {
            for(var i = 0; i < 10; i++)
            {
                var randomString1 = Utils.RandomString(length);
                var randomString2 = Utils.RandomString(length);

                Assert.Equal(length, randomString1.Length);
                Assert.Equal(length, randomString2.Length);
                Assert.NotEqual(randomString1, randomString2);
            }
        }

        [Fact]
        public void Test_Swap()
        {
            Test_SwapImplementation(5, 10);
            Test_SwapImplementation(5.5, 10.10);
            Test_SwapImplementation(5.5m, 10.10m);
            Test_SwapImplementation("Hello", "World");
            Test_SwapImplementation("Hello", null);
            Test_SwapImplementation(null, "World");
            Test_SwapImplementation("Hello", new string(new[] { 'H', 'e', 'l', 'l', 'o' } ));
            Test_SwapImplementation(new UtilsTests(), new UtilsTests());
        }

        private void Test_SwapImplementation<T>(T a, T b)
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
    }
}
