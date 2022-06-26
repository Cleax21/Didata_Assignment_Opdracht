using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;

namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// Main class which converts orders in a .JSON file into a .CSV file.
    /// </summary>
    public class Didata_Assignment_Opdracht
    {
        /// <summary>
        /// A list of orders converted from .JSON file(s).
        /// </summary>
        private readonly List<Order> Orders;

        /// <summary>
        /// The class in which contains the necessarily argument information 
        /// for the application.
        /// </summary>
        private readonly Argument Argument;

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="args">Supplied argument which contains .JSON files, 
        /// directory paths and other important data.</param>
        public Didata_Assignment_Opdracht(string[] args)
        {
            Argument = new(args);
            Orders = new();
        }

        /// <summary>
        /// Execute the program.
        /// </summary>
        public void Execute()
        {
            ValidateFiles();
            WriteToCSV(Argument.GetFilename());

        }

        /// <summary>
        /// Print the processed Orders on the Console.
        /// </summary>
        public void ViewOrdersInConsole()
        {
            if (Orders != null)
            {
                foreach (Order order in Orders)
                {
                    order.WriteToConsole();
                }
            }
        }

        /// <summary>
        /// Checks if all given files within the arguments are valid files and
        /// no errors are within the .JSON files.
        /// </summary>
        private void ValidateFiles()
        {
            Dictionary<ArgumentTypes, List<string>> list = Argument.GetArguments();

            // Handle -d argument files.
            if (Argument.GetFoldername() != String.Empty)
            {
                string[] files = Directory.GetFiles(Argument.GetFoldername(), "*.JSON")
                    ?? Array.Empty<string>();

                foreach (string file in files)
                {
                    ValidateFile(file);
                }
            }

            if (list.ContainsKey(ArgumentTypes.F))
            {
                foreach (string item in list[ArgumentTypes.F])
                {
                    ValidateFile(item);
                }
            }


        }

        /// <summary>
        /// Checks a single file if it is valid for the order class.
        /// </summary>
        /// <param name="file">The file which is being checked on.</param>
        /// <exception cref="Exception">Occurs when a .JSON file is invalid.</exception>
        private void ValidateFile(string file)
        {
            string text = File.ReadAllText(file);
            string message = "";

            JsonSerializerSettings settings = new()
            {
                Error = (s, e) =>
                {
                    // De-serializing has not been successful.
                    message = e.ErrorContext.Error.Message;
                    e.ErrorContext.Handled = true;
                }
            };
            List<Order>? newOrder = JsonConvert.DeserializeObject<List<Order>>(text, settings);

            // Check if the newOrder object is set and contains an valid order.
            if (newOrder != null && newOrder.Count > 0)
            {
                Orders.Add(newOrder.First());
            }
            else
            {
                // Checks if the newOrder is valid.
                if (newOrder != null && newOrder.Count == 0)
                {
                    throw new Exception($"Error on file '{file}': {message}");
                }
                else
                {
                    // The file is not an orders.
                }
            }
        }

        /// <summary>
        /// Writes the Orders in a .CSV file.
        /// </summary>
        /// <param name="filename">The name of the file for the .CSV file.</param>
        public void WriteToCSV(string filename)
        {
            string directory = Environment.CurrentDirectory + Settings.dataLocation;
            Directory.CreateDirectory(directory);
            string csvPath = Path.Combine(directory, filename);
            using StreamWriter streamWriter = new(csvPath);
            CultureInfo nfi = CultureInfo.GetCultureInfo("en-US");
            CsvHelper.Configuration.CsvConfiguration configure = new(nfi)
            {
                HasHeaderRecord = false,
                Delimiter = ";",
                SanitizeForInjection = false,
            };

            using CsvWriter csvWriter = new(streamWriter, configure);
            csvWriter.Context.RegisterClassMap<OrderClassMap>();
            csvWriter.WriteRecords(Orders);
        }
    }
}
