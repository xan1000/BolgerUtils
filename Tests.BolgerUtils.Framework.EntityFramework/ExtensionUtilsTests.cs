using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BolgerUtils.Framework.EntityFramework;
using Tests.BolgerUtils.Framework.EntityFramework.Models;
using Xunit;

namespace Tests.BolgerUtils.Framework.EntityFramework
{
    public class ExtensionUtilsTests
    {
        #region DbContext

        private bool IsUnableConnectToDatabase()
        {
            try
            {
                using(var database = new TestContext())
                {
                    database.Database.Connection.Open();
                    return false;
                }
            }
            catch(SqlException e)
            {
                if(e.ErrorCode == -2146232060)
                    return true;
                throw;
            }
        }

        [Fact]
        public void Test_LogSql()
        {
            if(IsUnableConnectToDatabase())
                return;

            using(var database = new TestContext())
            using(var log = new StringWriter())
            {
                Debug.Listeners.Add(new TextWriterTraceListener(log));

                foreach(var _ in database.People.ToList())
                { }

                Assert.Equal(string.Empty, log.ToString());

                database.LogSql();

                foreach(var _ in database.People.ToList())
                { }

                Assert.Contains("SELECT", log.ToString());
            }
        }

        [Fact]
        public void Test_PropertyMaximumLength()
        {
            if(IsUnableConnectToDatabase())
                return;

            using(var database = new TestContext())
            {
                Assert.Equal(60, database.PropertyMaximumLength(nameof(database.People), nameof(Person.Name)));
            }
        }

        #endregion

        #region String

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void Test_ToSqlConnectionStringBuilder(string item) =>
            Assert.IsType<SqlConnectionStringBuilder>(item.ToSqlConnectionStringBuilder());

        #endregion
    }
}
