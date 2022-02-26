namespace FileCabinetApp
{
    /// <summary>
    /// Contains of methods to iteration of data in the storage.
    /// </summary>
    internal class MemoryIterator : IRecordIterator
    {
        private Dictionary<string, List<FileCabinetRecord>> dictionary;
        private string key;
        private int positionInStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="dictionary">The current dictionary for iterate its records by input key.</param>
        /// <param name="key">The key for search to the dictionary.</param>
        public MemoryIterator(Dictionary<string, List<FileCabinetRecord>> dictionary, string key)
        {
            this.dictionary = dictionary;
            this.key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        public MemoryIterator()
        {
            this.dictionary = new ();
            this.key = string.Empty;
        }

        /// <summary>
        /// Returns the next record in current file cabinet storage.
        /// </summary>
        /// <returns>
        /// The next record.
        /// </returns>
        public FileCabinetRecord GetNext()
        {
            FileCabinetRecord record = this.dictionary[this.key][this.positionInStorage];
            this.positionInStorage++;
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

            if (this.positionInStorage < this.dictionary[this.key].Count)
            {
                result = true;
            }

            return result;
        }
    }
}
