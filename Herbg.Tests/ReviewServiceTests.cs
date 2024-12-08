using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Models;
using Herbg.Services.Services;
using Herbg.ViewModels.Review;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class ReviewServiceTests
{
	private ReviewService _reviewService = null!;
	private DbContextOptions<ApplicationDbContext> _dbContextOptions = null!;
	private ApplicationDbContext _dbContext = null!;

	[SetUp]
	public void SetUp()
	{
		_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "HerbgReviewTestDb")
			.Options;

		_dbContext = new ApplicationDbContext(_dbContextOptions);

		_dbContext.Database.EnsureCreated();

		// Create repositories and initialize ReviewService
		var clientRepository = new Repository<ApplicationUser>(_dbContext);
		var reviewRepository = new Repository<Review>(_dbContext);
		_reviewService = new ReviewService(clientRepository, reviewRepository);

		// Clean initial data
		_dbContext.Reviews.RemoveRange(_dbContext.Reviews);
		_dbContext.Users.RemoveRange(_dbContext.Users);
		_dbContext.SaveChanges();
	}

	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
		_dbContext.Dispose();
	}

	[Test]
	public async Task GetReviewFormAsync_ShouldReturnReviewForm_WithClientDetails_WhenClientExists()
	{
		// Arrange
		var client = new ApplicationUser
		{
			Id = "client1",
			Email = "test@example.com",
			UserName = "testuser"
		};

		_dbContext.Users.Add(client);
		await _dbContext.SaveChangesAsync();

		var productId = 1;

		// Act
		var result = await _reviewService.GetReviewFormAsync(client.Id, productId);

		// Assert
		Assert.That(result, Is.Not.Null, "The review form should not be null.");
		Assert.That(result.Id, Is.EqualTo(productId), "The product ID should match.");
		Assert.That(result.ReviewerName, Is.EqualTo("test@example.com"), "The reviewer name should match the client's email.");
	}

	[Test]
	public async Task GetReviewFormAsync_ShouldReturnReviewForm_WithAnonymousReviewer_WhenClientDoesNotExist()
	{
		// Arrange
		var clientId = "nonexistent-client";
		var productId = 2;

		// Act
		var result = await _reviewService.GetReviewFormAsync(clientId, productId);

		// Assert
		Assert.That(result, Is.Not.Null, "The review form should not be null.");
		Assert.That(result.Id, Is.EqualTo(productId), "The product ID should match.");
		Assert.That(result.ReviewerName, Is.EqualTo("Anonymouse"), "The reviewer name should default to 'Anonymouse'.");
	}

	[Test]
	public async Task UpdateReviewAsync_ShouldUpdateReview_WhenReviewExists()
	{
		// Arrange: Add a client, product, and a review
		var client = new ApplicationUser { Id = "1", UserName = "JohnDoe" };
		var product = new Product { Id = 1, Name = "Mint" };
		var existingReview = new Review
		{
			ClientId = "1",
			ProductId = 1,
			Description = "Old review",
			Rating = 3
		};

		_dbContext.Users.Add(client);
		_dbContext.Products.Add(product);
		_dbContext.Reviews.Add(existingReview);
		await _dbContext.SaveChangesAsync();

		var model = new ReviewViewModel
		{
			Id = 1, // ProductId
			Description = "Updated review",
			Rating = 5
		};

		// Act: Call UpdateReviewAsync to update the review
		var result = await _reviewService.UpdateReviewAsync("1", model);

		// Assert: Verify that the review was updated
		var updatedReview = await _dbContext.Reviews
			.FirstOrDefaultAsync(r => r.ClientId == "1" && r.ProductId == 1);

		Assert.That(result, Is.True);
		Assert.That(updatedReview, Is.Not.Null);
		Assert.That(updatedReview.Description, Is.EqualTo("Updated review"));
		Assert.That(updatedReview.Rating, Is.EqualTo(5));
	}

	[Test]
	public async Task UpdateReviewAsync_ShouldCreateNewReview_WhenNoReviewExistsForClient()
	{
		// Arrange: Add a client and product but no review
		var client = new ApplicationUser { Id = "2", UserName = "JaneDoe" };
		var product = new Product { Id = 1, Name = "Basil" };

		_dbContext.Users.Add(client);
		_dbContext.Products.Add(product);
		await _dbContext.SaveChangesAsync();

		var model = new ReviewViewModel
		{
			Id = 1, // ProductId
			Description = "Great product!",
			Rating = 5
		};

		// Act: Call UpdateReviewAsync to create a new review
		var result = await _reviewService.UpdateReviewAsync("2", model);

		// Assert: Verify that a new review was created
		var createdReview = await _dbContext.Reviews
			.FirstOrDefaultAsync(r => r.ClientId == "2" && r.ProductId == 1);

		Assert.That(result, Is.True);
		Assert.That(createdReview, Is.Not.Null);
		Assert.That(createdReview.Description, Is.EqualTo("Great product!"));
		Assert.That(createdReview.Rating, Is.EqualTo(5));
	}

	[Test]
	public async Task UpdateReviewAsync_ShouldReturnFalse_WhenClientDoesNotExist()
	{
		// Arrange: Create a review model with a non-existent client
		var model = new ReviewViewModel
		{
			Id = 1, // ProductId
			Description = "Test review",
			Rating = 4
		};

		// Act: Call UpdateReviewAsync with a non-existent client
		var result = await _reviewService.UpdateReviewAsync("999", model);

		// Assert: Verify that the result is false (client does not exist)
		Assert.That(result, Is.False);
	}

	[Test]
	public async Task UpdateReviewAsync_ShouldReturnFalse_WhenProductDoesNotExist()
	{
		// Arrange: Add a client but no product
		var client = new ApplicationUser { Id = "3", UserName = "ChrisDoe" };
		_dbContext.Users.Add(client);
		await _dbContext.SaveChangesAsync();

		var model = new ReviewViewModel
		{
			Id = 999, // Non-existent ProductId
			Description = "Review for non-existent product",
			Rating = 2
		};

		// Act: Call UpdateReviewAsync with a non-existent product ID
		var result = await _reviewService.UpdateReviewAsync("3", model);

		// Assert: Verify that the result is false (product does not exist)
		Assert.That(result, Is.False);
	}

}
