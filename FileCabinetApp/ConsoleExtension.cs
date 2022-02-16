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
        /// Reads a information from console, converts to  necessary format and verify its valid by special conditionals.
        /// </summary>
        /// <typeparam name="T">Generic parameter.</typeparam>
        /// <param name="converter">A special converter of entered string.</param>
        /// <param name="validator">A special validator of convertered string.</param>
        /// <returns>
        /// The value of T type which converted and validated successfully.
        /// </returns>
        internal static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
