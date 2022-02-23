using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CustomValidators
{
    /// <summary>
    /// Validates serie of pass number parameter of record by custom rules.
    /// </summary>
    internal class CustomSerieOfPassNumberValidator : IRecordValidator
    {
        /// <summary>
        /// Validates serie of pass number parameter of record by custom rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
    }
}
