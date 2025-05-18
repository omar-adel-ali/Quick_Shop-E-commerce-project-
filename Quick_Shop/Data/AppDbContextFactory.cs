using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quick_Shop.Data;

namespace Quick_Shop.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-84FOA9O;Database=final;Integrated Security=True;TrustServerCertificate=True;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}