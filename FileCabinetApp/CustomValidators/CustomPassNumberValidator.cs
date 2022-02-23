using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CustomValidators
{
    /// <summary>
    /// Validates pass number parameter of record by custom rules.
    /// </summary>
    internal class CustomPassNumberValidator : IRecordValidator
    {
        /// <summary>
        /// Validates pass number parameter of record by custom rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
    }
}
