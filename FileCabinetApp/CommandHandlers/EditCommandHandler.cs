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
            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                int requestedID = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (this.fileCabinetService.GetStat().Item1 < requestedID)
                {
                    Console.WriteLine($"#{requestedID} record is not found");
                    return;
                }

                this.recordValidator.ValidateParameters(out string firstName, out string lastName, out DateTime birthDate, out char serieOfPassNumber, out short passNumber, out decimal bankAccount);

                FileCabinetRecord editedRecord = new (0, firstName, lastName, birthDate, serieOfPassNumber, passNumber, bankAccount);

                this.fileCabinetService.EditRecord(requestedID, editedRecord);
            }
            else
            {
                Console.WriteLine($"Parameters did not enter");
            }
        }
    }
}
