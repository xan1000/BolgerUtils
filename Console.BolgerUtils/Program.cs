using System;
using BolgerUtils;
using static System.Console;

namespace Console.BolgerUtils
{
    public static class Program
    {
        private static void Main()
        {
            WriteLine("--- Console.BolgerUtils ---");

            var _int = 0;
            Test(_int, _int.IsZero());
            _int = 1;
            Test(_int, _int.IsZero());
            _int = -1;
            Test(_int, _int.IsZero());
            WriteLine();

            var _double = 0d;
            Test(_double, _double.IsZero());
            _double = 0.1;
            Test(_double, _double.IsZero());
            _double = -0.1;
            Test(_double, _double.IsZero());
            WriteLine();

            var _decimal = 0m;
            Test(_decimal, _decimal.IsZero());
            _decimal = 0.1m;
            Test(_decimal, _decimal.IsZero());
            _decimal = -0.1m;
            Test(_decimal, _decimal.IsZero());
            WriteLine();

            var _float = 0f;
            Test(_float, _float.IsZero());
            _float = 0.1f;
            Test(_float, _float.IsZero());
            _float = -0.1f;
            Test(_float, _float.IsZero());
            WriteLine();

            var _long = 0L;
            Test(_long, _long.IsZero());
            _long = 1L;
            Test(_long, _long.IsZero());
            _long = -1L;
            Test(_long, _long.IsZero());
            WriteLine();

            var _byte = (byte) 0;
            Test(_byte, _byte.IsZero());
            _byte = 1;
            Test(_byte, _byte.IsZero());
            WriteLine();

            var _datetime = new DateTime();
            Test(_datetime, _datetime.IsZero());
            _datetime = DateTime.MinValue;
            Test(_datetime, _datetime.IsZero());
            _datetime = DateTime.MaxValue;
            Test(_datetime, _datetime.IsZero());
            _datetime = DateTime.Today;
            Test(_datetime, _datetime.IsZero());
            WriteLine();
        }

        private static void Test(object item, bool isZero) =>
            WriteLine($"Type: {item.GetType()}, Value: {item}, IsZero: {isZero}");
    }
}
