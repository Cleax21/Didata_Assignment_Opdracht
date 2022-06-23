using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public class Order
    {
        public Order(int orderId, string description, int customerId, List<Product> products)
        {
            OrderId = orderId;
            Description = description;
            CustomerId = customerId;
            Products = products;
        }
        public int OrderId { get; }
        public string Description { get; }
        public int CustomerId { get; }
        public List<Product> Products { get; }

        public void WriteToConsole()
        {
            Console.WriteLine($"Order ID: {OrderId}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Customer ID: {CustomerId}");
            Console.WriteLine("Products:");
            Console.WriteLine("{");

            foreach (Product product in Products)
            {
                Console.WriteLine("    -----");
                product.WriteToConsole();
                if (product == Products.Last())
                {
                    Console.WriteLine("    -----");
                }
            }
            Console.WriteLine("}");
        }
    }
}
