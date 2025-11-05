using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Request
{
    public class OrderRequestDto
    {
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string TownOrCity { get; set; }
        public string Address { get; set; }
    }
}
