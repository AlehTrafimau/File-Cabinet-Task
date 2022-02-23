using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates bank account parameter of record by default rules.
    /// </summary>
    internal class DefaultBankAccountValidator : IRecordValidator
    {
        /// <summary>
        /// Validates bank account parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
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
