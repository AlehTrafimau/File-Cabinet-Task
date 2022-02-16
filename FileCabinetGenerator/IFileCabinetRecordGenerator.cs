using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Consists a set of methods to auto-generate filds' value of record.
    /// </summary>
    internal interface IFileCabinetRecordGenerator
    {
        /// <summary>
        /// Generates setted number of records (validation rules - default).
        /// </summary>
        /// <param name="numberOfRecords">The necessary number of record.</param>
        /// <param name="startId">The id of the first record in the sequence.</param>
        /// <returns>
        /// The list of auto generated records
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRandomRecords(int numberOfRecords, int startId);

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public string FirstNameGenerate(Random generator);

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public string LastNameGenerate(Random generator);

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public DateTime BirthDateGenerate(Random generator);

        /// <summary>
        /// Generates a random serie of pass number.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random serie of pass number.
        /// </returns>
        public char SerieOfPassNumberGenerate(Random generator);

        /// <summary>
        /// Generates a random pass number.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random pass number.
        /// </returns>
        public short PassNumberGenerate(Random generator);

        /// <summary>
        /// Generates a random bank account.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random bank account.
        /// </returns>
        public decimal BankAccountGenerate(Random generator);
    }
}
