using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.Services.Services;
using Herbg.ViewModels.Order;
using Microsoft.EntityFrameworkCore;
using Herbg.Common.Enums;

[TestFixture]
public class OrderServiceTests
{
	private ApplicationDbContext _dbContext = null!;
	private IRepository<Order> _orderRepository = null!;
	private IRepository<Cart> _cartRepository = null!;
	private IOrderService _orderService = null!;

	[SetUp]
	public void SetUp()
	{
		// Create in-memory database options
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDb")
			.Options;

		_dbContext = new ApplicationDbContext(options);
		_orderRepository = new Repository<Order>(_dbContext);
		_cartRepository = new Repository<Cart>(_dbContext);
		_orderService = new OrderService(_cartRepository,_orderRepository);
	}

	[TearDown]
	public void TearDown()
	{
		_dbContext.Products.RemoveRange(_dbContext.Products);  // Remove all products
		_dbContext.SaveChanges();  // Save changes after removal
		_dbContext.Database.EnsureDeleted();
		_dbContext.Dispose();
	}


	//GetAllOrdersAsync

	[Test]
	public async Task GetAllOrdersAsync_ShouldReturnEmptyCollection_WhenNoOrdersExist()
	{
		// Arrange
		var clientId = "client1";

		// Act
		var result = await _orderService.GetAllOrdersAsync(clientId);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null.");
		Assert.That(result.Count, Is.EqualTo(0), "Result should be an empty collection.");
	}

	[Test]
	public async Task GetAllOrdersAsync_ShouldReturnOrders_ForSpecifiedClientId()
	{
		// Arrange
		var clientId = "client1";
		var orders = new List<Order>
		{
			new Order
			{
				Id = "guid",
				ClientId = clientId,
				Date = DateTime.UtcNow,
				TotalAmount = 50.0m,
				PaymentMethod = PaymentMethodEnum.Card,
				Address = "Test adress 2",
				ProductOrders = new List<ProductOrder>
				{
					new ProductOrder { ProductId = 1, Quantity = 2 },
					new ProductOrder { ProductId = 2, Quantity = 1 }
				}
			},
			new Order
			{
				Id = "guid2",
				ClientId = clientId,
				Date = DateTime.UtcNow.AddDays(-1),
				TotalAmount = 30.0m,
				PaymentMethod = PaymentMethodEnum.Card,
				Address = "Test adress 1",
				ProductOrders = new List<ProductOrder>
				{
					new ProductOrder { ProductId = 3, Quantity = 3 }
				}
			}
		};

		await _dbContext.Orders.AddRangeAsync(orders);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetAllOrdersAsync(clientId);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null.");
		Assert.That(result.Count, Is.EqualTo(2), "Result should contain all orders for the specified client ID.");

		var order1 = result.FirstOrDefault(o => o.OrderId == "guid");
		Assert.That(order1, Is.Not.Null, "Order with ID 1 should exist.");
		Assert.That(order1!.TotalItems, Is.EqualTo(3), "Order 1 should have the correct total items.");

		var order2 = result.FirstOrDefault(o => o.OrderId == "guid2");
		Assert.That(order2, Is.Not.Null, "Order with ID 2 should exist.");
		Assert.That(order2!.TotalItems, Is.EqualTo(3), "Order 2 should have the correct total items.");
	}

	[Test]
	public async Task GetAllOrdersAsync_ShouldReturnEmptyCollection_ForNonExistingClientId()
	{
		// Arrange
		var clientId = "nonexistentClient";

		// Act
		var result = await _orderService.GetAllOrdersAsync(clientId);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null.");
		Assert.That(result.Count, Is.EqualTo(0), "Result should be an empty collection for a nonexistent client ID.");
	}

	//Checkout

	[Test]
	public async Task GetCheckout_ShouldReturnNull_WhenCartDoesNotExist()
	{
		// Arrange
		var clientId = "client1";
		var cartId = "nonexistentCart";

		// Act
		var result = await _orderService.GetCheckout(clientId, cartId);

		// Assert
		Assert.That(result, Is.Null, "Result should be null when the cart does not exist.");
	}

