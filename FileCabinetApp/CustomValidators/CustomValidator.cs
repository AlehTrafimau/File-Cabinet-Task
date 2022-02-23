using System.Text;
using FileCabinetApp.CustomValidators;
using FileCabinetApp.GeneralValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of function for record validation according custom conditionals.
    /// </summary>
    /// <seealso cref="IRecordValidator"/>
    public class CustomValidator : IRecordValidator
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
                new FirstNameValidator(5, 30).ValidateParameters(record),
                new LastNameValidator(5, 30).ValidateParameters(record),
                new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now.AddYears(-10)).ValidateParameters(record),
                new CustomSerieOfPassNumberValidator().ValidateParameters(record),
                new CustomPassNumberValidator().ValidateParameters(record),
                new CustomBankAccountValidator().ValidateParameters(record),
            };

            foreach (var i in resultsOfValidation)
            {
                if (i.Item1 == false)
                {
                    hasError = false;
                    validateErrors.Append(i.Item2);
                    validateErrors.Append('\n');
                }
            }

            resultOfRecordValidation = new Tuple<bool, string>(hasError, validateErrors.ToString());
            return resultOfRecordValidation;
        }
    }
}
