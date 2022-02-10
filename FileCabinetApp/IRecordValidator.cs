using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consist of a set of functions to record validate.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Gets record parameters from console and validate them using check functions of this class
        /// </summary>
        /// <returns>
        /// The new correct record.
        /// </returns>
        public FileCabinetRecord ValidateParameters();

        /// <summary>
        /// Checks the name parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckName(string name);

        /// <summary>
        /// Checks the birth date.
        /// </summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckBirthDate(string birthDate);

        /// <summary>
        /// Checks the serie of pass number.
        /// </summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckSerieOfPassNumber(string serieOfPassNumber);

        /// <summary>
        /// Checks the pass number.
        /// </summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckPassNumber(string passNumber);

        /// <summary>
        /// Checks the bank account.
        /// </summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false, otherwise.
        /// </returns>
        public bool CheckBankAccount(string bankAccount);
    }
}
