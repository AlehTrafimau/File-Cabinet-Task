using System.Globalization;
using FileCabinetApp.CustomValidators;
using FileCabinetApp.DefaultValidators;
using FileCabinetApp.GeneralValidators;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Builds the validator of record data.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new ();

        /// <summary>
        /// Gets or sets the validator of the first name parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of the first name parameter of the record.
        /// </value>
        public IRecordValidator? FirstNameValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of the last name parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of the last name parameter of the record.
        /// </value>
        public IRecordValidator? LastNameValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of date of birth parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of date of birth parameter of the record.
        /// </value>
        public IRecordValidator? DateOfBirthValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of serie of pass number validator parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of serie of pass number validator parameter of the record.
        /// </value>
        public IRecordValidator? SerieOfPassNumberValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of pass number validator parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of pass number validator parameter of the record.
        /// </value>
        public IRecordValidator? PassNumberValidator { get; set; }

        /// <summary>
        /// Gets or sets the validator of v parameter of the record.
        /// </summary>
        /// <value>
        /// The validator of date of birth parameter of the record.
        /// </value>
        public IRecordValidator? BankAccountValidator { get; set; }

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
            if (this.FirstNameValidator == null || this.LastNameValidator == null || this.DateOfBirthValidator == null || this.SerieOfPassNumberValidator == null || this.PassNumberValidator == null || this.BankAccountValidator == null)
            {
                throw new ArgumentNullException(message: "One or more validators is null. Call of the Create() method available after initialize all validators.", null);
            }

            this.validators.AddRange(new IRecordValidator[] { this.FirstNameValidator, this.LastNameValidator, this.DateOfBirthValidator, this.SerieOfPassNumberValidator, this.PassNumberValidator, this.BankAccountValidator });
            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Creates new record validator by default conditions.
        /// </summary>
        /// <returns>The new record validator.</returns>
        public IRecordValidator CreateDefault()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("FileCabinetSettings.json")
                .Build();

            var appConfig = builder.GetSection("default");

            int minValueOfFirstName = int.Parse(appConfig.GetSection("firstName").GetSection("min").Value, CultureInfo.InvariantCulture);
            int maxValueOfFirstName = int.Parse(appConfig.GetSection("firstName").GetSection("max").Value, CultureInfo.InvariantCulture);
            this.FirstNameValidator = new FirstNameValidator(minValueOfFirstName, maxValueOfFirstName);

            int minValueOfLastName = int.Parse(appConfig.GetSection("LastName").GetSection("min").Value, CultureInfo.InvariantCulture);
            int maxValueOfLastName = int.Parse(appConfig.GetSection("LastName").GetSection("max").Value, CultureInfo.InvariantCulture);
            this.LastNameValidator = new LastNameValidator(minValueOfLastName, maxValueOfLastName);

            DateTime minValueOfDateOfBirth = DateTime.Parse(appConfig.GetSection("DateOfBirth").GetSection("from").Value, CultureInfo.InvariantCulture);
            DateTime maxValueOfDateOfBirth = DateTime.Parse(appConfig.GetSection("DateOfBirth").GetSection("to").Value, CultureInfo.InvariantCulture);
            this.DateOfBirthValidator = new DateOfBirthValidator(minValueOfDateOfBirth, maxValueOfDateOfBirth);

            this.SerieOfPassNumberValidator = new DefaultSerieOfPassNumberValidator();
            this.PassNumberValidator = new DefaultPassNumberValidator();
            this.BankAccountValidator = new DefaultBankAccountValidator();

            this.validators.AddRange(new IRecordValidator[] { this.FirstNameValidator, this.LastNameValidator, this.DateOfBirthValidator, this.SerieOfPassNumberValidator, this.PassNumberValidator, this.BankAccountValidator });
            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Creates new record validator by custom conditions.
        /// </summary>
        /// <returns>The new record validator.</returns>
        public IRecordValidator CreateCustom()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("FileCabinetSettings.json")
                .Build();

            var appConfig = builder.GetSection("custom");

            int minValueOfFirstName = int.Parse(appConfig.GetSection("firstName").GetSection("min").Value, CultureInfo.InvariantCulture);
            int maxValueOfFirstName = int.Parse(appConfig.GetSection("firstName").GetSection("max").Value, CultureInfo.InvariantCulture);
            this.FirstNameValidator = new FirstNameValidator(minValueOfFirstName, maxValueOfFirstName);

            int minValueOfLastName = int.Parse(appConfig.GetSection("LastName").GetSection("min").Value, CultureInfo.InvariantCulture);
            int maxValueOfLastName = int.Parse(appConfig.GetSection("LastName").GetSection("max").Value, CultureInfo.InvariantCulture);
            this.LastNameValidator = new LastNameValidator(minValueOfLastName, maxValueOfLastName);

            DateTime minValueOfDateOfBirth = DateTime.Parse(appConfig.GetSection("DateOfBirth").GetSection("from").Value, CultureInfo.InvariantCulture);
            DateTime maxValueOfDateOfBirth = DateTime.Parse(appConfig.GetSection("DateOfBirth").GetSection("to").Value, CultureInfo.InvariantCulture);
            this.DateOfBirthValidator = new DateOfBirthValidator(minValueOfDateOfBirth, maxValueOfDateOfBirth);

            this.SerieOfPassNumberValidator = new CustomSerieOfPassNumberValidator();
            this.PassNumberValidator = new CustomPassNumberValidator();
            this.BankAccountValidator = new CustomBankAccountValidator();

            this.validators.AddRange(new IRecordValidator[] { this.FirstNameValidator, this.LastNameValidator, this.DateOfBirthValidator, this.SerieOfPassNumberValidator, this.PassNumberValidator, this.BankAccountValidator });
            return new CompositeValidator(this.validators);
        }
    }
}