using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.Services.Services;
using Herbg.ViewModels.Category;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class CategoryServiceTests
{
	private ApplicationDbContext _dbContext = null!;
	private IRepository<Category> _categoryRepository = null!;
	private IRepository<Product> _productRepository = null!;
    private ICategoryService _categoryService = null!;

	[SetUp]
	public void SetUp()
	{
		// Create in-memory database options
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDb")
			.Options;

		_dbContext = new ApplicationDbContext(options);
		_categoryRepository = new Repository<Category>(_dbContext);
		_productRepository = new Repository<Product>(_dbContext);

		_categoryService = new CategoryService(_categoryRepository, _productRepository);
	}

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
	public async Task GetAllCategoriesAsync_ShouldReturnNull_WhenNoCategoriesExist()
	{
		// Act
		var result = await _categoryService.GetAllCategoriesAsync();

		// Assert
		Assert.That(result, Is.Not.Null);
	}

	[Test]
	public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
	{
		// Arrange
		var categories = new List<Category>
		{
			new Category { Id = 1, Name = "Herbs", ImagePath = "herbs.jpg", Description = "All about herbs" },
			new Category { Id = 2, Name = "Spices", ImagePath = "spices.jpg", Description = "All about spices" }
		};

		await _dbContext.Categories.AddRangeAsync(categories);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _categoryService.GetAllCategoriesAsync();

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null.");
		Assert.That(result.Count, Is.EqualTo(2), "Result should contain all categories.");
		Assert.That(result.Any(c => c.Name == "Herbs"), "Result should include 'Herbs' category.");
		Assert.That(result.Any(c => c.Name == "Spices"), "Result should include 'Spices' category.");
	}

    [Test]
    public async Task GetCategoriesAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Act: Call the service method when no categories exist
        var result = await _categoryService.GetCategoriesAsync();

        // Assert: The result should not be null, but should be an empty list
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0), "Result should be an empty list when no categories exist.");
    }

    [Test]
    public async Task GetCategoriesAsync_ShouldReturnAllCategories_WhenCategoriesExist()
    {
        // Arrange: Add categories to the in-memory database
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Herbs", ImagePath = "herbs.jpg", Description = "All about herbs", IsDeleted = false },
            new Category { Id = 2, Name = "Spices", ImagePath = "spices.jpg", Description = "All about spices", IsDeleted = false }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        // Act: Call the service method
        var result = await _categoryService.GetCategoriesAsync();

        // Assert: Verify the result
        Assert.That(result, Is.Not.Null, "Result should not be null.");
        Assert.That(result.Count(), Is.EqualTo(2), "Result should contain all categories.");
        Assert.That(result.Any(c => c.Name == "Herbs"), "Result should include 'Herbs' category.");
        Assert.That(result.Any(c => c.Name == "Spices"), "Result should include 'Spices' category.");
    }

    [Test]
    public async Task GetCategoriesAsync_ShouldNotReturnDeletedCategories()
    {
        // Arrange: Add categories with one marked as deleted
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Herbs", ImagePath = "herbs.jpg", Description = "All about herbs", IsDeleted = false },
            new Category { Id = 2, Name = "Spices", ImagePath = "spices.jpg", Description = "All about spices", IsDeleted = true }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        // Act: Call the service method
        var result = await _categoryService.GetCategoriesAsync();

        // Assert: Verify the results
        Assert.That(result, Is.Not.Null, "Result should not be null.");
        Assert.That(result.Count(), Is.EqualTo(1), "Only non-deleted categories should be returned.");
        Assert.That(result.Any(c => c.Name == "Herbs"), "Result should include 'Herbs' category.");
        Assert.That(result.All(c => c.Name != "Spices"), "Result should not include 'Spices' category as it is deleted.");
    }

    [Test]
    public async Task GetCategoriesNamesAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Act: Call the service method when no categories exist
        var result = await _categoryService.GetCategoriesNamesAsync();

        // Assert: The result should not be null, but should be an empty list
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(0), "Result should be an empty list when no categories exist.");
    }

    [Test]
    public async Task GetCategoriesNamesAsync_ShouldReturnCategoryNames_WhenCategoriesExist()
    {
        // Arrange: Add categories to the in-memory database
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Herbs", ImagePath = "herbs.jpg", Description = "All about herbs", IsDeleted = false },
            new Category { Id = 2, Name = "Spices", ImagePath = "spices.jpg", Description = "All about spices", IsDeleted = false }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        // Act: Call the service method
        var result = await _categoryService.GetCategoriesNamesAsync();

        // Assert: Verify the result contains the category names
        Assert.That(result, Is.Not.Null, "Result should not be null.");
        Assert.That(result.Count(), Is.EqualTo(2), "Result should contain category names.");
        Assert.That(result.Contains("Herbs"), "Result should include 'Herbs' category name.");
        Assert.That(result.Contains("Spices"), "Result should include 'Spices' category name.");
    }

    [Test]
    public async Task GetCategoriesNamesAsync_ShouldNotReturnDeletedCategoryNames()
    {
        // Arrange: Add categories with one marked as deleted
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Herbs", ImagePath = "herbs.jpg", Description = "All about herbs", IsDeleted = false },
            new Category { Id = 2, Name = "Spices", ImagePath = "spices.jpg", Description = "All about spices", IsDeleted = true }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        // Act: Call the service method
        var result = await _categoryService.GetCategoriesNamesAsync();

        // Assert: Verify the results
        Assert.That(result, Is.Not.Null, "Result should not be null.");
        Assert.That(result.Count(), Is.EqualTo(1), "Only non-deleted categories should be returned.");
        Assert.That(result.Contains("Herbs"), "Result should include 'Herbs' category name.");
        Assert.That(result.Contains("Spices"), Is.False, "Result should not include 'Spices' category name as it is deleted.");
    }

    [Test]
    public async Task AddCategoryAsync_ShouldReturnTrue_WhenCategoryIsAddedSuccessfully()
    {
        // Arrange: Create a model for the new category
        var model = new CategoryCardViewModel
        {
            Name = "Herbs",
            ImagePath = "herbs.jpg",
            Description = "All about herbs"
        };

        // Act: Call the service method to add the category
        var result = await _categoryService.AddCategoryAsync(model);

        // Assert: Verify that the result is true (successful addition)
        Assert.That(result, Is.True, "The category should be added successfully.");

        // Verify that the category was actually added to the database
        var categoryInDb = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Herbs");
        Assert.That(categoryInDb, Is.Not.Null, "The category should be present in the database.");
        Assert.That(categoryInDb?.Name, Is.EqualTo("Herbs"), "The category name should be 'Herbs'.");
    }

    [Test]
    public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange: Add a category to the in-memory database
        var category = new Category
        {
            Id = 1,
            Name = "Herbs",
            ImagePath = "herbs.jpg",
            Description = "All about herbs"
        };
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        // Act: Call the GetCategoryByIdAsync method
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert: Verify the result is not null and contains the correct category data
        Assert.That(result, Is.Not.Null, "Category should be found.");
        Assert.That(result?.Id, Is.EqualTo(1), "Category ID should be 1.");
        Assert.That(result?.Name, Is.EqualTo("Herbs"), "Category name should be 'Herbs'.");
        Assert.That(result?.ImagePath, Is.EqualTo("herbs.jpg"), "Category image path should be 'herbs.jpg'.");
    }

    [Test]
    public async Task GetCategoryByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Act: Call the GetCategoryByIdAsync method with an invalid ID
        var result = await _categoryService.GetCategoryByIdAsync(999); // ID does not exist

        // Assert: Verify the result is null
        Assert.That(result, Is.Null, "Category should not be found.");
    }

    [Test]
    public async Task EditCategoryAsync_ShouldReturnTrue_WhenCategoryExists()
    {
        // Arrange: Add a category to the in-memory database
        var category = new Category
        {
            Id = 1,
            Name = "Herbs",
            ImagePath = "herbs.jpg",
            Description = "All about herbs"
        };
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var categoryEditModel = new CategoryCardViewModel
        {
            Id = 1,
            Name = "Updated Herbs",
            ImagePath = "updated_herbs.jpg",
            Description = "Updated description"
        };

        // Act: Call the EditCategoryAsync method
        var result = await _categoryService.EditCategoryAsync(categoryEditModel);

        // Assert: Verify the result is true and the category was updated
        Assert.That(result, Is.True, "Category should be updated.");
        var updatedCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == 1);
        Assert.That(updatedCategory, Is.Not.Null, "Category should exist after update.");
        Assert.That(updatedCategory?.Name, Is.EqualTo("Updated Herbs"), "Category name should be updated.");
        Assert.That(updatedCategory?.ImagePath, Is.EqualTo("updated_herbs.jpg"), "Category image path should be updated.");
        Assert.That(updatedCategory?.Description, Is.EqualTo("Updated description"), "Category description should be updated.");
    }

    [Test]
    public async Task EditCategoryAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Arrange: Prepare a model for a non-existing category
        var categoryEditModel = new CategoryCardViewModel
        {
            Id = 999, // Non-existing ID
            Name = "Non-existing Category",
            ImagePath = "non_existing.jpg",
            Description = "This category does not exist"
        };

        // Act: Call the EditCategoryAsync method with an invalid ID
        var result = await _categoryService.EditCategoryAsync(categoryEditModel);

        // Assert: Verify the result is false
        Assert.That(result, Is.False, "Editing a non-existing category should return false.");
    }

    [Test]
    public async Task DeleteCategoryAsync_ShouldReturnTrue_WhenCategoryExists()
    {
        // Arrange: Add a category and products in that category
        var category = new Category
        {
            Id = 1,
            Name = "Herbs",
            ImagePath = "herbs.jpg",
            Description = "All about herbs"
        };
        var product1 = new Product
        {
            Id = 1,
            Name = "Herb Product 1",
            CategoryId = 1,
            Price = 50,
            Description = "Test description",
            IsDeleted = false
            
        };
        var product2 = new Product
        {
            Id = 2,
            Name = "Herb Product 2",
            CategoryId = 1,
            Price = 60,
            Description = "Test description",
            IsDeleted = false
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Products.AddRangeAsync(product1, product2);
        await _dbContext.SaveChangesAsync();

        // Act: Call the DeleteCategoryAsync method
        var result = await _categoryService.DeleteCategoryAsync(1);

        // Assert: Verify the result is true
        Assert.That(result, Is.True, "Category should be successfully deleted.");

        // Verify the category is marked as deleted
        var deletedCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == 1);
        Assert.That(deletedCategory?.IsDeleted, Is.True, "Category should be marked as deleted.");

        // Verify the products in the category are marked as deleted
        var deletedProducts = await _dbContext.Products.Where(p => p.CategoryId == 1).ToListAsync();
        Assert.That(deletedProducts.All(p => p.IsDeleted), Is.True, "All products in the category should be marked as deleted.");
    }

    [Test]
    public async Task DeleteCategoryAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Act: Call the DeleteCategoryAsync method for a non-existing category
        var result = await _categoryService.DeleteCategoryAsync(999); // Non-existing ID

        // Assert: Verify the result is false
        Assert.That(result, Is.False, "Deleting a non-existing category should return false.");
    }

    
}

