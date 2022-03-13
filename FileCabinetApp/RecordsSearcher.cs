using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    internal static class RecordsSearcher
    {
        public static FileCabinetRecord[] FindRecords(Tuple<string, string> findParameter, FileCabinetRecord[] usersRecords)
        {
            FileCabinetRecord[] findResult = Array.Empty<FileCabinetRecord>();

            if (findParameter.Item1.ToUpperInvariant() == "ID")
            {
                Tuple<bool, string, int> valueOfParameter = StringConverter.IntegerConvert(findParameter.Item2);
                findResult = valueOfParameter.Item1 ? usersRecords.Where(record => record.Id == valueOfParameter.Item3).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "FIRSTNAME")
            {
                findResult = findParameter.Item2 != string.Empty ? usersRecords.Where(record => record.FirstName.ToUpperInvariant() == findParameter.Item2.ToUpperInvariant()).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "LASTNAME")
            {
                findResult = findParameter.Item2 != string.Empty ? usersRecords.Where(record => record.LastName.ToUpperInvariant() == findParameter.Item2.ToUpperInvariant()).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "DATEOFBIRTH")
            {
                Tuple<bool, string, DateTime> valueOfParameter = StringConverter.DateTimeConvert(findParameter.Item2);
                findResult = valueOfParameter.Item1 ? usersRecords.Where(record => record.DateOfBirth == valueOfParameter.Item3).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "SERIEOFPASSNUMBER")
            {
                Tuple<bool, string, char> valueOfParameter = StringConverter.CharConvert(findParameter.Item2);
                findResult = valueOfParameter.Item1 ? usersRecords.Where(record => record.SerieOfPassNumber == valueOfParameter.Item3).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "PASSNUMBER")
            {
                Tuple<bool, string, short> valueOfParameter = StringConverter.ShortConvert(findParameter.Item2);
                findResult = valueOfParameter.Item1 ? usersRecords.Where(record => record.PassNumber == valueOfParameter.Item3).ToArray() : findResult;
            }
            else if (findParameter.Item1.ToUpperInvariant() == "BANKACCOUNT")
            {
                Tuple<bool, string, decimal> valueOfParameter = StringConverter.DecimalConvert(findParameter.Item2);
                findResult = valueOfParameter.Item1 ? usersRecords.Where(record => record.BankAccount == valueOfParameter.Item3).ToArray() : findResult;
            }

            return findResult;
        }
    }
}
