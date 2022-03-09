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
        /// <summary>
        /// The instance of current file cabinet service.
        /// </summary>
        protected IFileCabinetService service;
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
        public virtual int CreateRecord(FileCabinetRecord newRecord)
        {
            this.stopWatch.Restart();
            int newRecordId = this.service.CreateRecord(newRecord);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Create method execution duration is {timeSpan.Ticks} ticks.");
            return newRecordId;
        }

        /// <summary>
        /// Elemenates time of running find operation.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public virtual IEnumerable<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            this.stopWatch.Restart();
            IEnumerable<FileCabinetRecord> findResult = this.service.FindByDayOfBirth(birthDayParameter);
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
        public virtual IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopWatch.Restart();
            IEnumerable<FileCabinetRecord> findResult = this.service.FindByFirstName(firstName);
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
        public virtual IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopWatch.Restart();
            IEnumerable<FileCabinetRecord> findResult = this.service.FindByLastName(lastName);
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
        public virtual ReadOnlyCollection<FileCabinetRecord> GetRecords()
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
        public virtual (int, int) GetStat()
        {
            this.stopWatch.Restart();
            (int, int) result = this.service.GetStat();
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"GetStat method execution duration is {timeSpan.Ticks} ticks.");
            return result;
        }

        /// <summary>
        /// Elemenates time of running insert operation.
        /// </summary>
        /// <param name="insertRecord">The record for insert to current storage.</param>
        public virtual void InsertRecord(FileCabinetRecord insertRecord)
        {
            this.stopWatch.Restart();
            this.service.InsertRecord(insertRecord);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Insert method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// The snapshot of current state of the cabinet service.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot newSnapshot = this.service.MakeSnapshot();
            return newSnapshot;
        }

        /// <summary>
        /// Elemenates time of running restore operation.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public virtual void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopWatch.Restart();
            this.service.Restore(snapshot);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Restore method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Elemenates time of running update operation.
        /// </summary>
        /// <param name="newParameters">The record which consist of new parameters.</param>
        /// <param name="findConditions">The record which consist of fields as find conditions.</param>
        public virtual void Update(FileCabinetRecord newParameters, FileCabinetRecord findConditions)
        {
            this.stopWatch.Restart();
            this.service.Update(newParameters, findConditions);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Update method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Elemenates time of running delete operation.
        /// </summary>
        /// <param name="fieldName">The name of record field.</param>
        /// <param name="value">The value of record field.</param>
        public virtual void Delete(string fieldName, string value)
        {
            this.stopWatch.Restart();
            this.service.Delete(fieldName, value);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Delete method execution duration is {timeSpan.Ticks} ticks.");
        }

        /// <summary>
        /// Elemenates time of running select operation.
        /// </summary>
        /// <param name="fieldsOfRecordForSelect">The list of fields with values for select.</param>
        /// <param name="fieldsOfRecordsForDisplay">The list of necessary fields for display of selected records.</param>
        /// <param name="orderOfSelect">The definer of a order of select records, 'or' or 'and'.</param>
        public virtual void SelectRecords(List<Tuple<string, string>>? fieldsOfRecordForSelect, string[] fieldsOfRecordsForDisplay, string? orderOfSelect)
        {
            this.stopWatch.Restart();
            this.service.SelectRecords(fieldsOfRecordForSelect, fieldsOfRecordsForDisplay, orderOfSelect);
            this.stopWatch.Stop();
            TimeSpan timeSpan = this.stopWatch.Elapsed;
            Console.WriteLine($"Select method execution duration is {timeSpan.Ticks} ticks.");
        }
    }
}