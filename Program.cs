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

            try
            {
                Dictionary<ArgumentTypes, string> list = RetrieveArguments(args);
                ValidateArgs(list);
                string OrderJsonData = File.ReadAllText(list.FirstOrDefault(item => item.Key == ArgumentTypes.F).Value);
                orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData) ?? new();
                WriteToCSV(orders, list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
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

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--help":
                        list.Add(ArgumentTypes.HELP, "--help");
                        return list;
                    case "--h":
                        list.Add(ArgumentTypes.HELP, "--h");
                        return list;
                    case "--?":
                        list.Add(ArgumentTypes.HELP, "--?");
                        return list;
                    case "-d":
                        if (i + 1 >= args.Length)
                        {
                            list.Add(ArgumentTypes.INVALID, $"Argument -d on index '{i}' is missing a directory path");
                            return list;
                        }
                        i++;
                        list.Add(ArgumentTypes.D, args[i]);
                        break;
                    case "-f":
                        if (i + 1 >= args.Length)
                        {
                            list.Add(ArgumentTypes.INVALID, $"Argument -f on index '{i}' is missing a .json file");
                            return list;

                        }
                        i++;
                        list.Add(ArgumentTypes.F, args[i]);
                        break;
                    default:
                        if (args.Last().Equals(args[i]))
                        {
                            if (args[i].Contains("-"))
                            {
                                list.Add(ArgumentTypes.INVALID, $"Unknown argument '{args[i]}'. Use --help for more information");
                                return list;
                            }
                            list.Add(ArgumentTypes.FILENAME, args.Last());
                        } 
                        else
                        {
                            list.Add(ArgumentTypes.INVALID, $"Unknown argument '{args[i]}'. Use --help for more information");
                            return list;
                        }
                        break;
                }
            }

            return list;
        }

        public static void ValidateArgs(Dictionary<ArgumentTypes, string> list)
        {
            if (list.ContainsKey(ArgumentTypes.HELP))
            {
                Help();
                throw new Exception("");
            }
            if (list.ContainsKey(ArgumentTypes.INVALID))
            {
                throw new Exception(list.FirstOrDefault(i => i.Key == ArgumentTypes.INVALID).Value);
            }

            bool foundArgD = false;
            bool foundArgF = false;

            if (list.ContainsKey(ArgumentTypes.D))
            {
                foundArgD = true;
                if (list.Where(i => i.Key == ArgumentTypes.D).Count() > 1)
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_ONLY_ONCE);
                }
            }

            if (list.ContainsKey(ArgumentTypes.F))
            {
                foundArgF = true;
            }

            if (!list.ContainsKey(ArgumentTypes.FILENAME))
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.NO_FILENAME_GIVEN);
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

            foreach(var item in list.Where(i => i.Key == ArgumentTypes.F))
            {
                if (Path.GetExtension(item.Value) != ".json")
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.INVALID_F_EXTENSION);
                }
                else if (!File.Exists(item.Value))
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.FILE_F_DOES_NOT_EXIST);
                }
            }

            if (Path.GetExtension(list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value) != ".csv")
            {
                Console.WriteLine("filename doesn't include the valid .csv extension. Adding it manually...");
                list[list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Key] = Path.ChangeExtension(list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value, ".csv");
            }

            Console.WriteLine($"filename: {list.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value}");


            if (foundArgD)
            {
                Console.WriteLine($"-d: {list.FirstOrDefault(i => i.Key == ArgumentTypes.D).Value}");
            }

            if (foundArgF)
            {
                foreach (var item in list.Where(i => i.Key == ArgumentTypes.F))
                {
                    Console.WriteLine($"-f: {item.Value}");
                }
            }

            Console.WriteLine("-----");
        }
    }
}
