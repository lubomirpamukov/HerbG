using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
