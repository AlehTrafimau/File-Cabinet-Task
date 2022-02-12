using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to storage states of records.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        /// <summary>
        /// Storages snapshots of records status.
        /// </summary>
        private readonly FileCabinetRecord[] latestSnapshotOfRecordsState;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="newSnapshot">The new snapshot.</param>
        internal FileCabinetServiceSnapshot(FileCabinetRecord[] newSnapshot)
        {
            this.latestSnapshotOfRecordsState = newSnapshot;
        }

        /// <summary>
        /// Saves to CSV.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        internal void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter saveSCV = new (streamWriter);
            saveSCV.Write(this.latestSnapshotOfRecordsState);
        }
    }
}
