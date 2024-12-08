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
	private ApplicationDbContext _dbContext;
	private IRepository<Category> _categoryRepository;
	private ICategoryService _categoryService;

	[SetUp]
	public void SetUp()
	{
		// Create in-memory database options
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDb")
			.Options;

		_dbContext = new ApplicationDbContext(options);
		_categoryRepository = new Repository<Category>(_dbContext);
		_categoryService = new CategoryService(_categoryRepository);
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

	[TearDown]
	public void TearDown()
	{
		_dbContext.Dispose();
	}
}

