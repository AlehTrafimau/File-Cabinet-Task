using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Prints all available commands this program.
    /// </summary>
    internal class HelpCommandHandler : CommandHandlerBase
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
            new string[] { "export", "exports records data in special format", "The 'export' command exports records data in special format" },
            new string[] { "import", "imports records data from file system", "The 'export' command imports records data from file system" },
            new string[] { "find", "finds all records in the storage by special parameters", "The 'find' command Finds all records in the storage by special parameters" },
            new string[] { "insert", "inserts the new record to the storage.", "The 'insert' inserts the new record to the storage." },
            new string[] { "delete", "deletes records from the storage which match input condition.", "The 'delete' deletes records from the storage which match input condition." },
            new string[] { "update", "updates record from current storage by input parameters.", "The 'update' updates record from current storage by input parameters." },
            new string[] { "select", "selects records by input parameters", "The 'select' selects records by input parameters." },
        };

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "HELP")
            {
                PrintHelp(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
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
    }
}
