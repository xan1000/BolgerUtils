using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace BolgerUtils.Framework.WebForms
{
    public static class ExtensionUtils
    {
        #region DropDownList

        public static DayOfWeek? SelectedValueToDayOfWeekOrNull(this DropDownList item)
        {
            var value = item.SelectedValueToIntOrNull();

            return value.HasValue ? Utils.GetEnumValue<DayOfWeek>(value.Value) : (DayOfWeek?) null;
        }

        public static int? SelectedValueToIntOrNull(this DropDownList item) =>
            int.TryParse(item.SelectedValue, out var value) ? value : (int?) null;

        #endregion

        #region HttpRequest

        public static string BeginUrl(this HttpRequest item, bool appendForwardSlash = true) =>
            $"{item.Url.Scheme}://{item.ServerVariables["Http_Host"]}{(appendForwardSlash ? "/" : string.Empty)}";

        public static int? GetFormIntOrNull(this HttpRequest item, string name) =>
            int.TryParse(item.GetFormValue(name), out var value) ? value : (int?) null;

        public static string GetFormValue(this HttpRequest item, string name) => item.Form[name];
        
        public static string GetFormValueNotNull(this HttpRequest item, string name) =>
            item.GetFormValue(name) ?? string.Empty;

        #endregion

        #region Label

        public static void HasError(this Label item, out bool isFormInvalid, string text = null)
        {
            if(text != null)
                item.Text = text;
            item.Visible = isFormInvalid = true;
        }

        #endregion

        #region Session
        
        public static T Get<T>(this HttpSessionState item, string key) => item[key] != null ? (T) item[key] : default;

        public static void Set<T>(this HttpSessionState item, string key, T value) => item[key] = value;

        #endregion

        #region String
        
        public static string ConnectionString(this string item) => item.ConnectionStringSettings().ConnectionString;

        public static ConnectionStringSettings ConnectionStringSettings(this string item) =>
            WebConfigurationManager.ConnectionStrings[item];

        #endregion

        #region TextBox
        
        public static int? ToIntOrNull(this TextBox item) =>
            int.TryParse(item.Text, out var value) ? value : (int?) null;

        public static TimeSpan? ToTimeSpanOrNull(this TextBox item, string format = "h:mm tt") =>
            DateTime.TryParseExact(item.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateTime) ? dateTime.TimeOfDay : (TimeSpan?) null;

        #endregion
    }
}
