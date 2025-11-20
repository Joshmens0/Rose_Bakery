using Rose_Bakery.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rose_Bakery.Models
{
    public class UserModel : BaseModel
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        [Required][Phone] public string PhoneNumber { get; set; }
        [Required][PasswordPropertyText] public string Password { get; set; }
    }
}
