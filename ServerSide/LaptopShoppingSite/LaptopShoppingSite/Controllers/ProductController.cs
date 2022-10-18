using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaptopShoppingSite.DAL.DAO;
using LaptopShoppingSite.DAL.DomainClasses;
using LaptopShoppingSite.DAL;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShoppingSite.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        StoreContext _db;
        public ProductController(StoreContext context)
        {
            _db = context;
        }

        //we need tp pass a id as a parameter on the route.
        [Route("{brandid}")]
        public async Task<ActionResult<List<Product>>> Index(int brandid)
        {
            ProductDAO dao = new ProductDAO(_db);
            List<Product> itemsForCategory = await dao.GetAllByBrand(brandid);
            return itemsForCategory;
        }


    }
}
