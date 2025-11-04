using Rose_Bakery.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using static Rose_Bakery.GlobalEnum;

namespace Rose_Bakery.Models
{
    public class OrderModel: BaseModel
    {
        public string UserName { get; set; }
        public  int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))] public CategoryModel Category { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))] public ProductModel Product { get; set; }
        public Payment Payment { get; set; }
        public Delivery Delivery { get; set; }
        public string PhoneNumber { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string TownOrCity { get; set; }
        public string Address { get; set; }
        public float? AmountPaid { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? OrderClosedDate { get; set; }
    }
}
