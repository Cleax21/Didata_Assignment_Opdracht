﻿using System.Text.Json;

namespace Didata_Assignment_Opdracht
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Order>? orders = null;
            if(Settings.isExample)
            {
                string path = "Order.json";

                string OrderJsonData = File.ReadAllText(path);

                orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData);
            } 
            else
            {
                if(args.Length != 0)
                {
                    bool foundArgD = false;
                    string dArgument = "";

                    bool foundArgF = false;
                    List<string> fArguments = new List<string>();

                    var filename = "output.csv";

                    for (int i = 0; i < args.Length; i++)
                    {
                        switch(args[i])
                        {
                            case "-d":
                                if(!foundArgD)
                                {
                                    foundArgD = true;
                                    i++;
                                    dArgument = args[i];
                                }
                                else
                                {
                                    Console.WriteLine("Argument -d can only be used once.");
                                    return;
                                }

                                break;
                            case "-f":
                                foundArgF = true;
                                i++;
                                fArguments.Add(args[i]);
                                break;
                        }
                    }

                    if (foundArgD && foundArgF)
                    {
                        Console.WriteLine("Both -d and -f argument can only be used exclusively.");
                        return;
                    }
                    else if(!foundArgD && !foundArgF)
                    {
                        Console.WriteLine("Invalid arguments. use -d or -f.");
                        return;
                    }

                    if(foundArgD && !Directory.Exists(dArgument))
                    {
                        Console.WriteLine($"argument -d is invalid. Cannot find directory path: '{dArgument}' ");
                        return;
                    }

                    foreach(var fArgument in fArguments)
                    {  
                        if (Path.GetExtension(fArgument) != ".json")
                        {
                            Console.WriteLine($"argument: {fArgument} is an invalid -f argument. all -f argument need an .json extension");
                            return;
                        }
                        else if(!File.Exists(fArgument))
                        {
                            Console.WriteLine($"file '{fArgument}' does not exist.");
                            return;
                        }
                    }

                    if (args.Last() != dArgument && args.Last() != fArguments.Last())
                    {
                        filename = args.Last();
                        if (Path.GetExtension(filename) != ".csv")
                        {
                            Console.WriteLine("filename doesn't include the valid .csv extension. Adding it manually");
                            filename = Path.ChangeExtension(filename, ".csv");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Warning! no filename given. filename defaulted to '{filename}'");
                    }

                    if (foundArgD)
                    {
                        Console.WriteLine($"-d: {dArgument}");
                    }

                    if(foundArgF)
                    {
                        foreach (var fArgument in fArguments)
                        {
                            Console.WriteLine($"-f: {fArgument}");
                        }
                    }

                    Console.WriteLine($"filename: {filename}");
                } 
                else
                {
                    Console.WriteLine("No arguments has been supplied. Use --help for further information.");
                    return;
                }
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
    }
}
