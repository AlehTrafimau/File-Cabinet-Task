namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public char SerieOfPassNumber { get; set; }

        public short PassNumber { get; set; }

        public decimal BankAccount { get; set; }
    }
}
