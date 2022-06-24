using CsvHelper;
using System.Text.Json;
using System.Globalization;

namespace Didata_Assignment_Opdracht
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Order>? orders;
            if (Settings.isExample)
            {
                orders = GetExampleValues();

                WriteToCSV(orders, "output.csv");
            }
            else
            {
                Dictionary<ArgumentTypes, string> list = RetrieveArguments(args);
                string OrderJsonData = File.ReadAllText(list.FirstOrDefault(item => item.Key == ArgumentTypes.F).Value);

                try
                {
                    orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                
                WriteToCSV(orders, list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value);
            }

            if (Settings.isDebug)
            {
                if (orders != null)
                {
                    ViewOrdersInConsole(orders);
                    Console.ReadLine();
                }
            }
        }

        public static void ViewOrdersInConsole(List<Order> orders)
        {
            foreach (var order in orders)
            {
                order.WriteToConsole();
            }
        }

        public static void Help()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Syntax: Program-name [OPTIONS] FILENAME");
            Console.WriteLine("");
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("-d   Name of an existing directory where the");
            Console.WriteLine("     program searches for .json files. Can only be");
            Console.WriteLine("     used once. Exclusive to -f.");
            Console.WriteLine("");
            Console.WriteLine("-f   Name of the input file. Can be used multiple");
            Console.WriteLine("     times. Exclusive to -d");
            Console.WriteLine("");
            Console.WriteLine("FILENAME");
            Console.WriteLine("     Name of the output file");
            Console.WriteLine("--------------------------------------------------");
        }

        public static List<Order> GetExampleValues()
        {
            string path = "Order.json";
            string OrderJsonData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
        }

        public static void WriteToCSV(List<Order> orders, string filename)
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

        private static Dictionary<ArgumentTypes, string> RetrieveArguments(string[] args)
        {
            Dictionary<ArgumentTypes, string> list = new();
            bool foundArgD = false;
            bool foundArgF = false;

            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--help":
                            list.Add(ArgumentTypes.HELP, "--help");
                            break;
                        case "--h":
                            list.Add(ArgumentTypes.HELP, "--h");
                            break;
                        case "--?":
                            list.Add(ArgumentTypes.HELP, "--?");
                            break;
                        case "-d":
                            if (!foundArgD)
                            {
                                foundArgD = true;
                                i++;
                                list.Add(ArgumentTypes.D, args[i]);
                            }
                            else
                            {
                                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_ONLY_ONCE);
                            }
                            break;
                        case "-f":
                            foundArgF = true;
                            i++;
                            list.Add(ArgumentTypes.F, args[i]);
                            break;
                        default:
                            if (!args.Last().Equals(args[i]))
                            {
                                ErrorHandler.writeErrorToConsole(ErrorTypes.INVALID_ARGUMENTS);
                            }
                            break;
                    }
                }

                if (foundArgD && foundArgF)
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_AND_F_INVALID);
                }
                else if (!foundArgD && !foundArgF)
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_OR_F_MISSING);
                }

                if (foundArgD && !Directory.Exists(list.FirstOrDefault(t => t.Key == ArgumentTypes.D).Value))
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_INVALID);
                }

                List<string> fArguments = new();

                foreach (var item in list)
                {
                    if(item.Key == ArgumentTypes.F)
                    {
                        fArguments.Add(item.Value);
                    }
                }

                foreach (var fArgument in fArguments)
                {
                    if (Path.GetExtension(fArgument) != ".json")
                    {
                        ErrorHandler.writeErrorToConsole(ErrorTypes.INVALID_F_EXTENSION);
                    }
                    else if (!File.Exists(fArgument))
                    {
                        ErrorHandler.writeErrorToConsole(ErrorTypes.FILE_F_DOES_NOT_EXIST);
                    }
                }

                if (args.Last() != list.FirstOrDefault(item => item.Key == ArgumentTypes.D).Value && args.Last() != fArguments.Last())
                {
                    list.Add(ArgumentTypes.FILENAME, args.Last());
                    if (Path.GetExtension(list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value) != ".csv")
                    {
                        Console.WriteLine("filename doesn't include the valid .csv extension. Adding it manually");
                        list[list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Key] = Path.ChangeExtension(list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value, ".csv");
                    }
                }
                else
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.NO_FILENAME_GIVEN);
                }

                if (foundArgD)
                {
                    Console.WriteLine($"-d: {list.FirstOrDefault(i => i.Key == ArgumentTypes.D).Value}");
                }

                if (foundArgF)
                {
                    foreach (var fArgument in fArguments)
                    {
                        Console.WriteLine($"-f: {fArgument}");
                    }
                }

                Console.WriteLine($"filename: {list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value}");
            }
            else
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.NO_ARGUMENT);
            }

            return list;
        }
    }
}
