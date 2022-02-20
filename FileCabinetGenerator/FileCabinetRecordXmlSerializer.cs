using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Convert records to xml format.
    /// </summary>
    internal class FileCabinetRecordXmlSerializer
    {
        private readonly StreamWriter fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlSerializer"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        internal FileCabinetRecordXmlSerializer(StreamWriter fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// sd.
        /// </summary>
        /// <param name="records">dsds.</param>
        public void Write(FileCabinetRecord[] records)
        {
            XmlSerializer formatter = new (typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));
            formatter.Serialize(this.fileStream, records);
            Console.WriteLine("Объект сериализован");
        }
    }
}
