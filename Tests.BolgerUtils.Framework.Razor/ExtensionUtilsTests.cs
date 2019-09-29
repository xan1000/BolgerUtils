using BolgerUtils.Framework.Razor;
using RazorEngine.Text;
using Xunit;

namespace Tests.BolgerUtils.Framework.Razor
{
    public class ExtensionUtilsTests
    {
        #region String

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        [InlineData("<div>Hello World</div>")]
        public void Test_ToRawString(string item)
        {
            Assert.IsType<RawString>(item.ToRawString());
            Assert.Equal(item, item.ToRawString().ToString());
        }

        #endregion
    }
}
