using Herbg.Data;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Wishlist;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbg.Controllers;

public class WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWishlistService wishlistService) : Controller
{
    private readonly ApplicationDbContext _context = context;
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

        var isItemRemoved = await _wishlistService.RemoveFromWishlist(clientId, productId);

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

        // Check if the product exists
        var productToAdd = await _context.Products.FindAsync(productId);
        if (productToAdd == null)
        {
            return NotFound("Product not found.");
        }

        // Check if client has a cart
        var clientCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                                             .FirstOrDefaultAsync(c => c.ClientId == clientId);
        if (clientCart == null)
        {
            clientCart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
            _context.Carts.Add(clientCart);
            await _context.SaveChangesAsync();
        }

        // Check if the product is already in the cart
        var existingCartItem = clientCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingCartItem == null)
        {
            // Add product to cart
            var cartItem = new CartItem 
            { 
                ProductId = productId,
                CartId = clientCart.Id,
                Price = productToAdd.Price,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
        }

        // Remove product from wishlist
        var productWishlist = await _context.Wishlists
            .FirstOrDefaultAsync(w => w.ClientId == clientId && w.ProductId == productId);
        if (productWishlist == null)
        {
            return NotFound("Product not found in wishlist.");
        }
        _context.Wishlists.Remove(productWishlist);

        // Save changes
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Wishlist");
    }

    public async Task<IActionResult> GetWishlistItemCount()
    {
        var clientId = _userManager.GetUserId(User);
        var wishlist = await _context.Wishlists
            .Where(w => w.ClientId == clientId)
            .ToArrayAsync();

        var wishlistCount = wishlist?.Count() ?? 0;
        return Json(wishlistCount);  // Returns the count as JSON
    }
}
