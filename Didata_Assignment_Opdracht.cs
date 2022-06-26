using CsvHelper;
using System.Globalization;
using System.Text.Json;

namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// Main class which converts orders in a .JSON file into a .CSV file.
    /// </summary>
    public class Didata_Assignment_Opdracht
    {
        /// <summary>
        /// A list of orders converted from .json file(s).
        /// </summary>
        private List<Order>? Orders;

        /// <summary>
        /// The class in which contains the nessesarly validated argument data for the application.
        /// </summary>
        private readonly Argument Argument;

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="args">Supplied argument which contains json files, directory paths and other important data.</param>
        public Didata_Assignment_Opdracht(string[] args)
        {
            Argument = new(args);
        }

        /// <summary>
        /// Execute the program.
        /// </summary>
        public void Execute()
        {
            Dictionary<ArgumentTypes, List<string>> list;
            if (Settings.isExample)
            {
                Orders = GetExampleValues();
                WriteToCSV("output.csv");
            }
            else
            {
                list = Argument.GetArguments();

                if (list[ArgumentTypes.D].Count > 0)
                {
                    string[] tests = Directory.GetFiles(list[ArgumentTypes.D].First(), "*.json");

                    foreach(var test in tests)
                    {
                        string orderJsonData = File.ReadAllText(test);
                        Orders = JsonSerializer.Deserialize<List<Order>>(orderJsonData) ?? new();
                        WriteToCSV(Argument.GetFilename());
                    }
                }
                else if(list[ArgumentTypes.F].Count > 0)
                {
                    string orderJsonData = File.ReadAllText(list[ArgumentTypes.F].First());
                    Orders = JsonSerializer.Deserialize<List<Order>>(orderJsonData) ?? new();
                    WriteToCSV(Argument.GetFilename());
                }                
            }
        }

        /// <summary>
        /// Print the processed Orders on the Console.
        /// </summary>
        public void ViewOrdersInConsole()
        {
            if (Orders != null)
            {
                foreach (var order in Orders)
                {
                    order.WriteToConsole();
                }
            }
        }

        /// <summary>
        /// Get the orderlist from an example in the application. Presentation purposes only.
        /// </summary>
        /// <returns>An <see cref="List{Order}"/> of orders</returns>
        public static List<Order> GetExampleValues()
        {
            string path = "Order.json";
            string OrderJsonData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
        }

        /// <summary>
        /// Writes the Orders in a .CSV file
        /// </summary>
        /// <param name="filename">The name of the file for the .CSV file</param>
        public void WriteToCSV(string filename)
        {
            var csvPath = Path.Combine(Environment.CurrentDirectory, filename);
            using var streamWriter = new StreamWriter(csvPath);
            CultureInfo nfi = CultureInfo.GetCultureInfo("en-US");
            CsvHelper.Configuration.CsvConfiguration configure = new(nfi)
            {
                HasHeaderRecord = false,
                Delimiter = ";",
                SanitizeForInjection = false,
            };


            using var csvWriter = new CsvWriter(streamWriter, configure);
            csvWriter.Context.RegisterClassMap<OrderClassMap>();
            csvWriter.WriteRecords(Orders);
        }

    }
}
