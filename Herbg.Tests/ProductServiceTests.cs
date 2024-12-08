using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Herbg.Models;
using Herbg.Data;
using Herbg.ViewModels;
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
				Id = 5,
				Name = "Product 1",
				Description = "Description 1",
				ImagePath = "Path/To/Image1.jpg",
				Price = 100,
				IsDeleted = false
			};
			var product2 = new Product
			{
				Id = 6,
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
			Assert.That(result.Count, Is.EqualTo(6)); //I have 4 pre seeded items
			Assert.That(resultList[4].Name, Is.EqualTo("Product 1"));
			Assert.That(resultList[5].Name, Is.EqualTo("Product 2"));
		}

		[Test]
		public async Task GetAllProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
		{
			// Act: Call the GetAllProductsAsync method when there are no products
			var result = await _productService.GetAllProductsAsync();

			// Assert: Verify the results
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(4)); // I have 4 products seeded on initialization
		}

		[Test]
		public async Task GetAllProductsAsync_ShouldNotReturnDeletedProducts()
		{
			// Arrange: Add test data with one product marked as deleted
			var product1 = new Product
			{
				Id = 5,
				Name = "Product 1",
				Description = "Description 1",
				ImagePath = "Path/To/Image1.jpg",
				Price = 100,
				IsDeleted = false
			};
			var product2 = new Product
			{
				Id = 40,
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
			Assert.That(result.Count(), Is.EqualTo(5)); //include the preloaded products
		}

		[Test]
		public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
		{
			// Arrange: Add a test product
			var product = new Product
			{
				Id = 5,
				Name = "Test Product",
				Description = "Test Description",
				ImagePath = "Path/To/Image.jpg",
				Price = 100,
				IsDeleted = false
			};

			_dbContext.Products.Add(product);
			await _dbContext.SaveChangesAsync();

			// Act: Call GetProductByIdAsync with the product ID
			var result = await _productService.GetProductByIdAsync(5);

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

		
	}
}
