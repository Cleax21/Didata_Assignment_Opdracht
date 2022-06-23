using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public class Product
    {
        public string ProductId { get; }

        public string Description { get; }
        public float Amount { get; }
        public float Price { get; }

        public Product(string productId, string description, float amount, float price)
        {
            ProductId = productId;
            Description = description;
            Amount = amount;
            Price = price;
        }

        public void WriteToConsole()
        {
            Console.WriteLine($"    ID: {ProductId}");
            Console.WriteLine($"    Description: {Description}");
            Console.WriteLine($"    Amount: {Amount}");
            Console.WriteLine($"    Price: {Price}");
        }
    }
}
