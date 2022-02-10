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
    /// Consist of a set of function for record validation according dafault conditionals.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator"/>
    internal class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Gets record parameters from console and validate them. It uses default conditionals.
        /// </summary>
        /// <returns>
        /// The new record with valid parameters.
        /// </returns>
        public FileCabinetRecord ValidateParameters()
        {
            var record = new FileCabinetRecord
            {
                FirstName = ConsoleExtension.ReadTillSuccess("First name: ", "Invalid first name parameter", this.CheckName),
                LastName = ConsoleExtension.ReadTillSuccess("Last name: ", "Invalid last name parameter", this.CheckName),
                DateOfBirth = DateTime.Parse(ConsoleExtension.ReadTillSuccess("Birth date: ", "Invalid birth date parameter. Correct format: \"Year - Month - Day\"", this.CheckBirthDate), CultureInfo.InvariantCulture),
                SerieOfPassNumber = char.Parse(ConsoleExtension.ReadTillSuccess("Serie of pass number: ", "Invalid Serie of pass number parameter. Correct format: \"A\" (one letter)", this.CheckSerieOfPassNumber)),
                PassNumber = short.Parse(ConsoleExtension.ReadTillSuccess("Pass number: ", "Invalid pass number parameter. Correct format: \"1111\" (1-4 digits)", this.CheckPassNumber), CultureInfo.InvariantCulture),
                BankAccount = decimal.Parse(ConsoleExtension.ReadTillSuccess("Bank account: ", "Invalid bank account parameter", this.CheckBankAccount), CultureInfo.InvariantCulture),
            };

            return record;
        }

        /// <summary>
        /// Checks the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckName(string name)
        {
            bool result = false;

            if (name != null && Regex.IsMatch(name, @"^\S{2,60}$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks the birth date.
        /// </summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckBirthDate(string birthDate)
        {
            bool result = false;

            if (birthDate == null)
            {
                return false;
            }

            bool convertToDateTime = DateTime.TryParse(birthDate, out DateTime dayOfBirth);
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new (1950, 1, 1);

            if (convertToDateTime && dayOfBirth.CompareTo(currentDate) < 0 && dayOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks the serie of pass number.
        /// </summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckSerieOfPassNumber(string serieOfPassNumber)
        {
            bool result = false;

            if (serieOfPassNumber != null && Regex.IsMatch(serieOfPassNumber, @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks the pass number.
        /// </summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckPassNumber(string passNumber)
        {
            bool result = false;

            if (passNumber != null && Regex.IsMatch(passNumber, @"^\d{1,4}$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks the bank account.
        /// </summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckBankAccount(string bankAccount)
        {
            try
            {
                decimal convertToDecimal = decimal.Parse(bankAccount, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}