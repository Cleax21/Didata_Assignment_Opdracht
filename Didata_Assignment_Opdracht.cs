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

        private Argument Argument;

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="args">Supplied argument which contains json files, directory paths and other important data.</param>
        public Didata_Assignment_Opdracht(string[] args)
        {
            Argument = new(args);
        }

        public void Execute()
        {
            Dictionary<ArgumentTypes, string> list = Argument.GetArguments();

            string OrderJsonData = File.ReadAllText(list.FirstOrDefault(item => item.Key == ArgumentTypes.F).Value);
            Orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
            WriteToCSV(Orders, list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value);

        }

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

        public static List<Order> GetExampleValues()
        {
            string path = "Order.json";
            string OrderJsonData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
        }

        public void WriteToCSV(List<Order> orders, string filename)
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
            csvWriter.WriteRecords(orders);
        }

    }
}
