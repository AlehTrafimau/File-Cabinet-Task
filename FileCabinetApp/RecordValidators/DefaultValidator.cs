using System.Text;
using FileCabinetApp.DefaultValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of function for record validation according dafault conditionals.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator"/>
    internal class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates record by default rules.
        /// </summary>
        /// <param name="record">The file cabinet record instance for validation.</param>
        /// <returns>The result of record validation.</returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> resultOfRecordValidation;
            var validateErrors = new StringBuilder();
            bool hasError = true;
            List<Tuple<bool, string>> resultsOfValidation = new ()
            {
                new DefaultFirstNameValidator().ValidateParameters(record),
                new DefaultLastNameValidator().ValidateParameters(record),
                new DefaultBankAccountValidator().ValidateParameters(record),
                new DefaultSerieOfPassNumberValidator().ValidateParameters(record),
                new DefaultPassNumberValidator().ValidateParameters(record),
                new DefaultBankAccountValidator().ValidateParameters(record),
            };

            foreach (var i in resultsOfValidation)
            {
                if (i.Item1 == false)
                {
                    hasError = false;
                    validateErrors.Append(i.Item2);
                }
            }

            resultOfRecordValidation = new Tuple<bool, string>(hasError, validateErrors.ToString());
            return resultOfRecordValidation;
        }
    }
}