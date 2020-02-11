using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace BolgerUtils
{
    public static class Utils
    {
        #region Constants

        public const string BrElement = "<br />";
        public const string CarriageReturn = "\r";
        public const char CarriageReturnChar = '\r';
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

        #region Numbers

        public const int Zero = 0;
        public const int Ten = 10;
        public const int Hundred = 100;
        public const int Thousand = 1000;
        public const int Million = 1000000;
        public const int Billion = 1000000000;

        #endregion

        #endregion

        #region Methods

        public static string ConnectionString { get; set; }
        public static Func<string, DbConnection> CreateConnectionFunc { get; set; }
        public static Func<DbCommand, DbDataAdapter> CreateDataAdapter { get; set; }

        public static DbConnection CreateAndOpenConnection() => CreateAndOpenConnection<DbConnection>();

        public static T CreateAndOpenConnection<T>() where T : DbConnection
        {
            var connection = CreateConnection<T>();
            connection.Open();

            return connection;
        }

        public static DbConnection CreateConnection() => CreateConnection<DbConnection>();

        public static T CreateConnection<T>() where T : DbConnection
        {
            if(ConnectionString == null)
                throw new InvalidOperationException("Must set the Utils.ConnectionString property.");
            if(CreateConnectionFunc == null)
                throw new InvalidOperationException("Must set the Utils.CreateConnectionFunc property.");

            return (T) CreateConnectionFunc(ConnectionString);
        }

        public static List<DateTime> EachDay(DateTime startDate, int daysForward) =>
            EachDay(startDate, startDate.AddDays(daysForward));

        public static List<DateTime> EachDay(DateTime startDate, DateTime endDate)
        {
            if(startDate > endDate)
                throw new ArgumentException("startDate cannot be greater than endDate.");

            var days = new List<DateTime>((int) endDate.Subtract(startDate).TotalDays + 1);
            for(var day = startDate; day <= endDate; day = day.Tomorrow())
            {
                days.Add(day);
            }

            return days;
        }

        public static double From(int constant, double value) => constant * value;
        public static int FromTens(int value) => Ten * value;
        public static int FromHundreds(int value) => Hundred * value;
        public static int FromThousands(int value) => Thousand * value;
        public static int FromMillions(int value) => Million * value;
        public static int FromBillions(int value) => Billion * value;

        public static T GetEnumValue<T>(int value) where T : Enum
        {
            var enumValue = Enum.Parse(typeof(T), value.ToString());
            if(!Enum.IsDefined(typeof(T), enumValue))
                throw new ArgumentException("value is not an enum of the type provided.", nameof(value));

            return (T) enumValue;
        }

        public static List<T> GetEnumValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
        public static T Max<T>(T a, T b) where T : struct, IComparable<T> => a.CompareTo(b) > 0 ? a : b;
        
        public static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;
        public static T Min<T>(T a, T b) where T : struct, IComparable<T> => a.CompareTo(b) < 0 ? a : b;
        
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

        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        #endregion
    }
}
