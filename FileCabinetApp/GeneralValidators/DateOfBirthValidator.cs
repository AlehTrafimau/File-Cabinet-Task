using System.Globalization;

namespace FileCabinetApp.GeneralValidators
{
    /// <summary>
    /// Validates date of birth parameter of record by input rules.
    /// </summary>
    internal class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="minDate">Min date of birth parameter.</param>
        /// <param name="maxDate">Max date of birth parameter.</param>
        public DateOfBirthValidator(DateTime minDate, DateTime maxDate)
        {
            this.minDate = minDate;
            this.maxDate = maxDate;
        }

        /// <summary>
        /// Validates date of birth parameter of record by default rules.
        /// </summary>
        /// <param name="record">The record for validate.</param>
        /// <returns>
        /// Results of validation: success of validation and errors in verification.
        /// </returns>
        public Tuple<bool, string> ValidateParameters(FileCabinetRecord record)
        {
            Tuple<bool, string> result;

            if (record.DateOfBirth.CompareTo(this.maxDate) < 0 && record.DateOfBirth.CompareTo(this.minDate) >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, $"Incorrect date of birth. This parameter must be from {this.minDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)} to {this.maxDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)}.");
            }

            return result;
        }
    }
}
