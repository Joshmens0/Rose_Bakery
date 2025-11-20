using Rose_Bakery.Data.Implementation;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Service.Implementation;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Extensions
{
    public static class AddScopedServicesExtension
    {
        public static void AddScopedServices(this IServiceCollection service)
        {
              service.AddScoped<ICategoryService,CategoryService>()
                .AddScoped<IProductService,ProductService>()
                .AddScoped<IBakeryCollectionService,BakeryCollectionService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IUserService,UserService>();
        }
    }
}
