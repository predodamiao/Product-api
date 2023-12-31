namespace Service.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long AvailableQuantity { get; set; }
        public decimal Price { get; set; }

        public CreateProductDto(string name, string description, long availableQuantity, decimal price) {
            Name = name;
            Description = description;
            AvailableQuantity = availableQuantity;
            Price = price;
        }
    }
}
