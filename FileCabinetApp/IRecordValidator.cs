namespace FileCabinetApp
{
    /// <summary>
    /// Consists a function to record validate.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates record by default rules.
        /// </summary>
        /// <param name="record">The file cabinet record instance for validation.</param>
        /// <returns>The result of record validation.</returns>
        public Tuple<bool, string[]> ValidateParameters(FileCabinetRecord record);
    }
}
