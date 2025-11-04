using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;
using Rose_Bakery.Extensions;
using Rose_Bakery.Models;
using Rose_Bakery.Service.Interface;
using System.Collections;
using System.Linq;
using System.Text.Json;

namespace Rose_Bakery.Service.Implementation
{
    public class ProductService(ILogger<ProductService> logger, IBakeryDbContext bakery) : IProductService
    {
        private readonly IBakeryDbContext _bakery=bakery;
        private readonly ILogger<ProductService> _logger=logger;

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
        {
            try
            {
                //check for nulls
                var nulls = request
                    .GetType()
                    .GetProperties()
                    .Where(
                    p =>
                    {
                        var value = p.GetValue(request);
                        if (value == null)
                        {
                            return true;
                        }
                        if (value is string str && string.IsNullOrWhiteSpace(str))
                        {
                            return true;
                        }
                        if (p.PropertyType.IsValueType && Activator.CreateInstance(p.PropertyType).Equals(value))
                        {
                            return true;
                        }
                        return false;
                    }).ToList();

                if (nulls.Count>0)
                {
                    _logger.LogInformation("{request} is empty",string.Join(", ",nulls));
                    return  new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage =$"Request {string.Join(",", nulls)} Has Nulls"
                    };
                }
                //check if category exist
                var category = await _bakery.Categories
                    .Where(c=>c.Name==request.CategoryName)
                    .FirstOrDefaultAsync();
                if (category == null)
                {
                    _logger.LogInformation("category name does not exist in database");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Category Not Found"
                    };
                }
                //create a new record
                var product = new ProductModel()
                {
                    Category = category,
                    CategoryId = category.Id,
                    CreatedOn = DateTime.UtcNow,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    Name = request.Name,
                    Price = request.Price

                };
                await _bakery.Products.AddAsync(product);
                int result= await _bakery.SaveChangesAsync();
                if (result == 0)
                {
                    _logger.LogInformation("failure to save new product record to the database");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = "Record Creation Failed"
                    };
                }
                _logger.LogInformation("product has been sucessfully saved to the database");
                var response = new ProductResponseDto()
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product Record Created Succesfully"
                };
                return response;
            }
            catch(Exception ex) 
            {
                _logger.LogInformation("Operation Failed");
                return new ProductResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message,
                };
            }
        }

        public async Task<ProductResponseDto> DeleteProductAsync(string ProductName)
        {
            try
            {
                //check if request is null
                if (string.IsNullOrEmpty(ProductName))
                {
                    _logger.LogInformation("product name was not provided");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Product Name Is Empty Or Null"
                    };
                }
                //check if the product exists
                var product = await _bakery.Products
                    .Where(p => p.Name == ProductName)
                    .FirstOrDefaultAsync();
                if (product == null)
                {
                    _logger.LogInformation("product does not exist in the database");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Product Not Found"
                    };
                }
                //persist and delete from database
                _logger.LogInformation("Deletion of record initiated");
                await _bakery.Products.
                    Where(p => p == product)
                    .ExecuteDeleteAsync();
                var result = await _bakery.SaveChangesAsync();
                if (result >= 0)
                {
                    _logger.LogInformation("records deleted from the database");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Product Deleted Successfully",
                        CategoryName = product.Category.Name,
                        Name = product.Name,
                        Description = product.Description,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                    };
                }
                _logger.LogInformation("product record deletion was not committed/saved to database");
                return new ProductResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Product Deletion Failed To Commit"
                };
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return new ProductResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message,
                };
            }
        }

        public async Task<IList<ProductResponseDto>> GetAllProductsAsync()
        {
            try
            {
                // retrieve all the products from the database
                var productList= await _bakery.Products.ToListAsync();
                if (productList.Count > 0) 
                {

                    if (productList.Count() <= 0)
                    {
                        _logger.LogInformation("there are no product records in the database");
                        return new List<ProductResponseDto>().Select(x => new ProductResponseDto()
                        {
                            StatusMessage = "No Products Found, Failed!",
                            StatusCode = StatusCodes.Status404NotFound
                        }).ToList();
                    
                    }
                }
                
               var category= await _bakery.Categories.ToListAsync();     
                var response = productList
                       .Select
                       (
                         
                         x => new ProductResponseDto()
                        { 
                            CategoryName=x.Category.Name,
                            Name = x.Name,
                            Description = x.Description,
                            ImageUrl = x.ImageUrl,
                            Price = x.Price,
                            StatusCode = StatusCodes.Status200OK,
                            StatusMessage = "Products Retrieved Successfully"
                        }
                       ).ToList();
                
                return response;

            }   
            catch (Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return new List<ProductResponseDto>()
                {
                    new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = ex.Message,
                    }
                };
            }
        }

        public async Task<ProductResponseDto> GetProductAsync(GetProductRequest ProductRequest)
        {
            try
            {
                //check if the request is null
                if (string.IsNullOrEmpty(ProductRequest.Product))
                {
                    _logger.LogInformation("product name was not provided");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Product Name Is Empty Or Null"
                    };
                }
                if (string.IsNullOrEmpty(ProductRequest.Category))
                {
                    _logger.LogInformation("product category was not provided");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Category Name Is Empty Or Null, Failed!"
                    };
                }
                //check if category exist
                var category = await _bakery.Categories
                    .Where(c => c.Name == ProductRequest.Category).FirstOrDefaultAsync();
                if(category==null)
                {
                    _logger.LogInformation("category was not found in the databse");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Category Not Found, Failed!"
                    };
                }
                //check if the product exist
                var product = await _bakery.Products
                    .Where(p=>p.Name== ProductRequest.Product && p.Category==category)
                    .FirstOrDefaultAsync();
                if (product == null)
                {
                    _logger.LogInformation("product was not found in the database");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Product Not Found, Failed!"
                    };
                }
                //return the result
                var response = new ProductResponseDto()
                {
                    CategoryName = product.Category.Name,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product Retrieved Sucessfully!"
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return new ProductResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message,
                };
            }
        }

        public async Task<ProductResponseDto> UpdateProductAsync(UpdateProductRequestDto updateRequest)
        {
            try
            {
                if (updateRequest == null)
                {
                    _logger.LogInformation("request is null or empty");
                    return new ProductResponseDto()
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Resquest Is Null, Failed"
                        
                    };
                }
                //call the product 
                var GetProduct = await GetProductAsync(new GetProductRequest() 
                { 
                    Category=updateRequest.Category,
                    Product=updateRequest.Name
                });
                if (GetProduct == null || GetProduct.StatusCode!=StatusCodes.Status200OK)
                {
                    _logger.LogInformation("product was not found in the database");
                    return new ProductResponseDto
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        StatusMessage = "Product Not Found, Failed"
                    };
                }
                if(string.IsNullOrEmpty(updateRequest.Description))
                {
                    updateRequest.Description = GetProduct.Description;
                }
                if(string.IsNullOrEmpty(updateRequest.ImageUrl))
                {
                    updateRequest.ImageUrl = GetProduct.ImageUrl;
                }
                if (updateRequest.Price==default)
                {
                    updateRequest.Price = GetProduct.Price;
                }
                var request = await _bakery.Products
                    .Where(x=>x.Name==updateRequest.Name && x.Category.Name==updateRequest.Category)
                    .FirstOrDefaultAsync();
                request.Price=updateRequest.Price;
                request.UpdatedOn=DateTime.UtcNow;
                request.Description = updateRequest.Description;
                request.ImageUrl=updateRequest.ImageUrl;
                await _bakery.SaveChangesAsync();
                return new ProductResponseDto()
                {
                    Description = request.Description,
                    ImageUrl = updateRequest.ImageUrl,
                    CategoryName = updateRequest.Category,
                    Price = updateRequest.Price,
                    Name = updateRequest.Name,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product Updated Sucessfully!"
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Operation Failed");
                return new ProductResponseDto()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = ex.Message,
                };
            }
        }
    }
}
