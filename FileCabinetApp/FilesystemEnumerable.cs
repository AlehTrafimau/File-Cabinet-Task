using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Defines the order of iteration current collection.
    /// </summary>
    internal class FilesystemEnumerable
    {
        private List<long> collection;
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemEnumerable"/> class.
        /// </summary>
        /// <param name="collection">The collection for iteration.</param>
        /// <param name="fileStream">The instatnce of current fike stream.</param>
        public FilesystemEnumerable(List<long> collection, FileStream fileStream)
        {
            this.collection = collection;
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Gets the enummerator for current collection.
        /// </summary>
        /// <returns>The enummerator for current collection.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            foreach (var i in this.collection)
            {
                yield return this.ReadRecordFromFile(i);
            }
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
