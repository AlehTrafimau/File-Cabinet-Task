using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            FileCabinetRecordCsvWriter csvWriter = new (streamWriter);
            csvWriter.Write(this.latestSnapshotOfRecordsState);
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        internal void SaveToXml(StreamWriter streamWriter)
        {
            var sts = new XmlWriterSettings()
            {
                Indent = true,
            };

            XmlWriter xmlWriter = XmlWriter.Create(streamWriter, sts);
            FileCabinetRecordXmlWriter converterToXml = new (xmlWriter);
            converterToXml.Write(this.latestSnapshotOfRecordsState);
        }
    }
}