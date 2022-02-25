using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Elemenates time of running any operation with IFileCabinetRecord instance.
    /// </summary>
    internal class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;
        private Stopwatch stopWatch = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">The instance of current file cabinet service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Elemenates time of running create operation.
        /// </summary>
        /// <param name="newRecord">The new record.</param>
        /// <returns>The Id number of the new record.</returns>
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            this.stopWatch.Restart();
            int newRecordId = this.service.CreateRecord(newRecord);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Create method execution duration is {timeSpan.Ticks} ticks.");
            return newRecordId;
        }

        /// <summary>
        /// Elemenates time of running edit operation.
        /// </summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited paremeters of record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            this.stopWatch.Restart();
            this.service.EditRecord(editRecordId, editedRecord);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Edit method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Elemenates time of running find operation.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            this.stopWatch.Restart();
            ReadOnlyCollection<FileCabinetRecord> findResult = this.service.FindByDayOfBirth(birthDayParameter);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"FindByDayOfBirth method execution duration is {timeSpan.Ticks} ticks.");
            return findResult;
        }

        /// <summary>
        /// Elemenates time of running find operation.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The list of records which consist of this first name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopWatch.Restart();
            ReadOnlyCollection<FileCabinetRecord> findResult = this.service.FindByFirstName(firstName);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"FindByFirstName method execution duration is {timeSpan.Ticks} ticks.");
            return findResult;
        }

        /// <summary>
        /// Elemenates time of running find operation.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The list of records which consist of this last name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopWatch.Restart();
            ReadOnlyCollection<FileCabinetRecord> findResult = this.service.FindByLastName(lastName);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"FindByLastName method execution duration is {timeSpan.Ticks} ticks.");
            return findResult;
        }

        /// <summary>
        /// Elemenates time of running get record operation.
        /// </summary>
        /// <returns>
        /// The list of created records at the present time.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopWatch.Restart();
            ReadOnlyCollection<FileCabinetRecord> result = this.service.GetRecords();
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"GetRecords method execution duration is {timeSpan.Ticks} ticks.");
            return result;
        }

        /// <summary>
        /// Elemenates time of running get stat operation.
        /// </summary>
        /// <returns>
        /// The count of created records.
        /// </returns>
        public (int, int) GetStat()
        {
            this.stopWatch.Restart();
            (int, int) result = this.service.GetStat();
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"GetStat method execution duration is {timeSpan.Ticks} ticks.");
            return result;
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

        /// <summary>
        /// Elemenates time of running remove opeation.
        /// </summary>
        /// <param name="recordId"> The id if record for remove.</param>
        public void RemoveRecord(int recordId)
        {
            this.stopWatch.Restart();
            this.service.RemoveRecord(recordId);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"RemoveRecord method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Elemenates time of running remove opeation.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopWatch.Restart();
            this.service.Restore(snapshot);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Restore method execution duration is {timeSpan.Ticks} ticks.");
        }
    }
}