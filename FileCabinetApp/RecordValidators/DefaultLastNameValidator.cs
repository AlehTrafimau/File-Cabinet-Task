using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates the last name parameter of record by default rules.
    /// </summary>
    internal class DefaultLastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validates last name parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.LastName != null && Regex.IsMatch(record.LastName, @"^\S{2,60}$"))
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "The length of last name must be from 2 to 60 symbols");
            }

            return result;
        }
    }
}
