using CsvHelper.Configuration;

namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// The <see cref="OrderClassMap"/> describes the <see cref="CsvHelper"/> 
    /// on how to parse .JSON files into an <see cref="Order"/> object.
    /// </summary>
    public class OrderClassMap : ClassMap<Order>
    {
        /// <summary>
        /// The constructor of the <see cref="OrderClassMap"/>.
        /// </summary>
        public OrderClassMap()
        {
            Map(o => o.OrderId).Index(1);
            Map(o => o.Description).Index(2);
            Map(o => o.CustomerId).Index(3);
            Map(o => o.Products.Count).Index(4);
            Map(o => o.TotalPrice).Index(5);
        }
    }

    /// <summary>
    /// The <see cref="Order"/> class is a model class for received orders.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The ID of the order class.
        /// </summary>
        public int OrderId { get; }

        /// <summary>
        /// The description of the order class.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The customerID of the order class.
        /// </summary>
        public int CustomerId { get; }

        /// <summary>
        /// A list of products within the order class.
        /// </summary>
        public List<Product> Products { get; }

        /// <summary>
        /// The total price of the order.
        /// </summary>
        public decimal TotalPrice { get; }

        /// <summary>
        /// The constructor of the class <see cref="Order"/>.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="description">The description of the order.</param>
        /// <param name="customerId">the customer ID of the order.</param>
        /// <param name="products">The List of products inside the order.</param>
        public Order(int orderId, string description, int customerId,
            List<Product> products)
        {
            OrderId = orderId;
            Description = description;
            CustomerId = customerId;
            Products = products;

            // Calculates the total price, based on the sum total price of
            // all listed products within the order.
            TotalPrice = Products.Sum(product => product.Price);

            // Validate all order properties.
            Validate();
        }

        /// <summary>
        /// Writes to the console what the order contains.
        /// </summary>
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

        /// <summary>
        /// Check if all values within the order are valid and correct.
        /// </summary>
        /// <exception cref="Exception">Any errors that makes the order invalid.</exception>
        public void Validate()
        {
            if (OrderId <= 0)
            {
                throw new Exception("OrderId must be higher than 0.");
            }

            if (Description.Length > 100)
            {
                throw new Exception("Description title is to long. " +
                    "Max 100 characters allowed.");
            }

            if (CustomerId <= 0)
            {
                throw new Exception("CustomerId must be higher than 0.");
            }

            if (Products.Count <= 0)
            {
                throw new Exception("Order contains no products.");
            }
        }
    }
}
