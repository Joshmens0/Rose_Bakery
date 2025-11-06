using Microsoft.EntityFrameworkCore;
using Rose_Bakery.Models;

namespace Rose_Bakery.Extensions
{
    public static class BuilderExtension
    {
        public static void Conversion(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderModel>()
                .Property(p => p.Delivery)
                .HasConversion<string>();
            modelBuilder.Entity<OrderModel>()
                .Property(p=>p.Payment)
                .HasConversion<string>();
        }
    }
}
