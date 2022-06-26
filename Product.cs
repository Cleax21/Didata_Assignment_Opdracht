namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// A product which can be part of a <see cref="Order"/> list.
    /// See <see cref="Order.Products"/> for more info.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The ID of the product class.
        /// </summary>
        public string ProductId { get; }

        /// <summary>
        /// Describes what the product is.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The Quantity inside the product.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// The price of the product (how much it costs).
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The constructor of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="productId">The given id of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="amount">the quantity of inside the product.</param>
        /// <param name="price">The cost of the product.</param>
        public Product(string productId, string description, decimal amount,
            decimal price)
        {
            ProductId = productId;
            Description = description;
            Amount = amount;
            Price = price;

            Validate();
        }

        /// <summary>
        /// Writes to the console what the product contains.
        /// </summary>
        public void WriteToConsole()
        {
            Console.WriteLine($"    ID: {ProductId}");
            Console.WriteLine($"    Description: {Description}");
            Console.WriteLine($"    Amount: {Amount}");
            Console.WriteLine($"    Price: {Price}");
        }

        /// <summary>
        /// Check if all values within the product are valid and correct.
        /// </summary>
        /// <exception cref="Exception">Any errors that makes the product invalid.</exception>
        public void Validate()
        {
            if (ProductId.Length > 50)
            {
                throw new Exception("ID is too long. " +
                    "Max 50 characters allowed.");
            }

            if (Description.Length > 200)
            {
                throw new Exception("Description is too long. " +
                    "Max 200 characters allowed.");
            }

            if (Decimal.Round(Amount, 3) != Amount)
            {
                throw new Exception("Amount contains too many decimals. " +
                    "Max 2 allowed.");
            }

            if (Decimal.Round(Price, 3) != Price)
            {
                throw new Exception("Price contains too many decimals. " +
                    "Max 2 allowed.");
            }
        }
    }
}
