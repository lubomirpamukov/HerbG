using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
