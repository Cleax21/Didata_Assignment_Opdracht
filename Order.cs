using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public class OrderClassMap : ClassMap<Order>
    {
        public OrderClassMap()
        {
            Map(o => o.OrderId).Index(1);
            Map(o => o.Description).Index(2);
            Map(o => o.CustomerId).Index(3);
            Map(o => o.Products.Count).Index(4);
            Map(o => o.TotalPrice).Index(5);
        }
    }

    public class Order
    {
        public Order(int orderId, string description, int customerId, List<Product> products)
        {
            OrderId = orderId;
            Description = description;
            CustomerId = customerId;
            Products = products;

            TotalPrice = Products.Sum(product => product.Price);

            try
            {
                Validate();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int OrderId { get; }
        public string Description { get; }
        public int CustomerId { get; }
        public List<Product> Products { get; }
        public decimal TotalPrice { get; }

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

        public void Validate()
        {
            if(OrderId <= 0)
            {
                throw new Exception("OrderId must be higher than 0");
            }

            if (Description.Length > 100)
            {
                throw new Exception("Description title is to long. Max 100 characters allowed");
            }

            if(CustomerId <= 0)
            {
                throw new Exception("CustomerId must be higher than 0");
            }

            if(Products.Count <= 0)
            {
                throw new Exception("Order contains no products");
            }

            foreach(var product in Products)
            {
                product.validate();
            }
        }
    }
}
