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
        public decimal Amount { get; }
        public decimal Price { get; }

        public Product(string productId, string description, decimal amount, decimal price)
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

        public void validate()
        {
            if(ProductId.Length > 50)
            {
                throw new Exception("ID is too long. Max 50 characters allowed.");
            }

            if(Description.Length > 200)
            {
                throw new Exception("Description is too long. Max 200 characters allowed.");
            }

            if(Decimal.Round(Amount, 3) != Amount)
            {
                throw new Exception("Amount contains too many decimals. Max 2 allowed.");
            }

            if (Decimal.Round(Price, 3) != Price)
            {
                throw new Exception("Price contains too many decimals. Max 2 allowed.");
            }
        }
    }
}
