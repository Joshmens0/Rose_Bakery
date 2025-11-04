using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Data.Implementation;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;
using Rose_Bakery.Models;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Service.Implementation
{
    public class CategoryService(IBakeryDbContext bakery, ILogger<CategoryService> logger) : ICategoryService
    {
        private readonly IBakeryDbContext _bakery=bakery;
        private readonly ILogger<CategoryService> _logger=logger;

        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequest)
        {
            try
            {
                //let's check if the request is null or empty
                if (categoryRequest == null)
                {
                    _logger.LogInformation("category name was not provided");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Request Is Empty"
                    };
                }
                //let's check if category already exist
                var categoryExists =await _bakery.Categories
                    .Where(c=>c.Name == categoryRequest.Name)
                    .ToListAsync();
                if (categoryExists.Count>0)
                {
                    _logger.LogInformation("catory name {category} already exists in the database", categoryRequest.Name.ToLower());
                    return new CategoryResponseDto()
                    { StatusCode = StatusCodes.Status409Conflict, StatusMessage = "category already exist" };

                }
                //let's add to the database
                var category = new CategoryModel()
                {
                    Name = categoryRequest.Name,
                    CreatedOn = DateTime.UtcNow,
                };
                await _bakery.Categories.AddAsync(category);
                int result = await _bakery.SaveChangesAsync();
                if (result >= 0)
                {
                    _logger.LogInformation("new category {category}, has been successfully created ", category.Name.ToUpper());
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "category record successfully created",
                        Name = categoryRequest.Name,
                    };
                }
                _logger.LogInformation("category record was not saved");
                return new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status304NotModified,
                    StatusMessage = "request failed"
                };
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Operation failed");
                return new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = $"{ex}"
                };
            }
        }

        public async Task<CategoryResponseDto> DeleteCategoryAsync(string CategoryName)
        {
            try
            {
                //let's check for null
                if (string.IsNullOrEmpty(CategoryName))
                {
                    _logger.LogInformation("category name was not provided");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "request is null or empty"
                    };
                }
                //let's get the category from the request
                var getCategory = await _bakery.Categories
                    .Where(c => c.Name == CategoryName)
                    .FirstOrDefaultAsync();
                if (getCategory == null)
                {
                    _logger.LogInformation("category does not exist");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "category does not exist"
                    };
                }
                //lets delete the category
                await _bakery.Categories
                    .Where(c => c == getCategory)
                    .ExecuteDeleteAsync();
                await _bakery.SaveChangesAsync();
                return new CategoryResponseDto()
                {
                    Name = getCategory.Name,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category Deleted Successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message
                };
            }
               
        }

        public async Task<IList<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            try
            {
                // retieve all data from the database
                var categoryList=await _bakery.Categories
                    .ToListAsync();
                if (categoryList.Count<=0)
                {
                    _logger.LogInformation("database does not have any record");
                    return (IList<CategoryResponseDto>)new List<CategoryResponseDto>().Select(c => new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "No data Found"
                    }
                    );
                }
                var response = categoryList.Select(c => new CategoryResponseDto()
                {
                    Name = c.Name,
                    StatusMessage = "Records Retrieved Successfully",
                    StatusCode = StatusCodes.Status200OK,
                }
                );
                return response.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return (IList<CategoryResponseDto>)new List<CategoryResponseDto>().Select(c => new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message
                });
               
            }
        }

        public Task<CategoryResponseDto> GetCategoryAsync(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto UpdateCategoryRequest)
        {
            try
            {
                //let's check if request is null
                if(UpdateCategoryRequest.Id!=default)
                {
                    _logger.LogInformation("category name was not provided");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Request Is Empty"
                    };
                }
                //let's check if category exist
                var getCategory = await _bakery.Categories
                    .Where(c=>c.Id == UpdateCategoryRequest.Id)
                    .FirstOrDefaultAsync();
                if (getCategory==null)
                {
                    _logger.LogInformation("category name was not found");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Category Does Not Exist"
                    };
                }
                //Update the record
                getCategory.UpdatedOn= DateTime.UtcNow;
                getCategory.Name= UpdateCategoryRequest.Name;
                int result=await _bakery.SaveChangesAsync();
                if(result>0)
                {
                    _logger.LogInformation("record has been upadated");
                    return new CategoryResponseDto()
                    {
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Record Upated Successfully"
                    };
                }
                _logger.LogInformation("record update did not save to database");
                return new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Record Update Failed"
                };

            }
            catch (Exception ex) 
            {
                _logger.LogInformation("Operation Failed");
                return new CategoryResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message
                };
            }
        }
    }
}
