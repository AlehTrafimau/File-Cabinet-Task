using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Contains of methods to iteration of data in the storage.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Returns the next record in current file cabinet storage.
        /// </summary>
        /// <returns>
        /// The next record.
        /// </returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Stores information about exist of a record in the current storage.
        /// </summary>
        /// <returns>
        /// True if the current storage has a next record, and false otherwise.
        /// </returns>
        public bool HasMore();
    }
}
