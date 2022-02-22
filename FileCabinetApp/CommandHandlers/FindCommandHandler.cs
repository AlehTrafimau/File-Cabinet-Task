using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Finds all records in the storage by special parameters.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="printer">The handler of records to output to console.</param>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "FIND")
            {
                this.Find(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Find(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            string command = inputs[0].ToUpperInvariant();
            string parameterForSearch;

            if (inputs.Length == 2)
            {
                parameterForSearch = inputs[1];

                if (Regex.IsMatch(parameterForSearch, @"^\W{1}\S+\W{1}$"))
                {
                    parameterForSearch = parameterForSearch.Trim(new char[] { '"', '+', '/', '\\', '*' });
                }
            }
            else
            {
                Console.WriteLine($"Enter parameter for search");
                return;
            }

            ReadOnlyCollection<FileCabinetRecord> resultOfSearch;

            switch (command)
            {
                case "FIRSTNAME":
                    resultOfSearch = this.fileCabinetService.FindByFirstName(parameterForSearch);
                    break;
                case "LASTNAME":
                    resultOfSearch = this.fileCabinetService.FindByLastName(parameterForSearch);
                    break;
                case "DATEOFBIRTH":
                    resultOfSearch = this.fileCabinetService.FindByDayOfBirth(parameterForSearch);
                    break;
                default:
                    Console.WriteLine($"Invalid command: {command}.");
                    return;
            }

            if (resultOfSearch.Count != 0)
            {
                this.printer.Print(resultOfSearch.ToArray());
            }
            else
            {
                Console.WriteLine("Records are not found");
            }
        }
    }
}
