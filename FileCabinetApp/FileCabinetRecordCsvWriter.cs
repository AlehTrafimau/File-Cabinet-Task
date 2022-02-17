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
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Convert list of records to csv format.
        /// </summary>
        /// <param name="records">The snapshot of current list of records.</param>
        public void Write(FileCabinetRecord[] records)
        {
            StringBuilder csvFormat = new ();

            foreach (FileCabinetRecord i in records)
            {
                csvFormat.AppendLine(CultureInfo.InvariantCulture, $"{i.Id}, {i.FirstName}, {i.LastName}, {i.DateOfBirth.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}, {i.SerieOfPassNumber}, {i.PassNumber}, {i.BankAccount}");
            }

            this.writer.Write(csvFormat);
        }
    }
}
