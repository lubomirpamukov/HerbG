using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