	[Test]
	public async Task GetCheckout_ShouldReturnCheckoutViewModel_WhenCartExists()
	{
		// Arrange
		var clientId = "client1";
		var cartId = "cart1";

		var client = new ApplicationUser
		{
			Id = clientId,
			Address = "123 Test Street"
		};

		var product1 = new Product { Id = 1, Name = "Product1", ImagePath = "path1.jpg", Price = 10, Description = "Test description" };
		var product2 = new Product { Id = 2, Name = "Product2", ImagePath = "path2.jpg", Price = 20, Description = "Test description" };

		var cart = new Cart
		{
			Id = cartId,
			ClientId = clientId,
			Client = client,
			CartItems = new List<CartItem>
			{
				new CartItem { ProductId = 1, Product = product1, Quantity = 2, Price = 10 },
				new CartItem { ProductId = 2, Product = product2, Quantity = 1, Price = 20 }
			}
		};

		await _dbContext.Users.AddAsync(client);
		await _dbContext.Products.AddRangeAsync(product1, product2);
		await _dbContext.Carts.AddAsync(cart);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetCheckout(clientId, cartId);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null when the cart exists.");
		Assert.That(result.Address, Is.EqualTo(client.Address), "The address should match the client's address.");
		Assert.That(result.CartItems.Count, Is.EqualTo(2), "The cart should have 2 items.");
		Assert.That(result.Subtotal, Is.EqualTo(40), "Subtotal should match the sum of item prices (10*2 + 20).");
		Assert.That(result.Total, Is.EqualTo(50), "Total should include shipping cost (40 + 10).");
	}

	[Test]
	public async Task GetCheckout_ShouldCalculateTotalsCorrectly()
	{
		// Arrange
		var clientId = "client2";
		var cartId = "cart2";

		var client = new ApplicationUser
		{
			Id = clientId,
			Address = "456 Example Lane"
		};

		var product = new Product { Id = 1, Name = "SingleProduct", ImagePath = "product.jpg", Price = 15 , Description = "Test description"};

		var cart = new Cart
		{
			Id = cartId,
			ClientId = clientId,
			Client = client,
			CartItems = new List<CartItem>
			{
				new CartItem { ProductId = 1, Product = product, Quantity = 3, Price = 15 }
			}
		};

		await _dbContext.Users.AddAsync(client);
		await _dbContext.Products.AddAsync(product);
		await _dbContext.Carts.AddAsync(cart);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetCheckout(clientId, cartId);

		// Assert
		Assert.That(result.Subtotal, Is.EqualTo(45), "Subtotal should match the product price * quantity (15 * 3).");
		Assert.That(result.Total, Is.EqualTo(55), "Total should include shipping cost (45 + 10).");
	}

	[Test]
	public async Task GetOrderConfirmed_ShouldReturnNull_WhenCartDoesNotExist()
	{
		// Arrange
		var clientId = "client1";
		var model = new CheckoutViewModel
		{
			Address = "123 Test Street",
			PaymentMethod = PaymentMethodEnum.Card
		};

		// Act
		var result = await _orderService.GetOrderConfirmed(clientId, model);

		// Assert
		Assert.That(result, Is.Null, "Result should be null when the cart does not exist.");
	}

	[Test]
	public async Task GetOrderConfirmed_ShouldCreateOrderAndRemoveCart_WhenCartExists()
	{
		// Arrange
		var clientId = "client2";
		var cartId = "cart1";
		var product = new Product { Id = 1, Name = "Product1", Price = 20 , ImagePath ="images/img/test.jpg", Description ="Test description"};

		var cart = new Cart
		{
			Id = cartId,
			ClientId = clientId,
			CartItems = new List<CartItem>
			{
				new CartItem { ProductId = product.Id, Product = product, Price = 20, Quantity = 2 }
			}
		};

		var model = new CheckoutViewModel
		{
			Address = "456 Test Avenue",
			PaymentMethod = PaymentMethodEnum.Card
		};

		await _dbContext.Products.AddAsync(product);
		await _dbContext.Carts.AddAsync(cart);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetOrderConfirmed(clientId, model);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null when an order is created.");
		var createdOrder = await _dbContext.Orders.Include(o => o.ProductOrders).FirstOrDefaultAsync(o => o.Id == result);

		Assert.That(createdOrder, Is.Not.Null, "The order should be saved in the database.");
		Assert.That(createdOrder.ClientId, Is.EqualTo(clientId), "Order should have the correct client ID.");
		Assert.That(createdOrder.Address, Is.EqualTo(model.Address), "Order should have the correct address.");
		Assert.That(createdOrder.TotalAmount, Is.EqualTo(40), "Order total amount should match the cart items' calculated total.");
		Assert.That(createdOrder.ProductOrders.Count, Is.EqualTo(1), "Order should have 1 product order.");
		Assert.That(await _dbContext.Carts.AnyAsync(c => c.Id == cartId), Is.False, "The cart should be removed after order confirmation.");
	}

