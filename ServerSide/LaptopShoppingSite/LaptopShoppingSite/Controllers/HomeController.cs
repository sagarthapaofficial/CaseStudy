using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShoppingSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {


        //It is the default method for controller.
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "Server started";
        }
    }
}
