using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Define the function to display records.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Display the list of records.
        /// </summary>
        /// <param name="records">The list of records.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
