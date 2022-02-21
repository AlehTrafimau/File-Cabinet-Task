using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Imports records from file system.
    /// </summary>
    internal class ImportCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "IMPORT")
            {
                Import(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private static void Import(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            string importFormat = inputs[0];
            string pathToFile = inputs[1];

            if ((importFormat.ToUpperInvariant() != "CSV" || importFormat.ToUpperInvariant() != "XML") && !Regex.IsMatch(pathToFile.ToUpperInvariant(), @"^\S*(.CSV|.XML)$"))
            {
                Console.WriteLine($"Invalid command: \"{importFormat}\" or file format: \"{pathToFile}\"");
                return;
            }

            if (!File.Exists(pathToFile))
            {
                Console.WriteLine($"Import error: file {pathToFile} is not exist.");
            }

            using StreamReader importStream = new (pathToFile);
            FileCabinetServiceSnapshot lastestSnaphot = new (Array.Empty<FileCabinetRecord>());
            if (importFormat.ToUpperInvariant() == "CSV")
            {
                lastestSnaphot.ReadFromCsv(importStream);
            }
            else
            {
                lastestSnaphot.ReadFromXml(importStream);
            }

            Program.fileCabinetService.Restore(lastestSnaphot);
            Console.WriteLine($"{lastestSnaphot.Records.Count} records were imported from {pathToFile}");
        }
    }
}
