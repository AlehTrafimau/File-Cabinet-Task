using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Program for autogenerating new records and performing operations with them to export csv, xml format, write to fily system..
    /// </summary>
    public static class FileCabinetGenerator
    {
        private static int startId;
        private static int numberOfRecords;
        private static string pathToFile = string.Empty;
        private static string exportFormat = string.Empty;

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            SetConsoleParameters(args);
            FileCabinetRecordGenerator recordsGenerator = new ();
            ReadOnlyCollection<FileCabinetRecord> newRecords = recordsGenerator.GetRandomRecords(numberOfRecords, startId);
            if (!File.Exists(pathToFile))
            {
                Console.WriteLine($"Export failed: file {pathToFile} is not exist.");
                return;
            }

            if (exportFormat.ToUpperInvariant() != pathToFile[^3..].ToUpperInvariant())
            {
                Console.WriteLine($"Export failed: format of export: {exportFormat} in not the same of file format: {pathToFile}.");
                return;
            }

            using StreamWriter streamWriter = new (pathToFile, false, System.Text.Encoding.Default);
            switch (exportFormat.ToUpperInvariant())
            {
                case "CSV":
                    streamWriter.WriteLine("Id, FirstName, LastName, DateOfBirth, SerieOfPassNumber, PassNumber, BankAccount");
                    FileCabinetRecordCsvWriter csvWriter = new (streamWriter);
                    csvWriter.Write(newRecords.ToArray());
                    break;
                case "XML":
                    FileCabinetRecordXmlSerializer xmlWriter = new (streamWriter);
                    xmlWriter.Write(newRecords.ToArray());
                    break;
                default:
                    return;
            }

            Console.WriteLine($"{numberOfRecords} records were written to {pathToFile}");
        }

        private static void SetConsoleParameters(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Enter parameters for record generate");
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (Regex.IsMatch(args[i].ToUpperInvariant(), @"^--OUTPUT-TYPE=\S*$"))
                {
                    string[] exportTypeParameters = args[i].Split('=', 2);
                    exportFormat = exportTypeParameters[1].ToUpperInvariant();
                    continue;
                }

                if (Regex.IsMatch(args[i].ToUpperInvariant(), @"^--OUTPUT=\S*$"))
                {
                    string[] outputParameters = args[i].Split('=', 2);
                    pathToFile = outputParameters[1];
                    continue;
                }

                if (Regex.IsMatch(args[i].ToUpperInvariant(), @"^--RECORDS-AMOUNT=\d+$"))
                {
                    string[] recordsAmountParameters = args[i].Split('=', 2);
                    bool isNumber = int.TryParse(recordsAmountParameters[1], out int number);
                    if (isNumber)
                    {
                        numberOfRecords = number;
                    }

                    continue;
                }

                if (Regex.IsMatch(args[i].ToUpperInvariant(), @"^--START-ID=\d+$"))
                {
                    string[] startIdParameters = args[i].Split('=', 2);
                    bool isIdNumber = int.TryParse(startIdParameters[1], out int startIdNumber);
                    if (isIdNumber)
                    {
                        startId = startIdNumber;
                    }

                    continue;
                }

                switch (args[i].ToUpperInvariant())
                {
                    case "-T":
                        if (args.Length > i + 1)
                        {
                            exportFormat = args[i + 1].ToUpperInvariant();
                            i++;
                        }

                        break;
                    case "-O":
                        if (args.Length > i + 1)
                        {
                            pathToFile = args[i + 1];
                            i++;
                        }

                        break;
                    case "-A":
                        if (args.Length > i + 1)
                        {
                            bool isAmountNumber = int.TryParse(args[i + 1], out int amountNumber);
                            if (isAmountNumber)
                            {
                                numberOfRecords = amountNumber;
                            }

                            i++;
                        }

                        break;
                    case "-I":
                        if (args.Length > i + 1)
                        {
                            bool isIdNumber = int.TryParse(args[i + 1], out int startIdNumber);
                            if (isIdNumber)
                            {
                                startId = startIdNumber;
                            }

                            i++;
                        }

                        break;
                }
            }
        }
    }
}