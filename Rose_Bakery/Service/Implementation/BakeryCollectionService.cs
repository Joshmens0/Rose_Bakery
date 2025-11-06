using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Dto.Response;
using Rose_Bakery.Models;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Service.Implementation
{
    public class BakeryCollectionService(ILogger<BakeryCollectionService> logger, IBakeryDbContext bakery) : IBakeryCollectionService
    {
        private readonly IBakeryDbContext _bakery=bakery;
        private readonly ILogger<BakeryCollectionService> _logger=logger;

        public async Task<IList<BakeryResponseDto>> GetBakeryCollectionAsync()
        {
            try
            {
                var category = await _bakery.Categories
                    .Include(x => x.Products)
                    .ToListAsync();

                if (category.Count <=0)
                {
                    return new List<BakeryResponseDto>
                    {
                        new ()
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            StatusMessage = "No Data Found, Failed"
                        }
                    };
                }

                var collection = category.Select(c => new BakeryResponseDto()
                {
                    CatgoryName = c.Name,
                    Products = c.Products
                        .Select(p => new ProductResponseDto()
                        {
                            Name = p.Name,
                            ImageUrl = p.ImageUrl,
                            Description = p.Description,
                            Price = p.Price,
                            StatusCode = StatusCodes.Status200OK,
                            StatusMessage = "Product Retrieved Successfully!"
                        }).ToList(),
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Success"
                });
                return collection.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the bakery collection");
                return new List<BakeryResponseDto>
                {
                    new ()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = ex.Message
                    }
                };
            }
        }
    }
}
