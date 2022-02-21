using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Consists a set of handlers.
    /// </summary>
    public class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints notes statistics", "The 'stat' command prints notes' statistics." },
            new string[] { "create", "saves user's date and returns user's ID", "The 'create' command saves user's date and returns user's ID." },
            new string[] { "purge", "defragments records in file system", "The 'purge' command defragments records in file sustem" },
            new string[] { "list", "prints all records of this service", "The 'help' command prints all records of this service." },
            new string[] { "edit", "edits record in sevice according input ID", "The 'edit' command record note in sevice according input ID" },
            new string[] { "export", "exports records data in special format", "The 'export' command exports records data in special format" },
            new string[] { "import", "imports records data from file system", "The 'export' command imports records data from file system" },
        };

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
        };

        /// <summary>
        /// Defines a handle for handling current request.
        /// </summary>
        /// <param name="handlingRequest">The request to hadnling.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(handlingRequest.Command, StringComparison.Ordinal));
            if (index >= 0)
            {
                commands[index].Item2(handlingRequest.Parameters);
            }
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.Ordinal));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }

        private static void Export(string parameters)
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

            FileCabinetServiceSnapshot snapShot = Program.fileCabinetService.MakeSnapshot();
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

        private static void Edit(string parameters)
        {
            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                int requestedID = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (Program.fileCabinetService.GetStat().Item1 < requestedID)
                {
                    Console.WriteLine($"#{requestedID} record is not found");
                    return;
                }

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

                FileCabinetRecord editedRecord = new (0, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);

                Program.fileCabinetService.EditRecord(requestedID, editedRecord);
            }
            else
            {
                Console.WriteLine($"Parameters did not enter");
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.Item1} record(s).");
            Console.WriteLine($"{recordsCount.Item2} removed record(s).");
        }

        private static void Create(string parameters)
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
            int userId = Program.fileCabinetService.CreateRecord(newRecord);

            Console.WriteLine($"Record #{userId} is created.");
        }

        private static void Remove(string parameters)
        {
            int requestedIdRecord = 0;

            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                requestedIdRecord = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (Program.fileCabinetService.GetStat().Item1 < requestedIdRecord || requestedIdRecord < 1)
                {
                    Console.WriteLine($"#{requestedIdRecord} record is not found");
                    return;
                }
            }

            Program.fileCabinetService.RemoveRecord(requestedIdRecord);
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

        private static void Find(string parameters)
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
                    resultOfSearch = Program.fileCabinetService.FindByFirstName(parameterForSearch);
                    break;
                case "LASTNAME":
                    resultOfSearch = Program.fileCabinetService.FindByLastName(parameterForSearch);
                    break;
                case "DATEOFBIRTH":
                    resultOfSearch = Program.fileCabinetService.FindByDayOfBirth(parameterForSearch);
                    break;
                default:
                    Console.WriteLine($"Invalid command: {command}.");
                    return;
            }

            if (resultOfSearch.Count != 0)
            {
                RecordsOperation.PrintRecord(resultOfSearch.ToArray());
            }
            else
            {
                Console.WriteLine("Records are not found");
            }
        }

        private static void Purge(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFileSystemService)
            {
                Program.fileCabinetService.Purge();
            }
            else
            {
                Console.WriteLine("This command is available for file cabinet file system service only.");
            }
        }
    }
}
