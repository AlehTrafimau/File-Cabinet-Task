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
        private Dictionary<string, List<long>> firstNameDictionary = new ();
        private Dictionary<string, List<long>> lastNameDictionary = new ();
        private Dictionary<string, List<long>> dateOfBirthDictionary = new ();
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
        /// Restores records from file system to the current repository.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> newRecords = snapshot.Records;

            for (int i = 0; i < newRecords.Count; i++)
            {
                long position = (newRecords[i].Id - 1) * BytesInRecord;
                this.fileStream.Seek(position, SeekOrigin.Begin);
                this.WriteRecordToFile(newRecords[i], 0);
            }

            this.InitializeDictionaries();
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

        /// <summary>
        /// Deletes record from current storage by input conditions.
        /// </summary>
        /// <param name="fieldName">The name of record field.</param>
        /// <param name="value">The value of record field.</param>
        public void Delete(string fieldName, string value)
        {
            if (fieldName == null || value == null)
            {
                Console.WriteLine("Parameters are invalid");
                return;
            }

            void DeleteById(params long[] recordsPosition)
            {
                foreach (var position in recordsPosition)
                {
                    int isDeleted = 1;
                    this.fileStream.Seek(position, SeekOrigin.Begin);
                    byte[] input = Encoding.Default.GetBytes(isDeleted.ToString(CultureInfo.InvariantCulture));
                    Array.Resize(ref input, 2);
                    this.fileStream.Write(input, 0, input.Length);
                    this.fileStream.Flush();
                }
            }

            static void PrintMessage(long[] recordsId)
            {
                string resultMessage = "Record ";
                foreach (var i in recordsId)
                {
                    resultMessage += $"#{(i / BytesInRecord) + 1} ";
                }

                Console.WriteLine(resultMessage + "are deleted");
            }

            switch (fieldName)
            {
                case "ID":
                    bool isDigit = int.TryParse(value, out int valueOfId);
                    if (isDigit)
                    {
                        DeleteById((valueOfId - 1) * BytesInRecord);
                        Console.WriteLine($"Record #{valueOfId} is deleted.");
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine("Id value must be a digit");
                    }

                    break;
                case "FIRSTNAME":
                    if (this.firstNameDictionary.ContainsKey(value.ToUpperInvariant()))
                    {
                        long[] result = this.firstNameDictionary[value.ToUpperInvariant()].ToArray();
                        DeleteById(result);
                        PrintMessage(result);
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine("Records with this first name are not found");
                    }

                    break;
                case "LASTNAME":
                    if (this.lastNameDictionary.ContainsKey(value.ToUpperInvariant()))
                    {
                        long[] result = this.lastNameDictionary[value.ToUpperInvariant()].ToArray<long>();
                        DeleteById(result);
                        PrintMessage(result);
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine("Records with this last name are not found");
                    }

                    break;
                case "DATEOFBIRTH":
                    Tuple<bool, string, DateTime> date = StringConverter.DateTimeConvert(value);
                    string dateOfBirth = " ";
                    if (date.Item1)
                    {
                        dateOfBirth = date.Item3.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
                    {
                        long[] result = this.dateOfBirthDictionary[dateOfBirth].ToArray<long>();
                        DeleteById(result);
                        PrintMessage(result);
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine("Records with this field are not found");
                    }

                    break;
                case "SERIEOFPASSNUMBER":
                    Tuple<bool, string, char> serieOfPassnumber = StringConverter.CharConvert(value);
                    if (serieOfPassnumber.Item1)
                    {
                        this.fileStream.Seek(0, SeekOrigin.Begin);
                        List<long> mesage = new ();
                        while (this.fileStream.Position != this.fileStream.Length)
                        {
                            long curretnPosition = this.fileStream.Position;
                            (FileCabinetRecord, bool) record = this.ReadRecordFromFile(curretnPosition);
                            if (!record.Item2 && record.Item1.SerieOfPassNumber == serieOfPassnumber.Item3)
                            {
                                this.fileStream.Seek(curretnPosition, SeekOrigin.Begin);
                                mesage.Add(curretnPosition);
                                this.WriteRecordToFile(record.Item1, 1);
                            }
                        }

                        PrintMessage(mesage.ToArray<long>());
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine(serieOfPassnumber.Item2);
                    }

                    break;
                case "PASSNUMBER":
                    Tuple<bool, string, int> passNumber = StringConverter.IntegerConvert(value);
                    if (passNumber.Item1)
                    {
                        this.fileStream.Seek(0, SeekOrigin.Begin);
                        List<long> mesage = new ();
                        while (this.fileStream.Position != this.fileStream.Length)
                        {
                            long curretnPosition = this.fileStream.Position;
                            (FileCabinetRecord, bool) record = this.ReadRecordFromFile(curretnPosition);
                            if (!record.Item2 && record.Item1.PassNumber == passNumber.Item3)
                            {
                                this.fileStream.Seek(curretnPosition, SeekOrigin.Begin);
                                mesage.Add(curretnPosition);
                                this.WriteRecordToFile(record.Item1, 1);
                            }
                        }

                        PrintMessage(mesage.ToArray<long>());
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine(passNumber.Item2);
                    }

                    break;
                case "BANKACCOUNT":
                    Tuple<bool, string, decimal> bankAccount = StringConverter.DecimalConvert(value);
                    if (bankAccount.Item1)
                    {
                        this.fileStream.Seek(0, SeekOrigin.Begin);
                        List<long> mesage = new ();
                        while (this.fileStream.Position != this.fileStream.Length)
                        {
                            long curretnPosition = this.fileStream.Position;
                            (FileCabinetRecord, bool) record = this.ReadRecordFromFile(curretnPosition);
                            if (!record.Item2 && record.Item1.BankAccount == bankAccount.Item3)
                            {
                                this.fileStream.Seek(curretnPosition, SeekOrigin.Begin);
                                mesage.Add(curretnPosition);
                                this.WriteRecordToFile(record.Item1, 1);
                            }
                        }

                        PrintMessage(mesage.ToArray<long>());
                        this.InitializeDictionaries();
                    }
                    else
                    {
                        Console.WriteLine(bankAccount.Item2);
                    }

                    break;
                default:
                    Console.WriteLine($"The record haven't got this field {fieldName}");
                    break;
            }
        }

        /// <summary>
        /// Updates record from current storage by input parameters.
        /// </summary>
        /// <param name="newParameters">The record which consist of new parameters.</param>
        /// <param name="findConditions">The record which consist of fields as find conditions.</param>
        public void Update(FileCabinetRecord newParameters, FileCabinetRecord findConditions)
        {
            bool IsFindRecord(FileCabinetRecord record)
            {
                List<bool> result = new ();
                if (findConditions.Id != default)
                {
                    result.Add(record.Id == findConditions.Id);
                }

                if (findConditions.FirstName != string.Empty)
                {
                    result.Add(record.FirstName.ToUpperInvariant() == findConditions.FirstName.ToUpperInvariant());
                }

                if (findConditions.LastName != string.Empty)
                {
                    result.Add(record.LastName.ToUpperInvariant() == findConditions.LastName.ToUpperInvariant());
                }

                if (findConditions.SerieOfPassNumber != default)
                {
                    result.Add(record.SerieOfPassNumber == findConditions.SerieOfPassNumber);
                }

                if (findConditions.PassNumber != default)
                {
                    result.Add(record.PassNumber == findConditions.PassNumber);
                }

                if (findConditions.BankAccount != default)
                {
                    result.Add(record.BankAccount == findConditions.BankAccount);
                }

                if (result.Count == 0 || result.Contains(false))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            this.fileStream.Seek(0, SeekOrigin.Begin);
            while (this.fileStream.Position != this.fileStream.Length)
            {
                long curretnPosition = this.fileStream.Position;
                (FileCabinetRecord, bool) record = this.ReadRecordFromFile(curretnPosition);

                if (!record.Item2 && IsFindRecord(record.Item1))
                {
                    if (newParameters.FirstName != string.Empty)
                    {
                        record.Item1.FirstName = newParameters.FirstName;
                    }

                    if (newParameters.LastName != string.Empty)
                    {
                        record.Item1.LastName = newParameters.LastName;
                    }

                    if (newParameters.DateOfBirth != default)
                    {
                        record.Item1.DateOfBirth = newParameters.DateOfBirth;
                    }

                    if (newParameters.SerieOfPassNumber != default)
                    {
                        record.Item1.SerieOfPassNumber = newParameters.SerieOfPassNumber;
                    }

                    if (newParameters.PassNumber != default)
                    {
                        record.Item1.PassNumber = newParameters.PassNumber;
                    }

                    if (newParameters.BankAccount != default)
                    {
                        record.Item1.BankAccount = newParameters.BankAccount;
                    }

                    this.fileStream.Seek(curretnPosition, SeekOrigin.Begin);
                    this.WriteRecordToFile(record.Item1, 0);
                }
            }

            this.InitializeDictionaries();
        }

        /// <summary>
        /// Select record from current storage by input parameters.
        /// </summary>
        /// <param name="fieldsOfRecordForSelect">The list of fields with values for select.</param>
        /// <param name="fieldsOfRecordsForDisplay">The list of necessary fields for display of selected records.</param>
        /// <param name="orderOfSelect">The definer of a order of select records, 'or' or 'and'.</param>
        public void SelectRecords(List<Tuple<string, string>> fieldsOfRecordForSelect, string[] fieldsOfRecordsForDisplay, string orderOfSelect)
        {
            List<FileCabinetRecord> selectedRecordsByParameters = new ();

            if (fieldsOfRecordForSelect.Count == 0)
            {
                selectedRecordsByParameters = this.GetRecords().ToList<FileCabinetRecord>();
            }
            else if (orderOfSelect.ToUpperInvariant() == "AND" || orderOfSelect.ToUpperInvariant() == string.Empty)
            {
                FileCabinetRecord[] records = this.GetRecords().ToList<FileCabinetRecord>().ToArray<FileCabinetRecord>();
                foreach (var i in fieldsOfRecordForSelect)
                {
                    records = RecordsSearcher.FindRecords(i, records);
                }

                selectedRecordsByParameters.AddRange(records);
            }
            else if (orderOfSelect.ToUpperInvariant() == "OR")
            {
                FileCabinetRecord[] records = Array.Empty<FileCabinetRecord>();
                foreach (var i in fieldsOfRecordForSelect)
                {
                    records = RecordsSearcher.FindRecords(i, this.GetRecords().ToList<FileCabinetRecord>().ToArray<FileCabinetRecord>());
                    if (records != Array.Empty<FileCabinetRecord>())
                    {
                        break;
                    }
                }

                selectedRecordsByParameters.AddRange(records);
            }

            if (selectedRecordsByParameters.Count == 0)
            {
                Console.WriteLine("Records are not found");
                return;
            }

            SelectPrinter.Printer(selectedRecordsByParameters, fieldsOfRecordsForDisplay);
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
            if (recordId > this.fileStream.Length / BytesInRecord)
            {
                return;
            }

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

            Console.WriteLine($"Record #{recordId} is removed.");
        }

        private void InitializeDictionaries()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            long numberOfRecordInFile = this.fileStream.Length / BytesInRecord;
            short isDeleted;
            long startOfRecordByte;
            this.firstNameDictionary = new ();
            this.lastNameDictionary = new ();
            this.dateOfBirthDictionary = new ();

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
