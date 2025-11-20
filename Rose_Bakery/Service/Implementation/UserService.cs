using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Service.Implementation
{
    public class UserService(IBakeryDbContext bakeryDbContext, ILogger<UserService> logger ):IUserService
    {
        private readonly IBakeryDbContext _bakery=bakeryDbContext;
        private readonly ILogger<UserService> _logger=logger;
         public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                //check if request is null
                if(request== null)
                    return new CreateUserResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "Request is empty"
                    };
                // check if password and confirm password match
                if(request.Password!= request.ConfirmPassword)
                    return new CreateUserResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "Password and Confirm Password do not match"
                    };
                if(string.IsNullOrEmpty(request.Email))
                    return new CreateUserResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "Email is required"
                    };
                if (string.IsNullOrEmpty(request.PhoneNumber))
                    return new CreateUserResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "Phone number is required"
                    };
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
                    return new CreateUserResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "First name and last name are required"
                    };
                //check if email exist
                var emailExists= await _bakery.Users.Where(u=> u.Email== request.Email)
                    .FirstOrDefaultAsync();
                if(emailExists != null)
                    return new CreateUserResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = "Email already exist"
                    };
                //persist and add user to database
                var newUser = new Models.UserModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                    CreatedOn = DateTime.UtcNow
                };
                _bakery.Users.Add(newUser);
                await _bakery.SaveChangesAsync();
                _logger.LogInformation($"User with email {request.Email} created successfully");
                return new CreateUserResponse()
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "User created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occured: {ex}");
                return new CreateUserResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage= ex.Message   
                };
            }
        }
    }
}
