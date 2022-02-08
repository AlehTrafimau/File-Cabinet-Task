using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> usersRecords = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char serieOfPassNumber, short passNumber, decimal currentBankAccount)
        {
            var record = new FileCabinetRecord
            {
                Id = this.usersRecords.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                BankAccount = currentBankAccount,
                PassNumber = passNumber,
                SerieOfPassNumber = serieOfPassNumber,
            };

            this.usersRecords.Add(record);
            AddToDictionary(this.firstNameDictionary, firstName, record);
            AddToDictionary(this.lastNameDictionary, lastName, record);
            AddToDictionary(this.dateOfBirthDictionary, dateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] usersRecordsCopy = new FileCabinetRecord[this.usersRecords.Count];

            for (int i = 0; i < this.usersRecords.Count; i++)
            {
                FileCabinetRecord recordCopy = new() { Id = this.usersRecords[i].Id, FirstName = this.usersRecords[i].FirstName, LastName = this.usersRecords[i].LastName, DateOfBirth = this.usersRecords[i].DateOfBirth, SerieOfPassNumber = this.usersRecords[i].SerieOfPassNumber, PassNumber = this.usersRecords[i].PassNumber, BankAccount = this.usersRecords[i].BankAccount };
                usersRecordsCopy[i] = recordCopy;
            }

            return usersRecordsCopy;
        }

        public int GetStat()
        {
            return this.usersRecords.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char serieOfPassNumber, short passNumber, decimal currentBankAccount)
        {
            int recordIndex = id - 1;

            string? firstNameBeforeEdit = this.usersRecords[recordIndex].FirstName;
            this.usersRecords[recordIndex].FirstName = firstName;

            if (firstNameBeforeEdit != null)
            {
                EditDictionary(this.firstNameDictionary, firstNameBeforeEdit, firstName, id);
            }

            string? lastNameBeforeEdit = this.usersRecords[recordIndex].LastName;
            this.usersRecords[recordIndex].LastName = lastName;

            if (lastNameBeforeEdit != null)
            {
                EditDictionary(this.lastNameDictionary, lastNameBeforeEdit, lastName, id);
            }

            DateTime dateOfBirthBeforeEdit = this.usersRecords[recordIndex].DateOfBirth;
            this.usersRecords[recordIndex].DateOfBirth = dateOfBirth;
            EditDictionary(this.dateOfBirthDictionary, dateOfBirthBeforeEdit.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), dateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture), id);

            this.usersRecords[recordIndex].PassNumber = passNumber;
            this.usersRecords[recordIndex].SerieOfPassNumber = serieOfPassNumber;
            this.usersRecords[recordIndex].BankAccount = currentBankAccount;
            Console.WriteLine($"Record #{id} is updated");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new ();

            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                result = this.firstNameDictionary[firstName.ToUpperInvariant()];
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (FileCabinetRecord currentRecord in this.usersRecords)
            {
                if (currentRecord.LastName != null && currentRecord.LastName.ToUpperInvariant() == lastName.ToUpperInvariant())
                {
                    result.Add(currentRecord);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByDayOfBirth(string dateParameter)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool isDateTime = DateTime.TryParse(dateParameter, out DateTime dayOfBirth);

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
                FileCabinetRecord recordForMove = new();

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
