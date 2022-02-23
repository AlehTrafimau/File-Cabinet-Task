namespace FileCabinetApp.DefaultValidators
{
    /// <summary>
    /// Validates date of birth parameter of record by default rules.
    /// </summary>
    internal class DefaultDateOfBirthValidator : IRecordValidator
    {
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
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new (1950, 1, 1);

            if (record.DateOfBirth.CompareTo(currentDate) < 0 && record.DateOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = new (true, string.Empty);
            }
            else
            {
                result = new (false, "Incorrect date of birth. This parameter must be from \"1950-1-1\" to current date.");
            }

            return result;
        }
    }
}
