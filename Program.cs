using CsvHelper;
using System.Text.Json;
using System.Globalization;


namespace Didata_Assignment_Opdracht
{
    public class Program
    {
        /// <summary>
        /// The root function where the application starts.
        /// </summary>
        /// <param name="args">The supplied arguments to the application via the console.</param>
        public static void Main(string[] args)
        {
            Didata_Assignment_Opdracht program = new(args);

            try
            {
                program.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (Settings.isDebug)
            {
                program.ViewOrdersInConsole();
                Console.ReadLine();
            }
        }
    }
}
