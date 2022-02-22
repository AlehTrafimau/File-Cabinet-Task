using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of function for record validation according dafault conditionals.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator"/>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates records data.
        /// </summary>
        /// <param name="firstName">The first name validator.</param>
        /// <param name="lastName">The last name validator.</param>
        /// <param name="birthDate">The birth day validator.</param>
        /// <param name="serieOfPassNumber">The serie of pas number validator.</param>
        /// <param name="passNumber">The pass number validator.</param>
        /// <param name="bankAccount">The bank account validator.</param>
        public void ValidateParameters(out string firstName, out string lastName, out DateTime birthDate, out char serieOfPassNumber, out short passNumber, out decimal bankAccount)
        {
            Console.Write("First name: ");
            firstName = ConsoleExtension.ReadInput(StringConverter.StringConvert, this.CheckName);

            Console.Write("Last name: ");
            lastName = ConsoleExtension.ReadInput(StringConverter.StringConvert, this.CheckName);

            Console.Write("Birth date: ");
            birthDate = ConsoleExtension.ReadInput(StringConverter.DateTimeConvert, this.CheckBirthDate);

            Console.Write("Serie of pass number: ");
            serieOfPassNumber = ConsoleExtension.ReadInput(StringConverter.CharConvert, this.CheckSerieOfPassNumber);

            Console.Write("Pass number: ");
            passNumber = ConsoleExtension.ReadInput(StringConverter.ShortConvert, this.CheckPassNumber);

            Console.Write("Bank account: ");
            bankAccount = ConsoleExtension.ReadInput(StringConverter.DecimalConvert, this.CheckBankAccount);
        }

        private Tuple<bool, string> CheckName(string name)
        {
            Tuple<bool, string> result;

            if (name != null && Regex.IsMatch(name, @"^\S{2,60}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The length of name: from 2 to 60 symbols");
            }

            return result;
        }

        private Tuple<bool, string> CheckBirthDate(DateTime birthDate)
        {
            Tuple<bool, string> result;
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new (1950, 1, 1);

            if (birthDate.CompareTo(currentDate) < 0 && birthDate.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: \"Year - Month - Day\". This parameter is from \"1950-1-1\" to current date.");
            }

            return result;
        }

        private Tuple<bool, string> CheckSerieOfPassNumber(char serieOfPassNumber)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(serieOfPassNumber.ToString(), @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: \"A\" (one letter)");
            }

            return result;
        }

        private Tuple<bool, string> CheckPassNumber(short passNumber)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(passNumber.ToString(CultureInfo.InvariantCulture), @"^\d{1,4}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: (1-4 digits only)");
            }

            return result;
        }

        private Tuple<bool, string> CheckBankAccount(decimal bankAccount)
        {
            Tuple<bool, string> result;

            if (bankAccount >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The bank account must be more than zero or equal zero");
            }

            return result;
        }
    }
}