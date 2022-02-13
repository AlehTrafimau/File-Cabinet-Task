using System.Collections.ObjectModel;
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
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService();
        private static IRecordValidator recordValidator = new DefaultValidator();

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
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints notes statistics", "The 'stat' command prints notes' statistics." },
            new string[] { "create", "saves user's date and returns user's ID", "The 'create' command saves user's date and returns user's ID." },
            new string[] { "list", "prints all records of this service", "The 'help' command prints all records of this service." },
            new string[] { "edit", "edits record in sevice according input ID", "The 'edit' command record note in sevice according input ID" },
            new string[] { "export", "exports records data in special format", "The 'export' command exports records data in special format" },
        };

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            SetConsoleParameters(args);
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

        private static void SetConsoleParameters(string[] args)
        {
            if ((args.Length == 1 && args[0].ToUpperInvariant() == "--VALIDATION-RULES=CUSTOM") || (args.Length == 2 && args[0].ToUpperInvariant() == "-V" && args[1].ToUpperInvariant() == "CUSTOM"))
            {
                recordValidator = new CustomValidator();
                Console.WriteLine("Using custom validation rules.");
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
            }

            if ((args.Length == 1 && args[0].ToUpperInvariant() == "--STORAGE=FILE") || (args.Length == 2 && args[0].ToUpperInvariant() == "-S" && args[1].ToUpperInvariant() == "FILE"))
            {
                Console.WriteLine("Data storage will take place in the file system.");
                FileStream fileStream = new ("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFileSystemService(fileStream);
            }
            else
            {
                Console.WriteLine("Data storage will take place in the memory of program.");
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
            if (File.Exists(pathToFile) && exportFormat.ToUpperInvariant() != "XML")
            {
                Console.WriteLine($"File is exist - rewrite {pathToFile}? (Yes/No)");
                string? retrivePermission = Console.ReadLine();
                if (retrivePermission != null && retrivePermission.ToUpperInvariant() == "NO")
                {
                    retriveExistsFile = true;
                }
            }
            else if (!Regex.IsMatch(pathToFile.ToUpperInvariant(), @"^[A-Z]*(.CSV|.XML)$"))
            {
                Console.WriteLine($"Export failed: can't open file {pathToFile}.");
                return;
            }

            FileCabinetServiceSnapshot snapShot = fileCabinetService.MakeSnapshot();
            using (StreamWriter streamWriter = new (pathToFile, retriveExistsFile, System.Text.Encoding.Default))
            {
                switch (exportFormat.ToUpperInvariant())
                {
                    case "CSV":
                        if (retriveExistsFile == false)
                        {
                            streamWriter.WriteLine("FirstName, LastName, DateOfBirth, SerieOfPassNumber, PassNumber, BankAccount");
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

                Console.Write("First name: ");
                var firstName = ReadInput(StringConverter.StringConvert, recordValidator.CheckName);

                Console.Write("Last name: ");
                var lastName = ReadInput(StringConverter.StringConvert, recordValidator.CheckName);

                Console.Write("Birth date: ");
                var dateOfBirth = ReadInput(StringConverter.DateTimeConvert, recordValidator.CheckBirthDate);

                Console.Write("Serie of pass number: ");
                var serieOfPassNumber = ReadInput(StringConverter.CharConvert, recordValidator.CheckSerieOfPassNumber);

                Console.Write("Pass number: ");
                var passNumber = ReadInput(StringConverter.ShortConvert, recordValidator.CheckPassNumber);

                Console.Write("Bank account: ");
                var bankAccount = ReadInput(StringConverter.DecimalConvert, recordValidator.CheckBankAccount);

                FileCabinetRecord editedRecord = new (0, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);

                fileCabinetService.EditRecord(requestedID, editedRecord);
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
            Console.Write("First name: ");
            var firstName = ReadInput(StringConverter.StringConvert, recordValidator.CheckName);

            Console.Write("Last name: ");
            var lastName = ReadInput(StringConverter.StringConvert, recordValidator.CheckName);

            Console.Write("Birth date: ");
            var dateOfBirth = ReadInput(StringConverter.DateTimeConvert, recordValidator.CheckBirthDate);

            Console.Write("Serie of pass number: ");
            var serieOfPassNumber = ReadInput(StringConverter.CharConvert, recordValidator.CheckSerieOfPassNumber);

            Console.Write("Pass number: ");
            var passNumber = ReadInput(StringConverter.ShortConvert, recordValidator.CheckPassNumber);

            Console.Write("Bank account: ");
            var bankAccount = ReadInput(StringConverter.DecimalConvert, recordValidator.CheckBankAccount);

            FileCabinetRecord newRecord = new (0, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);
            int userId = fileCabinetService.CreateRecord(newRecord);

            Console.WriteLine($"Record #{userId} is created.");
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                string? input = Console.ReadLine();
                Tuple<bool, string, T> conversionResult;

                if (input != null)
                {
                    conversionResult = converter(input);
                }
                else
                {
                    continue;
                }

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> cabinetRecords = fileCabinetService.GetRecords();

            if (cabinetRecords.Count == 0)
            {
                Console.WriteLine("There are no records here");
            }

            foreach (FileCabinetRecord record in cabinetRecords)
            {
                Console.WriteLine(@$"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}, pass number:{record.SerieOfPassNumber} {record.PassNumber}, currentBankAccount: {record.BankAccount}$");
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