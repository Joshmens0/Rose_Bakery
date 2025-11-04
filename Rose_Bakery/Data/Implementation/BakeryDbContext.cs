using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Models;

namespace Rose_Bakery.Data.Implementation
{
    public class BakeryDbContext(DbContextOptions<BakeryDbContext> options) : DbContext(options), IBakeryDbContext
    {
        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<OrderModel> Orders { get; set; }
    }
}
