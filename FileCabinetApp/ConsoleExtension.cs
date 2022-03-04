using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists of a set of function for work to console's data.
    /// </summary>
    internal class ConsoleExtension
    {
        /// <summary>
        /// Reads a information from console, converts to necessary format..
        /// </summary>
        /// <typeparam name="T">Generic parameter of necessary type.</typeparam>
        /// <param name="converter">A special converter of entered string.</param>
        /// <returns>
        /// The value of T type which converted successfully.
        /// </returns>
        internal static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter)
        {
            do
            {
                T value;

                string? input = Console.ReadLine();
                Tuple<bool, string, T> conversionResult;

                if (input != null)
                {
                    conversionResult = converter(input);
                }
                else
                {
                    continue;
                }

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;
                return value;
            }
            while (true);
        }

        //internal (string, string)? IdentificateRecordField(string field)
        //{
        //    (string, string)? recordField = null;
        //    string[] input = field.Split(new char[] { ',', '=', '\"' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (input.Length != 2)
        //    {
        //        return recordField;
        //    }

        //    string fieldName = input[0].ToUpperInvariant();
        //    string filesValue = input[1];

        //    switch (fieldName)
        //    {
        //        case "ID":
        //            var idconverter = StringConverter.IntegerConvert(filesValue);

        //            break;
        //        case "FIRSTNAME":
        //            recordField = ("FIRSTNAME", filesValue);

        //            break;
        //        case "LASTNAME":
        //            recordField = ("LASTNAME", filesValue);

        //            break;
        //        case "DATEOFBIRTH":
        //            Tuple<bool, string, DateTime> date = StringConverter.DateTimeConvert(value);
        //            string dateOfBirth = " ";
        //            if (date.Item1)
        //            {
        //                dateOfBirth = date.Item3.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            }

        //            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
        //            {
        //                int[] result = this.dateOfBirthDictionary[dateOfBirth].Select(record => record.Id).ToArray<int>();
        //                DeleteById(result);
        //                PrintMessage(result);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Records with this field are not found");
        //            }

        //            break;
        //        case "SERIEOFPASSNUMBER":
        //            Tuple<bool, string, char> serieOfPassnumber = StringConverter.CharConvert(value);
        //            if (serieOfPassnumber.Item1)
        //            {
        //                int[] recordsId = this.usersRecords.Where(record => record.SerieOfPassNumber == serieOfPassnumber.Item3).Select(person => person.Id).ToArray<int>();
        //                DeleteById(recordsId);
        //                PrintMessage(recordsId);
        //            }
        //            else
        //            {
        //                Console.WriteLine(serieOfPassnumber.Item2);
        //            }

        //            break;
        //        case "PASSNUMBER":
        //            Tuple<bool, string, int> passNumber = StringConverter.IntegerConvert(value);

        //            if (passNumber.Item1)
        //            {
        //                int[] recordsId = this.usersRecords.Where(record => record.PassNumber == passNumber.Item3).Select(person => person.Id).ToArray<int>();
        //                DeleteById(recordsId);
        //                PrintMessage(recordsId);
        //            }
        //            else
        //            {
        //                Console.WriteLine(passNumber.Item2);
        //            }

        //            break;
        //        case "BANKACCOUNT":
        //            Tuple<bool, string, decimal> bankAccount = StringConverter.DecimalConvert(value);
        //            if (bankAccount.Item1)
        //            {
        //                int[] recordsId = this.usersRecords.Where(record => record.PassNumber == bankAccount.Item3).Select(person => person.Id).ToArray();
        //                DeleteById(recordsId);
        //                PrintMessage(recordsId);
        //            }
        //            else
        //            {
        //                Console.WriteLine(bankAccount.Item2);
        //            }

        //            break;
        //        default:
        //            Console.WriteLine($"The record haven't got this field {fieldName}");
        //            break;
        //    }
        //}
    }
}
