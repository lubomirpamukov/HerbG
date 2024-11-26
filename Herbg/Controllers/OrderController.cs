using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
