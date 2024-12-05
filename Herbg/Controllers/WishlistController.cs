using Herbg.Data;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Wishlist;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class WishlistController(UserManager<ApplicationUser> userManager, IWishlistService wishlistService) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IWishlistService _wishlistService = wishlistService;

    
    public async Task<IActionResult> Index()
    {
        //Check if client exist
        var clientId = _userManager.GetUserId(User);
        if (clientId == null)
        { 
            return NotFound();
        }

        var clientWishlists = await _wishlistService.GetClientWishlistAsync(clientId);

        return View(clientWishlists);
    }

    public async Task<IActionResult> Add(int productId) 
    {
        //Check if client exist
        var clientId = _userManager.GetUserId(User);
        if (clientId == null)
        {
            return NotFound();
        }

        var isItemAddedToWishlist = await _wishlistService.AddToWishlistAsync(clientId, productId);

        if (!isItemAddedToWishlist)
        {
            return NotFound();
        }

        return RedirectToAction("Index","Product");
    }

    public async Task<IActionResult> RemoveFromWishlist(int productId) 
    {
        //Check if client exist
        var clientId = _userManager.GetUserId(User);
        if (clientId == null)
        {
            return NotFound();
        }

        var isItemRemoved = await _wishlistService.RemoveFromWishlistAsync(clientId, productId);

        if (!isItemRemoved)
        {
            return NotFound();
        }

        return RedirectToAction("Index", "Wishlist");
    }

    public async Task<IActionResult> MoveToCart(int productId)
    {
        // Check if client exists
        var clientId = _userManager.GetUserId(User);
        if (clientId == null)
        {
            return NotFound("Client not found.");
        }

       var isProductMoved = await _wishlistService.MoveToCartAsync(clientId, productId);

        if (!isProductMoved)
        {
            return NotFound();
        }

        return RedirectToAction("Index", "Wishlist");
    }

    public async Task<IActionResult> GetWishlistItemCount()
    {
        var clientId = _userManager.GetUserId(User);

        if (string.IsNullOrWhiteSpace(clientId))
        {
            return NotFound();
        }

        var wishlistCount = await _wishlistService.GetWishlistItemCountAsync(clientId);
        return Json(wishlistCount);  // Returns the count as JSON
    }
}
