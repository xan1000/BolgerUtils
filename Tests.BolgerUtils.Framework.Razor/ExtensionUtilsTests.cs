using BolgerUtils.Framework.Razor;
using RazorEngine.Text;
using Xunit;

namespace Tests.BolgerUtils.Framework.Razor
{
    public class ExtensionUtilsTests
    {
        #region IRazorEngineService

        //public static string Parse<T>(this IRazorEngineService item, FileInfo templateFileInfo, T model = null,
        //    DynamicViewBag viewBag = null) where T : class

        //public static string Parse(this IRazorEngineService item, FileInfo templateFileInfo,
        //    Type modelType = null, object model = null, DynamicViewBag viewBag = null)

        #endregion

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
