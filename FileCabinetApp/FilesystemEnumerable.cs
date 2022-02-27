using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Defines the order of iteration current collection.
    /// </summary>
    internal class FilesystemEnumerable : IEnumerable<FileCabinetRecord>
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
            return new FilesystemRecordEnumerator(this.collection, this.fileStream);
        }

        /// <summary>
        /// Gets the enummerator for current collection.
        /// </summary>
        /// <returns>The enummerator for current collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
