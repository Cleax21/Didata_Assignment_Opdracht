using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public class Product
    {
        public Product(string productId, string description, float amount, float price)
        {
            ProductId = productId;
            Description = description;
            Amount = amount;
            Price = price;
        }
        public string ProductId { get; }

        public string Description { get; }
        public float Amount { get; }
        public float Price { get; }
    }
}
