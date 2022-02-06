using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }

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

            if (firstNameBeforeEdit != null && firstNameBeforeEdit.ToUpperInvariant() != firstName.ToUpperInvariant())
            {
                FileCabinetRecord recordForMove = new FileCabinetRecord();

                for (int i = 0; i < this.firstNameDictionary[firstNameBeforeEdit.ToUpperInvariant()].Count; i++)
                {
                    if (this.firstNameDictionary[firstNameBeforeEdit.ToUpperInvariant()][i].Id == id)
                    {
                        recordForMove = this.firstNameDictionary[firstNameBeforeEdit.ToUpperInvariant()][i];
                        this.firstNameDictionary[firstNameBeforeEdit.ToUpperInvariant()].RemoveAt(i);
                        if (this.firstNameDictionary[firstNameBeforeEdit.ToUpperInvariant()].Count == 0)
                        {
                            this.firstNameDictionary.Remove(firstNameBeforeEdit.ToUpperInvariant());
                        }

                        break;
                    }
                }

                if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
                {
                    this.firstNameDictionary[firstName.ToUpperInvariant()].Add(recordForMove);
                }
                else
                {
                    this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { recordForMove });
                }
            }

            this.list[numberOfRecordForEdit].DateOfBirth = dateOfBirth;
            this.list[numberOfRecordForEdit].LastName = lastName;
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
    }
}
