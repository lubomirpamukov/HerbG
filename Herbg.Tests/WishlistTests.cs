using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Models;
using Herbg.Services.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace YourNamespace.Tests
{
	[TestFixture]
	public class WishlistServiceTests
	{
		private DbContextOptions<ApplicationDbContext> _dbContextOptions= null!;
		private ApplicationDbContext _dbContext = null!;
		private WishlistService _wishlistService = null!;

		[SetUp]
		public void Setup()
		{
			// Set up the in-memory database for each test
			_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_dbContext = new ApplicationDbContext(_dbContextOptions);

			// Initialize WishlistService
			var wishlistRepo = new Repository<Wishlist>(_dbContext);
			var productRepo = new Repository<Product>(_dbContext);
			var cartRepo = new Repository<Cart>(_dbContext);

			_wishlistService = new WishlistService(wishlistRepo, productRepo, cartRepo);

			// Seed the database
			SeedDatabase();
		}

		[TearDown]
		public void TearDown()
		{
			// Clean up the in-memory database after each test
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}

		private void SeedDatabase()
		{
			// Seed products
			_dbContext.Products.AddRange(new List<Product>
			{
				new Product { Id = 1, Name = "Product 1", Description = "Description 1", ImagePath = "/images/product1.jpg", Price = 10.0m },
				new Product { Id = 2, Name = "Product 2", Description = "Description 2", ImagePath = "/images/product2.jpg", Price = 20.0m }
			});

			// Seed wishlists
			_dbContext.Wishlists.AddRange(new List<Wishlist>
			{
				new Wishlist { ClientId = "client1", ProductId = 1 },
				new Wishlist { ClientId = "client1", ProductId = 2 }
			});

			// Seed carts
			_dbContext.Carts.Add(new Cart
			{
				Id = "cartGuild",
				ClientId = "client1",
				CartItems = new List<CartItem>()
			});

			_dbContext.SaveChanges();
		}

		[Test]
		public async Task AddToWishlistAsync_ShouldAddItem_WhenItemDoesNotExist()
		{
			// Arrange
			var clientId = "client2";
			var productId = 1;

			// Act
			var result = await _wishlistService.AddToWishlistAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.True);
			var wishlistItem = await _dbContext.Wishlists.FirstOrDefaultAsync(w => w.ClientId == clientId && w.ProductId == productId);
			Assert.That(wishlistItem, Is.Not.Null);
		}

		[Test]
		public async Task GetClientWishlistAsync_ShouldReturnCorrectItems()
		{
			// Arrange
			var clientId = "client1";

			// Act
			var result = await _wishlistService.GetClientWishlistAsync(clientId);

			// Assert
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetWishlistItemCountAsync_ShouldReturnCorrectCount()
		{
			// Arrange
			var clientId = "client1";

			// Act
			var count = await _wishlistService.GetWishlistItemCountAsync(clientId);

			// Assert
			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public async Task MoveToCartAsync_ShouldMoveProductToCart()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;

			// Act
			var result = await _wishlistService.MoveToCartAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.True);
			var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId);
			Assert.That(cartItem, Is.Not.Null);

			var wishlistItem = await _dbContext.Wishlists.FirstOrDefaultAsync(w => w.ClientId == clientId && w.ProductId == productId);
			Assert.That(wishlistItem, Is.Null);
		}

		[Test]
		public async Task RemoveFromWishlistAsync_ShouldRemoveItem()
		{
			// Arrange
			var clientId = "client1";
			var productId = 1;

			// Act
			var result = await _wishlistService.RemoveFromWishlistAsync(clientId, productId);

			// Assert
			Assert.That(result, Is.True);
			var wishlistItem = await _dbContext.Wishlists.FirstOrDefaultAsync(w => w.ClientId == clientId && w.ProductId == productId);
			Assert.That(wishlistItem, Is.Null);
		}
	}
}
