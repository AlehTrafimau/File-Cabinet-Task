using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Removes the specified record from the storage.
    /// </summary>
    public class RemoveCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "REMOVE")
            {
                this.Remove(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Remove(string parameters)
        {
            int requestedIdRecord = 0;

            if (parameters != string.Empty && Regex.IsMatch(parameters, @"^(0*[1-9]{1}\d*)$"))
            {
                requestedIdRecord = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (this.fileCabinetService.GetStat().Item1 < requestedIdRecord || requestedIdRecord < 1)
                {
                    Console.WriteLine($"#{requestedIdRecord} record is not found");
                    return;
                }
            }

            this.fileCabinetService.RemoveRecord(requestedIdRecord);
        }
    }
}
