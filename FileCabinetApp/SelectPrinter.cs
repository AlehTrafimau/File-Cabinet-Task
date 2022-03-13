using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Displays of data of records.
    /// </summary>
    internal class SelectPrinter
    {
        /// <summary>
        /// Displays of necessary fields of records.
        /// </summary>
        /// <param name="selectedRecords">The list of selected records.</param>
        /// <param name="fields">The list of necessary fields of records for display.</param>
        public static void Printer(List<FileCabinetRecord> selectedRecords, string[] fields)
        {
            int[] columnsWidth = IdentificateOfColumnsWidth(fields, selectedRecords);
            string line = DefineOfLine(columnsWidth);
            PrintLine(line);
            PrintTitleRow(fields, columnsWidth);

            foreach (var i in selectedRecords)
            {
                PrintLine(line);
                List<(string, int, string)> recordFieldsForPrint = GetNecessaryFieldsOfRecord(i, fields);
                PrintValueRow(recordFieldsForPrint, columnsWidth);
            }

            PrintLine(line);
        }

        private static int[] IdentificateOfColumnsWidth(string[] fields, List<FileCabinetRecord> records)
        {
            List<int> result = new List<int>();
            if (fields.Contains("id"))
            {
                int maxId = 0;
                foreach (var i in records)
                {
                    maxId = i.Id > maxId ? i.Id : maxId;
                }

                int maxLengthOfIdColumn = maxId.ToString(CultureInfo.InvariantCulture).Length;
                if (maxLengthOfIdColumn < 5)
                {
                    maxLengthOfIdColumn = 5;
                }

                result.Add(maxLengthOfIdColumn);
            }

            if (fields.Contains("firstname"))
            {
                int maxLengthOfFirstName = 0;
                foreach (var i in records)
                {
                    maxLengthOfFirstName = i.FirstName.Length > maxLengthOfFirstName ? i.FirstName.Length : maxLengthOfFirstName;
                }

                maxLengthOfFirstName += 5;
                if (maxLengthOfFirstName < 15)
                {
                    maxLengthOfFirstName = 15;
                }

                result.Add(maxLengthOfFirstName);
            }

            if (fields.Contains("lastname"))
            {
                int maxLengthOfLastName = 0;
                foreach (var i in records)
                {
                    maxLengthOfLastName = i.LastName.Length > maxLengthOfLastName ? i.LastName.Length : maxLengthOfLastName;
                }

                maxLengthOfLastName += 5;
                if (maxLengthOfLastName < 15)
                {
                    maxLengthOfLastName = 15;
                }

                result.Add(maxLengthOfLastName);
            }

            if (fields.Contains("dateofbirth"))
            {
                result.Add(15);
            }

            if (fields.Contains("serieofpassnumber"))
            {
                result.Add(20);
            }

            if (fields.Contains("passnumber"))
            {
                result.Add(15);
            }

            if (fields.Contains("bankaccount"))
            {
                result.Add(20);
            }

            return result.ToArray<int>();
        }

        private static void PrintLine(string line)
        {
            Console.WriteLine(line);
        }

        private static string DefineOfLine(int[] widthOfElementTable)
        {
            string line = "+";
            foreach (int i in widthOfElementTable)
            {
                line += new string('-', i) + "+";
            }

            return line;
        }

        private static void PrintValueRow(List<(string, int, string)> columnsInfo, int[] widthOfElementTable)
        {
            string row = "|";

            for (int i = 0; i < columnsInfo.Count; i++)
            {
                row += AlignCentre(columnsInfo[i].Item1, widthOfElementTable[i], columnsInfo[i].Item2, columnsInfo[i].Item3) + "|";
            }

            Console.WriteLine(row);
        }

        private static void PrintTitleRow(string[] columnsInfo, int[] widthOfElementTable)
        {
            string row = "|";

            for (int i = 0; i < columnsInfo.Length; i++)
            {
                row += AlignCentre(columnsInfo[i], widthOfElementTable[i]) + "|";
            }

            Console.WriteLine(row);
        }

        private static string AlignCentre(string text, int lengthOfTitleColumn, int rigth = 0, string position = "default")
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', lengthOfTitleColumn);
            }
            else if (rigth != 0 && position == "right")
            {
                return text.PadRight(lengthOfTitleColumn - 2 - (rigth - text.Length)).PadLeft(lengthOfTitleColumn);
            }
            else
            {
                return text.PadRight(lengthOfTitleColumn - 2).PadLeft(lengthOfTitleColumn);
            }
        }

        private static List<(string, int, string)> GetNecessaryFieldsOfRecord(FileCabinetRecord record, string[] fieldsForPrint)
        {
            List<(string, int, string)> fieldsOfRecord = new ();

            if (fieldsForPrint.Contains("id"))
            {
                fieldsOfRecord.Add((record.Id.ToString(CultureInfo.InvariantCulture), 2, "right"));
            }

            if (fieldsForPrint.Contains("firstname"))
            {
                fieldsOfRecord.Add((record.FirstName, 0, "default"));
            }

            if (fieldsForPrint.Contains("lastname"))
            {
                fieldsOfRecord.Add((record.LastName, 0, "default"));
            }

            if (fieldsForPrint.Contains("dateofbirth"))
            {
                fieldsOfRecord.Add((record.DateOfBirth.ToString("d", CultureInfo.CurrentCulture), 11, "right"));
            }

            if (fieldsForPrint.Contains("serieofpassnumber"))
            {
                fieldsOfRecord.Add((record.SerieOfPassNumber.ToString(), 17, "right"));
            }

            if (fieldsForPrint.Contains("passnumber"))
            {
                fieldsOfRecord.Add((record.PassNumber.ToString(CultureInfo.InvariantCulture), 10, "right"));
            }

            if (fieldsForPrint.Contains("bankaccount"))
            {
                fieldsOfRecord.Add((record.BankAccount.ToString(CultureInfo.InvariantCulture), 11, "right"));
            }

            return fieldsOfRecord;
        }
    }
}
