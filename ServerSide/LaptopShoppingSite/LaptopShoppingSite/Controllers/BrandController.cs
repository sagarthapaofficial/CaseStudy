using LaptopShoppingSite.DAL;
using LaptopShoppingSite.DAL.DomainClasses;
using LaptopShoppingSite.DAL.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShoppingSite.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        StoreContext _db;
        public BrandController(StoreContext context)
        {
            _db = context;
        }
        public async Task<ActionResult<List<Brand>>> Index()
        {
            BrandDAO dao = new BrandDAO(_db);
            List<Brand> allCategories = await dao.GetAll();
            return allCategories;
        }
    }
}
