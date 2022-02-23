using System.Text.RegularExpressions;

namespace FileCabinetApp.GeneralValidators
{
    /// <summary>
    /// Validates the first name parameter of record by input rules.
    /// </summary>
    internal class FirstNameValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="min">Min length of the first name.</param>
        /// <param name="max">Max length of the first name.</param>
        public FirstNameValidator(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validates first name parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.FirstName != null && Regex.IsMatch(record.FirstName, @"^\S+$") && record.FirstName.Length <= this.max && record.FirstName.Length >= this.min)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, $"The length of first name must be from {this.min} to {this.max} symbols");
            }

            return result;
        }
    }
}
