namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
                this.list[numberOfRecordForEdit].DateOfBirth = dateOfBirth;
                this.list[numberOfRecordForEdit].FirstName = firstName;
                this.list[numberOfRecordForEdit].LastName = lastName;
                this.list[numberOfRecordForEdit].PassNumber = passNumber;
                this.list[numberOfRecordForEdit].SerieOfPassNumber = serieOfPassNumber;
                this.list[numberOfRecordForEdit].CurrentBankAccount = currentBankAccount;
                Console.WriteLine($"Record #{id} is updated");
        }
    }
}
