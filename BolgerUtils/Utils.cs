using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public const string Localhost = "localhost";
        public const int MonthsPerYear = 12;
        public const string NewLine = "\n";
        public const char NewLineChar = '\n';
        public const string NonBreakingSpace = "&nbsp;";
        public const string SingleQuote = "'";
        public const char SingleQuoteChar = '\'';
        public const string Space = " ";
        public const char SpaceChar = ' ';
        public static readonly TimeSpan WeekTimeSpan = FromWeeks(1);

        #region Numbers

        public const int Zero = 0;
        public const int Ten = 10;
        public const int Hundred = 100;
        public const int Thousand = 1000;
        public const int Million = 1000000;
        public const int Billion = 1000000000;

        #endregion

        #region Characters

        public const string UppercaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string LowercaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        public const string DigitCharacters = "0123456789";
        public const string UppercaseLowercaseAndDigitCharacters =
            UppercaseCharacters + LowercaseCharacters + DigitCharacters;

        #endregion

        #endregion

        #region Methods

        public static string ConnectionString { get; set; }
        public static Func<string, DbConnection> CreateConnectionFunc { get; set; }
        public static Func<DbCommand, DbDataAdapter> CreateDataAdapterFunc { get; set; }

        public static T CreateAndOpenConnection<T>() where T : DbConnection
        {
            var connection = CreateConnection<T>();
            connection.Open();

            return connection;
        }

        public static T CreateAndOpenConnection<T>(string connectionString) where T : DbConnection
        {
            var connection = CreateConnection<T>(connectionString);
            connection.Open();

            return connection;
        }

        public static T CreateConnection<T>() where T : DbConnection
        {
            if(ConnectionString == null)
                throw new InvalidOperationException("Must set the Utils.ConnectionString property.");

            return CreateConnection<T>(ConnectionString);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static T CreateConnection<T>(string connectionString) where T : DbConnection
        {
            if(connectionString == null)
                throw new ArgumentException("connectionString cannot be null", nameof(connectionString));
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

        public static Exception ExecuteWithTryCatch(Action action)
        {
            try
            {
                action();
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }

        public static Result<T> ExecuteWithTryCatch<T>(Func<T> func)
        {
            try
            {
                return new Result<T>(func());
            }
            catch(Exception e)
            {
                return new Result<T>(e);
            }
        }

        public static async Task<Exception> ExecuteWithTryCatchAsync(Func<Task> func)
        {
            try
            {
                await func();
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }

        public static async Task<Result<T>> ExecuteWithTryCatchAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return new Result<T>(await func());
            }
            catch(Exception e)
            {
                return new Result<T>(e);
            }
        }

        public static double From(int constant, double value) => constant * value;
        public static int FromTens(int value) => Ten * value;
        public static int FromHundreds(int value) => Hundred * value;
        public static int FromThousands(int value) => Thousand * value;
        public static int FromMillions(int value) => Million * value;
        public static int FromBillions(int value) => Billion * value;

        public static TimeSpan FromWeeks(double weeks) => TimeSpan.FromDays(weeks * DaysPerWeek);

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

        public static string RandomString(int length, string characters = UppercaseLowercaseAndDigitCharacters)
        {
            var stringBuilder = new StringBuilder(length, length);
            var random = new Random();

            for(var i = 0; i < length; i++)
            {
                stringBuilder.Append(characters[random.Next(characters.Length)]);
            }

            return stringBuilder.ToString();
        }

        // https://stackoverflow.com/questions/7075201/rounding-up-to-2-decimal-places-in-c-sharp
        public static decimal RoundDown(decimal value, int decimals)
        {
            if(decimals < 0)
                throw new ArgumentException("Decimals cannot be negative.", nameof(decimals));
            
            var multiplier = (decimal) Math.Pow(10, decimals);

            return Math.Floor(value * multiplier) / multiplier;
        }
        
        public static decimal RoundUp(decimal value, int decimals)
        {
            if(decimals < 0)
                throw new ArgumentException("Decimals cannot be negative.", nameof(decimals));

            var multiplier = (decimal) Math.Pow(10, decimals);

            return Math.Ceiling(value * multiplier) / multiplier;
        }

        public static T Self<T>(T item) => item;

        public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

        #endregion
    }
}
