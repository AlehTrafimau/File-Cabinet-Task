using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Deletes record from current storage by input conditions.
    /// </summary>
    internal class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The instance of file cabinet service instance.</param>
        public DeleteCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public override void Handle(AppCommandRequest handlingRequest)
        {
            if (handlingRequest.Command.ToUpperInvariant() == "DELETE")
            {
                this.Delete(handlingRequest.Parameters);
                return;
            }
            else if (this.NextHandler != null)
            {
                this.NextHandler.Handle(handlingRequest);
            }
        }

        private void Delete(string parameters)
        {
            string[] commands = parameters.Split(new char[] { ' ', '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            if (commands.Length < 3 || commands[0].ToUpperInvariant() != "WHERE")
            {
                Console.WriteLine($"Invalid parameters {parameters}");
                return;
            }

            string fieldIndicatorForDelete = commands[1].ToUpperInvariant();
            string valueOfField = commands[2];
            this.fileCabinetService.Delete(fieldIndicatorForDelete, valueOfField);
        }
    }
}
