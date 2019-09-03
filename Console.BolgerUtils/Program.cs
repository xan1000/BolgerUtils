using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BolgerUtils;
using BolgerUtils.EnumDisplay;
using BolgerUtils.FileToObjectMapping;
using Newtonsoft.Json;
using static System.Console; 

namespace Console.BolgerUtils
{
    public static class Program
    {
        private static void Main()
        {
            ProgramEnumDisplay.Run();
            ProgramFileToObjectsMapping.Run();
        }
    }

    #region EnumDisplay

    public enum ShapeType
    {
        Square = 10,
        Triangle = 20,
        Circle = 30
    }

    public enum AccountType
    {
        RegularAccount = 10,
        [Add("Savings Account (Custom)")]
        SavingsAccount = 20,
        CheckingAccount = 30
    }

    public static class ProgramEnumDisplay
    {
        public static void Run()
        {
            EnumDisplayTest();
            //PerformanceTest();
        }

        // ReSharper disable once UnusedMember.Local
        private static void EnumDisplayTest()
        {
            ShapeType.Square.Add("Square (Custom)");
            AccountType.RegularAccount.Add("Account (Custom)");

            EnumDisplayExtensionUtils.EnumDisplayUtils.AddAll<ShapeType>();
            EnumDisplayExtensionUtils.EnumDisplayUtils.AddAll<AccountType>();

            WriteLine(ShapeType.Square.Display());
            WriteLine(ShapeType.Triangle.Display());
            WriteLine(ShapeType.Circle.Display());

            WriteLine(AccountType.RegularAccount.Display());
            WriteLine(AccountType.SavingsAccount.Display());
            WriteLine(AccountType.CheckingAccount.Display());

            WriteLine();
        }

        // ReSharper disable once UnusedMember.Local
        private static void EnumDisplayPerformanceTest()
        {
            var stopWatch = Stopwatch.StartNew();

            var enumDisplayUtils = new EnumDisplayUtils(false);
            var shapeTypes = Utils.GetEnumValues<ShapeType>().ToArray();
            var accountTypes = Utils.GetEnumValues<AccountType>().ToArray();

            const int loopCount = 1000000;
            for(int i = 0; i < loopCount; i++)
            {
                foreach(var x in shapeTypes)
                {
                    enumDisplayUtils.Display(x);
                }
                foreach(var x in accountTypes)
                {
                    enumDisplayUtils.Display(x);
                }
            }

            stopWatch.Stop();
            WriteLine(stopWatch.Elapsed);
        }
    }

    #endregion

    #region FileToObjectsMapping

    // ReSharper disable once ClassNeverInstantiated.Global
    public class Person
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string Name { get; set; }
        public int Age { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class Account
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }

    public static class ProgramFileToObjectsMapping
    {
        private const string PeoplePath = "Data/people.json";
        private const string AccountPath = "Data/accounts.json";

        private static List<Person> People =>
            FileToObjectUtils.Map(PeoplePath, JsonConvert.DeserializeObject<List<Person>>);
        //JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(PeoplePath));

        private static List<Account> Accounts => FileToObjectUtils.Load<List<Account>>(AccountPath);
        //JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(AccountPath));

        public static void Run()
        {
            FileToObjectUtils.Register(AccountPath, JsonConvert.DeserializeObject<List<Account>>);

            Test();
            //FileToObjectMappingPerformanceTest();
        }

        private static void Test()
        {
            for(int i = 0; i < 5; i++)
            {
                PrintPeople();
                PrintAccounts();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static void FileToObjectMappingPerformanceTest()
        {
            var stopWatch = Stopwatch.StartNew();

            const int loopCount = 100000;
            for(int i = 0; i < loopCount; i++)
            {
                WriteLine(i);

                // ReSharper disable once UnusedVariable
                foreach(var x in People)
                {
                }

                // ReSharper disable once UnusedVariable
                foreach(var x in Accounts)
                {
                }
            }

            stopWatch.Stop();
            WriteLine(stopWatch.Elapsed);
        }

        private static void PrintPeople()
        {
            WriteLine("--- People ---");
            foreach(var x in People)
            {
                WriteLine($"Name: {x.Name}, Age: {x.Age}");
            }
            WriteLine();
        }

        private static void PrintAccounts()
        {
            WriteLine("--- Accounts ---");
            foreach(var x in Accounts)
            {
                WriteLine($"AccountNumber: {x.AccountNumber}, Balance: {x.Balance:C}");
            }
            WriteLine();
        }
    }

    #endregion
}
