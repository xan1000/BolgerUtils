using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BolgerUtils.EnumDisplay
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumDisplayAttribute : Attribute
    {
        public string Value { get; }

        public EnumDisplayAttribute(string value) => Value = value;
    }

    public sealed class EnumDisplayUtils
    {
        public static string DefaultDisplay(Enum key)
        {
            var addAttribute = key.GetAttributeOfType<EnumDisplayAttribute>();

            return addAttribute != null ? addAttribute.Value :
                key.ToString().UpperCaseFirstLetterAndInsertSpaceBeforeEveryProceedingUpperCaseLetter();
        }

        public bool CacheValueOnDisplay { get; }

        private readonly Dictionary<Enum, string> _dictionary = new Dictionary<Enum, string>();
        private readonly Func<Enum, string> _defaultDisplay = DefaultDisplay;

        public EnumDisplayUtils(bool cacheValueOnDisplay = true, Func<Enum, string> defaultDisplay = null)
        {
            CacheValueOnDisplay = cacheValueOnDisplay;
            if(defaultDisplay != null)
                _defaultDisplay = defaultDisplay;
        }

        public void Add(Enum key, string value = null)
        {
            value ??= _defaultDisplay(key);

            _dictionary.Add(key, value);
        }

        public void AddAll<T>() where T : Enum
        {
            // Source code origin:
            // https://github.com/xan1000/BolgerUtils/blob/master/BolgerUtils/Utils.cs -> GetEnumValue
            foreach(var key in Enum.GetValues(typeof(T)).Cast<T>().Where(key => !ContainsKey(key)))
            {
                Add(key);
            }
        }

        public void Clear() => _dictionary.Clear();
        public bool ContainsKey(Enum key) => _dictionary.ContainsKey(key);

        public string Display(Enum key)
        {
            if(_dictionary.TryGetValue(key, out var value))
                return value;

            value = _defaultDisplay(key);
            if(CacheValueOnDisplay)
                Add(key, value);

            return value;
        }

        public bool Remove(Enum key) => _dictionary.Remove(key);
    }

    public static class EnumDisplayExtensionUtils
    {
        // https://stackoverflow.com/a/9276348
        internal static T GetAttributeOfType<T>(this Enum key) where T : Attribute
        {
            var memberInfo = key.GetType().GetMember(key.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return attributes.OfType<T>().FirstOrDefault();
        }

        public static EnumDisplayUtils EnumDisplayUtils { get; } = new EnumDisplayUtils();

        public static void Add(this Enum key, string value = null) => EnumDisplayUtils.Add(key, value);
        public static bool ContainsKey(this Enum key) => EnumDisplayUtils.ContainsKey(key);
        public static string Display(this Enum key) => EnumDisplayUtils.Display(key);
        public static bool Remove(this Enum key) => EnumDisplayUtils.Remove(key);

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
