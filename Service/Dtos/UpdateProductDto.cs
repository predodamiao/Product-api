namespace Service.Dtos
{
    /// <summary>
    /// Update Product Dto
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary> Product Name </summary>
        public string? Name { get; set; }
        /// <summary> Product Description </summary>
        public string? Description { get; set; }
        /// <summary> Product Available Quantity </summary>
        public long? AvailableQuantity { get; set; }
        /// <summary> Product Price </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Update Product Dto Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="availableQuantity"></param>
        /// <param name="price"></param>
        public UpdateProductDto(string? name, string? description, long? availableQuantity, decimal? price)
        {
            Name = name;
            Description = description;
            AvailableQuantity = availableQuantity;
            Price = price;
        }   
    }
}
