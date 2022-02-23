using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates the pass number parameter of record by default rules.
    /// </summary>
    internal class DefaultPassNumberValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the pass number parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (Regex.IsMatch(record.PassNumber.ToString(CultureInfo.InvariantCulture), @"^\d{1,4}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Invalid pass number. Correct format: (1-4 digits only)");
            }

            return result;
        }
    }
}
