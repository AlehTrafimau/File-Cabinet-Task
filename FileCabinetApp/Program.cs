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

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
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
            FileCabinetApp.FileCabinetService fileService = new FileCabinetService();
            var recordsCount = fileService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create()
        {
            string? firstNameOfUser = null;
            while (firstNameOfUser == null)
            {
                Console.WriteLine("First name:");
                firstNameOfUser = Console.ReadLine();
            }

            string? lastNameOfUser = null;
            while (lastNameOfUser == null)
            {
                Console.WriteLine("Last name:");
                lastNameOfUser = Console.ReadLine();
            }

            DateTime dateOfBirth = default;
            while (dateOfBirth == default)
            {
                Console.WriteLine("Date of birth: ");
                string? dateFromConsole = Console.ReadLine();
                if (dateFromConsole != null)
                {
                    dateOfBirth = ConvertToDateTime(dateFromConsole);
                }
            }

            FileCabinetService currentUser = new FileCabinetService();
            int userId = currentUser.CreateRecord(firstNameOfUser, lastNameOfUser, dateOfBirth);

            Console.WriteLine($"Record #{userId} is created.");
        }

        private static DateTime ConvertToDateTime(string source)
        {
            if (source == null)
            {
                return default(DateTime);
            }

            Regex customBirthDateFormat = new Regex(@"[0-1]?[0-9]{1}\D\s?[0-3]?[0-9]{1}\D\s?[1-2]{1}[0-9]{3}$");

            if (!customBirthDateFormat.IsMatch(source))
            {
                return default(DateTime);
            }

            char[] separatorsForDateOfBirth = new char[] { '/', '\\', '.', ',', ' ', '*', '-', '+' };
            source = string.Concat(source.Split(separatorsForDateOfBirth));

            DateTime birthDateOfUser = default(DateTime);
            int dateOfBirth = int.Parse(source, CultureInfo.CurrentCulture);

            int yearOfBirth = dateOfBirth % 10000;
            birthDateOfUser.AddYears(yearOfBirth);
            dateOfBirth /= 10000;

            int dayOfBirth = dateOfBirth % 100;
            birthDateOfUser.AddDays(dayOfBirth);

            int monthOfBirth = dateOfBirth / 100;
            birthDateOfUser.AddMonths(monthOfBirth);

            return birthDateOfUser;
        }
    }
}
