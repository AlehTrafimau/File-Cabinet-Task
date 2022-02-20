using System.Collections.ObjectModel;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Consists of methods to create the collection of auto generated records.
    /// </summary>
    internal class FileCabinetRecordGenerator : IFileCabinetRecordGenerator
    {
        private readonly string[] firstNames = new string[]
        {
            "Anatoliy", "Anton", "Arkadiy", "Artur", "Boris", "Dmitriy", "Vadim", "Valentin", "Valeriy", "Viktor", "Vitaliy",
            "Vladimir", "Vladislav", "Vyacheslav", "Gennadiy", "Georgiy", "Denis", "Dmitriy", "Egor", "Ivan", "Igor", "Ilya",
            "Kirill", "Konstantin", "Leonid", "Maksim", "Mikhail", "Nikita", "Nikolai", "Oleg", "Pavel", "Pyotr", "Roman", "Ruslan",
            "Sergey", "Stepan", "Timofey", "Fedor", "Yan",
        };

        private readonly string[] lastNames = new string[]
        {
           "Ivanov", "Smirnov", "Kuznetsov", "Popov", "Vasilyev", "Petrov", "Falcov", "Mikhailov", "Novikov", "Fedorov",
           "Morozov", "Volkov", "Alexeyev", "Lebedev", "Semyonov", "Egorov", "Pavlov", "Kozlov", "Stepanov", "Nikolaev",
           "Orlov", "Sidorov", "Andreev", "Makarov", "Nikitin", "Zakharov", "Filipov",
        };

        /// <summary>
        /// Generates setted number of records (validation rules - default).
        /// </summary>
        /// <param name="numberOfRecords">The necessary number of record.</param>
        /// <param name="startId">The id of the first record in the sequence.</param>
        /// <returns>
        /// The list of auto generated records.
        /// </returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRandomRecords(int numberOfRecords, int startId)
        {
            List<FileCabinetRecord> result = new ();
            Random random = new ();

            while (numberOfRecords > 0)
            {
                string firstName = this.FirstNameGenerate(random);
                string lastName = this.LastNameGenerate(random);
                DateTime dateOfBirth = this.BirthDateGenerate(random);
                char serieOfPassNumber = this.SerieOfPassNumberGenerate(random);
                short passNumber = this.PassNumberGenerate(random);
                decimal bankAccount = this.BankAccountGenerate(random);
                result.Add(new FileCabinetRecord(startId, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount));
                numberOfRecords--;
                startId++;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public string FirstNameGenerate(Random generator)
        {
            int randomIndex = generator.Next(this.firstNames.Length - 1);
            string result = this.firstNames[randomIndex];
            return result;
        }

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public string LastNameGenerate(Random generator)
        {
            int randomIndex = generator.Next(this.lastNames.Length - 1);
            string result = this.lastNames[randomIndex];
            return result;
        }

        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random first name.
        /// </returns>
        public DateTime BirthDateGenerate(Random generator)
        {
            int day = generator.Next(1, 31);
            int month = generator.Next(1, 12);
            int year = generator.Next(1950, DateTime.Now.Year);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            return new (year, month, day);
        }

        /// <summary>
        /// Generates a random serie of pass number.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random serie of pass number.
        /// </returns>
        public char SerieOfPassNumberGenerate(Random generator)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int randomIndex = generator.Next(chars.Length - 1);
            char result = chars[randomIndex];
            return result;
        }

        /// <summary>
        /// Generates a random pass number.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random pass number.
        /// </returns>
        public short PassNumberGenerate(Random generator)
        {
            short result = (short)generator.Next(1, 9999);
            return result;
        }

        /// <summary>
        /// Generates a random bank account.
        /// </summary>
        /// <param name="generator">The random number-generator.</param>
        /// <returns>
        /// Generated random bank account.
        /// </returns>
        public decimal BankAccountGenerate(Random generator)
        {
            decimal result = new (generator.Next(), generator.Next(), generator.Next(), false, (byte)generator.Next(27));
            return result;
        }
    }
}
