using System.Globalization;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to create, storage, edit, find and display records about users. It uses dafault conditional to create new record.
    /// </summary>
    internal class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates dafault validator for check user's parameters.
        /// </summary>
        /// <returns>
        /// The new  default record validator for user's parameters.
        /// </returns>
        public override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}