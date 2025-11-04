using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Models;

namespace Rose_Bakery.Data.Interface
{
    public interface IBakeryDbContext
    {
        DbSet<CategoryModel> Categories { get; set; }
        DbSet<ProductModel> Products { get; set; }
        DbSet<OrderModel> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);

    }
}
