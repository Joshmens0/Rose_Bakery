using Rose_Bakery.Dto.Base;

namespace Rose_Bakery.Dto.Response
{
    public class CreateUserResponse:Status
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
