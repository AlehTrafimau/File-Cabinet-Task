using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class UserValidation
    {
        public static bool CheckName(string name)
        {
            bool result = false;

            if (name != null && name.Length >= 2 && name.Length <= 60 && !Regex.IsMatch(name, @"^\s+$"))
            {
                result = true;
            }

            return result;
        }

        public static bool CheckBirthDate(string birthDate)
        {
            bool result = false;

            if (birthDate == null)
            {
                return false;
            }

            bool convertToDateTime = DateTime.TryParse(birthDate, out DateTime dayOfBirth);
            DateTime currentDate = DateTime.Now;
            DateTime minValueDateOfBirth = new DateTime(1950, 1, 1);

            if (convertToDateTime && dayOfBirth.CompareTo(currentDate) < 0 && dayOfBirth.CompareTo(minValueDateOfBirth) >= 0)
            {
                result = true;
            }

            return result;
        }

        public static bool CheckSerieOfPassNumber(string serieOfPassNumber)
        {
            bool result = false;

            if (serieOfPassNumber != null && Regex.IsMatch(serieOfPassNumber, @"^([A-Z]{1}|[a-z]{1})$"))
            {
                result = true;
            }

            return result;
        }

        public static bool CheckPassNumber(string passNumber)
        {
            bool result = false;

            if (passNumber != null && Regex.IsMatch(passNumber, @"^(\d{4}|\d{3}|\d{2}|\d{1})$"))
            {
                result = true;
            }

            return result;
        }

        public static bool CheckBankAccount(string bankAccount)
        {
            try
            {
                decimal convertToDecimal = decimal.Parse(bankAccount, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}