using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char serieOfPassNumber, short passNumber, decimal currentBankAccount)
        {
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                CurrentBankAccount = currentBankAccount,
                PassNumber = passNumber,
                SerieOfPassNumber = serieOfPassNumber,
            };

            this.list.Add(record);
            AddToDictionary(this.firstNameDictionary, firstName, record);
            AddToDictionary(this.lastNameDictionary, lastName, record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            List<FileCabinetRecord> copyInnerListOfService = new List<FileCabinetRecord>(this.list);
            return copyInnerListOfService.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char serieOfPassNumber, short passNumber, decimal currentBankAccount)
        {
            int numberOfRecordForEdit = id - 1;

            string? firstNameBeforeEdit = this.list[numberOfRecordForEdit].FirstName;
            this.list[numberOfRecordForEdit].FirstName = firstName;

            if (firstNameBeforeEdit != null)
            {
                EditDictionary(this.firstNameDictionary, firstNameBeforeEdit, firstName, id);
            }

            string? lastNameBeforeEdit = this.list[numberOfRecordForEdit].LastName;
            this.list[numberOfRecordForEdit].LastName = lastName;

            if (lastNameBeforeEdit != null)
            {
                EditDictionary(this.lastNameDictionary, lastNameBeforeEdit, lastName, id);
            }

            this.list[numberOfRecordForEdit].DateOfBirth = dateOfBirth;
            this.list[numberOfRecordForEdit].PassNumber = passNumber;
            this.list[numberOfRecordForEdit].SerieOfPassNumber = serieOfPassNumber;
            this.list[numberOfRecordForEdit].CurrentBankAccount = currentBankAccount;

            Console.WriteLine($"Record #{id} is updated");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                result = this.firstNameDictionary[firstName.ToUpperInvariant()];
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (FileCabinetRecord currentRecord in this.list)
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
            bool isConversation = DateTime.TryParse(dateParameter, out DateTime dayOfBirth);

            if (isConversation)
            {
                foreach (FileCabinetRecord currentRecord in this.list)
                {
                    if (currentRecord.DateOfBirth == dayOfBirth)
                    {
                        result.Add(currentRecord);
                    }
                }

                return result.ToArray();
            }

            Console.WriteLine("Conversation error. Format date of birth parameter: \"1994 - Jul - 30\"");
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
            if (oldParameter != null && oldParameter.ToUpperInvariant() != newParameter.ToUpperInvariant())
            {
                FileCabinetRecord recordForMove = new FileCabinetRecord();

                for (int i = 0; i < dictionary[oldParameter.ToUpperInvariant()].Count; i++)
                {
                    if (dictionary[oldParameter.ToUpperInvariant()][i].Id == sourceId)
                    {
                        recordForMove = dictionary[oldParameter.ToUpperInvariant()][i];
                        dictionary[oldParameter.ToUpperInvariant()].RemoveAt(i);
                        if (dictionary[oldParameter.ToUpperInvariant()].Count == 0)
                        {
                            dictionary.Remove(oldParameter.ToUpperInvariant());
                        }

                        break;
                    }
                }

                if (dictionary.ContainsKey(newParameter.ToUpperInvariant()))
                {
                    dictionary[newParameter.ToUpperInvariant()].Add(recordForMove);
                }
                else
                {
                    dictionary.Add(newParameter.ToUpperInvariant(), new List<FileCabinetRecord>() { recordForMove });
                }
            }
        }
    }
}
