using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;
using Rose_Bakery.Service.Interface;
using System.Threading.Tasks;

namespace Rose_Bakery.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IBakeryDbContext _bakeryDbContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IBakeryDbContext bakeryDbContext, ILogger<OrderService> logger)
        {
            _bakeryDbContext = bakeryDbContext;
            _logger = logger;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto request)
        {
            try
            {
                var category = await _bakeryDbContext.Categories.FirstOrDefaultAsync(c => c.Name == request.CategoryName);
                if (category == null)
                {
                    return new OrderResponseDto { StatusCode = 404, StatusMessage = "Category not found" };
                }

                var product = await _bakeryDbContext.Products.FirstOrDefaultAsync(p => p.Name == request.ProductName && p.CategoryId == category.Id);
                if (product == null)
                {
                    return new OrderResponseDto { StatusCode = 404, StatusMessage = "Product not found" };
                }

                var newOrder = new Models.OrderModel
                {
                    UserName = request.UserName,
                    CategoryId = category.Id,
                    Category = category,
                    Product = product,
                    CreatedOn = DateTime.UtcNow,
                    ProductId = product.Id,
                    Payment = GlobalEnum.Payment.Pending,
                    Delivery = GlobalEnum.Delivery.Pending,
                    PhoneNumber = request.PhoneNumber,
                    AltPhoneNumber = request.AltPhoneNumber,
                    TownOrCity = request.TownOrCity,
                    Address = request.Address,
                };

                await _bakeryDbContext.Orders.AddAsync(newOrder);
                await _bakeryDbContext.SaveChangesAsync();

                return new OrderResponseDto
                {
                    StatusCode = 200,
                    StatusMessage = "Order created successfully",
                    ProductName = product.Name,
                    Category = category.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order");
                return new OrderResponseDto { StatusCode = 500, StatusMessage = "An internal server error occurred" };
            }
        }

        Task<OrderResponseDto> IOrderService.CreateOrderAsync(OrderRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<GetAllOrdersResponseDto>> GetAllOrdersAsync()
        {
            try
            {
                // Retrieve all orders with related product and category data
                var orders = await _bakeryDbContext.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Category)
                    .ToListAsync();
                if (orders == null || !orders.Any())
                {
                    return new List<GetAllOrdersResponseDto>
                    {
                        new GetAllOrdersResponseDto
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            StatusMessage = "No orders found"
                        }
                    };
                }
                // Map orders to OrderResponseDto
                return orders.Select(o => new GetAllOrdersResponseDto
                {
                    Address = o.Address,
                    CustomerName = o.UserName,
                    PhoneNumber = o.PhoneNumber,
                    OrderDate = o.CreatedOn,
                    ProductCategory = o.Category.Name,
                    ProductName = o.Product.Name,
                    TownOrCity = o.TownOrCity,
                    TotalAmount = o.Product.Price,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Orders retrieved successfully"

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders");
                return new List<GetAllOrdersResponseDto>().Select
                    (
                        o => new GetAllOrdersResponseDto
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            StatusMessage = "An internal server error occurred"
                        }
                    ).ToList();
            }
        }
    }
}
