using System;
using System.Collections.Generic;
using System.Threading;
using BolgerUtils.EnumDisplay;
using BolgerUtils.FileToObjectMapping;
using Newtonsoft.Json;
using static System.Console;

namespace Console.BolgerUtils.EnumDisplayAndFileToObjectMapping
{
    public static class Program
    {
        private static void Main()
        {
            WriteLine("--- Console.BolgerUtils.EnumDisplayAndFileToObjectMapping ---");
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
        [EnumDisplay("Savings Account (Custom)")]
        SavingsAccount = 20,
        CheckingAccount = 30
    }

    public static class ProgramEnumDisplay
    {
        public static void Run()
        {
            Test();
        }

        private static void Test()
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

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static List<Person> People =>
            FileToObjectUtils.Map(PeoplePath, JsonConvert.DeserializeObject<List<Person>>);

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static List<Account> Accounts => FileToObjectUtils.Load<List<Account>>(AccountPath);

        public static void Run()
        {
            FileToObjectUtils.Register(AccountPath, JsonConvert.DeserializeObject<List<Account>>);

            Test();
        }

        private static void Test()
        {
            for(var i = 0; i < 5; i++)
            {
                PrintPeople();
                PrintAccounts();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
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
