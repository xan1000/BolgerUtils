using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using SendGrid.Helpers.Mail;

namespace BolgerUtils
{
    public static class ExtensionUtils
    {
        #region Boolean
        
        public static string Checked(this bool value) => value ? "checked" : string.Empty;
        
        public static string Disabled(this bool value) => value ? "disabled" : string.Empty;
        
        public static string Display(this bool value, string trueDisplay, string falseDisplay = "") =>
            value ? trueDisplay : falseDisplay;

        public static string HasError(this bool value) => value ? "has-error" : string.Empty;

        public static string Selected(this bool value) => value ? "selected" : string.Empty;
        
        public static string YesOrNo(this bool value) => value ? "Yes" : "No";

        #endregion

        #region Byte

        public static string HexString(this byte[] bytes)
        {
            const int hexadecimalCharactersPerByte = 2;

            var hexString = new StringBuilder(bytes.Length * hexadecimalCharactersPerByte);
            foreach(var x in bytes)
            {
                hexString.AppendFormat("{0:X2}", x);
            }

            return hexString.ToString();
        }

        #endregion

        #region DateTime
        
        public static DateTime AddWeeks(this DateTime value, double weeks) => value.AddDays(weeks * Utils.DaysPerWeek);

        private static DateTime FindDayOfWeek(this DateTime value, DayOfWeek dayOfWeek, bool forward)
        {
            var days = forward ? 1 : -1;
            while(value.DayOfWeek != dayOfWeek)
            {
                value = value.AddDays(days);
            }
            return value;
        }
        
        public static DateTime FindDayOfWeekBackward(this DateTime value, DayOfWeek dayOfWeek) =>
            value.FindDayOfWeek(dayOfWeek, false);
        
        public static DateTime FindDayOfWeekForward(this DateTime value, DayOfWeek dayOfWeek) =>
            value.FindDayOfWeek(dayOfWeek, true);

        public static DateTime Yesterday(this DateTime value) => value.AddDays(-1);

        #endregion

        #region DayOfWeek

        public static IEnumerable<DayOfWeek> OrderByDayOfWeekStartingOnMonday(this IEnumerable<DayOfWeek> daysOfWeek) =>
            daysOfWeek.OrderBy(x => x == DayOfWeek.Sunday ? x + Utils.DaysPerWeek : x);

        #endregion

        #region EmailAddress

        public static string ToEmailAndNameString(this EmailAddress emailAddress) =>
            !emailAddress.Name.IsNullOrWhiteSpace() ? $"{emailAddress.Name} <{emailAddress.Email}>" :
                emailAddress.Email;

        #endregion

        #region Enum
        
        public static int ToInt(this Enum value) => Convert.ToInt32(value);

        public static string ToValueString(this Enum value) => value.ToInt().ToString();

        #endregion

        #region Generic

        public static bool IsContainedIn<T>(this T value, params T[] values) => values.Contains(value);
        
        public static List<T> ScalarToList<T>(this T t) => new List<T>(1) { t };

        #endregion

        #region ICollection

        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> objects)
        {
            foreach(var x in objects)
            {
                collection.Add(x);
            }
        }

