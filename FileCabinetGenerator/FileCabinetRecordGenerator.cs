using System.Collections.ObjectModel;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Consists of methods to create the collection of auto generated records.
    /// </summary>
    internal static class FileCabinetRecordGenerator
    {
        private static readonly string[] FirstNames = new string[]
        {
            "Anatoliy", "Anton", "Arkadiy", "Artur", "Boris", "Dmitriy", "Vadim", "Valentin", "Valeriy", "Viktor", "Vitaliy",
            "Vladimir", "Vladislav", "Vyacheslav", "Gennadiy", "Georgiy", "Denis", "Dmitriy", "Egor", "Ivan", "Igor", "Ilya",
            "Kirill", "Konstantin", "Leonid", "Maksim", "Mikhail", "Nikita", "Nikolai", "Oleg", "Pavel", "Pyotr", "Roman", "Ruslan",
            "Sergey", "Stepan", "Timofey", "Fedor", "Yan",
        };

        private static readonly string[] LastNames = new string[]
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
        internal static ReadOnlyCollection<FileCabinetRecord> GetRandomRecords(int numberOfRecords, int startId)
        {
            List<FileCabinetRecord> result = new ();
            Random random = new ();

            while (numberOfRecords > 0)
            {
                string firstName = FirstNameGenerate(random);
                string lastName = LastNameGenerate(random);
                DateTime dateOfBirth = BirthDateGenerate(random);
                char serieOfPassNumber = SerieOfPassNumberGenerate(random);
                short passNumber = PassNumberGenerate(random);
                decimal bankAccount = BankAccountGenerate(random);
                result.Add(new FileCabinetRecord(startId, firstName, lastName, dateOfBirth, serieOfPassNumber, passNumber, bankAccount));
                numberOfRecords--;
                startId++;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        private static string FirstNameGenerate(Random generator)
        {
            int randomIndex = generator.Next(FirstNames.Length - 1);
            string result = FirstNames[randomIndex];
            return result;
        }

        private static string LastNameGenerate(Random generator)
        {
            int randomIndex = generator.Next(LastNames.Length - 1);
            string result = LastNames[randomIndex];
            return result;
        }

        private static DateTime BirthDateGenerate(Random generator)
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

        private static char SerieOfPassNumberGenerate(Random generator)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int randomIndex = generator.Next(chars.Length - 1);
            char result = chars[randomIndex];
            return result;
        }

        private static short PassNumberGenerate(Random generator)
        {
            short result = (short)generator.Next(1, 9999);
            return result;
        }

        private static decimal BankAccountGenerate(Random generator)
        {
            decimal result = new (generator.Next(), generator.Next(), generator.Next(), false, (byte)generator.Next(27));
            return result;
        }
    }
}
