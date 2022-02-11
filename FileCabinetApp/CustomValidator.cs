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
    /// Consists a set of function for record validation according custom conditionals.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator"/>
    internal class CustomValidator : IRecordValidator
    {
        /// <summary>Checks the name.</summary>
        /// <param name="name">The name.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckName(string name)
        {
            Tuple<bool, string> result;

            if (name != null && Regex.IsMatch(name, @"^([A-Z]{1,60}$)|(^[A-Z]{1,30}-[A-Z]{1,30}$)"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: \"NAME\" (Upper case)");
            }

            return result;
        }

        /// <summary>Checks the birth date.</summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckBirthDate(DateTime birthDate)
        {
            Tuple<bool, string> result;

            DateTime currentDate = DateTime.Now.AddYears(-10);
            DateTime minValueDateOfBirth = new (1900, 1, 1);

            if (birthDate.CompareTo(currentDate) < 0 && birthDate.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: \"Year - Month - Day\" and user must be older than 10 years.");
            }

            return result;
        }

        /// <summary>Checks the serie of pass number.</summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckSerieOfPassNumber(char serieOfPassNumber)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(serieOfPassNumber.ToString(), @"^([A-G]{1}|[a-g]{1})$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: \"A\" (one letter) and available letters: A-G");
            }

            return result;
        }

        /// <summary>Checks the pass number.</summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>True, if the parameter conform special conditionals, or false and error message, otherwise.</returns>
        public Tuple<bool, string> CheckPassNumber(short passNumber)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(passNumber.ToString(CultureInfo.InvariantCulture), @"^\d{1,3}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Correct format: (1-3 digits only)");
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
