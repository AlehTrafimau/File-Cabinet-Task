using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Sets user's command and parameters.
    /// </summary>
    public class AppCommandRequest
    {
        private string[] availableCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command for handlers.</param>
        /// <param name="parameters">Additional parameters for handlers of command.</param>
        /// <param name="availableCommands">The list of available commands this program.</param>
        public AppCommandRequest(string command, string parameters, string[] availableCommands)
        {
            this.Command = command.ToUpperInvariant();
            this.Parameters = parameters;
            this.availableCommands = availableCommands ?? Array.Empty<string>();
        }

        /// <summary>
        /// Gets a command for handlers.
        /// </summary>
        /// <value>
        /// A command for handlers.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets additional parameters for handlers of command.
        /// </summary>
        /// <value>
        /// Additional parameters for handlers of command.
        /// </value>
        public string Parameters { get; }

        /// <summary>
        /// Check input command.
        /// </summary>
        /// <returns>
        /// True, if input command is exists in the list of the available command, or false, otherwise.
        /// </returns>
        public bool CheckCommand()
        {
            if (!this.availableCommands.Contains(this.Command))
            {
                List<string> similarCommand = new ();
                foreach (var i in this.availableCommands)
                {
                    if (i.StartsWith(this.Command[0]))
                    {
                        similarCommand.Add(i);
                    }
                }

                if (similarCommand.Count != 0)
                {
                    Console.WriteLine($"{this.Command} is not a available command. See 'help'.\n\nThe most similar commands are:");
                    foreach (var i in similarCommand)
                    {
                        Console.WriteLine($"\t {i.ToLowerInvariant()}");
                    }
                }

                return false;
            }

            return true;
        }
    }
}