        public static bool RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> objects)
        {
            var success = true;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach(var x in objects)
            {
                success &= collection.Remove(x);
            }

            return success;
        }

        // Note this extension method is usually used in conjunction with EF.
        public static bool RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            var success = true;
            foreach(var x in collection.Where(predicate).ToList())
            {
                success &= collection.Remove(x);
            }

            return success;
        }

        #endregion

        #region IEnumerable

        public static bool NotAll<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.All(x => !predicate(x));
        
        public static bool NotAny<T>(this IEnumerable<T> source) => !source.Any();

        public static bool NotAny<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Any(x => !predicate(x));

        public static IEnumerable<T> NotWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(x => !predicate(x));

        // Note this extension method is usually used in conjunction with EF.
        public static HashSet<T> ToListToHashSet<T>(this IEnumerable<T> objects) => objects.ToList().ToHashSet();

        #endregion

        #region List

        public static HashSet<T> ToHashSet<T>(this List<T> list) => new HashSet<T>(list);

        #endregion

        #region Object

        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return (ExpandoObject) expando;
        }

        #endregion

        #region String

        public static string Abbreviate(this string value, int length)
        {
            if(value.Length <= length)
                return value;

            const string abbreviation = "...";
            if(length <= abbreviation.Length)
                throw new Exception();

            return value.Truncate(length - abbreviation.Length) + abbreviation;
        }

        public static DbConnectionStringBuilder DbConnectionStringBuilder(this string connectionString) =>
            new DbConnectionStringBuilder { ConnectionString = connectionString };

        public static string GetLast8Digits(this string mobile)
        {
            var digits = new string(mobile.Where(char.IsDigit).ToArray());

            return digits.Length < 8 ? null : digits.Substring(digits.Length - 8);
        }

        public static bool IsEmpty(this string value) => value.Length == 0;
        
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        public static string NewLineToBr(this string value) => value.Replace("\n", "<br />");

        public static string ReadOnlyCheckbox(this bool value) =>
            $"<input type='checkbox' disabled {(value ? "checked" : string.Empty)} />";

        public static string RemoveDoubleQuotation(this string value) => value.Replace("\"", string.Empty);

        // https://stackoverflow.com/a/37592018/9798310
        public static string RemoveRedundantWhitespace(this string item)
        {
            var length = item.Length;
            var array = item.ToCharArray();
            var arrayIndex = 0;
            var skip = false;
            for(var i = 0; i < length; i++)
            {
                var character = array[i];
                switch(character)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                        if(skip)
                            continue;
                        array[arrayIndex++] = character;
                        skip = true;
                        break;
                    default:
                        array[arrayIndex++] = character;
                        skip = false;
                        break;
                }
            }

            return new string(array, 0, arrayIndex);
        }

        public static string RemoveSpaceAndApostrophe(this string value) =>
            value.Replace(" ", string.Empty).Replace("'", string.Empty);

        public static string SpaceToNbsp(this string value) => value.Replace(" ", "&nbsp;");

        public static string ToAustralianMobileNumber(this string mobile)
        {
            if(mobile.IsNullOrWhiteSpace())
                return mobile;

            // Remove everything except for numbers.
            mobile = new string(mobile.Where(char.IsDigit).ToArray());

            if(mobile.Length < 8 || mobile.Length > 11)
                return "INVALID " + mobile;
            // Standard mobile number format.
            if(mobile[0] == '0' && mobile.Length == 10)
                return long.Parse(mobile).ToString("0### ### ###");
            // Country code format.
            if(mobile[0] == '6' && mobile.Length == 11)
                return long.Parse(mobile).ToString("+## ### ### ###");

            return mobile.Insert(2, " ");
        }

        public static string ToEmptyIfNullOrWhiteSpace(this string value) =>
            value.IsNullOrWhiteSpace() ? string.Empty : value;

        public static FileInfo ToFileInfo(this string item) => new FileInfo(item);

        public static string ToNullIfNullOrWhiteSpace(this string value) => value.IsNullOrWhiteSpace() ? null : value;

        public static string Truncate(this string value, int length) =>
            value.Length <= length ? value : value.Substring(0, Math.Min(value.Length, length));

        public static string UpperCaseFirstLetterAndInsertSpaceBeforeEveryUpperCaseLetter(this string value)
        {
            var stringBuilder = new StringBuilder(value);

            if(char.IsLower(stringBuilder[0]))
                stringBuilder[0] = char.ToUpper(stringBuilder[0]);

            for(var i = 1; i < stringBuilder.Length; i++)
            {
                // ReSharper disable once InvertIf
                if(char.IsUpper(stringBuilder[i]))
                {
                    stringBuilder.Insert(i, ' ');
                    i++;
                }
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region TimeSpan

        public static string ToTimeString(this TimeSpan value, string format = "h:mm tt") =>
            DateTime.MinValue.Add(value).ToString(format);

        #endregion
    }
}
