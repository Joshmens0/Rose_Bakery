using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Response
{
    public class GetAllOrdersResponseDto:Status
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string TownOrCity { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
