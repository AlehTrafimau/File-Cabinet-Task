using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Reads records from xml file in.
    /// </summary>
    internal class FileCabinetRecordXmlReader
    {
        private readonly StreamReader streamReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="streamReader">The stream reader.</param>
        public FileCabinetRecordXmlReader(StreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        /// <summary>
        /// Reads recors data from xml file.
        /// </summary>
        /// <returns>
        /// List of records from xml file.
        /// </returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> newRecords = new ();

            XmlSerializer formatter = new (typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));

            var importRecords = formatter.Deserialize(XmlReader.Create(this.streamReader)) as FileCabinetRecord[];
            if (importRecords != null)
            {
                newRecords.AddRange(importRecords);
            }

            return newRecords;
        }
    }
}
