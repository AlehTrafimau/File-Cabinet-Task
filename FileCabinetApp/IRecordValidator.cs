using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a function to record validate.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates records data.
        /// </summary>
        /// <param name="firstName">The first name validator.</param>
        /// <param name="lastName">The last name validator.</param>
        /// <param name="birthDate">The birth day validator.</param>
        /// <param name="serieOfPassNumber">The serie of pas number validator.</param>
        /// <param name="passNumber">The pass number validator.</param>
        /// <param name="bankAccount">The bank account validator.</param>
        public void ValidateParameters(out string firstName, out string lastName, out DateTime birthDate, out char serieOfPassNumber, out short passNumber, out decimal bankAccount);
    }
}
