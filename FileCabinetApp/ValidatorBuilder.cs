using FileCabinetApp.DefaultValidators;
using FileCabinetApp.GeneralValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// Builds the validator of record data.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBuilder"/> class by default validation rules.
        /// </summary>
        public ValidatorBuilder()
        {
            this.FirstNameValidator = new FirstNameValidator(2, 60);
            this.LastNameValidator = new LastNameValidator(2, 60);
            this.DateOfBirthValidator = new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now);
            this.SerieOfPassNumberValidator = new DefaultSerieOfPassNumberValidator();
            this.PassNumberValidator = new DefaultPassNumberValidator();
            this.BankAccountValidator = new DefaultBankAccountValidator();
        }

        /// <summary>
        /// Gets or sets the validator of the first name parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of the first name parameter of the record.
        /// </value>
        public IRecordValidator FirstNameValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of the last name parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of the last name parameter of the record.
        /// </value>
        public IRecordValidator LastNameValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of date of birth parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of date of birth parameter of the record.
        /// </value>
        public IRecordValidator DateOfBirthValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of serie of pass number validator parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of serie of pass number validator parameter of the record.
        /// </value>
        public IRecordValidator SerieOfPassNumberValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of pass number validator parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of pass number validator parameter of the record.
        /// </value>
        public IRecordValidator PassNumberValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of v parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of date of birth parameter of the record.
        /// </value>
        public IRecordValidator BankAccountValidator { get; set; }

        /// <summary>
        /// Sets first name validator.
        /// </summary>
        /// <param name="minSymbols">Min length of the first name.</param>
        /// <param name="maxSymbols">Max length of the first name.</param>
        /// <returns>The first name validator.</returns>
        public ValidatorBuilder ValidateFirstName(int minSymbols, int maxSymbols)
        {
            this.FirstNameValidator = new FirstNameValidator(minSymbols, maxSymbols);
            return this;
        }

        /// <summary>
        /// Sets last name validator.
        /// </summary>
        /// <param name="minSymbols">Min length of the last name.</param>
        /// <param name="maxSymbols">Max length of the last name.</param>
        /// <returns>The last name validator.</returns>
        public ValidatorBuilder ValidateLastName(int minSymbols, int maxSymbols)
        {
            this.LastNameValidator = new LastNameValidator(minSymbols, maxSymbols);
            return this;
        }

        /// <summary>
        /// Sets date of birth validator.
        /// </summary>
        /// <param name="minDate">Min value of the date of birth.</param>
        /// <param name="maxDate">Max value of the date of birth.</param>
        /// <returns>The date of birth validator.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime minDate, DateTime maxDate)
        {
            this.DateOfBirthValidator = new DateOfBirthValidator(minDate, maxDate);
            return this;
        }

        /// <summary>
        /// Sets serie of pass number validator.
        /// </summary>
        /// <param name="serieOfPassNumberValidator">The custom serie of pass number validator.</param>
        /// <returns>The serie of pass number validator.</returns>
        public ValidatorBuilder ValidateSerieOfPassNumber(IRecordValidator serieOfPassNumberValidator)
        {
            this.SerieOfPassNumberValidator = serieOfPassNumberValidator;
            return this;
        }

        /// <summary>
        /// Sets pass number validator.
        /// </summary>
        /// <param name="passNumberValidator">The custom pass number validator.</param>
        /// <returns>The pass number validator.</returns>
        public ValidatorBuilder ValidatePassNumber(IRecordValidator passNumberValidator)
        {
            this.PassNumberValidator = passNumberValidator;
            return this;
        }

        /// <summary>
        /// Sets bank account validator.
        /// </summary>
        /// <param name="bankAccountValidator">The custom bank account validator.</param>
        /// <returns>The bank account validator.</returns>
        public ValidatorBuilder ValidateBankAccount(IRecordValidator bankAccountValidator)
        {
            this.BankAccountValidator = bankAccountValidator;
            return this;
        }

        /// <summary>
        /// Creates new record validator.
        /// </summary>
        /// <returns>The new record validator.</returns>
        public IRecordValidator Create()
        {
            this.validators.AddRange(new IRecordValidator[] { this.FirstNameValidator, this.LastNameValidator, this.DateOfBirthValidator, this.SerieOfPassNumberValidator, this.PassNumberValidator, this.BankAccountValidator });
            return new CompositeValidator(this.validators);
        }
    }
}
