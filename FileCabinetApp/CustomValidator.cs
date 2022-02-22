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
        /// Validates record by default rules.
        /// </summary>
        /// <param name="record">The file cabinet record instance for validation.</param>
        /// <returns>The result of record validation.</returns>
        public Tuple<bool, string[]> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string[]> resultOfRecordValidation;
            List<string> validateErrors = new ();
            bool hasError = true;
            List<Tuple<bool, string>> resultsOfValidation = new ()
            {
                CheckFirstName(record),
                CheckLastName(record),
                CheckBirthDate(record),
                CheckSerieOfPassNumber(record),
                CheckPassNumber(record),
                CheckBankAccount(record),
            };

            foreach (var i in resultsOfValidation)
            {
                if (i.Item1 == false)
                {
                    hasError = false;
                    validateErrors.Add(i.Item2);
                }
            }

            resultOfRecordValidation = new Tuple<bool, string[]>(hasError, validateErrors.ToArray());
            return resultOfRecordValidation;
        }

        private static Tuple<bool, string> CheckFirstName(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.FirstName != null && Regex.IsMatch(record.FirstName, @"^([A-Z]{1,60}$)|(^[A-Z]{1,30}-[A-Z]{1,30}$)"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The length of first name must be from 2 to 60 upper case symbols. Correct format: \"NAME\" (Upper case)");
            }

            return result;
        }

        private static Tuple<bool, string> CheckLastName(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.LastName != null && Regex.IsMatch(record.LastName, @"^([A-Z]{1,60}$)|(^[A-Z]{1,30}-[A-Z]{1,30}$)"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The length of last name must be from 2 to 60 upper case symbols. Correct format: \"LAST-NAME\" (Upper case)");
            }

            return result;
        }

        private static Tuple<bool, string> CheckBirthDate(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            DateTime currentDate = DateTime.Now.AddYears(-10);
            DateTime minValueDateOfBirth = new (1900, 1, 1);

            if (record.DateOfBirth.CompareTo(currentDate) < 0 && record.DateOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Incorrect date of birth. This parameter must be from \"1950-1-1\" till 10 years before present time");
            }

            return result;
        }

        private static Tuple<bool, string> CheckSerieOfPassNumber(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(record.SerieOfPassNumber.ToString(), @"^([A-G]{1}|[a-g]{1})$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Invalid serie of pass number. Correct format of pass number serie: \"A\" (one letter) and available letters: A-G");
            }

            return result;
        }

        private static Tuple<bool, string> CheckPassNumber(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(record.PassNumber.ToString(CultureInfo.InvariantCulture), @"^\d{1,3}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Invalid pass number. Correct format: (1-3 digits only)");
            }

            return result;
        }

        private static Tuple<bool, string> CheckBankAccount(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.BankAccount >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Invalid bank account. The bank account must be more than zero or equal zero");
            }

            return result;
        }
    }
}
