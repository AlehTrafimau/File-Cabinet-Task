using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Consists of a set of paremeters for record operation.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command for handlers.</param>
        /// <param name="parameters">Additional parameters for handlers of command.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
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
    }
}
