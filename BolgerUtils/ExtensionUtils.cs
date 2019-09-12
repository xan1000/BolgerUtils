using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace BolgerUtils
{
    public static class ExtensionUtils
    {
        #region Boolean
        
        public static string Checked(this bool item) => item ? "checked" : string.Empty;
        
        public static string Disabled(this bool item) => item ? "disabled" : string.Empty;
        
        public static string Display(this bool item, string trueDisplay, string falseDisplay = "") =>
            item ? trueDisplay : falseDisplay;

        public static string HasError(this bool item) => item ? "has-error" : string.Empty;

        public static string ReadOnlyCheckbox(this bool item) =>
            $"<input type=checkbox disabled {(item ? "checked" : string.Empty)} />";

        public static string Selected(this bool item) => item ? "selected" : string.Empty;
        
        public static string YesOrNo(this bool item) => item ? "Yes" : "No";

        #endregion

        #region Byte

        public static string ToHexString(this byte[] item)
        {
            const int hexadecimalCharactersPerByte = 2;

            var hexString = new StringBuilder(item.Length * hexadecimalCharactersPerByte);
            foreach(var x in item)
            {
                hexString.AppendFormat("{0:X2}", x);
            }

            return hexString.ToString();
        }

        #endregion

        #region DateTime
        
        public static DateTime AddWeeks(this DateTime item, double weeks) => item.AddDays(weeks * Utils.DaysPerWeek);

        private static DateTime FindDayOfWeek(this DateTime item, DayOfWeek dayOfWeek, bool forward)
        {
            var days = forward ? 1 : -1;
            while(item.DayOfWeek != dayOfWeek)
            {
                item = item.AddDays(days);
            }
            return item;
        }
        
        public static DateTime FindDayOfWeekBackward(this DateTime item, DayOfWeek dayOfWeek) =>
            item.FindDayOfWeek(dayOfWeek, false);
        
        public static DateTime FindDayOfWeekForward(this DateTime item, DayOfWeek dayOfWeek) =>
            item.FindDayOfWeek(dayOfWeek, true);

        public static DateTime Yesterday(this DateTime item) => item.AddDays(-1);

        #endregion

        #region DayOfWeek

        public static IEnumerable<DayOfWeek> OrderByDayOfWeekStartingOnMonday(this IEnumerable<DayOfWeek> source) =>
            source.OrderBy(x => x == DayOfWeek.Sunday ? x + Utils.DaysPerWeek : x);

        #endregion

        #region Enum
        
        public static int ToInt(this Enum item) => Convert.ToInt32(item);

        public static string ToValueString(this Enum item) => item.ToInt().ToString();

        #endregion

        #region Generic

        public static bool IsContainedIn<T>(this T item, params T[] values) => values.Contains(item);
        
        public static List<T> ScalarToList<T>(this T item) => new List<T>(1) { item };

        #endregion

        #region ICollection

        public static void AddAll<T>(this ICollection<T> item, IEnumerable<T> objects)
        {
            foreach(var x in objects)
            {
                item.Add(x);
            }
        }

        public static bool RemoveAll<T>(this ICollection<T> item, IEnumerable<T> objects)
        {
            var success = true;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach(var x in objects)
            {
                success &= item.Remove(x);
            }

            return success;
        }

        // Note this extension method is usually used in conjunction with EF.
        public static bool RemoveAll<T>(this ICollection<T> item, Func<T, bool> predicate)
        {
            var success = true;
            foreach(var x in item.Where(predicate).ToList())
            {
                success &= item.Remove(x);
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
        public static HashSet<T> ToListToHashSet<T>(this IEnumerable<T> source) => source.ToList().ToHashSet();

        #endregion

        #region List

        public static HashSet<T> ToHashSet<T>(this List<T> item) => new HashSet<T>(item);

        #endregion

        #region Object

        public static dynamic ToDynamic(this object item)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
            {
                expando.Add(property.Name, property.GetValue(item));
            }

            return (ExpandoObject) expando;
        }

        #endregion

        #region String

        public static string Abbreviate(this string item, int length)
        {
            const string abbreviation = "...";

            if(item.Length <= length)
                return item;
            if(length <= abbreviation.Length)
                throw new Exception();

            return item.Truncate(length - abbreviation.Length) + abbreviation;
        }

        public static string GetLast8Digits(this string item)
        {
            var digits = new string(item.Where(char.IsDigit).ToArray());

            return digits.Length < 8 ? null : digits.Substring(digits.Length - 8);
        }

        public static bool IsEmpty(this string item) => item.Length == 0;
        
        public static bool IsInvalidEmail(this string item)
        {
            MailAddress mailAddress = null;
            try
            {
                mailAddress = new MailAddress(item);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            { }

            return mailAddress == null;
        }

        public static bool IsInvalidInt(this string item) => !int.TryParse(item, out _);

        private static readonly Regex _moneyRegex = new Regex(@"^((\d+)|(\d+\.\d{1,2}))$");
        public static bool IsInvalidMoney(this string item) => !_moneyRegex.IsMatch(item) || decimal.Parse(item) < 0;

        public static bool IsNullOrEmpty(this string item) => string.IsNullOrEmpty(item);
        
        public static bool IsNullOrWhiteSpace(this string item) => string.IsNullOrWhiteSpace(item);

        public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);

        public static string NewLineToBr(this string item) => item.Replace("\n", "<br />");

        public static string RemoveDoubleQuotation(this string item) => item.Replace(@"""", string.Empty);

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

        public static string RemoveSpaceAndApostrophe(this string item) =>
            item.Replace(" ", string.Empty).Replace("'", string.Empty);

        public static string SpaceToNbsp(this string item) => item.Replace(" ", "&nbsp;");

        public static string ToAustralianMobileNumber(this string item)
        {
            if(item.IsNullOrWhiteSpace())
                return item;

            // Remove everything except for numbers.
            item = new string(item.Where(char.IsDigit).ToArray());

            if(item.Length < 8 || item.Length > 11)
                return "INVALID " + item;
            // Standard mobile number format.
            if(item[0] == '0' && item.Length == 10)
                return long.Parse(item).ToString("0### ### ###");
            // Country code format.
            if(item[0] == '6' && item.Length == 11)
                return long.Parse(item).ToString("+## ### ### ###");

            return item.Insert(2, " ");
        }

        public static DbConnectionStringBuilder ToDbConnectionStringBuilder(this string item) =>
            new DbConnectionStringBuilder { ConnectionString = item };

        public static string ToEmptyIfNullOrWhiteSpace(this string item) =>
            item.IsNullOrWhiteSpace() ? string.Empty : item;

        public static FileInfo ToFileInfo(this string item) => new FileInfo(item);

        public static string ToNullIfNullOrWhiteSpace(this string item) => item.IsNullOrWhiteSpace() ? null : item;

        public static string Truncate(this string item, int length) =>
            item.Length <= length ? item : item.Substring(0, Math.Min(item.Length, length));

        public static string UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter(this string item)
        {
            var stringBuilder = new StringBuilder(item);

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

        public static string ToTimeString(this TimeSpan item, string format = "h:mm tt") =>
            DateTime.MinValue.Add(item).ToString(format);

        #endregion
    }
}
