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
    }
}
