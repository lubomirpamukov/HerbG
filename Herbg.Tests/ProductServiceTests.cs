using Microsoft.EntityFrameworkCore;
using Herbg.Models;
using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Services.Services;
using Herbg.Services.Interfaces;
using Moq;
using Herbg.ViewModels.Product;

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

            // Create the repositories
            var productRepository = new Repository<Product>(_dbContext);
            var categoryRepository = new Repository<Category>(_dbContext);
            var manufacturerRepository = new Repository<Manufactorer>(_dbContext);

            // Create mock or simple in-memory services
            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(service => service.GetAllCategoriesDbModelAsync()).Returns(new List<Category>());

            var manufacturerService = new Mock<IManufactorerService>();
            manufacturerService.Setup(service => service.GetAllManufactorersDbModel()).Returns(new List<Manufactorer>());

            // Create and inject the ProductService with all required services
            _productService = new ProductService(productRepository, categoryService.Object, manufacturerService.Object);

            // Clean initial db data
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
            var products = result.Products.ToList();  // Access the Products property

            // Assert: Verify the results
            Assert.Multiple(() =>
            {
                Assert.That(products, Is.Not.Null);
                Assert.That(products.Count, Is.EqualTo(2)); // Should match the 2 pre-seeded products
                Assert.That(products[0].Name, Is.EqualTo("Product 2"));
                Assert.That(products[1].Name, Is.EqualTo("Product 1"));
            });
        }


        [Test]
        public async Task GetAllProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Act: Call the GetAllProductsAsync method when there are no products
            var result = await _productService.GetAllProductsAsync();
            var products = result.Products.ToList();  // Access the Products property

            // Assert: Verify the results
            Assert.Multiple(() =>
            {
                Assert.That(products, Is.Not.Null);
                Assert.That(products.Count, Is.EqualTo(0));  // Ensure that no products are returned
            });
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
            var resultList = result.Products.ToList();  // Access the Products property

            // Assert: Verify the results
            Assert.Multiple(() =>
            {
                Assert.That(resultList, Is.Not.Null);
                Assert.That(resultList.Count, Is.EqualTo(1)); // Only 1 product should be returned (product1)
                Assert.That(resultList[0].Name, Is.EqualTo("Product 1")); // Ensure the product returned is not deleted
            });
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
			var client = new ApplicationUser { Id = "1", UserName = "JohnDoe", ShippingInformationAddress = "Test address" };
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

		//GetProductByCategory

		[Test]
		public async Task GetProductsByCategoryAsync_ShouldReturnProducts_WhenProductsExistInCategory()
		{
			// Arrange: Add a category and related products
			var category = new Category { Id = 1, Name = "Herbs" , ImagePath="images/test/category.jpg", Description ="Test description for category"};
			_dbContext.Categories.Add(category);

			var products = new List<Product>
		{
			new Product { Id = 1, Name = "Mint", Description = "Fresh mint leaves", ImagePath = "images/mint.jpg", Price = 2.99m, CategoryId = 1, IsDeleted = false },
			new Product { Id = 2, Name = "Basil", Description = "Fresh basil leaves", ImagePath = "images/basil.jpg", Price = 3.99m, CategoryId = 1, IsDeleted = false }
		};
			_dbContext.Products.AddRange(products);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _productService.GetProductsByCategoryAsync(1);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result.Any(p => p.Name == "Mint"), Is.True);
			Assert.That(result.Any(p => p.Name == "Basil"), Is.True);
		}

		[Test]
		public async Task GetProductsByCategoryAsync_ShouldReturnEmptyCollection_WhenNoProductsExistInCategory()
		{
			// Arrange: Add a category without products
			var category = new Category { Id = 2, Name = "Empty Category" , ImagePath="images/category/her.jpg", Description="Test description for category"};
			_dbContext.Categories.Add(category);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _productService.GetProductsByCategoryAsync(2);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task GetProductsByCategoryAsync_ShouldExcludeDeletedProducts()
		{
			// Arrange: Add a category with some deleted products
			var category = new Category { Id = 3, Name = "Herbs", ImagePath = "images/category/her.jpg", Description = "Test description for category" };
			_dbContext.Categories.Add(category);

			var products = new List<Product>
		{
			new Product { Id = 3, Name = "Cilantro", Description = "Fresh cilantro", ImagePath = "images/cilantro.jpg", Price = 1.99m, CategoryId = 3, IsDeleted = false },
			new Product { Id = 4, Name = "Parsley", Description = "Fresh parsley", ImagePath = "images/parsley.jpg", Price = 2.49m, CategoryId = 3, IsDeleted = true }
		};
			_dbContext.Products.AddRange(products);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _productService.GetProductsByCategoryAsync(3);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result.Any(p => p.Name == "Cilantro"), Is.True);
			Assert.That(result.Any(p => p.Name == "Parsley"), Is.False);
		}

        [Test]
        public async Task GetHomePageProductsAsync_ShouldReturnTop4Products()
        {
            // Arrange: Add test data to the in-memory database
            var products = new List<Product>
			 {
			     new Product { Id = 1, Name = "Product 1", ImagePath = "Path/To/Image1.jpg", Description = "Description 1", Price = 100, IsDeleted = false },
			     new Product { Id = 2, Name = "Product 2", ImagePath = "Path/To/Image2.jpg", Description = "Description 2", Price = 200, IsDeleted = false },
			     new Product { Id = 3, Name = "Product 3", ImagePath = "Path/To/Image3.jpg", Description = "Description 3", Price = 300, IsDeleted = false },
			     new Product { Id = 4, Name = "Product 4", ImagePath = "Path/To/Image4.jpg", Description = "Description 4", Price = 400, IsDeleted = false },
			     new Product { Id = 5, Name = "Product 5", ImagePath = "Path/To/Image5.jpg", Description = "Description 5", Price = 500, IsDeleted = false }
			 };

            _dbContext.Products.AddRange(products);
            await _dbContext.SaveChangesAsync();

            // Act: Call the method
            var result = await _productService.GetHomePageProductsAsync();

            // Assert: Validate the results
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(4), "Should return exactly 4 products.");
            Assert.That(result.Any(p => p.Name == "Product 1"), "Should include 'Product 1'.");
            Assert.That(result.Any(p => p.Name == "Product 4"), "Should include 'Product 4'.");
            Assert.That(result.Any(p => p.Name == "Product 5"), Is.False, "'Product 5' should not be included as it exceeds the top 4.");
        }

        [Test]
        public async Task GetHomePageProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Act: Call the method when there are no products
            var result = await _productService.GetHomePageProductsAsync();

            // Assert: Validate the results
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(0), "Result should be an empty list.");
        }

        [Test]
        public async Task GetHomePageProductsAsync_ShouldExcludeDeletedProducts()
        {
            // Arrange: Add test data with some deleted products
            var products = new List<Product>
			 {
			     new Product { Id = 1, Name = "Product 1", ImagePath = "Path/To/Image1.jpg", Description = "Description 1", Price = 100, IsDeleted = false },
			     new Product { Id = 2, Name = "Product 2", ImagePath = "Path/To/Image2.jpg", Description = "Description 2", Price = 200, IsDeleted = false },
			     new Product { Id = 3, Name = "Product 3", ImagePath = "Path/To/Image3.jpg", Description = "Description 3", Price = 300, IsDeleted = true },
			     new Product { Id = 4, Name = "Product 4", ImagePath = "Path/To/Image4.jpg", Description = "Description 4", Price = 400, IsDeleted = false },
			     new Product { Id = 5, Name = "Product 5", ImagePath = "Path/To/Image5.jpg", Description = "Description 5", Price = 500, IsDeleted = false }
			 };

            _dbContext.Products.AddRange(products);
            await _dbContext.SaveChangesAsync();

            // Act: Call the method
            var result = await _productService.GetHomePageProductsAsync();

            // Assert: Validate the results
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(4), "Should return 4 products excluding the deleted one.");
            Assert.That(result.Any(p => p.Name == "Product 3"), Is.False, "'Product 3' should not be included as it is deleted.");
        }

        [Test]
        public async Task SoftDeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Act
            var result = await _productService.SoftDeleteProductAsync(999);

            // Assert
            Assert.That(result, Is.False, "SoftDeleteProductAsync should return false when product does not exist.");
        }

        [Test]
        public async Task SoftDeleteProductAsync_ShouldSoftDeleteProductAndRelatedEntities()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                IsDeleted = false,
                Description = "Test description 1",
                Reviews = new List<Review>
            {
                new Review { Id = 1, Description = "Great product!", ClientId ="guid" },
                new Review { Id = 2, Description = "Not bad.", ClientId ="guid" }
            }
            };

            var cartItems = new List<CartItem>
        {
            new CartItem { CartId = "guid", ProductId = 1 },
            new CartItem { CartId = "guid2", ProductId = 1 }
        };

            var wishlists = new List<Wishlist>
        {
            new Wishlist { Id = 1, ProductId = 1 ,ClientId ="guid" },
            new Wishlist { Id = 2, ProductId = 1, ClientId ="guid" }
        };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.CartItems.AddRangeAsync(cartItems);
            await _dbContext.Wishlists.AddRangeAsync(wishlists);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productService.SoftDeleteProductAsync(1);

            // Assert
            Assert.That(result, Is.True, "SoftDeleteProductAsync should return true when product is successfully soft-deleted.");
            var deletedProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == 1);
            Assert.That(deletedProduct!.IsDeleted, Is.True, "Product should be marked as deleted.");

            var remainingCartItems = await _dbContext.CartItems.Where(ci => ci.ProductId == 1).ToListAsync();
            Assert.That(remainingCartItems, Is.Empty, "Cart items associated with the product should be removed.");

            var remainingWishlists = await _dbContext.Wishlists.Where(w => w.ProductId == 1).ToListAsync();
            Assert.That(remainingWishlists, Is.Empty, "Wishlists associated with the product should be removed.");

            var remainingReviews = await _dbContext.Reviews.Where(r => r.ProductId == 1).ToListAsync();
            Assert.That(remainingReviews, Is.Empty, "Reviews associated with the product should be removed.");
        }

        [Test]
        public async Task SoftDeleteProductAsync_ShouldNotAffectUnrelatedEntities()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", IsDeleted = false, Description ="Test description 1" };
            var unrelatedProduct = new Product { Id = 2, Name = "Unrelated Product", IsDeleted = false, Description = "Test description 1" };

            await _dbContext.Products.AddRangeAsync(product, unrelatedProduct);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productService.SoftDeleteProductAsync(1);

            // Assert
            Assert.That(result, Is.True, "SoftDeleteProductAsync should return true when product is successfully soft-deleted.");
            var unaffectedProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == 2);
            Assert.That(unaffectedProduct!.IsDeleted, Is.False, "Unrelated product should not be affected.");
        }

        [Test]
        public async Task GetProductForEditAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _productService.GetProductForEditAsync(999);

            // Assert
            Assert.That(result, Is.Null, "GetProductForEditAsync should return null when the product does not exist.");
        }

        [Test]
        public async Task GetProductForEditAsync_ShouldReturnCorrectViewModel_WhenProductExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category", Description = "Test description", ImagePath = "img/test/category.jpg" };
            var manufacturer = new Manufactorer { Id = 1, Name = "Test Manufacturer", Address = "Test adress " };

            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100.00m,
                Description = "Test Description",
                ImagePath = "test-image.jpg",
                Category = category,
                Manufactorer = manufacturer,
                CategoryId = category.Id,
                ManufactorerId = manufacturer.Id
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.Manufactorers.AddAsync(manufacturer);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductForEditAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null, "GetProductForEditAsync should return a valid view model when the product exists.");
            Assert.That(result!.Id, Is.EqualTo(product.Id), "View model should have the correct product ID.");
            Assert.That(result.Name, Is.EqualTo(product.Name), "View model should have the correct product name.");
            Assert.That(result.Price, Is.EqualTo(product.Price), "View model should have the correct product price.");
            Assert.That(result.Description, Is.EqualTo(product.Description), "View model should have the correct product description.");
            Assert.That(result.ImagePath, Is.EqualTo(product.ImagePath), "View model should have the correct image path.");
            Assert.That(result.CategoryId, Is.EqualTo(product.CategoryId), "View model should have the correct category ID.");
            Assert.That(result.ManufactorerId, Is.EqualTo(product.ManufactorerId), "View model should have the correct manufacturer ID.");
        }

        [Test]
        public async Task GetProductForEditAsync_ShouldNotAffectOtherProducts()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category for Unrelated Product", Description="Test description", ImagePath="img/test/category.jpg" };
            var manufacturer = new Manufactorer { Id = 1, Name = "Manufacturer for Unrelated Product" , Address ="Test adress "};

            var unrelatedProduct = new Product
            {
                Id = 2,
                Name = "Unrelated Product",
                Price = 50.00m,
                Description = "Unrelated Description",
                ImagePath = "unrelated-image.jpg",
                Category = category,
                Manufactorer = manufacturer,
                CategoryId = category.Id,
                ManufactorerId = manufacturer.Id
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.Manufactorers.AddAsync(manufacturer);
            await _dbContext.Products.AddAsync(unrelatedProduct);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductForEditAsync(2);

            // Assert
            Assert.That(result, Is.Not.Null, "GetProductForEditAsync should return a valid view model when unrelated product exists.");
            Assert.That(result!.Id, Is.EqualTo(unrelatedProduct.Id), "Should return the correct unrelated product.");
            Assert.That(result.CategoryId, Is.EqualTo(unrelatedProduct.CategoryId), "Should return the correct category ID.");
            Assert.That(result.ManufactorerId, Is.EqualTo(unrelatedProduct.ManufactorerId), "Should return the correct manufacturer ID.");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var model = new CreateProductViewModel
            {
                Id = 999, // Non-existent product ID
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 200.00m,
                CategoryId = 1,
                ManufactorerId = 1,
                ImagePath = "updated-image.jpg"
            };

            // Act
            var result = await _productService.UpdateProductAsync(model);

            // Assert
            Assert.That(result, Is.False, "UpdateProductAsync should return false if the product does not exist.");
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1", Description = "Category Description", ImagePath = "img/category.jpg" };
            var manufacturer = new Manufactorer { Id = 1, Name = "Manufacturer 1", Address = "Address 1" };

            var product = new Product
            {
                Id = 1,
                Name = "Original Product",
                Description = "Original Description",
                Price = 100.00m,
                CategoryId = category.Id,
                ManufactorerId = manufacturer.Id,
                ImagePath = "original-image.jpg",
                Category = category,
                Manufactorer = manufacturer
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.Manufactorers.AddAsync(manufacturer);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var model = new CreateProductViewModel
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 200.00m,
                CategoryId = 1,
                ManufactorerId = 1,
                ImagePath = "updated-image.jpg"
            };

            // Act
            var result = await _productService.UpdateProductAsync(model);

            // Assert
            Assert.That(result, Is.True, "UpdateProductAsync should return true when the product is successfully updated.");

            var updatedProduct = await _dbContext.Products.FindAsync(1);
            Assert.That(updatedProduct, Is.Not.Null, "Product should still exist in the database.");
            Assert.That(updatedProduct!.Name, Is.EqualTo("Updated Product"), "Product name should be updated.");
            Assert.That(updatedProduct.Description, Is.EqualTo("Updated Description"), "Product description should be updated.");
            Assert.That(updatedProduct.Price, Is.EqualTo(200.00m), "Product price should be updated.");
            Assert.That(updatedProduct.ImagePath, Is.EqualTo("updated-image.jpg"), "Product image path should be updated.");
            Assert.That(updatedProduct.CategoryId, Is.EqualTo(1), "Category ID should remain the same.");
            Assert.That(updatedProduct.ManufactorerId, Is.EqualTo(1), "Manufacturer ID should remain the same.");
        }

        [Test]
        public async Task AddProductAsync_ShouldReturnTrue_WhenProductIsAddedSuccessfully()
        {
            // Arrange
            var model = new CreateProductViewModel
            {
                Name = "New Product",
                Price = 150.00m,
                Description = "New product description",
                ManufactorerId = 1,
                CategoryId = 1,
                ImagePath = "new-product.jpg"
            };

            // Act
            var result = await _productService.AddProductAsync(model);

            // Assert
            Assert.That(result, Is.True, "AddProductAsync should return true when the product is successfully added.");
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == "New Product");
            Assert.That(product, Is.Not.Null, "Product should exist in the database.");
            Assert.That(product!.Name, Is.EqualTo("New Product"), "Product name should match.");
            Assert.That(product.Price, Is.EqualTo(150.00m), "Product price should match.");
            Assert.That(product.Description, Is.EqualTo("New product description"), "Product description should match.");
            Assert.That(product.ImagePath, Is.EqualTo("new-product.jpg"), "Product image path should match.");
        }

        
    }
}
