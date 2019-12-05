using System;
using System.Web.UI.WebControls;

namespace BolgerUtils.Framework.WebForms
{
    public static class Utils
    {
        // Duplicate of BolgerUtils.Utils.GetEnumValue
        internal static T GetEnumValue<T>(int value) where T : Enum
        {
            var enumValue = Enum.Parse(typeof(T), value.ToString());
            if(!Enum.IsDefined(typeof(T), enumValue))
                throw new ArgumentException("value is not an enum of the type provided.");

            return (T) enumValue;
        }

        public static void TrimText(params TextBox[] textBoxes)
        {
            foreach(var textBox in textBoxes)
            {
                textBox.Text = textBox.Text.Trim();
            }
        }
    }
}
