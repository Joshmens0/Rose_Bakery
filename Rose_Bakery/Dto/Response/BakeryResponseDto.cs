using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Response
{
    public class BakeryResponseDto:Status
    {
        public string? CatgoryName { get; set; }
        public List<ProductResponseDto>? Products { get; set; }
    }
}
