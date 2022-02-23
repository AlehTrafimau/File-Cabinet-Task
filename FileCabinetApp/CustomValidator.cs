using System.Text;
using FileCabinetApp.CustomValidators;
using FileCabinetApp.GeneralValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates the record according custom conditions.
    /// </summary>
    /// <seealso cref="IRecordValidator"/>
    internal class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(5, 30),
                new LastNameValidator(5, 30),
                new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now.AddYears(-10)),
                new CustomSerieOfPassNumberValidator(),
                new CustomPassNumberValidator(),
                new CustomBankAccountValidator(),
            })
        {
        }
    }
}
