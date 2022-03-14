using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for processing of records by use the different system as storage.
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

        /// <summary>
        /// Select record from current storage by input parameters.
        /// </summary>
        /// <param name="fieldsOfRecordForSelect">The list of fields with values for select.</param>
        /// <param name="fieldsOfRecordsForDisplay">The list of necessary fields for display of selected records.</param>
        /// <param name="orderOfSelect">The definer of a order of select records, 'or' or 'and'.</param>
        public void SelectRecords(List<Tuple<string, string>> fieldsOfRecordForSelect, string[] fieldsOfRecordsForDisplay, string orderOfSelect);
    }
}
