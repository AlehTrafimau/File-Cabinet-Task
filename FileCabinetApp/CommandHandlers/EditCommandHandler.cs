using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edits record note in sevice according input ID.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        /// <param name="recordValidator">Sets rules of validation.</param>
        public EditCommandHandler(IFileCabinetService fileCabinetService, IRecordValidator recordValidator)
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
            if (handlingRequest.Command.ToUpperInvariant() == "EDIT")
            {
                this.Edit(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Edit(string parameters)
        {
            int requestedID = 0;
            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                requestedID = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (this.fileCabinetService.GetStat().Item1 < requestedID)
                {
                    Console.WriteLine($"#{requestedID} record is not found");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"Parameters did not enter");
                return;
            }

            FileCabinetRecord editedRecord;

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
                    Console.WriteLine($"\tValidation failed:");
                    Console.Write(validator.Item2);
                    Console.WriteLine($"\tPlease, correct your input.");
                }
                else
                {
                    editedRecord = recordForValidate;
                    break;
                }
            }

            this.fileCabinetService.EditRecord(requestedID, editedRecord);
        }
    }
}
