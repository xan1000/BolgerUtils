using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach(var key in Utils.GetEnumValues<T>().Where(key => !ContainsKey(key)))
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
    }
}
