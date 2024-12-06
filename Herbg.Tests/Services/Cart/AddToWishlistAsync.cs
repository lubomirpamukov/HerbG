using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Herbg.Services; 
using Herbg.Models;
using Herbg.Infrastructure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Services.Services;

namespace YourProject.Tests
{
    [TestFixture]
    public class WishlistServiceTests
    {
        private Mock<IRepository<Wishlist>> _wishlistRepositoryMock;
        private Mock<IRepository<Product>> _productRepositoryMock;
        private Mock<IRepository<Cart>> _cartRepositoryMock;
        private WishlistService _wishlistService;

        [SetUp]
        public void Setup()
        {
            _wishlistRepositoryMock = new Mock<IRepository<Wishlist>>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _cartRepositoryMock = new Mock<IRepository<Cart>>();
            _wishlistService = new WishlistService(_wishlistRepositoryMock.Object,_productRepositoryMock.Object,_cartRepositoryMock.Object);  // Adjust constructor as needed
        }

        [Test]
        public async Task AddToWishlistAsync_WhenItemDoesNotExist_ShouldAddNewItem()
        {
            // Arrange
            var clientId = "client123";
            var productId = 1;

            _wishlistRepositoryMock
                .Setup(w => w.GetAllAttached())
                .Returns(new List<Wishlist>().AsQueryable()); // No existing items in wishlist

            // Act
            var result = await _wishlistService.AddToWishlistAsync(clientId, productId);

            // Assert
            _wishlistRepositoryMock.Verify(w => w.AddAsync(It.Is<Wishlist>(wi => wi.ClientId == clientId && wi.ProductId == productId)), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddToWishlistAsync_WhenItemExists_ShouldNotAddNewItem()
        {
            // Arrange
            var clientId = "client123";
            var productId = 1;

            var existingWishlistItem = new Wishlist { ClientId = clientId, ProductId = productId };
            _wishlistRepositoryMock
                .Setup(w => w.GetAllAttached())
                .Returns(new List<Wishlist> { existingWishlistItem }.AsQueryable()); // Existing item in wishlist

            // Act
            var result = await _wishlistService.AddToWishlistAsync(clientId, productId);

            // Assert
            _wishlistRepositoryMock.Verify(w => w.AddAsync(It.IsAny<Wishlist>()), Times.Never); // Should not add
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddToWishlistAsync_WhenAddFails_ShouldReturnFalse()
        {
            // Arrange
            var clientId = "client123";
            var productId = 1;

            _wishlistRepositoryMock
                .Setup(w => w.GetAllAttached())
                .Returns(new List<Wishlist>().AsQueryable()); // No existing items in wishlist

            _wishlistRepositoryMock
                .Setup(w => w.AddAsync(It.IsAny<Wishlist>()))
                .ThrowsAsync(new Exception("Database error")); // Simulate failure in adding

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _wishlistService.AddToWishlistAsync(clientId, productId));
            Assert.That(ex.Message, Is.EqualTo("Database error"));
        }
    }
}
