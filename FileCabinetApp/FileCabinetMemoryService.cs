using System.Collections.ObjectModel;
using System.Globalization;

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
            newRecord.Id = this.usersRecords.Count + 1;

            this.usersRecords.Add(newRecord);

            AddToDictionary(this.firstNameDictionary, newRecord.FirstName, newRecord);
            AddToDictionary(this.lastNameDictionary, newRecord.LastName, newRecord);
            AddToDictionary(this.dateOfBirthDictionary, newRecord.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), newRecord);

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
        public int GetStat()
        {
            return this.usersRecords.Count;
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
                int lastElementId = this.usersRecords.Count;
                if (newRecords[i].Id > lastElementId)
                {
                    newRecords[i].Id = lastElementId + 1;
                    this.usersRecords.Add(newRecords[i]);

                    AddToDictionary(this.firstNameDictionary, newRecords[i].FirstName, newRecords[i]);
                    AddToDictionary(this.lastNameDictionary, newRecords[i].LastName, newRecords[i]);
                    AddToDictionary(this.dateOfBirthDictionary, newRecords[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), newRecords[i]);
                }
                else
                {
                    int indexOfRecord = newRecords[i].Id - 1;
                    EditDictionary(this.firstNameDictionary, this.usersRecords[indexOfRecord].FirstName, newRecords[i].FirstName, newRecords[i], newRecords[i].Id);
                    EditDictionary(this.lastNameDictionary, this.usersRecords[indexOfRecord].LastName, newRecords[i].LastName, newRecords[i], newRecords[i].Id);
                    EditDictionary(this.dateOfBirthDictionary, this.usersRecords[indexOfRecord].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), newRecords[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), newRecords[i], newRecords[i].Id);
                    this.usersRecords[indexOfRecord] = newRecords[i];
                }
            }
        }

        /// <summary>Edits the exist record by Id number.</summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        /// <param name="editedRecord">The edited paremeters of record.</param>
        public void EditRecord(int editRecordId, FileCabinetRecord editedRecord)
        {
            int recordIndex = editRecordId - 1;
            editedRecord.Id = editRecordId;

            string firstNameBeforeEdit = this.usersRecords[recordIndex].FirstName;
            this.usersRecords[recordIndex].FirstName = editedRecord.FirstName;
            EditDictionary(this.firstNameDictionary, firstNameBeforeEdit, editedRecord.FirstName, editedRecord, editedRecord.Id);

            string? lastNameBeforeEdit = this.usersRecords[recordIndex].LastName;
            this.usersRecords[recordIndex].LastName = editedRecord.LastName;
            EditDictionary(this.lastNameDictionary, lastNameBeforeEdit, editedRecord.LastName, editedRecord, editedRecord.Id);

            DateTime dateOfBirthBeforeEdit = this.usersRecords[recordIndex].DateOfBirth;
            this.usersRecords[recordIndex].DateOfBirth = editedRecord.DateOfBirth;
            EditDictionary(this.dateOfBirthDictionary, dateOfBirthBeforeEdit.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), editedRecord.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), editedRecord, editedRecord.Id);

            this.usersRecords[recordIndex].PassNumber = editedRecord.PassNumber;
            this.usersRecords[recordIndex].SerieOfPassNumber = editedRecord.SerieOfPassNumber;
            this.usersRecords[recordIndex].BankAccount = editedRecord.BankAccount;
            Console.WriteLine($"Record #{editedRecord.Id} is updated");
        }

        /// <summary>Finds the records by first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The list of records which consist of this first name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new ();

            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                result = this.firstNameDictionary[firstName.ToUpperInvariant()];
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>Finds the records by last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The list of records which consist of this last name.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new ();
            foreach (FileCabinetRecord currentRecord in this.usersRecords)
            {
                if (currentRecord.LastName != null && currentRecord.LastName.ToUpperInvariant() == lastName.ToUpperInvariant())
                {
                    result.Add(currentRecord);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>Finds the records by birth day.</summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDayOfBirth(string birthDayParameter)
        {
            List<FileCabinetRecord> result = new ();
            bool isDateTime = DateTime.TryParse(birthDayParameter, out DateTime dayOfBirth);

            if (isDateTime)
            {
                foreach (FileCabinetRecord currentRecord in this.usersRecords)
                {
                    if (currentRecord.DateOfBirth == dayOfBirth)
                    {
                        result.Add(currentRecord);
                    }
                }

                return new ReadOnlyCollection<FileCabinetRecord>(result);
            }

            Console.WriteLine("Convert error. Format date of birth parameter: \"Year - Month - Day\" ");
            return new ReadOnlyCollection<FileCabinetRecord>(result);
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

        private static void EditDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string oldKey, string newKey, FileCabinetRecord newRecord, int sourceId)
        {
            oldKey = oldKey.ToUpperInvariant();
            newKey = newKey.ToUpperInvariant();

            for (int i = 0; i < dictionary[oldKey].Count; i++)
            {
                if (dictionary[oldKey][i].Id == sourceId)
                {
                    dictionary[oldKey].RemoveAt(i);
                    if (dictionary[oldKey].Count == 0)
                    {
                        dictionary.Remove(oldKey);
                    }

                    break;
                }
            }

            AddToDictionary(dictionary, newKey, newRecord);
        }
    }
}