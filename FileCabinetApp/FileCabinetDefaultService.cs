using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to create, storage, edit, find and display records about users. It uses dafault conditional to create new record.
    /// </summary>
    internal class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <returns>The record which conform default conditionals.</returns>
        public override FileCabinetRecord ValidateParameters()
        {
            var record = new FileCabinetRecord
            {
                FirstName = ConsoleExtension.ReadTillSuccess("First name: ", "Invalid first name parameter", CheckName),
                LastName = ConsoleExtension.ReadTillSuccess("Last name: ", "Invalid last name parameter", CheckName),
                DateOfBirth = DateTime.Parse(ConsoleExtension.ReadTillSuccess("Birth date: ", "Invalid birth date parameter. Correct format: \"Year - Month - Day\"", CheckBirthDate), CultureInfo.InvariantCulture),
                SerieOfPassNumber = char.Parse(ConsoleExtension.ReadTillSuccess("Serie of pass number: ", "Invalid Serie of pass number parameter. Correct format: \"A\" (one letter)", CheckSerieOfPassNumber)),
                PassNumber = short.Parse(ConsoleExtension.ReadTillSuccess("Pass number: ", "Invalid pass number parameter. Correct format: \"1111\" (1-4 digits)", CheckPassNumber), CultureInfo.InvariantCulture),
                BankAccount = decimal.Parse(ConsoleExtension.ReadTillSuccess("Bank account: ", "Invalid bank account parameter", CheckBankAccount), CultureInfo.InvariantCulture),
            };

            return record;
        }

        private static bool CheckName(string name)
        {
            bool result = false;

            if (name != null && !Regex.IsMatch(name, @"^\S{2,60}$"))
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
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new (1950, 1, 1);

            if (convertToDateTime && dayOfBirth.CompareTo(currentDate) < 0 && dayOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckSerieOfPassNumber(string serieOfPassNumber)
        {
            bool result = false;

            if (serieOfPassNumber != null && Regex.IsMatch(serieOfPassNumber, @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = true;
            }

            return result;
        }

        private static bool CheckPassNumber(string passNumber)
        {
            bool result = false;

            if (passNumber != null && Regex.IsMatch(passNumber, @"^\d{1,4}$"))
            {
                result = true;
            }

            return result;
        }

        private static bool CheckBankAccount(string bankAccount)
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