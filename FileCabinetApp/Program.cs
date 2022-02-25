using System.Globalization;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CustomValidators;
using FileCabinetApp.DefaultValidators;

namespace FileCabinetApp
{
    /// <summary>
    /// The program for work with file cabinet service to create, storage, edit, find and display records about users.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Aleh Trafimau";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService();
        private static IRecordValidator recordValidator = new ValidatorBuilder().CreateDefault();

        private static bool isRunning = true;

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
                else
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    var commandhandler = CreateCommandHandlers();
                    commandhandler.Handle(new AppCommandRequest(command, parameters));
                }
            }
            while (isRunning);
        }

        private static void SetConsoleParameters(string[] args)
        {
            if ((args.Length == 1 && args[0].ToUpperInvariant() == "--VALIDATION-RULES=CUSTOM") || (args.Length == 2 && args[0].ToUpperInvariant() == "-V" && args[1].ToUpperInvariant() == "CUSTOM"))
            {
                recordValidator = new ValidatorBuilder().CreateCustom();

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

            if (args.Length == 1 && args[0].ToUpperInvariant() == "USE-STOPWATCH")
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }
        }

        private static void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records != null)
            {
                foreach (FileCabinetRecord currentRecord in records)
                {
                    Console.WriteLine($"#{currentRecord.Id}, {currentRecord.FirstName}, {currentRecord.LastName}, {currentRecord.DateOfBirth.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}," +
                        $" pass number: {currentRecord.SerieOfPassNumber} {currentRecord.PassNumber}, currentBankAccount: {currentRecord.BankAccount}$");
                }
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordPrinter = new DefaultRecordPrinter();

            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService, recordValidator);
            var editHandler = new EditCommandHandler(fileCabinetService, recordValidator);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, Print);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, Print);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler((bool run) => isRunning = run);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(removeHandler);
            removeHandler.SetNext(statHandler);
            statHandler.SetNext(findHandler);
            findHandler.SetNext(importHandler);
            importHandler.SetNext(exportHandler);
            exportHandler.SetNext(listHandler);
            listHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);

            return helpHandler;
        }
    }
}