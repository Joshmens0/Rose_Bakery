using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Request
{
    public class CreateProductRequestDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
