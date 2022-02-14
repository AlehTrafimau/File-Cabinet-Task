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
    /// Service for work to file system.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    internal class FileCabinetFileSystemService : IFileCabinetService
    {
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFileSystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Adds new record to the default storage in file system.
        /// </summary>
        /// <param name="newRecord">The new record.</param>
        /// <returns>
        /// The Id number of the new record in the default storage.
        /// </returns>
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            int lengthOfByteRecord = 276;
            int lastRecordInFile = (int)(this.fileStream.Position / lengthOfByteRecord);
            newRecord.Id = lastRecordInFile + 1;

            byte[] input = Encoding.Default.GetBytes(newRecord.Id.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.FirstName);
            Array.Resize(ref input, 120);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.LastName);
            Array.Resize(ref input, 120);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.DateOfBirth.Day.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.DateOfBirth.Month.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.DateOfBirth.Year.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.SerieOfPassNumber.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.PassNumber.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.BankAccount.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 16);
            this.fileStream.Write(input, 0, input.Length);

            return newRecord.Id;
        }

        /// <summary>
        /// Edits the exist record by Id number.
        /// </summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            long startByteOfRecordInFile = (editRecordId - 1) * 276;
            this.fileStream.Seek(startByteOfRecordInFile, SeekOrigin.Begin);
            editedRecord.Id = editRecordId;

            byte[] input = Encoding.Default.GetBytes(editedRecord.Id.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.FirstName);
            Array.Resize(ref input, 120);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.LastName);
            Array.Resize(ref input, 120);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.DateOfBirth.Day.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.DateOfBirth.Month.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.DateOfBirth.Year.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 4);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.SerieOfPassNumber.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.PassNumber.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(editedRecord.BankAccount.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 16);
            this.fileStream.Write(input, 0, input.Length);
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
            ReadOnlyCollection<FileCabinetRecord> allRecords = this.GetRecords();
            List<FileCabinetRecord> resultOfSearch = new List<FileCabinetRecord>();

            bool isDateTime = DateTime.TryParse(birthDayParameter, out DateTime dayOfBirth);

            if (isDateTime)
            {
                foreach (FileCabinetRecord currentRecord in allRecords)
                {
                    if (currentRecord.DateOfBirth == dayOfBirth)
                    {
                        resultOfSearch.Add(currentRecord);
                    }
                }

                return new ReadOnlyCollection<FileCabinetRecord>(resultOfSearch);
            }

            Console.WriteLine("Convert error. Format date of birth parameter: \"Year - Month - Day\" ");
            return new ReadOnlyCollection<FileCabinetRecord>(resultOfSearch);
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
            List<FileCabinetRecord> recordsFromFileSystem = new ();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            long numberOfRecordInFile = this.fileStream.Length / 276;

            while (numberOfRecordInFile > 0)
            {
                FileCabinetRecord currentRecord = new ();
                byte[] array = new byte[4];
                this.fileStream.Read(array, 0, array.Length);
                currentRecord.Id = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

                Array.Resize(ref array, 120);
                this.fileStream.Read(array, 0, array.Length);
                currentRecord.FirstName = Encoding.Default.GetString(array).Trim(default(char));

                Array.Clear(array);
                this.fileStream.Read(array, 0, array.Length);
                currentRecord.LastName = Encoding.Default.GetString(array).Trim(default(char));

                Array.Resize(ref array, 4);
                this.fileStream.Read(array, 0, array.Length);
                int day = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                this.fileStream.Read(array, 0, array.Length);
                int month = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                this.fileStream.Read(array, 0, array.Length);
                int year = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                currentRecord.DateOfBirth = new DateTime(year, month, day);

                Array.Resize(ref array, 2);
                this.fileStream.Read(array, 0, array.Length);
                currentRecord.SerieOfPassNumber = char.Parse(Encoding.Default.GetString(array).Trim(default(char)));

                this.fileStream.Read(array, 0, array.Length);
                currentRecord.PassNumber = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

                Array.Resize(ref array, 16);
                this.fileStream.Read(array, 0, array.Length);
                currentRecord.BankAccount = decimal.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

                recordsFromFileSystem.Add(currentRecord);
                numberOfRecordInFile--;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(recordsFromFileSystem);
        }

        /// <summary>
        /// Gets the count of created records.
        /// </summary>
        /// <returns>
        /// The number of created records.
        /// </returns>
        public int GetStat()
        {
            long numberOfRecordInFile = this.fileStream.Length / 276;
            return (int)numberOfRecordInFile;
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
