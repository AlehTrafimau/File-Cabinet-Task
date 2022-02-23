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
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Gets the next handler of handling request.
        /// </summary>
        /// <value>
        /// The next handler of handling request.
        /// </value>
        protected ICommandHandler? NextHandler
        {
            get { return this.nextHandler; }
        }

        /// <summary>
        /// Sets the next handler for current command.
        /// </summary>
        /// <param name="nextHandler">The next handler.</param>
        public void SetNext(ICommandHandler nextHandler)
        {
            this.nextHandler = nextHandler;
        }

        /// <summary>
        /// Handlings the input request or transmits further.
        /// </summary>
        /// <param name="handlingRequest">The input request.</param>
        public abstract void Handle(AppCommandRequest handlingRequest);
    }
}
