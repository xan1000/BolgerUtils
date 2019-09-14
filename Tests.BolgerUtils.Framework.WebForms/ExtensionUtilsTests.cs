using System;
using System.Web;
using BolgerUtils.Framework.WebForms;
using Xunit;

namespace Tests.BolgerUtils.Framework.WebForms
{
    public class ExtensionUtilsTests
    {
        #region DropDownList

        //public static DayOfWeek? SelectedDayOfWeekOrNull(this DropDownList item)
        //public static int? SelectedValueToIntOrNull(this DropDownList item)

        #endregion

        #region HttpRequest

        [Fact]
        public void BeginUrlTest() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.BeginUrl());

        [Fact]
        public void GetFormIntOrNullTest() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormIntOrNull("ID"));

        [Fact]
        public void GetFormValueNotNullTest() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormValueNotNull("ID"));

        [Fact]
        public void GetFormValueTest() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormValue("ID"));

        #endregion

        #region Label

        //public static void HasError(this Label item, out bool isFormInvalid, string text = null)

        #endregion

        #region Session

        [Fact]
        public void GetTest() =>
            // Its pretty hard to test HttpSessionState - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Session.Get<int>("ID"));

        [Fact]
        public void SetTest() =>
            // Its pretty hard to test HttpSessionState - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Session.Set("ID", 1));

        #endregion

        #region String

        //public static string ConnectionString(this string item)
        //public static ConnectionStringSettings ConnectionStringSettings(this string item)

        #endregion

        #region TextBox

        //public static int? ToIntOrNull(this TextBox item)
        //public static TimeSpan? ToTimeSpanOrNull(this TextBox item, string format = "h:mm tt")

        #endregion
    }
}
