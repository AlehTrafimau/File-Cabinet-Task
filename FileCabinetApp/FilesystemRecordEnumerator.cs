using System.Collections;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements IEnumerator for filesystem cabinet service.
    /// </summary>
    internal class FilesystemRecordEnumerator : IEnumerator<FileCabinetRecord>
    {
        private List<long> collection;
        private FileStream fileStream;
        private int position = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemRecordEnumerator"/> class.
        /// </summary>
        /// <param name="collection">The current collection for iteration it content.</param>
        /// <param name="fileStream">The current file stream instance for read records from filesystem.</param>
        public FilesystemRecordEnumerator(List<long> collection, FileStream fileStream)
        {
            this.collection = collection;
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Gets the current record from filesystem.
        /// </summary>
        /// <value>
        /// The current record from filesystem.
        /// </value>
        public FileCabinetRecord Current
        {
            get
            {
                if (this.position == -1 || this.position >= this.collection.Count)
                {
                    throw new ArgumentException();
                }

                return this.ReadRecordFromFile(this.collection[this.position]);
            }
        }

        /// <summary>
        ///  Gets the current record from filesystem.
        /// </summary>
        /// <value>
        /// The current record from filesystem.
        /// </value>
        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Moves the counter of position to 1 if the conditions is true.
        /// </summary>
        /// <returns>
        /// True if moved to one, or false if not.
        /// </returns>
        public bool MoveNext()
        {
            if (this.position < this.collection.Count - 1)
            {
                this.position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the position counter to initial place.
        /// </summary>
        public void Reset()
        {
            this.position = -1;
        }

        /// <summary>
        /// Disposes unmanaged resources.
        /// </summary>
        public void Dispose()
        {
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
