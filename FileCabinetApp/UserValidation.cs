using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// The set of functions for validation user data.
    /// </summary>
    public static class UserValidation
    {
        /// <summary>Checks the name.</summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// "True", if source conform to cpecial conditionals, and "False" otherwise.
        /// </returns>
        public static bool CheckName(string name)
        {
            bool result = false;

            if (name != null && name.Length >= 2 && name.Length <= 60 && !Regex.IsMatch(name, @"^\s+$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>Checks the birth date.</summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>
        /// "True", if source conform to cpecial conditionals, and "False" otherwise.
        /// </returns>
        public static bool CheckBirthDate(string birthDate)
        {
            bool result = false;

            if (birthDate == null)
            {
                return false;
            }

            bool convertToDateTime = DateTime.TryParse(birthDate, out DateTime dayOfBirth);
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new DateTime(1950, 1, 1);

            if (convertToDateTime && dayOfBirth.CompareTo(currentDate) < 0 && dayOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>Checks the serie of pass number.</summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>
        /// "True", if source conform to special conditionals, and "False" otherwise.
        /// </returns>
        public static bool CheckSerieOfPassNumber(string serieOfPassNumber)
        {
            bool result = false;

            if (serieOfPassNumber != null && Regex.IsMatch(serieOfPassNumber, @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>Checks the pass number.</summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>
        /// "True", if source conform to cpecial conditionals, and "False" otherwise.
        /// </returns>
        public static bool CheckPassNumber(string passNumber)
        {
            bool result = false;

            if (passNumber != null && Regex.IsMatch(passNumber, @"^(\d{4}|\d{3}|\d{2}|\d{1})$"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>Checks the bank account.</summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <returns>
        /// "True", if source conform to cpecial conditionals, and "False" otherwise.
        /// </returns>
        public static bool CheckBankAccount(string bankAccount)
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