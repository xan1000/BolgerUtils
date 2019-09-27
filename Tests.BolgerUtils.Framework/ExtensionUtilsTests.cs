﻿using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BolgerUtils.Framework;
using RazorEngine.Text;
using Tests.BolgerUtils.Framework.Models.EntityFramework;
using Xunit;

namespace Tests.BolgerUtils.Framework
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

        [Theory]
        [InlineData("Server=server;Database=database;User Id=username;Password=password")]
        [InlineData("Server=server;Database=database;Trusted_Connection=True")]
        public void Test_ToSqlConnectionStringBuilder(string item) =>
            Assert.IsType<SqlConnectionStringBuilder>(item.ToSqlConnectionStringBuilder());

        #endregion
    }
}
