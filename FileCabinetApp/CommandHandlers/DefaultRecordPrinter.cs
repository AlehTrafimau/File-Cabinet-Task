using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Outputs records to console by default format.
    /// </summary>
    internal class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Outputs records to console by default format.
        /// </summary>
        /// <param name="records">The sequence of record for display.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records != null)
            {
                foreach (FileCabinetRecord currentRecord in records)
                {
                    Console.WriteLine($"#{currentRecord.Id}, {currentRecord.FirstName}, {currentRecord.LastName}, {currentRecord.DateOfBirth.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}," +
                        $" pass number: {currentRecord.SerieOfPassNumber} {currentRecord.PassNumber}, currentBankAccount: {currentRecord.BankAccount}$");
                }
            }
        }
    }
}
