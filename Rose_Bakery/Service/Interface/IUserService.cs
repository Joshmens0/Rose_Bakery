using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;

namespace Rose_Bakery.Service.Interface
{
    public interface IUserService
    {
         Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
    }
}
