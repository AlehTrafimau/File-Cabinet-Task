using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of function for record validation according custom conditionals.
    /// </summary>
    /// <seealso cref="IRecordValidator"/>
    public class CustomValidator : IRecordValidator
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

        private Tuple<bool, string> CheckBirthDate(DateTime birthDate)
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

        private Tuple<bool, string> CheckSerieOfPassNumber(char serieOfPassNumber)
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

        private Tuple<bool, string> CheckPassNumber(short passNumber)
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
