using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[TestFixture]
public class CartServiceTests
{
    private Mock<IRepository<Product>> _productMock;
    private Mock<IRepository<Cart>> _cartMock;
    private Mock<IRepository<Wishlist>> _wishlistMock;
    private CartService _cartService;

    [SetUp]
    public void SetUp()
    {
        _productMock = new Mock<IRepository<Product>>();
        _cartMock = new Mock<IRepository<Cart>>();
        _wishlistMock = new Mock<IRepository<Wishlist>>();
        _cartService = new CartService(_cartMock.Object, _productMock.Object,  _wishlistMock.Object);
    }

    [Test]
    public async Task AddItemToCartAsync_ProductNotFound_ReturnsFalse()
    {
        // Arrange
        _productMock.Setup(p => p.FindByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Product)null);

        // Act
        var result = await _cartService.AddItemToCartAsync("client123", 1, 1);

        // Assert
        Assert.That(result, Is.False);
        _cartMock.Verify(c => c.GetAllAttached(), Times.Never);
    }
/*
    [Test]
    public async Task AddItemToCartAsync_CartNotExist_AddsNewCartWithItem()
    {
        // Arrange
        var product = new Product { Id = 1, Price = 10.00m };
        _productMock.Setup(p => p.FindByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(product);

        // Mock GetAllAttached to return an empty IQueryable wrapped in async behavior
        _cartMock.Setup(c => c.GetAllAttached())
                 .Returns(Enumerable.Empty<Cart>());  // Use ToAsyncEnumerable to simulate async query

        Cart addedCart = null!;
        _cartMock.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                 .Callback<Cart>(c => addedCart = c)
                 .Returns(Task.FromResult(true));  // Returns Task<bool>

        // Act
        var result = await _cartService.AddItemToCartAsync("client123", 1, 2);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(addedCart, Is.Not.Null);
        Assert.That(addedCart.ClientId, Is.EqualTo("client123"));
        Assert.That(addedCart.CartItems.Count, Is.EqualTo(1));
        Assert.That(addedCart.CartItems.First().ProductId, Is.EqualTo(1));
        Assert.That(addedCart.CartItems.First().Quantity, Is.EqualTo(2));
    }*/

    [Test]
    public async Task AddItemToCartAsync_ProductAlreadyInCart_UpdatesQuantity()
    {
        // Arrange
        var product = new Product { Id = 1, Price = 10.00m };
        var cart = new Cart
        {
            Id = "guid",
            ClientId = "client123",
            CartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 1, Price = 10.00m }
            }
        };

        _productMock.Setup(p => p.FindByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(product);

        _cartMock.Setup(c => c.GetAllAttached())
                 .Returns(new List<Cart> { cart }.AsQueryable());

        // Act
        var result = await _cartService.AddItemToCartAsync("client123", 1, 2);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(cart.CartItems.First().Quantity, Is.EqualTo(3));
    }

    [Test]
    public async Task AddItemToCartAsync_NewItemAddedToExistingCart()
    {
        // Arrange
        var product = new Product { Id = 2, Price = 20.00m };
        var cart = new Cart
        {
            Id = "guid",
            ClientId = "client123",
            CartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 1, Price = 10.00m }
            }
        };

        _productMock.Setup(p => p.FindByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(product);

        _cartMock.Setup(c => c.GetAllAttached())
         .Returns(Enumerable.Empty<Cart>().AsQueryable());


        // Act
        var result = await _cartService.AddItemToCartAsync("client123", 2, 1);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(cart.CartItems.Count, Is.EqualTo(2));
        Assert.That(cart.CartItems.Any(c => c.ProductId == 2 && c.Quantity == 1), Is.True);
    }
}
