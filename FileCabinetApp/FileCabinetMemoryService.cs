﻿using System.Collections.ObjectModel;
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

        /// <summary>
        /// Removes the record from current repositiry.
        /// </summary>
        /// <param name="recordId"> The id if record for remove.</param>
        public void RemoveRecord(int recordId)
        {
            int indexOfRemoveRecord = -1;

            for (int i = 0; i < this.usersRecords.Count; i++)
            {
                if (this.usersRecords[i].Id == recordId)
                {
                    indexOfRemoveRecord = i;
                    break;
                }
            }

            if (indexOfRemoveRecord == -1)
            {
                Console.WriteLine($"Record #{recordId} doesn't exists");
                return;
            }

            RemoveFromDictionary(this.firstNameDictionary, this.usersRecords[indexOfRemoveRecord].FirstName, recordId);
            RemoveFromDictionary(this.lastNameDictionary, this.usersRecords[indexOfRemoveRecord].LastName, recordId);
            RemoveFromDictionary(this.dateOfBirthDictionary, this.usersRecords[indexOfRemoveRecord].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), recordId);
            this.usersRecords.RemoveAt(indexOfRemoveRecord);
            Console.WriteLine($"Record #{recordId} is removed.");
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
        }

        /// <summary>Edits the exist record by Id number.</summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited paremeters of record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            int recordIndex = editRecordId - 1;
            editedRecord.Id = editRecordId;

            RemoveFromDictionary(this.firstNameDictionary, this.usersRecords[recordIndex].FirstName, editedRecord.Id);
            AddToDictionary(this.firstNameDictionary, editedRecord.FirstName, editedRecord);

            RemoveFromDictionary(this.lastNameDictionary, this.usersRecords[recordIndex].LastName, editedRecord.Id);
            AddToDictionary(this.lastNameDictionary, editedRecord.LastName, editedRecord);

            RemoveFromDictionary(this.dateOfBirthDictionary, this.usersRecords[recordIndex].DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), editedRecord.Id);
            AddToDictionary(this.dateOfBirthDictionary, editedRecord.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), editedRecord);

            this.usersRecords[recordIndex] = editedRecord;
            Console.WriteLine($"Record #{editedRecord.Id} is updated");
        }

        /// <summary>Finds the records by first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The list of records which consist of this first name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> recordsByKey = new ();

            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                recordsByKey = this.firstNameDictionary[firstName.ToUpperInvariant()];
            }

            return recordsByKey;
        }

        /// <summary>Finds the records by last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The list of records which consist of this last name.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> recordsByKey = new ();

            if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                recordsByKey = this.lastNameDictionary[lastName.ToUpperInvariant()];
            }

            return recordsByKey;
        }

        /// <summary>Finds the records by birth day.</summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            List<FileCabinetRecord> recordsByKey = new ();
            bool isDateTime = DateTime.TryParse(birthDayParameter, out DateTime dayOfBirth);

            if (!isDateTime)
            {
                Console.WriteLine("Convert error. Format date of birth parameter: \"Year - Month - Day\" ");
                return recordsByKey;
            }

            string correctFormatOfParameter = dayOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).ToUpperInvariant();
            if (this.dateOfBirthDictionary.ContainsKey(correctFormatOfParameter.ToUpperInvariant()))
            {
                recordsByKey = this.dateOfBirthDictionary[correctFormatOfParameter];
            }

            return recordsByKey;
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
    }
}