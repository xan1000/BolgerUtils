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
        public void BrElementTest() => Assert.Equal("<br />", Utils.BrElement);

        [Fact]
        public void CarriageReturnTest() => Assert.Equal("\r", Utils.CarriageReturn);

        [Fact]
        public void CarriageReturnCharTest() => Assert.Equal('\r', Utils.CarriageReturnChar);

        [Fact]
        public void DaysPerWeekTest() => Assert.Equal(7, Utils.DaysPerWeek);

        [Fact]
        public void DoubleQuoteTest() => Assert.Equal(@"""", Utils.DoubleQuote);

        [Fact]
        public void DoubleQuoteCharTest() => Assert.Equal('"', Utils.DoubleQuoteChar);

        [Fact]
        public void EmptyTest() => Assert.Equal(string.Empty, Utils.Empty);

        [Fact]
        public void NewLineTest() => Assert.Equal("\n", Utils.NewLine);

        [Fact]
        public void NewLineCharTest() => Assert.Equal('\n', Utils.NewLineChar);

        [Fact]
        public void NonBreakingSpaceTest() => Assert.Equal("&nbsp;", Utils.NonBreakingSpace);

        [Fact]
        public void SingleQuoteTest() => Assert.Equal("'", Utils.SingleQuote);

        [Fact]
        public void SingleQuoteCharTest() => Assert.Equal('\'', Utils.SingleQuoteChar);

        [Fact]
        public void SpaceTest() => Assert.Equal(" ", Utils.Space);

        [Fact]
        public void SpaceCharTest() => Assert.Equal(' ', Utils.SpaceChar);

        [Fact]
        public void ZeroTest() => Assert.Equal(0, Utils.Zero);

        #endregion

        [Fact]
        public void GetEnumValueTest()
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
        public void GetEnumValuesTest()
        {
            var testTypeList = new[] { TestType.Test1, TestType.Test2, TestType.Test3 }.ToList();
            Assert.Equal(testTypeList, Utils.GetEnumValues<TestType>());

            testTypeList.Remove(TestType.Test1);
            Assert.NotEqual(testTypeList, Utils.GetEnumValues<TestType>());

            Assert.Equal(new[] { TestAnotherType.Test1, TestAnotherType.Test2, TestAnotherType.Test3 },
                Utils.GetEnumValues<TestAnotherType>());
        }

        [Fact]
        public void MaxTest()
        {
            DateTime dateSmaller, dateBigger;

            void MaxDateSmallerAndDateBiggerTest()
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
            MaxDateSmallerAndDateBiggerTest();

            var date = DateTime.Now;
            dateSmaller = date;
            dateBigger = date.AddDays(1);
            MaxDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(-1);
            dateBigger = date;
            MaxDateSmallerAndDateBiggerTest();

            dateSmaller = date;
            dateBigger = date.Add(TimeSpan.FromHours(12));
            MaxDateSmallerAndDateBiggerTest();

            dateSmaller = date;
            dateBigger = date;
            MaxDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(1);
            dateBigger = date.AddHours(24);
            MaxDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(1);
            dateBigger = date.Add(TimeSpan.FromDays(1));
            MaxDateSmallerAndDateBiggerTest();
        }

        [Fact]
        public void MinTest()
        {
            DateTime dateSmaller, dateBigger;

            void MinDateSmallerAndDateBiggerTest()
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
            MinDateSmallerAndDateBiggerTest();

            var date = DateTime.Now;
            dateSmaller = date;
            dateBigger = date.AddDays(1);
            MinDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(-1);
            dateBigger = date;
            MinDateSmallerAndDateBiggerTest();

            dateSmaller = date;
            dateBigger = date.Add(TimeSpan.FromHours(12));
            MinDateSmallerAndDateBiggerTest();

            dateSmaller = date;
            dateBigger = date;
            MinDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(1);
            dateBigger = date.AddHours(24);
            MinDateSmallerAndDateBiggerTest();

            dateSmaller = date.AddDays(1);
            dateBigger = date.Add(TimeSpan.FromDays(1));
            MinDateSmallerAndDateBiggerTest();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void RandomStringTest(int length)
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
    }
}
