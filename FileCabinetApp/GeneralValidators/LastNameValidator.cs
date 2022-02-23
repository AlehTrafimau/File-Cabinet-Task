using System.Text.RegularExpressions;

namespace FileCabinetApp.GeneralValidators
{
    /// <summary>
    /// Validates the last name parameter of record by input rules.
    /// </summary>
    internal class LastNameValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="min">Min length of the last name.</param>
        /// <param name="max">Max length of the last name.</param>
        public LastNameValidator(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

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

            if (record.LastName != null && Regex.IsMatch(record.LastName, @"^\S+$") && record.LastName.Length < this.max && record.LastName.Length > this.min)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, $"The length of last name must be from {this.min} to {this.max} symbols");
            }

            return result;
        }
    }
}
