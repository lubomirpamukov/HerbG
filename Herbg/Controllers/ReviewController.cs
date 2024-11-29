using Herbg.Data;
using Herbg.Models;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id) 
        {
            //Check user
            var clientId = _userManager.GetUserId(User);

            if (clientId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = await  _context.Clients.FindAsync(clientId);
            //Create a view form
            var viewForm = new ReviewViewModel { Id = id,ReviewerName = client?.Email ?? "Anonymouse", };
            return View(viewForm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            //Check user
            var clientId = _userManager.GetUserId(User);

            if (clientId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = await _context.Clients.FindAsync(clientId);

            //Check if customer wrote a review for this product
            var customerReview = await _context.Reviews
                .FirstOrDefaultAsync(review => review.ProductId == model.Id && review.ClientId == clientId);
            if (customerReview != null)
            {
                //Delete old review
                _context.Reviews.Remove(customerReview);
                await _context.SaveChangesAsync();
            }

            //Create a review from the viewModel
            var newReview = new Review
            {
                ClientId = clientId,
                Description = model.Description,
                ProductId = model.Id,
                Rating = model.Rating,
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Product",new {id = model.Id });
        }
    }
}
