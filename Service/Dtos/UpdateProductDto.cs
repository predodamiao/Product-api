namespace Service.Dtos
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long? AvailableQuantity { get; set; }
        public decimal? Price { get; set; }
    }
}
