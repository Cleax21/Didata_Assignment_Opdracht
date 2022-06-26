﻿using CsvHelper;
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
        /// A list of orders converted from .json file(s).
        /// </summary>
        private readonly List<Order> Orders;

        /// <summary>
        /// The class in which contains the nessesarly validated argument data 
        /// for the application.
        /// </summary>
        private readonly Argument Argument;

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="args">Supplied argument which contains json files, 
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
                foreach (var order in Orders)
                {
                    order.WriteToConsole();
                }
            }
        }

        /// <summary>
        /// Checks if all given files within the arguments are valid files and
        /// no errors are within the .json files.
        /// </summary>
        private void ValidateFiles()
        {
            Dictionary<ArgumentTypes, List<string>> list = Argument.GetArguments();

            // Handle -d argument files
            if (Argument.GetFoldername() != String.Empty)
            {
                string[] files = Directory.GetFiles(Argument.GetFoldername(), "*.json")
                    ?? Array.Empty<string>();

                foreach (var file in files)
                {
                    ValidateFile(file);
                }
            }

            if (list.ContainsKey(ArgumentTypes.F))
            {
                foreach (var item in list[ArgumentTypes.F])
                {
                    ValidateFile(item);
                }
            }


        }

        /// <summary>
        /// Checks a single file if it is valid for the order class.
        /// </summary>
        /// <param name="file">The file which is being checked on.</param>
        /// <exception cref="Exception">Occures when a .json file is invalid.</exception>
        private void ValidateFile(string file)
        {
            string text = File.ReadAllText(file);
            string message = "";

            var settings = new JsonSerializerSettings()
            {
                Error = (s, e) =>
                {
                    message = e.ErrorContext.Error.Message;
                    e.ErrorContext.Handled = true; // Set the datetime to a
                                                   // default value if not Nullable
                }
            };
            var test = JsonConvert.DeserializeObject<List<Order>>(text, settings);
            //ndeps contains the serialized objects, messages contains the errors

            if (test != null && test.Count > 0)
            {
                Orders.Add(test.First());
            }
            else
            {
                if (test != null && test.Count == 0)
                {
                    throw new Exception($"Error on file '{file}': {message}");
                }
                else
                {
                    // The file is not valid for orders.
                }
            }
        }

        /// <summary>
        /// Writes the Orders in a .CSV file.
        /// </summary>
        /// <param name="filename">The name of the file for the .CSV file</param>
        public void WriteToCSV(string filename)
        {
            var directory = Environment.CurrentDirectory + Settings.dataLocation;
            Directory.CreateDirectory(directory);
            var csvPath = Path.Combine(directory, filename);
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
