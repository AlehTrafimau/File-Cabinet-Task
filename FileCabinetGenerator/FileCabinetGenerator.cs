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

            bool retriveExistsFile = false;
            if (File.Exists(pathToFile))
            {
                Console.WriteLine($"File is exist - rewrite {pathToFile}? (Yes/No)");
                string? retrivePermission = Console.ReadLine();
                if (retrivePermission != null && retrivePermission.ToUpperInvariant() == "NO")
                {
                    retriveExistsFile = true;
                }
            }
            else
            {
                Console.WriteLine($"Export failed: can't open file {pathToFile}");
            }

            using StreamWriter streamWriter = new (pathToFile, retriveExistsFile, System.Text.Encoding.Default);
            switch (exportFormat.ToUpperInvariant())
            {
                case "CSV":
                    if (retriveExistsFile == false)
                    {
                        streamWriter.WriteLine("Id, FirstName, LastName, DateOfBirth, SerieOfPassNumber, PassNumber, BankAccount");
                    }

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

            if (args.Length == 4)
            {
                if (Regex.IsMatch(args[0].ToLowerInvariant(), @"^--output-type=[c|x]{1}[s|m]{1}[v|l]{1}$"))
                {
                    exportFormat = args[0][^3..];
                }

                if (Regex.IsMatch(args[1].ToLowerInvariant(), @"^--output=\S*$"))
                {
                    pathToFile = args[1][9..];
                }

                if (Regex.IsMatch(args[2].ToLowerInvariant(), @"^--records-amount=\d+$"))
                {
                    bool isNumber = int.TryParse(args[2][17..], out int number);
                    if (isNumber)
                    {
                        numberOfRecords = number;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid parameter for \"--records-amount\"  {args[2][18..]}");
                    }
                }

                if (Regex.IsMatch(args[3].ToLowerInvariant(), @"^--start-id=\d+$"))
                {
                    bool isNumber = int.TryParse(args[3][11..], out int number);
                    if (isNumber)
                    {
                        startId = number;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid parameter for \"--start-id\"  {args[3][12..]}");
                    }
                }
            }
            else if (args.Length == 8)
            {
                if (args[0].ToLowerInvariant() == "-t")
                {
                    exportFormat = args[1];
                }

                if (args[2].ToLowerInvariant() == "-o")
                {
                    pathToFile = args[3];
                }

                if (args[4].ToLowerInvariant() == "-a")
                {
                    bool isNumber = int.TryParse(args[5], out int number);
                    if (isNumber)
                    {
                        numberOfRecords = number;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid parameter for \"-a\"  {args[5]}");
                    }
                }

                if (args[6].ToLowerInvariant() == "-i")
                {
                    bool isNumber = int.TryParse(args[7], out int number);
                    if (isNumber)
                    {
                        startId = number;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid parameter for \"--start-id\"  {args[7]}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid parameters");
            }
        }
    }
}