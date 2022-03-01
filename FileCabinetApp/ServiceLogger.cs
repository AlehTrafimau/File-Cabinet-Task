using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes information about service operations to file system storage.
    /// </summary>
    internal class ServiceLogger : ServiceMeter
    {
        private StreamWriter streamWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="streamWriter">The instance of stream writer.</param>
        /// <param name="service">The instance of current file cabinet srvice.</param>
        public ServiceLogger(StreamWriter streamWriter, IFileCabinetService service)
            : base(service)
        {
            this.streamWriter = streamWriter;
        }

        /// <summary>
        /// Writes information about create operation to file system storage.
        /// </summary>
        /// <param name="newRecord">The new record.</param>
        /// <returns>The Id number of the new record.</returns>
        public override int CreateRecord(FileCabinetRecord newRecord)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Create() with FirstName = '{newRecord.FirstName}', LastName = '{newRecord.LastName}', DateOfBirth = '{newRecord.DateOfBirth}', SerieOfPassNumber = '{newRecord.SerieOfPassNumber}', PassNumber = '{newRecord.PassNumber}', BankAccount = '{newRecord.BankAccount}'");
            int newRecordId = base.CreateRecord(newRecord);
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Create() returned '{newRecordId}'");
            this.streamWriter.Flush();
            return newRecordId;
        }

        /// <summary>
        /// Writes information about edit operation to file system storage.
        /// </summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited paremeters of record.</param>
        public override void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Create() with Id = '{editRecordId}', FirstName = '{editedRecord.FirstName}', LastName = '{editedRecord.LastName}', DateOfBirth = '{editedRecord.DateOfBirth}', SerieOfPassNumber = '{editedRecord.SerieOfPassNumber}', PassNumber = '{editedRecord.PassNumber}', BankAccount = '{editedRecord.BankAccount}'");
            base.EditRecord(editRecordId, editedRecord);
            this.streamWriter.Flush();
        }

        /// <summary>
        /// Writes information about edit operation to file system storage.
        /// </summary>
        /// <param name="insertRecord">The id of record for edit.</param>
        public override void InsertRecord(FileCabinetRecord insertRecord)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Insert() with Id = '{insertRecord.Id}', FirstName = '{insertRecord.FirstName}', LastName = '{insertRecord.LastName}', DateOfBirth = '{insertRecord.DateOfBirth}', SerieOfPassNumber = '{insertRecord.SerieOfPassNumber}', PassNumber = '{insertRecord.PassNumber}', BankAccount = '{insertRecord.BankAccount}'");
            base.InsertRecord(insertRecord);
            this.streamWriter.Flush();
        }

        /// <summary>
        /// Writes information about find by date of birth operation to file system storage.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public override IEnumerable<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Find() by day of birth with birth date = '{birthDayParameter}'");
            IEnumerable<FileCabinetRecord> findResult = base.FindByDayOfBirth(birthDayParameter);
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Find() returned records:");

            foreach (var record in findResult)
            {
                this.streamWriter.WriteLine($"Id = '{record.Id}', FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', SerieOfPassNumber = '{record.SerieOfPassNumber}', PassNumber = '{record.PassNumber}', BankAccount = '{record.BankAccount}'");
            }

            this.streamWriter.Flush();
            return findResult;
        }

        /// <summary>
        /// Writes information about find by first name operations to file system storage.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The list of records which consist of this first name.
        /// </returns>
        public override IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Find() by first name with birth date = '{firstName}'");
            IEnumerable<FileCabinetRecord> findResult = base.FindByFirstName(firstName);
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Find() returned records:");

            foreach (var record in findResult)
            {
                this.streamWriter.WriteLine($"Id = '{record.Id}', FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', SerieOfPassNumber = '{record.SerieOfPassNumber}', PassNumber = '{record.PassNumber}', BankAccount = '{record.BankAccount}'");
            }

            this.streamWriter.Flush();
            return findResult;
        }

        /// <summary>
        /// Writes information about find by last name operations to file system storage.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The list of records which consist of this last name.
        /// </returns>
        public override IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Find() by last name with birth date = '{lastName}'");
            IEnumerable<FileCabinetRecord> findResult = base.FindByLastName(lastName);
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Find() returned records.");

            foreach (var record in findResult)
            {
                this.streamWriter.WriteLine($"Id = '{record.Id}', FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', SerieOfPassNumber = '{record.SerieOfPassNumber}', PassNumber = '{record.PassNumber}', BankAccount = '{record.BankAccount}'");
            }

            this.streamWriter.Flush();
            return findResult;
        }

        /// <summary>
        /// Writes information about get record operation to file system storage.
        /// </summary>
        /// <returns>
        /// The list of created records at the present time.
        /// </returns>
        public override ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling List()");
            ReadOnlyCollection<FileCabinetRecord> allRecords = base.GetRecords();
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - List() returned {allRecords.Count} records.");

            foreach (var i in allRecords)
            {
                this.streamWriter.WriteLine($"Id = '{i.Id}', FirstName = '{i.FirstName}', LastName = '{i.LastName}', DateOfBirth = '{i.DateOfBirth}', SerieOfPassNumber = '{i.SerieOfPassNumber}', PassNumber = '{i.PassNumber}', BankAccount = '{i.BankAccount}'");
            }

            this.streamWriter.Flush();
            return allRecords;
        }

        /// <summary>
        /// Writes information about get stat operation to file system storage.
        /// </summary>
        /// <returns>
        /// The count of created records.
        /// </returns>
        public override (int, int) GetStat()
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Stat()");
            (int, int) result = this.service.GetStat();
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Stat() returned: all records {result.Item1}, removed {result.Item2}");
            this.streamWriter.Flush();
            return result;
        }

        /// <summary>
        /// Writes information about remove record operation to file system storage.
        /// </summary>
        /// <param name="recordId"> The id if record for remove.</param>
        public override void RemoveRecord(int recordId)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Remove() with ID = '{recordId}'");
            base.RemoveRecord(recordId);
            this.streamWriter.Flush();
        }

        /// <summary>
        /// Writes information about restore operation to file system storage.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.streamWriter.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.CurrentCulture)} - Calling Restore() with count of records = '{snapshot.Records.Count}'");
            base.Restore(snapshot);
            this.streamWriter.Flush();
        }
    }
}
