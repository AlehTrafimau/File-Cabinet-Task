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
    internal interface IFileCabinetService
    {
        /// <summary>Creates the new record.</summary>
        /// <param name="newRecord"> The new record.</param>
        /// <returns>The Id number of the new record.</returns>
        public int CreateRecord(FileCabinetRecord newRecord);

        /// <summary>
        /// Adds new record from file system to current repository.
        /// </summary>
        /// <param name="snapshot">The new records for add.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>Gets all records which created.</summary>
        /// <returns>
        /// The read only collection of created records at the present time.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>Gets the count of created records.</summary>
        /// <returns>
        /// The number of created records.
        /// </returns>
        public int GetStat();

        /// <summary>Edits the exist record by Id number.</summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord);

        /// <summary>Finds the records by first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The read only collection of records which consist of this first name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>Finds the records by last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The read only collection of records which consist of this last name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>Finds the records by birth day.</summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The read only collection of records which consist of this birth date.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>The snapshot of current state of the cabinet service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new FileCabinetRecord[] { new FileCabinetRecord() });
        }
    }
}
