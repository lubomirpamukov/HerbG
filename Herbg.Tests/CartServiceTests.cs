using Herbg.Models;
using Herbg.Services.Services;
using Herbg.Infrastructure.Interfaces;
using Moq;
using Microsoft.EntityFrameworkCore;

using Herbg.Infrastructure;
using Herbg.Data;

namespace Herbg.Services.Tests
{
	[TestFixture]
	public class CartServiceTests
	{
		private CartService _cartService;
		private Mock<IRepository<Product>> _productRepoMock;
		private Mock<IRepository<Wishlist>> _wishlistRepoMock;
		private DbContextOptions<ApplicationDbContext> _dbContextOptions;
		private ApplicationDbContext _dbContext;
		private IRepository<Cart> _cartRepo;

		[SetUp]
		public void SetUp()
		{
			// Setup In-Memory Database
			_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
								.UseInMemoryDatabase(databaseName: "HerbgTestDb")
								.Options;

			_dbContext = new ApplicationDbContext(_dbContextOptions);
			_cartRepo = new Repository<Cart>(_dbContext);
			_productRepoMock = new Mock<IRepository<Product>>();
			_wishlistRepoMock = new Mock<IRepository<Wishlist>>();

			_cartService = new CartService(_cartRepo, _productRepoMock.Object, _wishlistRepoMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			// Clear the in-memory database after each test
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}

		[Test]
		public async Task AddItemToCartAsync_ShouldAddItem_WhenProductExists()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 2;
			var product = new Product { Id = productId, Name = "Herb", Price = 10 , Description = "test test description", ImagePath = "images/test/test" };

			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync(product);

			// Act
			var result = await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Assert
			Assert.That(result, Is.True);
			var cart = await _cartRepo.GetAllAttached()
									   .FirstOrDefaultAsync(c => c.ClientId == clientId);
			var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
			Assert.That(cartItem, Is.Not.Null);
			Assert.That(cartItem.Quantity, Is.EqualTo(quantity));
		}
		



		[Test]
		public async Task AddItemToCartAsync_ShouldReturnFalse_WhenProductDoesNotExist()
		{
			// Arrange
			var clientId = "client1";
			var productId = 999;
			var quantity = 2;

			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync((Product)null);

			// Act
			var result = await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public async Task GetCartItemsCountAsync_ShouldReturnCorrectCount()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 2;
			var product = new Product { Id = productId, Name = "Herb", Price = 10 };
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			_dbContext.Carts.Add(cart);
			_dbContext.SaveChanges();
			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync(product);

			await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Act
			var itemCount = await _cartService.GetCartItemsCountAsync(clientId);

			// Assert
			Assert.That(itemCount, Is.EqualTo(quantity));
		}

		[Test]
		public async Task GetUserCartAsync_ShouldReturnCartViewModel_WhenCartExists()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 2;
			var product = new Product { Id = productId, Name = "Herb", Price = 10 };
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			_dbContext.Carts.Add(cart);
			_dbContext.SaveChanges();
			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync(product);

			await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Act
			var cartViewModel = await _cartService.GetUserCartAsync(clientId);

			

			// Assert
			Assert.That(cartViewModel, Is.Not.Null);

		}

		[Test]
		public async Task RemoveCartItemAsync_ShouldRemoveItem_WhenItemExists()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 2;
			var product = new Product { Id = productId, Name = "Herb", Price = 10 };
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			_dbContext.Carts.Add(cart);
			_dbContext.SaveChanges();
			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync(product);

