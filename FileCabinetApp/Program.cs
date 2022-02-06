using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Aleh Trafimau";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

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
            new string[] { "create", "saves user's dates and returns user's ID", "The 'create' command saves user's dates and returns user's ID." },
            new string[] { "list", "prints all notes of this service", "The 'help' command prints all notes of this service." },
            new string[] { "edit", "edits note in sevice according input ID", "The 'edit' command edits note in sevice according input ID" },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
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
                    Console.WriteLine(Program.HintMessage);
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

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.Ordinal));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
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
            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^([1-9]{1}(\d*))|(0+[1-9]{1}\d*)$"))
            {
                int requestedID = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (Program.fileCabinetService.GetStat() < requestedID)
                {
                    Console.WriteLine($"#{requestedID} record is not found");
                    return;
                }

                string? firstNameOfUser = VerifyEntryData.FirstNameCheck();
                string? lastNamуOfUser = VerifyEntryData.LastNameCheck();
                DateTime dateOfBirth = VerifyEntryData.DateOfBirthCheck();
                char serieOfPassNumber = VerifyEntryData.SerieOfPassNumberCheck();
                short passNumber = VerifyEntryData.PassNumberCheck();
                decimal summOnBankAccount = VerifyEntryData.BankAccountCheck();

                if (firstNameOfUser != null && lastNamуOfUser != null)
                {
                    Program.fileCabinetService.EditRecord(requestedID, firstNameOfUser, lastNamуOfUser, dateOfBirth, serieOfPassNumber, passNumber, summOnBankAccount);
                }
            }
            else
            {
                Console.WriteLine($"Invalid parameter: {parameters}");
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string? firstNameOfUser = VerifyEntryData.FirstNameCheck();
            string? lastNamуOfUser = VerifyEntryData.LastNameCheck();
            DateTime dateOfBirth = VerifyEntryData.DateOfBirthCheck();
            char serieOfPassNumber = VerifyEntryData.SerieOfPassNumberCheck();
            short passNumber = VerifyEntryData.PassNumberCheck();
            decimal summOnBankAccount = VerifyEntryData.BankAccountCheck();

            int userId = 0;
            if (firstNameOfUser != null && lastNamуOfUser != null)
            {
                userId = Program.fileCabinetService.CreateRecord(firstNameOfUser, lastNamуOfUser, dateOfBirth, serieOfPassNumber, passNumber, summOnBankAccount);
            }

            Console.WriteLine($"Record #{userId} is created.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] notesInformation = Program.fileCabinetService.GetRecords();

            if (notesInformation.Length == 0)
            {
                Console.WriteLine("There are no records here");
            }

            foreach (FileCabinetRecord currentRecord in notesInformation)
            {
                Console.WriteLine($"#{currentRecord.Id}, {currentRecord.FirstName}, {currentRecord.LastName}, {currentRecord.DateOfBirth:yyyy-MMM-dd}," +
                    $" pass number: {currentRecord.SerieOfPassNumber} {currentRecord.PassNumber}, currentBankAccount: {currentRecord.CurrentBankAccount}$");
            }
        }

        private static void Find(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            string command = inputs[0].ToUpperInvariant();
            string parameterForSearch;
            string[] availableCommands = new string[] { "FIRSTNAME", "LASTNAME", "DATEOFBIRTH" };

            if (inputs.Length == 2)
            {
                parameterForSearch = inputs[1];

                if (Regex.IsMatch(parameterForSearch, @"^\W{1}\w+\W{1}$"))
                {
                    parameterForSearch = parameterForSearch.Trim(new char[] { '"', '+', '/', '\\', '*' });
                }
            }
            else
            {
                Console.WriteLine($"Invalid command: {command}. Enter parameter for search");
                return;
            }

            List<FileCabinetRecord> notesInformation = new List<FileCabinetRecord>();

            if (availableCommands.Contains(command))
            {
                switch (command)
                {
                    case "FIRSTNAME":
                        notesInformation.AddRange(Program.fileCabinetService.FindByFirstName(parameterForSearch));
                        break;
                    case "LASTNAME":
                        notesInformation.AddRange(Program.fileCabinetService.FindByLastName(parameterForSearch));
                        break;
                    default:
                        break;
                }
            }

            if (notesInformation.Count != 0)
            {
                foreach (FileCabinetRecord currentRecord in notesInformation)
                {
                    Console.WriteLine($"#{currentRecord.Id}, {currentRecord.FirstName}, {currentRecord.LastName}, {currentRecord.DateOfBirth:yyyy-MMM-dd}," +
                        $" pass number: {currentRecord.SerieOfPassNumber} {currentRecord.PassNumber}, currentBankAccount: {currentRecord.CurrentBankAccount}$");
                }
            }
            else
            {
                Console.WriteLine("Notes are not found");
            }
        }
    }
}