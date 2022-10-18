using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShoppingSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        ////Dependency Injection
        //AppDbContext _ctx;
        //public DataController(AppDbContext context) // injected here
        //{
        //    _ctx = context;
        //}

        private async Task<String> getMenuItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/elauersen/info3067/master/mcdonalds.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<IActionResult> Index()
        {
            var json = await getMenuItemJsonFromWebAsync();
            return Content(json);
        }
    }
}
