using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// The program for work with file cabinet service to create, storage, edit, find and display records about users.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Aleh Trafimau";
        private const string DefaultValidationsMessages = "Using default validation rules.";
        private const string CustomValidationsMessages = "Using custom validation rules.";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetDefaultService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints notes statistics", "The 'stat' command prints notes' statistics." },
            new string[] { "create", "saves user's date and returns user's ID", "The 'create' command saves user's date and returns user's ID." },
            new string[] { "list", "prints all records of this service", "The 'help' command prints all records of this service." },
            new string[] { "edit", "edits record in sevice according input ID", "The 'edit' command record note in sevice according input ID" },
        };

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            SetFileCabinetServiceMode(args);
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.Ordinal));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void SetFileCabinetServiceMode(string[] args)
        {
            if (args.Length == 1 && args[0].ToUpperInvariant() == "--VALIDATION-RULES=CUSTOM")
            {
                fileCabinetService = new FileCabinetCustomService();
                Console.WriteLine(CustomValidationsMessages);
            }
            else if (args.Length == 2 && args[0].ToUpperInvariant() == "-V" && args[1].ToUpperInvariant() == "CUSTOM")
            {
                fileCabinetService = new FileCabinetCustomService();
                Console.WriteLine(CustomValidationsMessages);
            }
            else
            {
                Console.WriteLine(DefaultValidationsMessages);
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
            isRunning = false;
        }

        private static void Edit(string parameters)
        {
            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                int requestedID = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (Program.fileCabinetService.GetStat() < requestedID)
                {
                    Console.WriteLine($"#{requestedID} record is not found");
                    return;
                }

                fileCabinetService.EditRecord(requestedID);
            }
            else
            {
                Console.WriteLine($"Parameters did not enter");
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            int userId = fileCabinetService.CreateRecord();
            Console.WriteLine($"Record #{userId} is created.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] cabinetRecords = fileCabinetService.GetRecords();

            if (cabinetRecords.Length == 0)
            {
                Console.WriteLine("There are no records here");
            }

            foreach (FileCabinetRecord record in cabinetRecords)
            {
                Console.WriteLine(@$"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, pass number: {record.SerieOfPassNumber} {record.PassNumber}, currentBankAccount: {record.BankAccount}$");
            }
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

            List<FileCabinetRecord> cabinetRecords = new ();

            switch (command)
            {
                case "FIRSTNAME":
                    cabinetRecords.AddRange(fileCabinetService.FindByFirstName(parameterForSearch));
                    break;
                case "LASTNAME":
                    cabinetRecords.AddRange(fileCabinetService.FindByLastName(parameterForSearch));
                    break;
                case "DATEOFBIRTH":
                    cabinetRecords.AddRange(fileCabinetService.FindByDayOfBirth(parameterForSearch));
                    break;
                default:
                    Console.WriteLine($"Invalid command: {command}.");
                    return;
            }

            if (cabinetRecords.Count != 0)
            {
                foreach (FileCabinetRecord currentRecord in cabinetRecords)
                {
                    Console.WriteLine($"#{currentRecord.Id}, {currentRecord.FirstName}, {currentRecord.LastName}, {currentRecord.DateOfBirth:yyyy-MMM-dd}," +
                        $" pass number: {currentRecord.SerieOfPassNumber} {currentRecord.PassNumber}, currentBankAccount: {currentRecord.BankAccount}$");
                }
            }
            else
            {
                Console.WriteLine("Notes are not found");
            }
        }
    }
}