namespace FileCabinetApp
{
    /// <summary>
    /// Consists a set of functions to convert string source to special types.
    /// </summary>
    internal static class StringConverter
    {
        /// <summary>
        /// Converts string source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The result of convert to string type.</returns>
        public static Tuple<bool, string, string> StringConvert(string source)
        {
            return new Tuple<bool, string, string>(true, string.Empty, source);
        }

        /// <summary>
        /// Converts string source to DateTime type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The result of convert to DateTime type.</returns>
        public static Tuple<bool, string, DateTime> DateTimeConvert(string source)
        {
            bool convertToDateTime = DateTime.TryParse(source, out DateTime dateTime);
            Tuple<bool, string, DateTime> result;

            if (convertToDateTime)
            {
                result = new (true, string.Empty, dateTime);
            }
            else
            {
                result = new (false, "this parameter is not date", dateTime);
            }

            return result;
        }

        /// <summary>
        /// Converts string source to char type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The result of convert to char type.</returns>
        public static Tuple<bool, string, char> CharConvert(string source)
        {
            bool convertToChar = char.TryParse(source, out char charSymbol);
            Tuple<bool, string, char> result;

            if (convertToChar)
            {
                result = new (true, string.Empty, charSymbol);
            }
            else
            {
                result = new (false, "this parameter is not symbol", charSymbol);
            }

            return result;
        }

        /// <summary>
        /// Converts string source to short type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The result of convert to short type.</returns>
        public static Tuple<bool, string, short> ShortConvert(string source)
        {
            bool convertToShort = short.TryParse(source, out short shortTypeNumber);
            Tuple<bool, string, short> result;

            if (convertToShort)
            {
                result = new (true, string.Empty, shortTypeNumber);
            }
            else
            {
                result = new (false, "this parameter is not short type number", shortTypeNumber);
            }

            return result;
        }

        /// <summary>
        /// Convert string source to decimal type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The result of convert to decimal type.</returns>
        public static Tuple<bool, string, decimal> DecimalConvert(string source)
        {
            bool convertToDecimal = decimal.TryParse(source, out decimal decimalTypeNumber);
            Tuple<bool, string, decimal> result;

            if (convertToDecimal)
            {
                result = new (true, string.Empty, decimalTypeNumber);
            }
            else
            {
                result = new (false, "this parameter is not decimal type number", decimalTypeNumber);
            }

            return result;
        }
    }
}
