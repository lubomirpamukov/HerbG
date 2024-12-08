// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    function updateCartItemCount() {
        $.get("/Cart/GetCartItemCount", function (cartItemCount) {
            $('#cartItemCount').text(cartItemCount);
        }).fail(function () {
            console.error("Failed to fetch cart item count.");
        });
    }

    function updateWishlistItemCount() {
        $.get("/Wishlist/GetWishlistItemCount", function (wishlistItemCount) {
            $('#wishListItemCount').text(wishlistItemCount);
        }).fail(function () {
            console.error("Failed to fetch wishlist item count.");
        });
    }

    // Initial calls on page load
    updateCartItemCount();
    updateWishlistItemCount();

    // Optional: Trigger updates when forms are submitted
    $('form').submit(function () {
        updateCartItemCount();
        updateWishlistItemCount();
    });
});
