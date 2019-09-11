using System.Data.SqlClient;
using BolgerUtils.Framework;
using Xunit;

namespace Tests.BolgerUtils.Framework
{
    public class ExtensionUtilsTests
    {
        #region DbContext

        //public static void LogSql(this DbContext item) => item.Database.Log = s => Debug.WriteLine(s);

        //public static int PropertyMaximumLength(this DbContext item, string entitySet, string property)

        #endregion

        #region IRazorEngineService

        //public static string Parse<T>(this IRazorEngineService item, FileInfo templateFileInfo, T model = null,
        //    DynamicViewBag viewBag = null) where T : class

        //public static string Parse(this IRazorEngineService item, FileInfo templateFileInfo,
        //    Type modelType = null, object model = null, DynamicViewBag viewBag = null)

        #endregion

        #region String

        //public static IEncodedString ToRawString(this string item) => new RawString(item);

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void ToSqlConnectionStringBuilderTest(string item) =>
            Assert.IsType<SqlConnectionStringBuilder>(item.ToSqlConnectionStringBuilder());

        #endregion
    }
}
