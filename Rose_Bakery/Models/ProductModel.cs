using Rose_Bakery.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rose_Bakery.Models
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryModel Category { get; set; }
    }
}
