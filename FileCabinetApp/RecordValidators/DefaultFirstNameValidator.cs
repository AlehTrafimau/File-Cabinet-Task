using System.Text.RegularExpressions;

namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates the first name parameter of record by default rules.
    /// </summary>
    internal class DefaultFirstNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validates first name parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.FirstName != null && Regex.IsMatch(record.FirstName, @"^\S{2,60}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The length of first name must be from 2 to 60 symbols");
            }

            return result;
        }
    }
}
