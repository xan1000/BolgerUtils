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

        public static DayOfWeek? SelectedDayOfWeekOrNull(this DropDownList dropDownList)
        {
            var value = dropDownList.SelectedValueToIntOrNull();

            return value.HasValue ? (DayOfWeek?) Utils.GetEnumValue<DayOfWeek>(value.Value) : null;
        }

        public static int? SelectedValueToIntOrNull(this DropDownList dropDownList) =>
            int.TryParse(dropDownList.SelectedValue, out var value) ? (int?) value : null;

        #endregion

        #region HttpRequest

        public static string BeginUrl(this HttpRequest request) =>
            $"{request.Url.Scheme}://{request.ServerVariables["Http_Host"]}/";

        public static int? GetFormIntOrNull(this HttpRequest request, string name) =>
            int.TryParse(request.GetFormValue(name), out var value) ? (int?) value : null;

        public static string GetFormValue(this HttpRequest request, string name) => request.Form[name];
        
        public static string GetFormValueNotNull(this HttpRequest request, string name) =>
            request.GetFormValue(name) ?? string.Empty;

        #endregion

        #region Label

        public static void HasError(this Label label, out bool isFormInvalid, string text = null)
        {
            if(text != null)
                label.Text = text;
            label.Visible = isFormInvalid = true;
        }

        #endregion

        #region Session
        
        public static T Get<T>(this HttpSessionState session, string key) =>
            session[key] != null ? (T) session[key] : default(T);

        public static void Set<T>(this HttpSessionState session, string key, T value) => session[key] = value;

        #endregion

        #region String
        
        public static string ConnectionString(this string connectionStringName) =>
            connectionStringName.ConnectionStringSettings().ConnectionString;

        public static ConnectionStringSettings ConnectionStringSettings(this string connectionStringName) =>
            WebConfigurationManager.ConnectionStrings[connectionStringName];

        #endregion

        #region TextBox
        
        public static int? ToIntOrNull(this TextBox textBox) =>
            int.TryParse(textBox.Text, out var value) ? (int?) value : null;

        // Note the format parameter works out the box with the bootstrap timepicker used in this project.
        public static TimeSpan? ToTimeSpanOrNull(this TextBox textBox, string format = "h:mm tt") =>
            DateTime.TryParseExact(textBox.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateTime) ? (TimeSpan?) dateTime.TimeOfDay : null;

        #endregion
    }
}
