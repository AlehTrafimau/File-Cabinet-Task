using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Convert records to scv format.
    /// </summary>
    internal class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        internal FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Convert list of records to xml format.
        /// </summary>
        /// <param name="records">The snapshot of current list of records.</param>
        public void Write(FileCabinetRecord[] records)
        {
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("records");

            foreach (FileCabinetRecord i in records)
            {
                this.writer.WriteStartElement("record");
                this.writer.WriteAttributeString("Id", $"{i.Id}");

                this.writer.WriteStartElement("firstName");
                this.writer.WriteValue(i.FirstName);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("lastName");
                this.writer.WriteValue(i.LastName);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("dateOfBirth");
                this.writer.WriteValue(i.DateOfBirth);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("serieOfPassNumber");
                this.writer.WriteValue(i.SerieOfPassNumber);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("passNumber");
                this.writer.WriteValue(i.PassNumber);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("bankAccount");
                this.writer.WriteValue(i.BankAccount);
                this.writer.WriteEndElement();

                this.writer.WriteEndElement();
            }

            this.writer.WriteEndDocument();
            this.writer.Close();
        }
    }
}
