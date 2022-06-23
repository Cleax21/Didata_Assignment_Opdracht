using System.Text.Json;

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
                                foundArgD = true;
                                i++;
                                dArgument = args[i ];
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
                    else if(!foundArgD && !foundArgD)
                    {
                        Console.WriteLine("Invalid arguments. use -d or -f.");
                        return;
                    }

                    if (args.Last() != dArgument && args.Last() != fArguments.Last())
                    {
                        filename = args.Last();
                        Console.WriteLine($"Last valid argument: {filename}");
                    }

                    Console.WriteLine($"-d: {dArgument}");

                    foreach(var fArgument in fArguments)
                    {
                        Console.WriteLine($"-f: {fArgument}");
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
