namespace FileCabinetApp
{
    internal static class ConsoleExtension
    {
        internal static string ReadTillSuccess(string displayMessage, string errorMessage, Func<string, bool> check)
        {
            string? result;

            while (true)
            {
                Console.Write(displayMessage);
                result = Console.ReadLine();
                if (result != null && check(result) == true)
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
