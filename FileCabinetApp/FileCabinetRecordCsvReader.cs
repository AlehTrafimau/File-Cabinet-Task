using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// DFDF.
    /// </summary>
    internal class FileCabinetRecordCsvReader
    {
        private readonly StreamReader streamReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="streamReader">The text writer.</param>
        public FileCabinetRecordCsvReader(StreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        /// <summary>
        /// lllll.
        /// </summary>
        /// <returns>f.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> newRecords = new ();
            int currentRecordId = 0;

            while (!this.streamReader.EndOfStream)
            {
                string? currentFileString = this.streamReader.ReadLine();
                string[] recordFilds = new string[7];

                if (currentFileString != null)
                {
                    recordFilds = currentFileString.Split(',');
                }

                if (recordFilds[0].ToUpperInvariant() != "ID")
                {
                    try
                    {
                        ++currentRecordId;
                        FileCabinetRecord newRecord = new ();
                        newRecord.Id = int.Parse(recordFilds[0], CultureInfo.InvariantCulture);
                        newRecord.FirstName = recordFilds[1].Trim(' ');
                        newRecord.LastName = recordFilds[2].Trim(' ');
                        newRecord.DateOfBirth = DateTime.Parse(recordFilds[3], CultureInfo.InvariantCulture);
                        newRecord.SerieOfPassNumber = char.Parse(recordFilds[4].Trim(' '));
                        newRecord.PassNumber = short.Parse(recordFilds[5], CultureInfo.InvariantCulture);
                        newRecord.BankAccount = decimal.Parse(recordFilds[6], CultureInfo.InvariantCulture);
                        newRecords.Add(newRecord);
                    }
                    catch
                    {
                        Console.WriteLine($"The record {currentRecordId} is invalid");
                    }
                }
            }

            return newRecords;
        }
    }
}
