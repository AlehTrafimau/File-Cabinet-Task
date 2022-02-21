using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Display all records in the storage.
    /// </summary>
    internal class ListCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "LIST")
            {
                List(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = Program.fileCabinetService.GetRecords();

            if (records.Count == 0)
            {
                Console.WriteLine("There are no records here");
            }

            RecordsOperation.PrintRecord(records);
        }
    }
}
