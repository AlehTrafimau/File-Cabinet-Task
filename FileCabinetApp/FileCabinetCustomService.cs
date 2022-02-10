using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Service to create, storage, edit, find and display records about users. It uses custom conditional to create new record.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creates dafault validator for check user's parameters.
        /// </summary>
        /// <returns>
        /// The new custom record validator for user's parameters.
        /// </returns>
        public override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
