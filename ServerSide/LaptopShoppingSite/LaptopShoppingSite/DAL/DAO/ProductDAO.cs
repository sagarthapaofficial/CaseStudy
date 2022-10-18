using LaptopShoppingSite.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace LaptopShoppingSite.DAL.DAO
{
    public class ProductDAO
    {

        private StoreContext _db;
        public ProductDAO(StoreContext ctx)
        {
            _db = ctx;

        }
            public async Task<List<Product>> GetAllByBrand(int id)
            {
                return await _db.Products.Where(item => item.BrandID == id).ToListAsync();
            }
            public async Task<Product> GetById(string id)
            {
                Product product = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
                return product;
            }
        }
}
