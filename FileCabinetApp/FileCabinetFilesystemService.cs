using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for work to file system.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService"/>
    internal class FileCabinetFileSystemService : IFileCabinetService
    {
        private const int BytesInRecord = 278;
        private readonly Dictionary<string, List<long>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<long>> dateOfBirthDictionary = new ();
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFileSystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
            this.InitializeDictionaries();
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
            short statusOfRecord = 0;
            this.fileStream.Seek(0, SeekOrigin.End);
            long recordPosition = this.fileStream.Position;
            int lastRecordInFile = (int)(this.fileStream.Position / BytesInRecord);
            newRecord.Id = lastRecordInFile + 1;
            AddToDictionary(this.firstNameDictionary, newRecord.FirstName, recordPosition);
            AddToDictionary(this.lastNameDictionary, newRecord.LastName, recordPosition);
            AddToDictionary(this.dateOfBirthDictionary, newRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), recordPosition);
            this.WriteRecordToFile(newRecord, statusOfRecord);

            return newRecord.Id;
        }

        /// <summary>
        /// Inserts the new record to the current storage.
        /// </summary>
        /// <param name="insertRecord">The record for insert.</param>
        public void InsertRecord(FileCabinetRecord insertRecord)
        {
            long firstByteInsertRecordInFile = (insertRecord.Id - 1) * BytesInRecord;
            this.fileStream.Seek(firstByteInsertRecordInFile, SeekOrigin.Begin);
            List<(FileCabinetRecord, bool)> recordsAfterInsertRecord = new ();
            while (this.fileStream.Position != this.fileStream.Length)
            {
                var record = this.ReadRecordFromFile(firstByteInsertRecordInFile);
                recordsAfterInsertRecord.Add(record);
                firstByteInsertRecordInFile += BytesInRecord;
                this.RemoveFromDictionaries(record.Item1.Id);
            }

            firstByteInsertRecordInFile = (insertRecord.Id - 1) * BytesInRecord;
            this.fileStream.Seek(firstByteInsertRecordInFile, SeekOrigin.Begin);
            this.WriteRecordToFile(insertRecord, 0);
            AddToDictionary(this.firstNameDictionary, insertRecord.FirstName, firstByteInsertRecordInFile);
            AddToDictionary(this.lastNameDictionary, insertRecord.LastName, firstByteInsertRecordInFile);
            AddToDictionary(this.dateOfBirthDictionary, insertRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), firstByteInsertRecordInFile);

            foreach (var i in recordsAfterInsertRecord)
            {
                long recordPosition = this.fileStream.Position;
                i.Item1.Id += 1;
                if (i.Item2)
                {
                    this.WriteRecordToFile(i.Item1, 1);
                }
                else
                {
                    this.WriteRecordToFile(i.Item1, 0);
                }

                AddToDictionary(this.firstNameDictionary, i.Item1.FirstName, recordPosition);
                AddToDictionary(this.lastNameDictionary, i.Item1.LastName, recordPosition);
                AddToDictionary(this.dateOfBirthDictionary, i.Item1.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), recordPosition);
            }
        }

        /// <summary>
        /// Edits the exist record by Id number.
        /// </summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            long startByteOfRecordInFile = (editRecordId - 1) * BytesInRecord;
            this.RemoveFromDictionaries(editRecordId);
            this.fileStream.Seek(startByteOfRecordInFile, SeekOrigin.Begin);
            long recordPosition = this.fileStream.Position;
            editedRecord.Id = editRecordId;

            byte[] array = new byte[2];
            this.fileStream.Read(array, 0, array.Length);
            short statusOfRecord = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            if (statusOfRecord == 1)
            {
                Console.WriteLine($"Invalid operation. The record #{editRecordId} is removed.");
                this.fileStream.Seek(0, SeekOrigin.End);
                statusOfRecord = 0;
            }

            this.fileStream.Seek(-2, SeekOrigin.Current);
            this.WriteRecordToFile(editedRecord, statusOfRecord);
            AddToDictionary(this.firstNameDictionary, editedRecord.FirstName, recordPosition);
            AddToDictionary(this.lastNameDictionary, editedRecord.LastName, recordPosition);
            AddToDictionary(this.dateOfBirthDictionary, editedRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), recordPosition);
            Console.WriteLine($"Record #{editRecordId} is updated");
        }

        /// <summary>
        /// Finds the records by birth day.
        /// </summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The read only collection of records which consist of this birth date.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            bool isDateTime = DateTime.TryParse(birthDayParameter, out DateTime dayOfBirth);

            if (!isDateTime)
            {
                Console.WriteLine("Convert error. Format date of birth parameter: \"Year - Month - Day\" ");
                return Array.Empty<FileCabinetRecord>();
            }

            string correctFormatOfParameter = dayOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (this.dateOfBirthDictionary.ContainsKey(correctFormatOfParameter.ToUpperInvariant()))
            {
                FilesystemEnumerable fileEnum = new FilesystemEnumerable(this.dateOfBirthDictionary[correctFormatOfParameter], this.fileStream);

                List<FileCabinetRecord> result = new ();

                foreach (FileCabinetRecord i in fileEnum)
                {
                    result.Add(i);
                }

                return result;
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds the records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The read only collection of records which consist of this first name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                FilesystemEnumerable fileEnum = new FilesystemEnumerable(this.firstNameDictionary[firstName.ToUpperInvariant()], this.fileStream);

                List<FileCabinetRecord> result = new ();

                foreach (FileCabinetRecord i in fileEnum)
                {
                    result.Add(i);
                }

                return result;
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds the records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The read only collection of records which consist of this last name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                FilesystemEnumerable fileEnum = new FilesystemEnumerable(this.lastNameDictionary[lastName.ToUpperInvariant()], this.fileStream);

                List<FileCabinetRecord> result = new ();

                foreach (FileCabinetRecord i in fileEnum)
                {
                    result.Add(i);
                }

                return result;
            }

            return new List<FileCabinetRecord>();
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
            long numberOfRecordInFile = this.fileStream.Length / BytesInRecord;
            short statusOfRecord;

            while (numberOfRecordInFile > 0)
            {
                byte[] array = new byte[2];
                this.fileStream.Read(array, 0, array.Length);
                statusOfRecord = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

                if (statusOfRecord == 1)
                {
                    this.fileStream.Seek(276, SeekOrigin.Current);
                    numberOfRecordInFile--;
                    continue;
                }

                var currentRecord = this.ReadRecordFromFile(this.fileStream.Position - 2);
                if (!currentRecord.Item2)
                {
                    recordsFromFileSystem.Add(currentRecord.Item1);
                }

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
        public (int, int) GetStat()
        {
            int numberOfRecordInFile = (int)this.fileStream.Length / BytesInRecord;
            int removedRecordsInFile = numberOfRecordInFile - this.GetRecords().Count;

            (int NumberOfRecords, int removedRecords) stat = new (numberOfRecordInFile, removedRecordsInFile);
            return stat;
        }

        /// <summary>
        /// Makes the snapshot of records data in file system repository.
        /// </summary>
        /// <returns>
        /// The snapshot of current content of file system repository.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot newSnapshot = new (this.GetRecords().ToArray());
            return newSnapshot;
        }

        /// <summary>
        /// Removes the record from current repositiry.
        /// </summary>
        /// <param name="recordId"> The id if record for remove.</param>
        public void RemoveRecord(int recordId)
        {
            if (recordId <= 0)
            {
                Console.WriteLine("Invelid record ID");
                return;
            }

            int isDeleted = 1;
            long startByteOfRecordInFile = (recordId - 1) * BytesInRecord;
            this.fileStream.Seek(startByteOfRecordInFile, SeekOrigin.Begin);
            byte[] input = Encoding.Default.GetBytes(isDeleted.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);
            this.fileStream.Flush();
            this.RemoveFromDictionaries(recordId);
            Console.WriteLine($"Record #{recordId} is removed.");
        }

        /// <summary>
        /// Restores records from file system to the current repository.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> newRecords = snapshot.Records;

            for (int i = 0; i < newRecords.Count; i++)
            {
                this.EditRecord(newRecords[i].Id, newRecords[i]);
            }
        }

        /// <summary>
        /// Defragments records in file system repository.
        /// </summary>
        public void Purge()
        {
            ReadOnlyCollection<FileCabinetRecord> validRecords = this.GetRecords();
            int totalRecordsInRepo = this.GetStat().Item1;
            this.fileStream.SetLength(0);
            this.fileStream.Seek(0, SeekOrigin.Begin);
            foreach (var i in validRecords)
            {
                this.CreateRecord(i);
            }

            Console.WriteLine($"Data file processing is completed: {totalRecordsInRepo - validRecords.Count} of {totalRecordsInRepo} records were purged.");
        }

        private static void AddToDictionary(Dictionary<string, List<long>> dictionary, string key, long elementOfKey)
        {
            if (dictionary.ContainsKey(key.ToUpperInvariant()))
            {
                dictionary[key.ToUpperInvariant()].Add(elementOfKey);
            }
            else
            {
                dictionary.Add(key.ToUpperInvariant(), new List<long>() { elementOfKey });
            }
        }

        private void RemoveFromDictionaries(int recordId)
        {
            long indexOfRecord = (recordId - 1) * BytesInRecord;
            FileCabinetRecord removedRecord = this.ReadRecordFromFile(indexOfRecord).Item1;
            string firstNameKey = removedRecord.FirstName.ToUpperInvariant();

            if (this.firstNameDictionary.ContainsKey(firstNameKey))
            {
                this.firstNameDictionary[firstNameKey].Remove(indexOfRecord);
            }
            else
            {
                return;
            }

            if (this.firstNameDictionary[firstNameKey].Count == 0)
            {
                this.firstNameDictionary.Remove(firstNameKey);
            }

            string lastNameKey = removedRecord.LastName.ToUpperInvariant();
            this.lastNameDictionary[lastNameKey].Remove(indexOfRecord);

            if (this.lastNameDictionary[lastNameKey].Count == 0)
            {
                this.lastNameDictionary.Remove(lastNameKey);
            }

            string dateOfBirthKey = removedRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.dateOfBirthDictionary[dateOfBirthKey].Remove(indexOfRecord);

            if (this.dateOfBirthDictionary[dateOfBirthKey].Count == 0)
            {
                this.dateOfBirthDictionary.Remove(dateOfBirthKey);
            }
        }

        private void InitializeDictionaries()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            long numberOfRecordInFile = this.fileStream.Length / BytesInRecord;
            short isDeleted;
            long startOfRecordByte;

            while (numberOfRecordInFile > 0)
            {
                startOfRecordByte = this.fileStream.Position;
                byte[] array = new byte[2];
                this.fileStream.Read(array, 0, array.Length);
                isDeleted = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

                if (isDeleted == 1)
                {
                    this.fileStream.Seek(276, SeekOrigin.Current);
                    numberOfRecordInFile--;
                    continue;
                }
                else
                {
                    this.fileStream.Seek(4, SeekOrigin.Current);
                }

                Array.Resize(ref array, 120);
                this.fileStream.Read(array, 0, array.Length);
                string firstName = Encoding.Default.GetString(array).Trim(default(char));
                AddToDictionary(this.firstNameDictionary, firstName, startOfRecordByte);

                Array.Clear(array);
                this.fileStream.Read(array, 0, array.Length);
                string lastName = Encoding.Default.GetString(array).Trim(default(char));
                AddToDictionary(this.lastNameDictionary, lastName, startOfRecordByte);

                Array.Resize(ref array, 4);
                this.fileStream.Read(array, 0, array.Length);
                int day = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                this.fileStream.Read(array, 0, array.Length);
                int month = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                this.fileStream.Read(array, 0, array.Length);
                int year = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
                DateTime dateOfBirth = new DateTime(year, month, day);
                AddToDictionary(this.dateOfBirthDictionary, dateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), startOfRecordByte);
                this.fileStream.Seek(20, SeekOrigin.Current);

                numberOfRecordInFile--;
            }
        }

        private (FileCabinetRecord, bool) ReadRecordFromFile(long firstByteOfRecord)
        {
            FileCabinetRecord recordsFromFileSystem = new ();
            bool isRemoved = false;
            this.fileStream.Seek(firstByteOfRecord, SeekOrigin.Begin);
            byte[] array = new byte[2];
            this.fileStream.Read(array, 0, array.Length);
            int statusOfRecord = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            if (statusOfRecord == 1)
            {
                isRemoved = true;
            }

            Array.Resize(ref array, 4);
            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.Id = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            Array.Resize(ref array, 120);
            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.FirstName = Encoding.Default.GetString(array).Trim(default(char));

            Array.Clear(array);
            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.LastName = Encoding.Default.GetString(array).Trim(default(char));

            Array.Resize(ref array, 4);
            this.fileStream.Read(array, 0, array.Length);
            int day = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            this.fileStream.Read(array, 0, array.Length);
            int month = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            this.fileStream.Read(array, 0, array.Length);
            int year = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            recordsFromFileSystem.DateOfBirth = new DateTime(year, month, day);

            Array.Resize(ref array, 2);
            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.SerieOfPassNumber = char.Parse(Encoding.Default.GetString(array).Trim(default(char)));

            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.PassNumber = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            Array.Resize(ref array, 16);
            this.fileStream.Read(array, 0, array.Length);
            recordsFromFileSystem.BankAccount = decimal.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            return (recordsFromFileSystem, isRemoved);
        }

        private void WriteRecordToFile(FileCabinetRecord newRecord, short statusOfRecord)
        {
            byte[] input = Encoding.Default.GetBytes(statusOfRecord.ToString(CultureInfo.InvariantCulture));
            Array.Resize(ref input, 2);
            this.fileStream.Write(input, 0, input.Length);

            input = Encoding.Default.GetBytes(newRecord.Id.ToString(CultureInfo.InvariantCulture));
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
            this.fileStream.Flush();
        }
    }
}
