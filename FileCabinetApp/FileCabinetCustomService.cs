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
    /// Service to create, storage, edit, find and display records about users. It uses dafault conditional to create new record.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <returns>The record which conform default comditionals.</returns>
        public override FileCabinetRecord ValidateParameters()
        {
            var record = new FileCabinetRecord
            {
                FirstName = ConsoleExtension.ReadTillSuccess("First name: ", "Invalid first name parameter. Correct format: \"FIRSTNAME\" (Upper case)", CheckName),
                LastName = ConsoleExtension.ReadTillSuccess("Last name: ", "Invalid last name parameter. Correct format: \"LASTNAME\" (Upper case)", CheckName),
                DateOfBirth = DateTime.Parse(ConsoleExtension.ReadTillSuccess("Birth date: ", "Invalid birth date parameter. Correct format: \"Year - Month - Day\" and user must be older than 10 years.", CheckBirthDate), CultureInfo.InvariantCulture),
                SerieOfPassNumber = char.Parse(ConsoleExtension.ReadTillSuccess("Serie of pass number: ", "Invalid Serie of pass number parameter. Correct format: \"A\" (one letter) and available series: A-G", CheckSerieOfPassNumber)),
                PassNumber = short.Parse(ConsoleExtension.ReadTillSuccess("Pass number: ", "Invalid pass number parameter. Correct format: (1-3 digits only)", CheckPassNumber), CultureInfo.InvariantCulture),
                BankAccount = decimal.Parse(ConsoleExtension.ReadTillSuccess("Bank account: ", "Invalid bank account parameter. The bank account must be more than zero or equal zero", CheckBankAccount), CultureInfo.InvariantCulture),
            };

            return record;
        }

        private static bool CheckName(string name)
        {
            bool result = false;

            if (name != null && Regex.IsMatch(name, @"(^[A-Z]{1,60}$)|(^[A-Z]{1,30}-[A-Z]{1,30}$)"))
            {
                result = true;
            }

            return result;
        }

        private static bool CheckBirthDate(string birthDate)
        {
            bool result = false;

            if (birthDate == null)
            {
                return false;
            }

            bool convertToDateTime = DateTime.TryParse(birthDate, out DateTime dayOfBirth);
            DateTime currentDate = DateTime.Now.AddYears(-10);
            DateTime minValueDateOfBirth = new (1900, 1, 1);

            if (convertToDateTime && dayOfBirth.CompareTo(currentDate) < 0 && dayOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckSerieOfPassNumber(string serieOfPassNumber)
        {
            bool result = false;

            if (serieOfPassNumber != null && Regex.IsMatch(serieOfPassNumber, @"^([A-G]{1}|[a-g]{1})$"))
            {
                result = true;
            }

            return result;
        }

        private static bool CheckPassNumber(string passNumber)
        {
            bool result = false;

            if (passNumber != null && Regex.IsMatch(passNumber, @"^\d{1,3}$"))
            {
                result = true;
            }

            return result;
        }

        private static bool CheckBankAccount(string bankAccount)
        {
            bool result;
            try
            {
                decimal convertToDecimal = decimal.Parse(bankAccount, CultureInfo.InvariantCulture);

                result = convertToDecimal >= 0;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
