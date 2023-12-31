namespace Infrastructure.Dtos
{
    public class FindProductsDto
    {
        public string? NameToFind {get; set;}
        public string? PropertyToOrderBy { get; set;}
        public PaginationDto Pagination { get; set;}

        public FindProductsDto() {
            Pagination = new PaginationDto();
        }
    }
}
