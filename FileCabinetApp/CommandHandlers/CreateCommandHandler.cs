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
            this.recordValidator.ValidateParameters(out string firstName, out string lastName, out DateTime birthDate, out char serieOfPassNumber, out short passNumber, out decimal bankAccount);
            FileCabinetRecord newRecord = new (0, firstName, lastName, birthDate, serieOfPassNumber, passNumber, bankAccount);
            int userId = this.fileCabinetService.CreateRecord(newRecord);

            Console.WriteLine($"Record #{userId} is created.");
        }
    }
}
