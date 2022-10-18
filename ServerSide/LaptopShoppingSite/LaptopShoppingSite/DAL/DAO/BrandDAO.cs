
using LaptopShoppingSite.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace LaptopShoppingSite.DAL.DAO
{
    public class BrandDAO
    {
        //dependency injection
        private StoreContext _db;
        public BrandDAO(StoreContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Brand>> GetAll()
        {
            return await _db.Brands.ToListAsync<Brand>();
        }
    }
}

