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

        public static string Checked(this bool item) => item.Display("checked");

        public static string Disabled(this bool item) => item.Display("disabled");
        
        public static string Display(this bool item, string trueDisplay, string falseDisplay = Utils.Empty) =>
            item ? trueDisplay : falseDisplay;

        public static string HasError(this bool item) => item.Display("has-error");

        public static string ReadOnlyCheckbox(this bool item) =>
            $"<input type=checkbox disabled {(item ? "checked" : string.Empty)} />";

        public static string Selected(this bool item) => item.Display("selected");

        public static string YesOrNo(this bool item) => item.Display("Yes", "No");

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
        
        public static bool IsNull<T>(this T item) where T : class  => item == null;

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
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach(var x in item.Where(predicate).ToList())
            {
                success &= item.Remove(x);
            }

            return success;
        }

        #endregion

        #region IEnumerable

        public static bool IsEmpty<T>(this IEnumerable<T> source) => source.NotAny();

        public static bool NotAll<T>(this IEnumerable<T> source, Func<T, bool> predicate) => !source.All(predicate);
        
        public static bool NotAny<T>(this IEnumerable<T> source) => !source.Any();

        public static bool NotAny<T>(this IEnumerable<T> source, Func<T, bool> predicate) => !source.Any(predicate);

        public static IEnumerable<T> NotWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(x => !predicate(x));

        // Note this extension method is usually used in conjunction with EF.
        public static HashSet<T> ToListToHashSet<T>(this IEnumerable<T> source) => source.ToList().ToHashSet();

        #endregion

        #region List

        public static HashSet<T> ToHashSet<T>(this List<T> item) => new HashSet<T>(item);

        #endregion

        #region Numbers

        public static bool IsInRange(this int item, int min, int max)
        {
            if(min > max)
                throw new ArgumentException("min cannot be greater than max");

            return item >= min && item <= max;
        }

        public static bool IsInRange(this decimal item, decimal min, decimal max)
        {
            if(min > max)
                throw new ArgumentException("min cannot be greater than max");

            return item >= min && item <= max;
        }

        public static bool IsInRange(this double item, double min, double max)
        {
            if(min > max)
                throw new ArgumentException("min cannot be greater than max");

            return item >= min && item <= max;
        }

        public static bool IsInRange(this DateTime item, DateTime min, DateTime max)
        {
            if(min > max)
                throw new ArgumentException("min cannot be greater than max");

            return item >= min && item <= max;
        }

        public static bool IsInRange<T>(this T item, T min, T max) where T : struct, IComparable<T>
        {
            if(min.CompareTo(max) > 0)
                throw new ArgumentException("min cannot be greater than max");

            return item.CompareTo(min) >= 0 && item.CompareTo(max) <= 0;
        }

        public static bool IsNegative(this int item) => item < 0;
        public static bool IsNegative(this decimal item) => item < 0m;
        public static bool IsNegative(this double item) => item < 0d;
        public static bool IsNegative<T>(this T item) where T : struct, IComparable<T> => item.CompareTo(default) < 0;

        public static bool IsPositive(this int item) => item > 0;
        public static bool IsPositive(this decimal item) => item > 0m;
        public static bool IsPositive(this double item) => item > 0d;
        public static bool IsPositive<T>(this T item) where T : struct, IComparable<T> => item.CompareTo(default) > 0;

        public static bool IsZero(this int item) => item == 0;
        public static bool IsZero(this decimal item) => item == 0m;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public static bool IsZero(this double item) => item == 0d;
        public static bool IsZero<T>(this T item) where T : struct, IComparable<T> => item.CompareTo(default) == 0;

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

        public static bool IsInvalidDecimal(this string item) => !item.IsValidDecimal();

        public static bool IsInvalidDouble(this string item) => !item.IsValidDouble();

        public static bool IsInvalidEmail(this string item) => !item.IsValidEmail();

        public static bool IsInvalidInt(this string item) => !item.IsValidInt();

        public static bool IsInvalidMoney(this string item) => !item.IsValidMoney();

        public static bool IsNullOrEmpty(this string item) => string.IsNullOrEmpty(item);
        
        public static bool IsNullOrWhiteSpace(this string item) => string.IsNullOrWhiteSpace(item);

        public static bool IsValidDecimal(this string item) => double.TryParse(item, out _);

        public static bool IsValidDouble(this string item) => decimal.TryParse(item, out _);

        public static bool IsValidEmail(this string item)
        {
            MailAddress mailAddress = null;
            try
            {
                mailAddress = new MailAddress(item);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            { }

            return mailAddress != null;
        }

        public static bool IsValidInt(this string item) => int.TryParse(item, out _);

        private static readonly Regex _moneyRegex = new Regex(@"^\d+(\.\d{1,2})?$");
        public static bool IsValidMoney(this string item) => _moneyRegex.IsMatch(item) && decimal.Parse(item) >= 0;

        public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);

        public static string Join(this IEnumerable<string> source, char separator) =>
            source.Join(separator.ToString());

        public static string NewLineToBr(this string item) => item.Replace(Utils.NewLine, Utils.BrElement);

        public static string Remove(this string item, string value) => item.Replace(value, string.Empty);

        public static string RemoveCarriageReturn(this string item) => item.Remove(Utils.CarriageReturn);

        public static string RemoveDoubleQuotation(this string item) => item.Remove(Utils.DoubleQuote);

        public static string RemoveNewLine(this string item) => item.Remove(Utils.NewLine);

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
                    case '\r':
                        // Remove all \r.
                        continue;
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

        public static string RemoveSingleQuotation(this string item) => item.Remove(Utils.SingleQuote);

        public static string RemoveSpace(this string item) => item.Remove(Utils.Space);

        public static string RemoveSpaceAndSingleQuotation(this string item) =>
            item.RemoveSpace().RemoveSingleQuotation();

        public static string SpaceToNbsp(this string item) => item.Replace(Utils.Space, Utils.NonBreakingSpace);

        public static bool ToBoolean(this string item) => bool.Parse(item);

        public static DbConnectionStringBuilder ToDbConnectionStringBuilder(this string item) =>
            new DbConnectionStringBuilder { ConnectionString = item };

        public static decimal ToDecimal(this string item) => decimal.Parse(item);

        public static decimal? ToDecimalOrNull(this string item) =>
            decimal.TryParse(item, out var x) ? x : (decimal?) null;

        public static double ToDouble(this string item) => double.Parse(item);

        public static double? ToDoubleOrNull(this string item) =>
            double.TryParse(item, out var x) ? x : (double?) null;

        public static string ToEmptyIfNullOrWhiteSpace(this string item) =>
            item.IsNullOrWhiteSpace() ? string.Empty : item;

        public static FileInfo ToFileInfo(this string item) => new FileInfo(item);

        public static int ToInt(this string item) => int.Parse(item);

        public static int? ToIntOrNull(this string item) => int.TryParse(item, out var x) ? x : (int?) null;

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
