using System.Text.Json;

namespace Didata_Assignment_Opdracht
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = "Order.json";

            string OrderJsonData = File.ReadAllText(path);

            var orders = JsonSerializer.Deserialize<List<Order>>(OrderJsonData);

            if (Settings.isDebug)
            {
                if (orders != null)
                {
                    ViewOrdersInConsole(orders);
                }
            }

            Console.ReadLine();
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