	//GetOrderConfirmed tests

	[Test]
	public async Task GetOrderConfirmed_ShouldCorrectlyHandleMultipleCartItems()
	{
		// Arrange
		var clientId = "client3";
		var product1 = new Product { Id = 1, Name = "Product1", Price = 10, ImagePath = "product.jpg", Description = "Test description" };
		var product2 = new Product { Id = 2, Name = "Product2", Price = 15, ImagePath = "product1.jpg", Description = "Test description 2" };

		var cart = new Cart
		{
			ClientId = clientId,
			CartItems = new List<CartItem>
			{
				new CartItem { ProductId = product1.Id, Product = product1, Price = 10, Quantity = 3 },
				new CartItem { ProductId = product2.Id, Product = product2, Price = 15, Quantity = 2 }
			}
		};

		var model = new CheckoutViewModel
		{
			Address = "789 Example Road",
			PaymentMethod = PaymentMethodEnum.Card
		};

		await _dbContext.Products.AddRangeAsync(product1, product2);
		await _dbContext.Carts.AddAsync(cart);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetOrderConfirmed(clientId, model);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null when an order is created.");
		var createdOrder = await _dbContext.Orders.Include(o => o.ProductOrders).FirstOrDefaultAsync(o => o.Id == result);

		Assert.That(createdOrder, Is.Not.Null, "The order should be saved in the database.");
		Assert.That(createdOrder.TotalAmount, Is.EqualTo(60), "Order total amount should match the cart items' calculated total (10*3 + 15*2).");
		Assert.That(createdOrder.ProductOrders.Count, Is.EqualTo(2), "Order should have 2 product orders.");
	}


	//GetOrderDetailsAsync


	[Test]
	public async Task GetOrderDetailsAsync_ShouldReturnNull_WhenOrderDoesNotExist()
	{
		// Arrange
		var orderId = "nonexistentOrderId";

		// Act
		var result = await _orderService.GetOrderDetailsAsync(orderId);

		// Assert
		Assert.That(result, Is.Null, "Result should be null when the order does not exist.");
	}

	

	[Test]
	public async Task GetOrderDetailsAsync_ShouldReturnAnonymousCustomer_WhenClientNameIsNull()
	{
		// Arrange
		var clientId = "client2";
		var product = new Product { Id = 2, Name = "Product2", Price = 30 , ImagePath ="images/test/path/img.jpg" , Description="Best product ever testing test tested"};

		var order = new Order
		{
			Id = "order2",
			ClientId = clientId,
			Date = DateTime.UtcNow,
			TotalAmount = 60.0m,
			PaymentMethod = PaymentMethodEnum.Card,
			Address = "456 Test Avenue",
			Client = new ApplicationUser { UserName = null, Email = null }, // Null username and email
			ProductOrders = new List<ProductOrder>
			{
				new ProductOrder { ProductId = product.Id, Product = product, Price = 30, Quantity = 2 }
			}
		};

		await _dbContext.Products.AddAsync(product);
		await _dbContext.Orders.AddAsync(order);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _orderService.GetOrderDetailsAsync(order.Id);

		// Assert
		Assert.That(result, Is.Not.Null, "Result should not be null when the order exists.");
		Assert.That(result.CustomerName, Is.EqualTo("Anonymouse"), "Customer name should be 'Anonymouse' when username is null.");
		Assert.That(result.CustomerEmail, Is.EqualTo("Anonymouse"), "Customer email should be 'Anonymouse' when email is null.");
	}

	

}


