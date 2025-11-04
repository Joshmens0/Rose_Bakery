using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;

namespace Rose_Bakery.Service.Interface
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request);
        Task<ProductResponseDto> UpdateProductAsync(UpdateProductRequestDto request);
        Task<ProductResponseDto> DeleteProductAsync(string ProductName);
        Task<ProductResponseDto> GetProductAsync(GetProductRequest request);
        Task<IList<ProductResponseDto>> GetAllProductsAsync();
    }
}
