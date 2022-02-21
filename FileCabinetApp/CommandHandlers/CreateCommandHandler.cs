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
            Console.Write("First name: ");
            var firstName = ConsoleExtension.ReadInput(StringConverter.StringConvert, Program.recordValidator.CheckName);

            Console.Write("Last name: ");
            var lastName = ConsoleExtension.ReadInput(StringConverter.StringConvert, Program.recordValidator.CheckName);

            Console.Write("Birth date: ");
            var dateOfBirth = ConsoleExtension.ReadInput(StringConverter.DateTimeConvert, Program.recordValidator.CheckBirthDate);

            Console.Write("Serie of pass number: ");
            var serieOfPassNumber = ConsoleExtension.ReadInput(StringConverter.CharConvert, Program.recordValidator.CheckSerieOfPassNumber);

            Console.Write("Pass number: ");
            var passNumber = ConsoleExtension.ReadInput(StringConverter.ShortConvert, Program.recordValidator.CheckPassNumber);

            Console.Write("Bank account: ");
            var bankAccount = ConsoleExtension.ReadInput(StringConverter.DecimalConvert, Program.recordValidator.CheckBankAccount);

            FileCabinetRecord newRecord = new (0, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);
            int userId = this.fileCabinetService.CreateRecord(newRecord);

            Console.WriteLine($"Record #{userId} is created.");
        }
    }
}
