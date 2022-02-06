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
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints notes statistics", "The 'stat' command prints notes' statistics." },
            new string[] { "create", "saves user's dates and returns user's ID", "The 'create' command saves user's dates and returns user's ID." },
            new string[] { "list", "prints all notes of this service", "The 'help' command prints all notes of this service." },
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

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string? firstNameOfUser = null;
            while (firstNameOfUser == null)
            {
                Console.Write("First name: ");
                firstNameOfUser = Console.ReadLine();
            }

            string? lastNameOfUser = null;
            while (lastNameOfUser == null)
            {
                Console.Write("Last name: ");
                lastNameOfUser = Console.ReadLine();
            }

            DateTime dateOfBirth = default;
            while (dateOfBirth == default)
            {
                Console.Write("Date of birth: ");
                string? dateFromConsole = Console.ReadLine();
                if (dateFromConsole != null)
                {
                    dateOfBirth = ConvertToDateTime(dateFromConsole);
                }
            }

            char serieOfPassNumber = default;
            while (serieOfPassNumber == default)
            {
                Console.Write("Serie of your pass number: ");
                string? source = Console.ReadLine();
                Regex serieOfPassNumberFormat = new Regex(@"^[A-Z]{1}|[a-z]{1}$");
                if (source != null && serieOfPassNumberFormat.IsMatch(source))
                {
                    serieOfPassNumber = char.Parse(source);
                }
                else
                {
                    Console.WriteLine("Enter valid serie of number (1 letter)");
                }
            }

            short passNumber = default;
            while (passNumber == default)
            {
                Console.Write("Your pass number: ");
                string? source = Console.ReadLine();
                Regex passNumberFormat = new Regex(@"^(\d{4}|\d{3}|\d{2}|\d{1})$");
                if (source != null && passNumberFormat.IsMatch(source))
                {
                    passNumber = short.Parse(source, CultureInfo.InvariantCulture);
                }
                else
                {
                    Console.WriteLine("Enter valid pass number (4 digits)");
                }
            }

            decimal bankAccount = default;
            while (bankAccount == default)
            {
                Console.Write("Your current bank account ($): ");
                string? source = Console.ReadLine();
                Regex bankAccountFormat = new Regex(@"\d+(\.?\d+)?$");
                if (source != null && bankAccountFormat.IsMatch(source))
                {
                    bool isConvertedToDecimal = decimal.TryParse(source, out decimal result);
                    if (isConvertedToDecimal)
                    {
                        bankAccount = result;
                    }
                }
            }

            int userId = Program.fileCabinetService.CreateRecord(firstNameOfUser, lastNameOfUser, dateOfBirth, serieOfPassNumber, passNumber, bankAccount);

            Console.WriteLine($"Record #{userId} is created.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] notesInformation = Program.fileCabinetService.GetRecords();

            foreach (FileCabinetRecord i in notesInformation)
            {
                Console.WriteLine($"#{i.Id}, {i.FirstName}, {i.LastName}, {i.DateOfBirth:yyyy-MMM-dd}, pass number: {i.SerieOfPassNumber} {i.PassNumber}, currentBankAccount {i.CurrentBankAccount} $");
            }
        }

        private static DateTime ConvertToDateTime(string source)
        {
            if (source == null)
            {
                return default(DateTime);
            }

            Regex customBirthDateFormat = new Regex(@"^((0{1}|^)[1-9]{1}|1{1}[0-2]{1})\D\s?((0{1}[1-9]{1})|([1-9]{1})|([1-2]{1}[0-9]{1})|(3{1}[0-1]{1}))\D\s?(([0-1]{1}[0-9]{3})|(2{1}0{1}[0-1]{1}[0-9]{1})|(2{1}0{1}2{1}[0-2]{1}))$");

            if (!customBirthDateFormat.IsMatch(source))
            {
                Console.WriteLine("Invalid date of birth. Date format: month/ day/ year.");
                return default(DateTime);
            }

            char[] separatorsForDateOfBirth = new char[] { '/', '\\', '.', ',', ' ', '*', '-', '+' };
            string[] sourceSplit = source.Split(separatorsForDateOfBirth);

            int yearOfBirth = int.Parse(sourceSplit[2], CultureInfo.InvariantCulture);
            int dayOfBirth = int.Parse(sourceSplit[1], CultureInfo.InvariantCulture);
            int monthOfBirth = int.Parse(sourceSplit[0], CultureInfo.InvariantCulture);
            DateTime birthDateOfUser = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);

            return birthDateOfUser;
        }
    }
}
