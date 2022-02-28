using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Contains of methods to iteration of data in the storage.
    /// </summary>
    internal class FilesystemIterator : IRecordIterator
    {
        private Dictionary<string, List<long>> dictionary;
        private string key;
        private int indexInStorage;
        private FileStream? fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="dictionary">The current dictionary for iterate its records by input key.</param>
        /// <param name="key">The key for search to the dictionary.</param>
        /// <param name="fileStream">The file stream instance for read record from filesystem.</param>
        public FilesystemIterator(Dictionary<string, List<long>> dictionary, string key, FileStream fileStream)
        {
            this.dictionary = dictionary;
            this.key = key;
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        public FilesystemIterator()
        {
            this.dictionary = new ();
            this.key = string.Empty;
            this.fileStream = default;
        }

        /// <summary>
        /// Returns the next record in current file cabinet storage.
        /// </summary>
        /// <returns>
        /// The next record.
        /// </returns>
        public FileCabinetRecord GetNext()
        {
            FileCabinetRecord record = this.ReadRecordFromFile(this.dictionary[this.key][this.indexInStorage]);
            this.indexInStorage++;
            return record;
        }

        /// <summary>
        /// Stores information about exist of a record in the current storage.
        /// </summary>
        /// <returns>
        /// True if the current storage has a next record, and false otherwise.
        /// </returns>
        public bool HasMore()
        {
            bool result = false;

            if (this.indexInStorage < this.dictionary[this.key].Count)
            {
                result = true;
            }

            return result;
        }

        private FileCabinetRecord ReadRecordFromFile(long firstByteOfRecord)
        {
            FileCabinetRecord recordsFromFileSystem = new ();
            this.fileStream?.Seek(firstByteOfRecord + 2, SeekOrigin.Begin);

            byte[] array = new byte[4];
            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.Id = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            Array.Resize(ref array, 120);
            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.FirstName = Encoding.Default.GetString(array).Trim(default(char));

            Array.Clear(array);
            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.LastName = Encoding.Default.GetString(array).Trim(default(char));

            Array.Resize(ref array, 4);
            this.fileStream?.Read(array, 0, array.Length);
            int day = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            this.fileStream?.Read(array, 0, array.Length);
            int month = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            this.fileStream?.Read(array, 0, array.Length);
            int year = int.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);
            recordsFromFileSystem.DateOfBirth = new DateTime(year, month, day);

            Array.Resize(ref array, 2);
            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.SerieOfPassNumber = char.Parse(Encoding.Default.GetString(array).Trim(default(char)));

            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.PassNumber = short.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            Array.Resize(ref array, 16);
            this.fileStream?.Read(array, 0, array.Length);
            recordsFromFileSystem.BankAccount = decimal.Parse(Encoding.Default.GetString(array), CultureInfo.InvariantCulture);

            return recordsFromFileSystem;
        }
    }
}
