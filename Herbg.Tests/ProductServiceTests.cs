using Microsoft.EntityFrameworkCore;
using Herbg.Models;
using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Services.Services;

namespace Herbg.Tests
{
	[TestFixture]
	public class ProductServiceTests
	{
		private ProductService _productService = null!;
		private DbContextOptions<ApplicationDbContext> _dbContextOptions = null!;
		private ApplicationDbContext _dbContext = null!;

		[SetUp]
		public void SetUp()
		{
			// Set up in-memory database
			_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "HerbgTestDb1234")
				.Options;
			_dbContext = new ApplicationDbContext(_dbContextOptions);

			// Ensure the database is created
			_dbContext.Database.EnsureCreated();

			// Create and inject the ProductService
			var productRepository = new Repository<Product>(_dbContext);
			_productService = new ProductService(productRepository);

			//Clean Initial db data
			var productsToRemove = _dbContext.Products.ToArray();
			_dbContext.Products.RemoveRange(productsToRemove);

			var categoryToRemove = _dbContext.Categories.ToArray();
			_dbContext.Categories.RemoveRange(categoryToRemove);

			var manufacturerToRemove = _dbContext.Manufactorers.ToArray();
			_dbContext.Manufactorers.RemoveRange(manufacturerToRemove);

			_dbContext.SaveChanges();
		}

		[TearDown]
		public void TearDown()
		{
			_dbContext.Products.RemoveRange(_dbContext.Products);  // Remove all products
			_dbContext.SaveChanges();  // Save changes after removal
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}


