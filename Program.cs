
using System.Text.Json;

namespace Didata_Assignment_Opdracht
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "Order.json";

            string OrderJsonData = File.ReadAllText(path);

            var orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData);

            Console.ReadLine();
        }
    }
}
