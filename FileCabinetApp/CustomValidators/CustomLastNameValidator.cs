using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CustomValidators
{
    /// <summary>
    /// Validates the last name parameter of record by custom rules.
    /// </summary>
    internal class CustomLastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the last name parameter of record by custom rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
    }
}
