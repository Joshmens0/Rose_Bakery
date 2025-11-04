using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Response
{
    public class OrderResponseDto : Status
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
    }
}
