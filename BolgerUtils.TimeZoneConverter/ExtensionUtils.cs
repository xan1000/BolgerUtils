using System;
using System.Text;

namespace BolgerUtils.TimeZoneConverter
{
    public static class ExtensionUtils
    {
        public static DateTime ConvertTimeFromDefaultTimeZoneToUtc(this DateTime dateTime) =>
            Utils.ConvertTimeFromDefaultTimeZoneToUtc(dateTime);

        public static DateTime ConvertTimeFromTimeZoneToUtc(
            this DateTime dateTime, SystemTimeZoneInfoID sourceTimeZoneID) =>
            Utils.ConvertTimeFromTimeZoneToUtc(dateTime, sourceTimeZoneID);

        public static DateTime ConvertTimeFromUtcToDefaultTimeZone(this DateTime dateTimeUtc) =>
            Utils.ConvertTimeFromUtcToDefaultTimeZone(dateTimeUtc);

        public static DateTime ConvertTimeFromUtcToTimeZone(
            this DateTime dateTimeUtc, SystemTimeZoneInfoID targetTimeZoneID) =>
            Utils.ConvertTimeFromUtcToTimeZone(dateTimeUtc, targetTimeZoneID);

        // Source code origin:
        // https://github.com/xan1000/BolgerUtils/blob/master/BolgerUtils/ExtensionUtils.cs ->
        // UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter
        internal static string UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter(this string item)
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
    }
}
