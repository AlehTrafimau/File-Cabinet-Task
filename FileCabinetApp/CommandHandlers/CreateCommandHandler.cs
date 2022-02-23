using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Saves user's date and returns user's ID.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        /// <param name="recordValidator">Sets rules of validation.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService, IRecordValidator recordValidator)
            : base(fileCabinetService)
        {
            this.recordValidator = recordValidator;
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "CREATE")
            {
                this.Create(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Create(string parameters)
        {
            FileCabinetRecord newRecord;

            while (true)
            {
                Console.Write("First name: ");
                var firstName = ConsoleExtension.ReadInput(StringConverter.StringConvert);

                Console.Write("Last name: ");
                var lastName = ConsoleExtension.ReadInput(StringConverter.StringConvert);

                Console.Write("Birth date: ");
                var dateOfBirth = ConsoleExtension.ReadInput(StringConverter.DateTimeConvert);

                Console.Write("Serie of pass number: ");
                var serieOfPassNumber = ConsoleExtension.ReadInput(StringConverter.CharConvert);

                Console.Write("Pass number: ");
                var passNumber = ConsoleExtension.ReadInput(StringConverter.ShortConvert);

                Console.Write("Bank account: ");
                var bankAccount = ConsoleExtension.ReadInput(StringConverter.DecimalConvert);

                FileCabinetRecord recordForValidate = new (0, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);
                Tuple<bool, string> validator = this.recordValidator.ValidateParameters(recordForValidate);
                if (validator.Item1 == false)
                {
                    string[] validationErrors = validator.Item2.Split(' ');
                    Console.WriteLine($"\tValidation failed:");
                    foreach (var i in validationErrors)
                    {
                        Console.WriteLine(i);
                    }

                    Console.WriteLine($"\tPlease, correct your input.");
                }
                else
                {
                    newRecord = recordForValidate;
                    break;
                }
            }

            int userId = this.fileCabinetService.CreateRecord(newRecord);
            Console.WriteLine($"Record #{userId} is created.");
        }
    }
}
