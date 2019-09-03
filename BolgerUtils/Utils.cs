using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BolgerUtils
{
    public static class Utils
    {
        public const int DaysPerWeek = 7;

        public static T GetEnumValue<T>(int value) where T : Enum
        {
            var enumValue = Enum.Parse(typeof(T), value.ToString());
            if(!Enum.IsDefined(typeof(T), enumValue))
                throw new Exception();

            return (T) enumValue;
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();
        
        public static bool IsInvalidEmail(string emailAddress)
        {
            MailAddress mailAddress = null;
            try
            {
                mailAddress = new MailAddress(emailAddress);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            { }

            return mailAddress == null;
        }

        public static bool IsInvalidInt(string value) => !int.TryParse(value, out _);

        private static readonly Regex _moneyRegex = new Regex(@"^((\d+)|(\d+\.\d{1,2}))$");
        public static bool IsInvalidMoney(string value)
        {
            value = value.Trim();
            return !_moneyRegex.IsMatch(value) || decimal.Parse(value) < 0;
        }

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
