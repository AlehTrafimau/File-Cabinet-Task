using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Exists a set of functions to work with resords: create, storage, edit, list, find by various parameters.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates the new record.
        /// </summary>
        /// <param name="newRecord"> The new record.</param>
        /// <returns>The Id number of the new record.</returns>
        public int CreateRecord(FileCabinetRecord newRecord);

        /// <summary>
        /// Adds new record from file system to current repository.
        /// </summary>
        /// <param name="snapshot">The new records for add.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Gets all records which created.
        /// </summary>
        /// <returns>
        /// The read only collection of created records at the present time.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the count of created records.
        /// </summary>
        /// <returns>
        /// The number of created records.
        /// </returns>
        public (int, int) GetStat();

        /// <summary>
        /// Finds the records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The read only collection of records which consist of this first name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds the records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The read only collection of records which consist of this last name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds the records by birth day.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The read only collection of records which consist of this birth date.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>The snapshot of current state of the cabinet service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Defragments records in file system repository.
        /// </summary>
        public void Purge()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts the new record to the current storage.
        /// </summary>
        /// <param name="insertRecord">The record for insert.</param>
        public void InsertRecord(FileCabinetRecord insertRecord);

        /// <summary>
        /// Deletes record from current storage by input conditions.
        /// </summary>
        /// <param name="fieldName">The name of record field.</param>
        /// <param name="value">The value of record field.</param>
        public void Delete(string fieldName, string value);

        /// <summary>
        /// Updates record from current storage by input parameters.
        /// </summary>
        /// <param name="newParameters">The record which consist of new parameters.</param>
        /// <param name="findConditions">The record which consist of fields as find conditions.</param>
        public void Update(FileCabinetRecord newParameters, FileCabinetRecord findConditions);
    }
}
