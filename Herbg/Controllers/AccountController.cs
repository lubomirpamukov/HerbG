using Herbg.Models;
using Herbg.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Herbg.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateShippingAddress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // or a 404 error page
            }

            var model = new ShippingAddressViewModel
            {
                ShippingInformationAddress = user.ShippingInformationAddress,
                ShippingInformationFullName = user.ShippingInformationFullName,
                ShippingInformationCity = user.ShippingInformationCity,
                ShippingInformationZip = user.ShippingInformationZip,
                ShippingInformationCountry = user.ShippingInformationCountry
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShippingAddress(ShippingAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    user.ShippingInformationAddress = model.ShippingInformationAddress;
                    user.ShippingInformationFullName = model.ShippingInformationFullName;
                    user.ShippingInformationCity = model.ShippingInformationCity;
                    user.ShippingInformationZip = model.ShippingInformationZip;
                    user.ShippingInformationCountry = model.ShippingInformationCountry;

                    await _userManager.UpdateAsync(user);

                    TempData["SuccessMessage"] = "Shipping address updated successfully!";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }
    }
}
