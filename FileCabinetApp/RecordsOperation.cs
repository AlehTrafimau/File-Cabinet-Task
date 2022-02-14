using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of methods processing records.
    /// </summary>
    internal static class RecordsOperation
    {
        /// <summary>
        /// Display data of records to console.
        /// </summary>
        /// <param name="records">The sequence of record for display.</param>
        internal static void PrintRecord(IReadOnlyCollection<FileCabinetRecord> records)
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
