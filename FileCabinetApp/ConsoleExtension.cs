namespace FileCabinetApp
{
    /// <summary>
    /// Extension for console work.
    /// </summary>
    internal static class ConsoleExtension
    {
        /// <summary>
        /// Reads line from console till it is done some special conditionals.
        /// </summary>
        /// <param name="displayMessage">Display at console one time for iteration.</param>
        /// <param name="errorMessage">The message for display to console when conditionals are not done.</param>
        /// <param name="validationConditional">The function is for validation console line.</param>
        /// <returns>
        /// Source string which conform special conditionals.
        /// </returns>
        internal static string ReadTillSuccess(string displayMessage, string errorMessage, Func<string, bool> validationConditional)
        {
            string? result;

            while (true)
            {
                Console.Write(displayMessage);
                result = Console.ReadLine();
                if (result != null && validationConditional(result) == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return result;
        }
    }
}
