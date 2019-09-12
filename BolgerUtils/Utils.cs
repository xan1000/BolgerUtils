using System;
using System.Collections.Generic;
using System.Linq;

namespace BolgerUtils
{
    public static class Utils
    {
        #region Constants

        public const string BrElement = "<br />";
        public const int DaysPerWeek = 7;
        public const string DoubleQuote = @"""";
        public const char DoubleQuoteChar = '"';
        public const string Empty = "";
        public const string NewLine = "\n";
        public const char NewLineChar = '\n';
        public const string NonBreakingSpace = "&nbsp;";
        public const string SingleQuote = "'";
        public const char SingleQuoteChar = '\'';
        public const string Space = " ";
        public const char SpaceChar = ' ';

        #endregion

        public static T GetEnumValue<T>(int value) where T : Enum
        {
            var enumValue = Enum.Parse(typeof(T), value.ToString());
            if(!Enum.IsDefined(typeof(T), enumValue))
                throw new Exception();

            return (T) enumValue;
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();

        public static DateTime Max(DateTime date, DateTime otherDate) => date > otherDate ? date : otherDate;
        
        public static DateTime Min(DateTime date, DateTime otherDate) => date < otherDate ? date : otherDate;
        
        public static string RandomString(int length)
        {
            // ReSharper disable StringLiteralTypo
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            // ReSharper restore StringLiteralTypo

            var stringChars = new char[length];
            var random = new Random();

            for(var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
