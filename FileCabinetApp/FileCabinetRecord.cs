using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Description properties of record.
    /// </summary>
    [XmlType("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthDate">The birth date.</param>
        /// <param name="serieOfPassNumber">The serie of pass number.</param>
        /// <param name="passNumber">The pass number.</param>
        /// <param name="bankAccount">The bank account.</param>
        public FileCabinetRecord(int id, string firstName, string lastName, DateTime birthDate, char serieOfPassNumber, short passNumber, decimal bankAccount)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = birthDate;
            this.SerieOfPassNumber = serieOfPassNumber;
            this.PassNumber = passNumber;
            this.BankAccount = bankAccount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
            this.Id = default;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.DateOfBirth = default;
            this.SerieOfPassNumber = default;
            this.PassNumber = default;
            this.BankAccount = default;
        }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        /// <value>The first name.</value>
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        /// <value>The last name.</value>
        [XmlElement("lastName")]
        public string LastName { get; set; }

        /// <summary>Gets or sets the date of birth.</summary>
        /// <value>The date of birth.</value>
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>Gets or sets the serie of pass number.</summary>
        /// <value>The serie of pass number.</value>
        [XmlElement("serieOfPassNumber")]
        public char SerieOfPassNumber { get; set; }

        /// <summary>Gets or sets the pass number.</summary>
        /// <value>The pass number.</value>
        [XmlElement(ElementName = "passNumber")]
        public short PassNumber { get; set; }

        /// <summary>Gets or sets the bank account.</summary>
        /// <value>The bank account.</value>
        [XmlElement(ElementName = "bankAccount")]
        public decimal BankAccount { get; set; }
    }
}
