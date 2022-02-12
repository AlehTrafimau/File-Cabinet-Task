using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for work to file system.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    internal class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates the new record.
        /// </summary>
        /// <param name="newRecord">The new record.</param>
        /// <returns>
        /// The Id number of the new record.
        /// </returns>
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edits the exist record by Id number.
        /// </summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by birth day.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The read only collection of records which consist of this birth date.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The read only collection of records which consist of this first name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The read only collection of records which consist of this last name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all records which created.
        /// </summary>
        /// <returns>
        /// The read only collection of created records at the present time.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count of created records.
        /// </summary>
        /// <returns>
        /// The number of created records.
        /// </returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// The snapshot of current state of the cabinet service.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
