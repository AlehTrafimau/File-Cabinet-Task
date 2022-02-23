using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CustomValidators
{
    /// <summary>
    /// Validates the first name parameter of record by custom rules.
    /// </summary>
    internal class CustomFirstNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the first name parameter of record by custom rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
    }
}
