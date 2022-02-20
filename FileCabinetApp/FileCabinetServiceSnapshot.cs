using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private FileCabinetRecord[] recordsSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="newSnapshot">The new snapshot.</param>
        internal FileCabinetServiceSnapshot(FileCabinetRecord[] newSnapshot)
        {
            this.recordsSnapshot = newSnapshot;
        }

        /// <summary>
        /// Gets latest snaphot of records.
        /// </summary>
        /// <value>
        /// Latest snapshot.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.recordsSnapshot);
            }
        }

        /// <summary>
        /// Reads records from file system in csv format.
        /// </summary>
        /// <param name="streamReader">The stream reader for reading records from file.</param>
        internal void ReadFromCsv(StreamReader streamReader)
        {
            FileCabinetRecordCsvReader newReader = new (streamReader);
            FileCabinetRecord[] newRecords = newReader.ReadAll().ToArray();
            this.recordsSnapshot = newRecords;
        }

        /// <summary>
        /// Reads records from file system in xml format.
        /// </summary>
        /// <param name="streamReader">The stream reader for reading records from file.</param>
        internal void ReadFromXml(StreamReader streamReader)
        {
            FileCabinetRecordXmlReader newReader = new (streamReader);
            FileCabinetRecord[] newRecords = newReader.ReadAll().ToArray();
            this.recordsSnapshot = newRecords;
        }

        /// <summary>
        /// Saves to CSV.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        internal void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter csvWriter = new (streamWriter);
            csvWriter.Write(this.recordsSnapshot);
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
            converterToXml.Write(this.recordsSnapshot);
        }
    }
}