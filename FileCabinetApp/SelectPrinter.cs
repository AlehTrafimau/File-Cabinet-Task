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
            int[] columnsWidth = DeterminateOfColumnsWidth(fields, selectedRecords);
            string line = DefineOfLine(columnsWidth);
            PrintLine(line);
            PrintTitleRow(fields, columnsWidth);

            foreach (var i in selectedRecords)
            {
                PrintLine(line);
                List<(string, string)> recordFieldsForPrint = GetNecessaryFieldsOfRecord(i, fields);
                PrintValueRow(recordFieldsForPrint, columnsWidth);
            }

            PrintLine(line);
        }

        private static int[] DeterminateOfColumnsWidth(string[] fields, List<FileCabinetRecord> records)
        {
            List<int> result = new ();
            if (fields.Contains("ID"))
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

            if (fields.Contains("FIRSTNAME"))
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

            if (fields.Contains("LASTNAME"))
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

            if (fields.Contains("DATEOFBIRTH"))
            {
                result.Add(15);
            }

            if (fields.Contains("SERIEOFPASSNUMBER"))
            {
                result.Add(20);
            }

            if (fields.Contains("PASSNUMBER"))
            {
                result.Add(15);
            }

            if (fields.Contains("BANKACCOUNT"))
            {
                int maxLengthOfbankAccount = 0;
                foreach (var i in records)
                {
                    maxLengthOfbankAccount = i.BankAccount.ToString(CultureInfo.InvariantCulture).Length > maxLengthOfbankAccount ? i.BankAccount.ToString(CultureInfo.InvariantCulture).Length : maxLengthOfbankAccount;
                }

                maxLengthOfbankAccount += 5;
                if (maxLengthOfbankAccount < 15)
                {
                    maxLengthOfbankAccount = 15;
                }

                result.Add(maxLengthOfbankAccount);
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

        private static void PrintValueRow(List<(string, string)> columnsInfo, int[] widthOfElementTable)
        {
            string row = "|";

            for (int i = 0; i < columnsInfo.Count; i++)
            {
                row += AlignCentre(columnsInfo[i].Item1, widthOfElementTable[i], columnsInfo[i].Item2) + "|";
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

        private static string AlignCentre(string text, int lengthOfTitleColumn, string position = "DEFAULT")
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', lengthOfTitleColumn);
            }
            else if (text == "FIRSTNAME" || text == "LASTNAME" || text == "SERIEOFPASSNUMBER" || position == "LEFT")
            {
                return text.PadLeft(text.Length + 2).PadRight(lengthOfTitleColumn);
            }
            else
            {
                return text.PadRight(text.Length + 2).PadLeft(lengthOfTitleColumn);
            }
        }

        private static List<(string, string)> GetNecessaryFieldsOfRecord(FileCabinetRecord record, string[] fieldsForPrint)
        {
            List<(string, string)> fieldsOfRecord = new ();

            if (fieldsForPrint.Contains("ID"))
            {
                fieldsOfRecord.Add((record.Id.ToString(CultureInfo.InvariantCulture), "RIGHT"));
            }

            if (fieldsForPrint.Contains("FIRSTNAME"))
            {
                fieldsOfRecord.Add((record.FirstName, "LEFT"));
            }

            if (fieldsForPrint.Contains("LASTNAME"))
            {
                fieldsOfRecord.Add((record.LastName, "LEFT"));
            }

            if (fieldsForPrint.Contains("DATEOFBIRTH"))
            {
                fieldsOfRecord.Add((record.DateOfBirth.ToString("d", CultureInfo.CurrentCulture), "RIGHT"));
            }

            if (fieldsForPrint.Contains("SERIEOFPASSNUMBER"))
            {
                fieldsOfRecord.Add((record.SerieOfPassNumber.ToString(), "LEFT"));
            }

            if (fieldsForPrint.Contains("PASSNUMBER"))
            {
                fieldsOfRecord.Add((record.PassNumber.ToString(CultureInfo.InvariantCulture), "RIGHT"));
            }

            if (fieldsForPrint.Contains("BANKACCOUNT"))
            {
                fieldsOfRecord.Add((record.BankAccount.ToString(CultureInfo.InvariantCulture), "RIGHT"));
            }

            return fieldsOfRecord;
        }
    }
}
