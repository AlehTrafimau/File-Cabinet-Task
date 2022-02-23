using System.Text;
using FileCabinetApp.DefaultValidators;
using FileCabinetApp.GeneralValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validates the record according default conditions.
    /// </summary>
    /// <seealso cref="IRecordValidator"/>
    public class DefaultValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now),
                new DefaultSerieOfPassNumberValidator(),
                new DefaultPassNumberValidator(),
                new DefaultBankAccountValidator(),
            })
        {
        }
    }
}