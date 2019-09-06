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
        
        #endregion

        #region DateTime
        
        #endregion

        #region DayOfWeek

        #endregion

        #region EmailAddress

        #endregion

        #region Enum
        
        #endregion

        #region Generic

        #endregion

        #region ICollection

        #endregion

        #region IEnumerable

        #endregion

        #region List

        #endregion

        #region Object

        #endregion

        #region String

        #endregion

        #region TimeSpan

        #endregion
    }
}
