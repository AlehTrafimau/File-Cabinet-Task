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
    internal class DefaultValidator : IRecordValidator
    {
        /// <summary>Checks the name.</summary>
        /// <param name="name">The name.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckName(string name)
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

        /// <summary>Checks the birth date.</summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckBirthDate(DateTime birthDate)
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

        /// <summary>Checks the serie of pass number.</summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckSerieOfPassNumber(char serieOfPassNumber)
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

        /// <summary>Checks the pass number.</summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckPassNumber(short passNumber)
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

        /// <summary>Checks the bank account.</summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckBankAccount(decimal bankAccount)
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