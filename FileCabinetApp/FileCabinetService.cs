using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to create, storage, edit, find and display records about users.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> usersRecords = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly IRecordValidator recordValidator;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetService" /> class.</summary>
        /// <param name="recordValidator">The record validator.</param>
        protected FileCabinetService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        /// <summary>Creates the new record and adds to list.</summary>
        /// <returns>
        /// The Id number of the new record.
        /// </returns>
        public int CreateRecord()
        {
            FileCabinetRecord newRecord = this.recordValidator.ValidateParameters();

            newRecord.Id = this.usersRecords.Count + 1;

            this.usersRecords.Add(newRecord);

            if (newRecord.FirstName != null)
            {
                AddToDictionary(this.firstNameDictionary, newRecord.FirstName, newRecord);
            }

            if (newRecord.LastName != null)
            {
                AddToDictionary(this.lastNameDictionary, newRecord.LastName, newRecord);
            }

            AddToDictionary(this.dateOfBirthDictionary, newRecord.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), newRecord);

            return newRecord.Id;
        }

        /// <summary>Gets all records which created.</summary>
        /// <returns>
        /// The list of created records at the present time.
        /// </returns>
        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] usersRecordsCopy = new FileCabinetRecord[this.usersRecords.Count];

            for (int i = 0; i < this.usersRecords.Count; i++)
            {
                FileCabinetRecord recordCopy = new (this.usersRecords[i].Id, this.usersRecords[i].FirstName, this.usersRecords[i].LastName, this.usersRecords[i].DateOfBirth, this.usersRecords[i].SerieOfPassNumber, this.usersRecords[i].PassNumber, this.usersRecords[i].BankAccount);
                usersRecordsCopy[i] = recordCopy;
            }

            return usersRecordsCopy;
        }

        /// <summary>Gets the count of created records.</summary>
        /// <returns>
        /// The count of created records.
        /// </returns>
        public int GetStat()
        {
            return this.usersRecords.Count;
        }

        /// <summary>Edits the exist record by Id number.</summary>
        /// <param name="editRecordId">The id of record for edit.</param>
        public void EditRecord(int editRecordId)
        {
            int recordIndex = editRecordId - 1;
            FileCabinetRecord editRecord = this.recordValidator.ValidateParameters();
            editRecord.Id = editRecordId;

            string firstNameBeforeEdit = this.usersRecords[recordIndex].FirstName;
            this.usersRecords[recordIndex].FirstName = editRecord.FirstName;
            EditDictionary(this.firstNameDictionary, firstNameBeforeEdit, editRecord.FirstName, editRecord.Id);

            string? lastNameBeforeEdit = this.usersRecords[recordIndex].LastName;
            this.usersRecords[recordIndex].LastName = editRecord.LastName;
            EditDictionary(this.lastNameDictionary, lastNameBeforeEdit, editRecord.LastName, editRecord.Id);

            DateTime dateOfBirthBeforeEdit = this.usersRecords[recordIndex].DateOfBirth;
            this.usersRecords[recordIndex].DateOfBirth = editRecord.DateOfBirth;
            EditDictionary(this.dateOfBirthDictionary, dateOfBirthBeforeEdit.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), editRecord.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), editRecord.Id);

            this.usersRecords[recordIndex].PassNumber = editRecord.PassNumber;
            this.usersRecords[recordIndex].SerieOfPassNumber = editRecord.SerieOfPassNumber;
            this.usersRecords[recordIndex].BankAccount = editRecord.BankAccount;
            Console.WriteLine($"Record #{editRecord.Id} is updated");
        }

        /// <summary>Finds the records by first name.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// The list of records which consist of this first name.
        /// </returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new ();

            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                result = this.firstNameDictionary[firstName.ToUpperInvariant()];
            }

            return result.ToArray();
        }

        /// <summary>Finds the records by last name.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// The list of records which consist of this last name.
        /// </returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new ();
            foreach (FileCabinetRecord currentRecord in this.usersRecords)
            {
                if (currentRecord.LastName != null && currentRecord.LastName.ToUpperInvariant() == lastName.ToUpperInvariant())
                {
                    result.Add(currentRecord);
                }
            }

            return result.ToArray();
        }

        /// <summary>Finds the records by birth day.</summary>
        /// <param name="birthDayParameter">The date parameter.</param>
        /// <returns>
        /// The list of records which consist of this birth date.
        /// </returns>
        public FileCabinetRecord[] FindByDayOfBirth(string birthDayParameter)
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

                return result.ToArray();
            }

            Console.WriteLine("Convert error. Format date of birth parameter: \"Year - Month - Day\" ");
            return result.ToArray();
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

        private static void EditDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string oldParameter, string newParameter, int sourceId)
        {
            oldParameter = oldParameter.ToUpperInvariant();
            newParameter = newParameter.ToUpperInvariant();

            if (newParameter != null && oldParameter != newParameter)
            {
                FileCabinetRecord recordForMove = new ();

                for (int i = 0; i < dictionary[oldParameter].Count; i++)
                {
                    if (dictionary[oldParameter][i].Id == sourceId)
                    {
                        recordForMove = dictionary[oldParameter][i];
                        dictionary[oldParameter].RemoveAt(i);
                        if (dictionary[oldParameter].Count == 0)
                        {
                            dictionary.Remove(oldParameter);
                        }

                        break;
                    }
                }

                if (dictionary.ContainsKey(newParameter))
                {
                    dictionary[newParameter].Add(recordForMove);
                }
                else
                {
                    dictionary.Add(newParameter, new List<FileCabinetRecord>() { recordForMove });
                }
            }
        }
    }
}