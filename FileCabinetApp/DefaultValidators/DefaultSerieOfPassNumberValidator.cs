using System.Text.RegularExpressions;

namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates the serie of pas number parameter by default rules.
    /// </summary>
    internal class DefaultSerieOfPassNumberValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the serie of pas number parameter by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(record.SerieOfPassNumber.ToString(), @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Invalid serie of pass number. Correct format of pass number serie: \"A\" (available one letter only)");
            }

            return result;
        }
    }
}
