using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace FileCabinetApp
{
    /// <summary>
    /// Convert records to scv format.
    /// </summary>
    internal class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        internal FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Writes the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        public void Write(FileCabinetRecord[] records)
        {
            StringBuilder csvFormat = new ();

            foreach (FileCabinetRecord i in records)
            {
                csvFormat.AppendLine(CultureInfo.InvariantCulture, $"{i.FirstName}, {i.LastName}, {i.DateOfBirth}, {i.SerieOfPassNumber}, {i.PassNumber}, {i.BankAccount}");
            }

            using (this.writer)
            {
                this.writer.Write(csvFormat);
            }
        }
    }
}