		[Test]
		public async Task GetAllProductsAsync_ShouldReturnProducts_WhenProductsExist()
		{
			// Arrange: Add test data to the in-memory database


			var product1 = new Product
			{
				Id = 1,
				Name = "Product 1",
				Description = "Description 1",
				ImagePath = "Path/To/Image1.jpg",
				Price = 100,
				IsDeleted = false
			};
			var product2 = new Product
			{
				Id = 2,
				Name = "Product 2",
				Description = "Description 2",
				ImagePath = "Path/To/Image2.jpg",
				Price = 200,
				IsDeleted = false
			};

			_dbContext.Products.AddRange(product1, product2);
			await _dbContext.SaveChangesAsync();

			// Act: Call the GetAllProductsAsync method
			var result = await _productService.GetAllProductsAsync();
			var resultList = result.ToList();
			// Assert: Verify the results
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(2)); //I have 4 pre seeded items
			Assert.That(resultList[0].Name, Is.EqualTo("Product 2"));
			Assert.That(resultList[1].Name, Is.EqualTo("Product 1"));
		}

		[Test]
		public async Task GetAllProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
		{


			// Act: Call the GetAllProductsAsync method when there are no products
			var result = await _productService.GetAllProductsAsync();


			// Assert: Verify the results
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(0)); // I have 4 products seeded on initialization
		}

		[Test]
		public async Task GetAllProductsAsync_ShouldNotReturnDeletedProducts()
		{
			// Arrange: Add test data with one product marked as deleted

			var product1 = new Product
			{
				Id = 1,
				Name = "Product 1",
				Description = "Description 1",
				ImagePath = "Path/To/Image1.jpg",
				Price = 100,
				IsDeleted = false
			};
			var product2 = new Product
			{
				Id = 2,
				Name = "Product 2",
				Description = "Description 2",
				ImagePath = "Path/To/Image2.jpg",
				Price = 200,
				IsDeleted = true
			};

			_dbContext.Products.AddRange(product1, product2);
			await _dbContext.SaveChangesAsync();

			// Act: Call the GetAllProductsAsync method
			var result = await _productService.GetAllProductsAsync();
			var resultList = result.ToList();

			// Assert: Verify the results
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
		{
			//Arrange 
			var product = new Product
			{
				Id = 1,
				Name = "Test Product",
				Description = "Test Description",
				ImagePath = "Path/To/Image.jpg",
				Price = 100,
				IsDeleted = false
			};

			_dbContext.Products.Add(product);
			await _dbContext.SaveChangesAsync();

			// Act: Call GetProductByIdAsync with the product ID
			var result = await _productService.GetProductByIdAsync(1);

			// Assert: Verify the result is the correct product
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Name, Is.EqualTo("Test Product"));
			Assert.That(result.Description, Is.EqualTo("Test Description"));
		}

		[Test]
		public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
		{
			// Act: Call GetProductByIdAsync with a non-existing product ID
			var result = await _productService.GetProductByIdAsync(999);

			// Assert: Verify the result is null (or handle this case as appropriate)
			Assert.That(result, Is.Null);  // Adjust this depending on your method's behavior
		}

		//GetProductDetails tests

		[Test]
		public async Task GetProductDetailsAsync_ShouldReturnProductDetails_WhenProductExists()
		{
			// Arrange: Add related entities first
			var category = new Category { Id = 10, Name = "Herbs", ImagePath = "images/category/test.jpg", Description = "Test description for the category" };
			var manufacturer = new Manufactorer { Id = 1, Name = "Green Growers", Address = "test street 1938743" };
			var client = new ApplicationUser { Id = "1", UserName = "JohnDoe", Address = "Test address" };
			_dbContext.Categories.Add(category);
			_dbContext.Manufactorers.Add(manufacturer);
			_dbContext.Users.Add(client);
			await _dbContext.SaveChangesAsync();

			var review = new Review { Id = 1, Description = "Great product!", Rating = 5, Client = client };
			var product = new Product
			{
				Id = 19,
				Name = "Mint",
				Description = "Fresh mint leaves",
				ImagePath = "Path/To/Image.jpg",
				Price = 5.99m,
				Category = category,
				Manufactorer = manufacturer,
				Reviews = new List<Review> { review }
			};
			_dbContext.Products.Add(product);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _productService.GetProductDetailsAsync(19);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Id, Is.EqualTo(19));
			Assert.That(result.Name, Is.EqualTo("Mint"));
			Assert.That(result.Category, Is.EqualTo("Herbs"));
			Assert.That(result.Manufactorer, Is.EqualTo("Green Growers"));
			Assert.That(result.Reviews.Count, Is.EqualTo(1));
			Assert.That(result.Reviews[0].ReviewerName, Is.EqualTo("JohnDoe"));
			Assert.That(result.Reviews[0].Rating, Is.EqualTo(5));
		}

		[Test]
		public async Task GetProductDetailsAsync_ShouldReturnNull_WhenProductDoesNotExist()
		{
			// Act: Call the GetProductDetailsAsync method with a non-existent product ID
			var result = await _productService.GetProductDetailsAsync(999);

			// Assert: Verify the result is null
			Assert.That(result, Is.Null);
		}

		[Test]
		public async Task GetProductDetailsAsync_ShouldReturnProductWithoutReviews_WhenNoReviewsExist()
		{
			// Arrange: Add related entities first
			var category = new Category
			{
				Id = 1,
				Name = "Herbs",
				ImagePath = "images/category/herbs.jpg",
				Description = "Aromatic and flavorful herbs for your kitchen"
			};
			var manufacturer = new Manufactorer
			{
				Id = 1,
				Name = "Green Growers",
				Address = "123 Green Street"
			};

			var product = new Product
			{
				Id = 2,
				Name = "Basil",
				Description = "Fresh basil leaves",
				ImagePath = "Path/To/BasilImage.jpg",
				Price = 4.99m,
				Category = category,
				Manufactorer = manufacturer,
				Reviews = new List<Review>() // No reviews
			};

			// Add entities to the database
			_dbContext.Categories.Add(category);
			_dbContext.Manufactorers.Add(manufacturer);
			_dbContext.Products.Add(product);
			await _dbContext.SaveChangesAsync();

			// Act: Call the GetProductDetailsAsync method
			var result = await _productService.GetProductDetailsAsync(2);

			// Assert: Verify the product details without reviews
			Assert.That(result, Is.Not.Null, "Product details should not be null.");
			Assert.That(result.Id, Is.EqualTo(2), "The product ID should match.");
			Assert.That(result.Name, Is.EqualTo("Basil"), "The product name should match.");
			Assert.That(result.Description, Is.EqualTo("Fresh basil leaves"), "The product description should match.");
			Assert.That(result.ImagePath, Is.EqualTo("Path/To/BasilImage.jpg"), "The product image path should match.");
			Assert.That(result.Price, Is.EqualTo(4.99m), "The product price should match.");
			Assert.That(result.Category, Is.EqualTo("Herbs"), "The category name should match.");
			Assert.That(result.Manufactorer, Is.EqualTo("Green Growers"), "The manufacturer name should match.");
			Assert.That(result.Reviews.Count, Is.EqualTo(0), "The product should have no reviews.");
		}



	}
}
