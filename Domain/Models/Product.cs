namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long AvailableQuantity { get; set; }
        public Decimal Price { get; set; }


        public Product(string name, string description, long availableQuantity, decimal price)
        {
            Name = name;
            Description = description;
            AvailableQuantity = availableQuantity;
            Price = price;
        }
    }
}
