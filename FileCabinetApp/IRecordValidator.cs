using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of functions to record validate.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Checks the name parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false and error message, otherwise.
        /// </returns>
        public Tuple<bool, string> CheckName(string name);

        /// <summary>
        /// Checks the birth date.
        /// </summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false and error message, otherwise.
        /// </returns>
        public Tuple<bool, string> CheckBirthDate(DateTime birthDate);

        /// <summary>
        /// Checks the serie of pass number.
        /// </summary>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false and error message, otherwise.
        /// </returns>
        public Tuple<bool, string> CheckSerieOfPassNumber(char serieOfPassNumber);

        /// <summary>
        /// Checks the pass number.
        /// </summary>
        /// <param name="passNumber">The pass number.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false and error message, otherwise.
        /// </returns>
        public Tuple<bool, string> CheckPassNumber(short passNumber);

        /// <summary>
        /// Checks the bank account.
        /// </summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <returns>
        /// True, if the parameter conform special conditionals, or false and error message, otherwise.
        /// </returns>
        public Tuple<bool, string> CheckBankAccount(decimal bankAccount);
    }
}
