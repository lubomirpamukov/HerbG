using Herbg.Data;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Review;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IReviewService reviewService) : Controller
{
    private readonly IReviewService _reviewService = reviewService;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    
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

        var viewForm = await _reviewService.GetReviewFormAsync(clientId,id);
        
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

        var isReviewCreated = await _reviewService.UpdateReviewAsync(clientId, model);

        if (!isReviewCreated)
        {
            return NotFound();
        }

        return RedirectToAction("Details", "Product",new {id = model.Id });
    }
}
