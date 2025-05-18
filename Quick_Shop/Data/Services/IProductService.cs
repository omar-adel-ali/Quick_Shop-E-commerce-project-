using Quick_Shop.Data.Base;
using Quick_Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick_Shop.Data.Services
{
    public interface IProductsService : IEntityBaseRepository<Product>
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string searchString, string category = null, decimal? maxPrice = null);
        Task<IEnumerable<Product>> GetFeaturedAsync();
    }
}