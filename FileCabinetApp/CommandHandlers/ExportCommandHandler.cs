using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Exports all records in the storage to file system file.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "EXPORT")
            {
                this.Export(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Export(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            string exportFormat = inputs[0];
            string pathToFile = inputs[1];

            if ((exportFormat.ToUpperInvariant() != "CSV" && exportFormat.ToUpperInvariant() != "XML") || !Regex.IsMatch(pathToFile.ToUpperInvariant(), @"^\S*(.CSV|.XML)$"))
            {
                Console.WriteLine($"Invalid command: \"{exportFormat}\" or file format: \"{pathToFile}\"");
                return;
            }

            bool retriveExistsFile = false;
            if (File.Exists(pathToFile) && exportFormat.ToUpperInvariant() == "CSV")
            {
                Console.WriteLine($"File is exist - rewrite {pathToFile}? (Yes/No)");
                string? retrivePermission = Console.ReadLine();
                if (retrivePermission != null && retrivePermission.ToUpperInvariant() == "NO")
                {
                    retriveExistsFile = true;
                }
            }
            else if (File.Exists(pathToFile) && exportFormat.ToUpperInvariant() == "XML")
            {
                retriveExistsFile = false;
            }
            else if (!Regex.IsMatch(pathToFile.ToUpperInvariant(), @"^[A-Z]*(.CSV|.XML)$"))
            {
                Console.WriteLine($"Export failed: can't open file {pathToFile}.");
                return;
            }

            FileCabinetServiceSnapshot snapShot = this.fileCabinetService.MakeSnapshot();
            using StreamWriter streamWriter = new (pathToFile, retriveExistsFile, System.Text.Encoding.Default);
            switch (exportFormat.ToUpperInvariant())
            {
                case "CSV":
                    if (retriveExistsFile == false)
                    {
                        streamWriter.WriteLine("Id, FirstName, LastName, DateOfBirth, SerieOfPassNumber, PassNumber, BankAccount");
                    }

                    snapShot.SaveToCsv(streamWriter);
                    break;
                case "XML":
                    snapShot.SaveToXml(streamWriter);
                    break;
            }

            Console.WriteLine($"All records are exported to file {pathToFile}");
        }
    }
}
