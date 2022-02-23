using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Consinsts of a set of methods to work with handlers.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets the next handler for current command.
        /// </summary>
        /// <param name="nextHandler">The next handler.</param>
        public void SetNext(ICommandHandler nextHandler);

        /// <summary>
        /// Defines a handle for handling current request.
        /// </summary>
        /// <param name="handlingRequest">The request to hadnling.</param>
        public void Handle(AppCommandRequest handlingRequest);
    }
}