			await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Act
			var result = await _cartService.RemoveCartItemAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.True);
			cart = await _cartRepo.GetAllAttached()
								   .FirstOrDefaultAsync(c => c.ClientId == clientId);
			Assert.That(cart?.CartItems.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task MoveCartItemToWishListAsync_ShouldReturnTrue_WhenItemMoved()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 2;
			var product = new Product { Id = productId, Name = "Herb", Price = 10 };
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			_dbContext.Carts.Add(cart);
			_dbContext.SaveChanges();
			_productRepoMock.Setup(p => p.FindByIdAsync(productId)).ReturnsAsync(product);

			await _cartService.AddItemToCartAsync(clientId, productId, quantity);

			// Act
			var result = await _cartService.MoveCartItemToWishListAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public async Task UpdateCartItemQuantityAsync_ShouldReturnTrue_WhenItemExistsInCart()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var initialQuantity = 2;
			var newQuantity = 5;
			var product = new Product { Id = productId, Name = "Herb", Price = 10, Description = "test test description", ImagePath = "images/test/test" };

			_dbContext.Products.Add(product);
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			var cartItem = new CartItem { ProductId = productId, CartId = cart.Id, Quantity = initialQuantity, Price = product.Price };
			cart.CartItems.Add(cartItem);
			_dbContext.Carts.Add(cart);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _cartService.UpdateCartItemQuantityAsync(clientId, productId, newQuantity);

			// Assert
			Assert.That(result, Is.True);
			var updatedCart = await _dbContext.Carts.Include(c => c.CartItems)
													 .FirstOrDefaultAsync(c => c.ClientId == clientId);
			var updatedItem = updatedCart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
			Assert.That(updatedItem, Is.Not.Null);
			Assert.That(updatedItem?.Quantity, Is.EqualTo(newQuantity)); 
		}

		[Test]
		public async Task UpdateCartItemQuantityAsync_ShouldReturnFalse_WhenCartDoesNotExist()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 5;

			// Act
			var result = await _cartService.UpdateCartItemQuantityAsync(clientId, productId, quantity);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public async Task UpdateCartItemQuantityAsync_ShouldReturnFalse_WhenProductDoesNotExistInCart()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var quantity = 5;
			var nonExistentProductId = 999; // Product ID that does not exist
			var product = new Product { Id = nonExistentProductId, Name = "Non-Existent Product", Price = 20 ,Description = "test test description", ImagePath="images/test/test"};

			// Create and save the product and cart without the specific productId
			_dbContext.Products.Add(product);
			var cart = new Cart { ClientId = clientId, CartItems = new List<CartItem>() };
			_dbContext.Carts.Add(cart);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _cartService.UpdateCartItemQuantityAsync(clientId, nonExistentProductId, quantity);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public async Task GetUserCartAsync_ShouldCreateNewCart_WhenClientCartIsNull()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;
			var product = new Product { Id = productId, Name = "Herb", Price = 10, Description = "test test description", ImagePath = "images/test/test" };

			// Add product to the in-memory database
			_dbContext.Products.Add(product);
			await _dbContext.SaveChangesAsync();

			// Act
			var cartViewModel = await _cartService.GetUserCartAsync(clientId);

			// Assert
			Assert.That(cartViewModel, Is.Not.Null); 
			Assert.That(cartViewModel.Id, Is.Not.EqualTo(string.Empty)); 
			Assert.That(cartViewModel.CartItems, Is.Empty); 

			// Ensure that the cart was added to the database
			var clientCart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId);
			Assert.That(clientCart, Is.Not.Null); 
			Assert.That(clientCart.CartItems.Count, Is.EqualTo(0)); 
		}

		[Test]
		public async Task GetCartItemsCountAsync_ShouldCreateNewCart_WhenCartDoesNotExist()
		{
			// Arrange
			var clientId = "client1";

			var existingCart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId);
			Assert.That(existingCart, Is.Null, "A cart already exists for the client before the test starts.");

			// Act
			var itemCount = await _cartService.GetCartItemsCountAsync(clientId);

			// Assert
			Assert.That(itemCount, Is.EqualTo(0), "The item count should be 0 for a new cart.");

			// Verify that a new cart has been created
			var newCart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId);
			Assert.That(newCart, Is.Not.Null, "A new cart should have been created for the client.");
			Assert.That(newCart!.CartItems.Count, Is.EqualTo(0), "The newly created cart should not have any items.");
		}

		[Test]
		public async Task MoveCartItemToWishListAsync_ShouldReturnFalse_WhenCartIsNull()
		{
			// Arrange
			var clientId = "nonexistent-client";
			var productId = 1;

			// Act
			var result = await _cartService.MoveCartItemToWishListAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.False, "The method should return false when no cart exists for the client.");
		}

		[Test]
		public async Task MoveCartItemToWishListAsync_ShouldReturnFalse_WhenCartItemsIsEmpty()
		{
			// Arrange
			var clientId = "existing-client";
			var productId = 1;

			var cart = new Cart
			{
				ClientId = clientId,
				CartItems = new List<CartItem>() // Empty cart items
			};

			_dbContext.Carts.Add(cart);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _cartService.MoveCartItemToWishListAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.False, "The method should return false when the cart exists but has no items.");
		}

		[Test]
		public async Task RemoveCartItemAsync_ShouldReturnFalse_WhenClientCartIsNull()
		{
			// Arrange
			var clientId = "nonexistent-client";
			var productId = 1;

			// Act
			var result = await _cartService.RemoveCartItemAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.False, "The method should return false when the client's cart does not exist.");
		}

		[Test]
		public async Task RemoveCartItemAsync_ShouldReturnFalse_WhenProductToRemoveIsNull()
		{
			// Arrange
			var clientId = "existing-client";
			var productId = 1; // Non-existent product ID

			var cart = new Cart
			{
				ClientId = clientId,
				CartItems = new List<CartItem>() // Cart exists but does not contain the product
			};

			_dbContext.Carts.Add(cart);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _cartService.RemoveCartItemAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.False, "The method should return false when the product to remove is not in the cart.");
		}
	}
}
