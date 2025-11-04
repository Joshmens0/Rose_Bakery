using System.ComponentModel.DataAnnotations;

namespace Rose_Bakery.Models.Base
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public DateTime UpdatedOn { get; set; }

    }
}
