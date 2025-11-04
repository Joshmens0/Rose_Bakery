using Rose_Bakery.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rose_Bakery.Models
{
    public class CategoryModel:BaseModel
    {

            public string Name { get; set; }

            public List<ProductModel> Products { get; set; }
    }
}
