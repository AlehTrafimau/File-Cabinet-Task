namespace FileCabinetApp
{
    /// <summary>
    /// Description properties of record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>The first name.</value>
        public string? FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>The last name.</value>
        public string? LastName { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>The date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>Gets or sets the serie of pass number.</summary>
        /// <value>The serie of pass number.</value>
        public char SerieOfPassNumber { get; set; }

        /// <summary>Gets or sets the pass number.</summary>
        /// <value>The pass number.</value>
        public short PassNumber { get; set; }

        /// <summary>Gets or sets the bank account.</summary>
        /// <value>The bank account.</value>
        public decimal BankAccount { get; set; }
    }
}
