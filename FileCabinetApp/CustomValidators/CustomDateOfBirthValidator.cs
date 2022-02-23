using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CustomValidators
{
    internal class CustomDateOfBirthValidator : IRecordValidator
    {
        /// <summary>
        /// Validates first name parameter of record by custom rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
    }
}
