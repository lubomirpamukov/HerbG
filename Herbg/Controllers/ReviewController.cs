using Microsoft.AspNetCore.Mvc;

namespace Herbg.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
