using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;

namespace Rose_Bakery.Service.Interface
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequest);
        Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto UpdateCategoryRequest);
        Task<CategoryResponseDto> DeleteCategoryAsync(string CategoryName);
        Task<CategoryResponseDto> GetCategoryAsync(string CategoryName);
        Task<IList<CategoryResponseDto>> GetAllCategoriesAsync();
    }
}
