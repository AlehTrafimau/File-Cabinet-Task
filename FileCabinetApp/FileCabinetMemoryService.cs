using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to create, storage, edit, find and display records about users.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> usersRecords = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private Dictionary<string, List<FileCabinetRecord>> selectedRecords = new ();

        /// <summary>Creates the new record and adds to list.</summary>
        /// <param name="newRecord">The new record.</param>
        /// <returns>The Id number of the new record.</returns>
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            newRecord.Id = this.usersRecords.Count > 0 ? this.usersRecords[^1].Id + 1 : 1;

            this.usersRecords.Add(newRecord);

            AddToDictionary(this.firstNameDictionary, newRecord.FirstName, newRecord);
            AddToDictionary(this.lastNameDictionary, newRecord.LastName, newRecord);
            AddToDictionary(this.dateOfBirthDictionary, newRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), newRecord);

            return newRecord.Id;
        }

        /// <summary>Gets all records which created.</summary>
        /// <returns>
        /// The list of created records at the present time.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> usersRecordsCopy = new ();

            for (int i = 0; i < this.usersRecords.Count; i++)
            {
                FileCabinetRecord recordCopy = new (this.usersRecords[i].Id, this.usersRecords[i].FirstName, this.usersRecords[i].LastName, this.usersRecords[i].DateOfBirth, this.usersRecords[i].SerieOfPassNumber, this.usersRecords[i].PassNumber, this.usersRecords[i].BankAccount);
                usersRecordsCopy.Add(recordCopy);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(usersRecordsCopy);
        }

        /// <summary>Gets the count of created records.</summary>
        /// <returns>
        /// The count of created records.
        /// </returns>
        public (int, int) GetStat()
        {
            (int NumberOfRecords, int removedRecords) stat = new (this.usersRecords.Count, 0);
            return stat;
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// The snapshot of current state of the cabinet service.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot newSnapshot = new (this.usersRecords.ToArray());
            return newSnapshot;
        }

        /// <summary>
        /// Restores records from file system to this list of users records.
        /// </summary>
        /// <param name="snapshot"> The snapshot of import records.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> newRecords = snapshot.Records;

            for (int i = 0; i < newRecords.Count; i++)
            {
                int lastElementId = this.usersRecords.Count > 0 ? this.usersRecords[^1].Id : 0;
                if (newRecords[i].Id > lastElementId)
                {
                    newRecords[i].Id = lastElementId + 1;
                    this.usersRecords.Add(newRecords[i]);

                    AddToDictionary(this.firstNameDictionary, newRecords[i].FirstName, newRecords[i]);
                    AddToDictionary(this.lastNameDictionary, newRecords[i].LastName, newRecords[i]);
                    AddToDictionary(this.dateOfBirthDictionary, newRecords[i].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), newRecords[i]);
                }
                else
                {
                    int indexOfRecord = newRecords[i].Id - 1;
                    RemoveFromDictionary(this.firstNameDictionary, this.usersRecords[indexOfRecord].FirstName, newRecords[i].Id);
                    AddToDictionary(this.firstNameDictionary, newRecords[i].FirstName, newRecords[i]);

                    RemoveFromDictionary(this.lastNameDictionary, this.usersRecords[indexOfRecord].LastName, newRecords[i].Id);
                    AddToDictionary(this.lastNameDictionary, newRecords[i].LastName, newRecords[i]);

                    RemoveFromDictionary(this.dateOfBirthDictionary, this.usersRecords[indexOfRecord].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), newRecords[i].Id);
                    AddToDictionary(this.dateOfBirthDictionary, newRecords[i].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), newRecords[i]);

                    this.usersRecords[indexOfRecord] = newRecords[i];
                }
            }
        }

        /// <summary>
        /// Inserts the new record to the current storage.
        /// </summary>
        /// <param name="insertRecord">The record for insert.</param>
        public void InsertRecord(FileCabinetRecord insertRecord)
        {
            if (this.usersRecords.Count >= insertRecord.Id)
            {
                this.usersRecords.Insert(insertRecord.Id - 1, insertRecord);
                AddToDictionary(this.firstNameDictionary, insertRecord.FirstName, insertRecord);
                AddToDictionary(this.lastNameDictionary, insertRecord.LastName, insertRecord);
                AddToDictionary(this.dateOfBirthDictionary, insertRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), insertRecord);
                for (int i = insertRecord.Id; i < this.usersRecords.Count; i++)
                {
                    this.usersRecords[i].Id += 1;
                }
            }
            else
            {
                this.CreateRecord(insertRecord);
            }

            this.selectedRecords.Clear();
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

            void DeleteById(params int[] recordsId)
            {
                foreach (var id in recordsId)
                {
                    int indexOfRemoveRecord = -1;

                    for (int i = 0; i < this.usersRecords.Count; i++)
                    {
                        if (this.usersRecords[i].Id == id)
                        {
                            indexOfRemoveRecord = i;
                            break;
                        }
                    }

                    if (indexOfRemoveRecord == -1)
                    {
                        Console.WriteLine($"Record #{id} doesn't exists");
                        return;
                    }

                    RemoveFromDictionary(this.firstNameDictionary, this.usersRecords[indexOfRemoveRecord].FirstName, id);
                    RemoveFromDictionary(this.lastNameDictionary, this.usersRecords[indexOfRemoveRecord].LastName, id);
                    RemoveFromDictionary(this.dateOfBirthDictionary, this.usersRecords[indexOfRemoveRecord].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), id);
                    this.usersRecords.RemoveAt(indexOfRemoveRecord);
                }

                this.selectedRecords.Clear();
            }

            static void PrintMessage(int[] recordsId)
            {
                string resultMessage = "Record ";
                foreach (var i in recordsId)
                {
                    resultMessage += $"#{i} ";
                }

                Console.WriteLine(resultMessage + "are deleted");
            }

            switch (fieldName)
            {
                case "ID":
                    bool isDigit = int.TryParse(value, out int valueOfId);
                    if (isDigit)
                    {
                        DeleteById(valueOfId);
                        Console.WriteLine($"Record #{valueOfId} is deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Id value must be a digit");
                    }

                    break;
                case "FIRSTNAME":
                    if (this.firstNameDictionary.ContainsKey(value.ToUpperInvariant()))
                    {
                        int[] result = this.firstNameDictionary[value.ToUpperInvariant()].Select(record => record.Id).ToArray<int>();
                        DeleteById(result);
                        PrintMessage(result);
                    }
                    else
                    {
                        Console.WriteLine("Records with this first name are not found");
                    }

                    break;
                case "LASTNAME":
                    if (this.lastNameDictionary.ContainsKey(value.ToUpperInvariant()))
                    {
                        int[] result = this.lastNameDictionary[value.ToUpperInvariant()].Select(record => record.Id).ToArray<int>();
                        DeleteById(result);
                        PrintMessage(result);
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
                        int[] result = this.dateOfBirthDictionary[dateOfBirth].Select(record => record.Id).ToArray<int>();
                        DeleteById(result);
                        PrintMessage(result);
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
                        int[] recordsId = this.usersRecords.Where(record => record.SerieOfPassNumber == serieOfPassnumber.Item3).Select(person => person.Id).ToArray<int>();
                        DeleteById(recordsId);
                        PrintMessage(recordsId);
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
                        int[] recordsId = this.usersRecords.Where(record => record.PassNumber == passNumber.Item3).Select(person => person.Id).ToArray<int>();
                        DeleteById(recordsId);
                        PrintMessage(recordsId);
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
                        int[] recordsId = this.usersRecords.Where(record => record.PassNumber == bankAccount.Item3).Select(person => person.Id).ToArray();
                        DeleteById(recordsId);
                        PrintMessage(recordsId);
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
            FileCabinetRecord[] findResult = Array.Empty<FileCabinetRecord>();
            findResult = findConditions.Id != default ? this.usersRecords.Where(record => record.Id == findConditions.Id).ToArray() : findResult;
            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.FirstName != string.Empty ? this.usersRecords.Where(record => record.FirstName.ToUpperInvariant() == findConditions.FirstName.ToUpperInvariant()).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.FirstName != string.Empty ? findResult.Where(record => record.FirstName.ToUpperInvariant() == findConditions.FirstName.ToUpperInvariant()).ToArray() : findResult;
            }

            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.LastName != string.Empty ? this.usersRecords.Where(record => record.LastName.ToUpperInvariant() == findConditions.LastName.ToUpperInvariant()).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.LastName != string.Empty ? findResult.Where(record => record.LastName.ToUpperInvariant() == findConditions.LastName.ToUpperInvariant()).ToArray() : findResult;
            }

            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.DateOfBirth != default ? this.usersRecords.Where(record => record.DateOfBirth == findConditions.DateOfBirth).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.DateOfBirth != default ? findResult.Where(record => record.DateOfBirth == findConditions.DateOfBirth).ToArray() : findResult;
            }

            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.SerieOfPassNumber != default ? this.usersRecords.Where(record => record.SerieOfPassNumber == findConditions.SerieOfPassNumber).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.SerieOfPassNumber != default ? findResult.Where(record => record.SerieOfPassNumber == findConditions.SerieOfPassNumber).ToArray() : findResult;
            }

            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.PassNumber != default ? this.usersRecords.Where(record => record.PassNumber == findConditions.PassNumber).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.PassNumber != default ? findResult.Where(record => record.PassNumber == findConditions.PassNumber).ToArray() : findResult;
            }

            if (findResult == Array.Empty<FileCabinetRecord>())
            {
                findResult = findConditions.BankAccount != default ? this.usersRecords.Where(record => record.BankAccount == findConditions.BankAccount).ToArray() : findResult;
            }
            else
            {
                findResult = findConditions.BankAccount != default ? findResult.Where(record => record.BankAccount == findConditions.BankAccount).ToArray() : findResult;
            }

            int[] selectedRecordsId = findResult != Array.Empty<FileCabinetRecord>() ? findResult.Select(record => record.Id).ToArray() : Array.Empty<int>();

            if (selectedRecordsId != Array.Empty<int>())
            {
                foreach (var i in selectedRecordsId)
                {
                    if (newParameters.FirstName != string.Empty)
                    {
                        this.usersRecords[i - 1].FirstName = newParameters.FirstName;
                    }

                    if (newParameters.LastName != string.Empty)
                    {
                        this.usersRecords[i - 1].LastName = newParameters.LastName;
                    }

                    if (newParameters.DateOfBirth != default)
                    {
                        this.usersRecords[i - 1].DateOfBirth = newParameters.DateOfBirth;
                    }

                    if (newParameters.SerieOfPassNumber != default)
                    {
                        this.usersRecords[i - 1].SerieOfPassNumber = newParameters.SerieOfPassNumber;
                    }

                    if (newParameters.PassNumber != default)
                    {
                        this.usersRecords[i - 1].PassNumber = newParameters.PassNumber;
                    }

                    if (newParameters.BankAccount != default)
                    {
                        this.usersRecords[i - 1].BankAccount = newParameters.BankAccount;
                    }
                }

                this.selectedRecords.Clear();
            }
        }

        /// <summary>
        /// Select record from current storage by input parameters.
        /// </summary>
        /// <param name="fieldsOfRecordForSelect">The list of fields with values for select.</param>
        /// <param name="fieldsOfRecordsForDisplay">The list of necessary fields for display of selected records.</param>
        /// <param name="orderOfSelect">The definer of a order of select records, 'or' or 'and'.</param>
        public void SelectRecords(List<Tuple<string, string>> fieldsOfRecordForSelect, string[] fieldsOfRecordsForDisplay, string orderOfSelect)
        {
            List<FileCabinetRecord> selectedRecordsByParameters = this.GetSelectedRecords(fieldsOfRecordForSelect, orderOfSelect);
            if (selectedRecordsByParameters.Count != 0)
            {
                SelectPrinter.Printer(selectedRecordsByParameters, fieldsOfRecordsForDisplay);
                return;
            }

            if (fieldsOfRecordForSelect.Count == 0)
            {
                selectedRecordsByParameters = this.usersRecords;
            }
            else if (orderOfSelect == "AND" || orderOfSelect == string.Empty)
            {
                FileCabinetRecord[] records = this.usersRecords.ToArray<FileCabinetRecord>();
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
                    records = RecordsSearcher.FindRecords(i, this.usersRecords.ToArray<FileCabinetRecord>());
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

            this.MemoizeSelectedRecords(fieldsOfRecordForSelect, orderOfSelect, selectedRecordsByParameters);
            SelectPrinter.Printer(selectedRecordsByParameters, fieldsOfRecordsForDisplay);
        }

        private static void AddToDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string parameter, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(parameter.ToUpperInvariant()))
            {
                dictionary[parameter.ToUpperInvariant()].Add(record);
            }
            else
            {
                dictionary.Add(parameter.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }
        }

        private static void RemoveFromDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string key, int recordId)
        {
            key = key.ToUpperInvariant();
            if (!dictionary.ContainsKey(key))
            {
                return;
            }

            for (int i = 0; i < dictionary[key].Count; i++)
            {
                if (dictionary[key][i].Id == recordId)
                {
                    dictionary[key].RemoveAt(i);
                    if (dictionary[key].Count == 0)
                    {
                        dictionary.Remove(key);
                    }

                    break;
                }
            }
        }

        private static string KeyGenerate(List<Tuple<string, string>> fieldsOfRecordForSelect, string orderOfSelect)
        {
            List<string> key = new ();
            foreach (var i in fieldsOfRecordForSelect)
            {
                key.Add($"{i.Item1}:{i.Item2}");
            }

            string keyOf = string.Join('|', key);
            if (orderOfSelect == "or" || orderOfSelect == "and")
            {
                keyOf += orderOfSelect == "or" ? "or" : "and";
            }

            return keyOf;
        }

        private void MemoizeSelectedRecords(List<Tuple<string, string>> fieldsOfRecordForSelect, string orderOfSelect, List<FileCabinetRecord> selectedRecords)
        {
            string key = KeyGenerate(fieldsOfRecordForSelect, orderOfSelect);
            this.selectedRecords.Add(key, selectedRecords);
        }

        private List<FileCabinetRecord> GetSelectedRecords(List<Tuple<string, string>> fieldsOfRecordForSelect, string orderOfSelect)
        {
            List<FileCabinetRecord> selectedRecords = new ();
            string key = KeyGenerate(fieldsOfRecordForSelect, orderOfSelect);
            if (this.selectedRecords.ContainsKey(key))
            {
                selectedRecords = this.selectedRecords[key];
            }

            return selectedRecords;
        }
    }
}