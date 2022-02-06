using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class VerifyEntryData
    {
        public static string? FirstNameCheck()
        {
            string? firstNameOfUser;
            while (true)
            {
                Console.Write("First name: ");
                firstNameOfUser = Console.ReadLine();
                if (firstNameOfUser != null && firstNameOfUser.Length >= 2 && firstNameOfUser.Length <= 60 && !Regex.IsMatch(firstNameOfUser, @"^\s+$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format first name");
                }
            }

            return firstNameOfUser;
        }

        public static string? LastNameCheck()
        {
            string? lastNameOfUser;
            while (true)
            {
                Console.Write("Last name: ");
                lastNameOfUser = Console.ReadLine();
                if (lastNameOfUser != null && lastNameOfUser.Length >= 2 && lastNameOfUser.Length <= 60 && !Regex.IsMatch(lastNameOfUser, @"^\s+$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format last name");
                }
            }

            return lastNameOfUser;
        }

        public static DateTime DateOfBirthCheck()
        {
            DateTime dateOfBirth = default;
            while (dateOfBirth == default)
            {
                Console.Write("Date of birth: ");
                string? dateFromConsole = Console.ReadLine();
                if (dateFromConsole != null)
                {
                    dateOfBirth = ConvertToDateTime(dateFromConsole);
                }
            }

            return dateOfBirth;
        }

        public static char SerieOfPassNumberCheck()
        {
            char serieOfPassNumber = default;
            while (true)
            {
                Console.Write("Serie of your pass number: ");
                string? source = Console.ReadLine();
                if (source != null && Regex.IsMatch(source, @"^[A-Z]{1}|[a-z]{1}$"))
                {
                    serieOfPassNumber = char.Parse(source);
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid serie of number (1 letter)");
                }
            }

            return serieOfPassNumber;
        }

        public static short PassNumberCheck()
        {
            short passNumber = default;
            while (true)
            {
                Console.Write("Your pass number: ");
                string? source = Console.ReadLine();
                if (source != null && Regex.IsMatch(source, @"^(\d{4}|\d{3}|\d{2}|\d{1})$"))
                {
                    passNumber = short.Parse(source, CultureInfo.InvariantCulture);
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid pass number (4 digits)");
                }
            }

            return passNumber;
        }

        public static decimal BankAccountCheck()
        {
            decimal bankAccount = default;
            while (true)
            {
                Console.Write("Your current bank account ($): ");
                string? source = Console.ReadLine();
                if (source != null && Regex.IsMatch(source, @"\d+(\.?\d+)?$"))
                {
                    bool isConvertedToDecimal = decimal.TryParse(source, out decimal result);
                    if (isConvertedToDecimal)
                    {
                        bankAccount = result;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter valid bank account");
                    }
                }
            }

            return bankAccount;
        }

        private static DateTime ConvertToDateTime(string source)
        {
            if (source == null)
            {
                return default;
            }

            Regex customBirthDateFormat = new Regex(@"^((0{1}|^)[1-9]{1}|1{1}[0-2]{1})\D\s?((0{1}[1-9]{1})|([1-9]{1})|([1-2]{1}[0-9]{1})|(3{1}[0-1]{1}))\D\s?((1{1}9{1}[5-9]{1}[0-9]{1})|(2{1}0{1}[0-9]{2}))$");

            if (!customBirthDateFormat.IsMatch(source))
            {
                Console.WriteLine("Invalid date of birth. Date format: month/ day/ year.");
                return default;
            }

            char[] separatorsForDateOfBirth = new char[] { '/', '\\', '.', ',', ' ' };
            string[] sourceSplit = source.Split(separatorsForDateOfBirth);

            int yearOfBirth = int.Parse(sourceSplit[2], CultureInfo.InvariantCulture);
            int dayOfBirth = int.Parse(sourceSplit[1], CultureInfo.InvariantCulture);
            int monthOfBirth = int.Parse(sourceSplit[0], CultureInfo.InvariantCulture);
            DateTime birthDateOfUser = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth, 0, 0, 0);
            DateTime currentDate = DateTime.Now;

            int resultOfCompare = birthDateOfUser.CompareTo(currentDate);

            if (resultOfCompare >= 0)
            {
                Console.WriteLine("Invalid date of birth (greater than current time or same as current time)");
                return default;
            }

            return birthDateOfUser;
        }
    }
}