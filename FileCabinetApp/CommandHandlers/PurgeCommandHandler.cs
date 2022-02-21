using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Defrangents the file system storage.
    /// </summary>
    internal class PurgeCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "PURGE")
            {
                Purge(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private static void Purge(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFileSystemService)
            {
                Program.fileCabinetService.Purge();
            }
            else
            {
                Console.WriteLine("This command is available for file cabinet file system service only.");
            }
        }
    }
}
