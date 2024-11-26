using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
