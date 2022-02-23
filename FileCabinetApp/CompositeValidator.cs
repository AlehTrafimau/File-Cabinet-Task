using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates the record according to the entry conditions.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">A list of validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators.AddRange(validators);
        }

        /// <summary>
        /// Validates record.
        /// </summary>
        /// <param name="record">The file cabinet record instance for validation.</param>
        /// <returns>The result of record validation.</returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> resultOfRecordValidation;
            var validateErrors = new StringBuilder();
            bool hasError = true;
            List<Tuple<bool, string>> resultsOfValidation = new ();

            foreach (var validator in this.validators)
            {
                resultsOfValidation.Add(validator.ValidateParameters(record));
            }

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
