using Herbg.Data;
using Herbg.Infrastructure;
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.Services.Services;
using Herbg.ViewModels.Manufactorer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class ManufactorerServiceTests
{
    private ApplicationDbContext _dbContext = null!;
    private IRepository<Manufactorer> _manufactorerRepository = null!;
    private IManufactorerService _manufactorerService = null!;

    [SetUp]
    public void SetUp()
    {
        // Set up an in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _manufactorerRepository = new Repository<Manufactorer>(_dbContext);
        _manufactorerService = new ManufactorerService(_manufactorerRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public void GetAllManufactorersDbModel_ShouldReturnAllManufactorers()
    {
        // Arrange
        var manufactorers = new List<Manufactorer>
        {
            new Manufactorer { Id = 1, Name = "Manufacturer 1", Address="Test address 1"  },
            new Manufactorer { Id = 2, Name = "Manufacturer 2", Address="Test address 1"  }
        };

        _dbContext.Manufactorers.AddRange(manufactorers);
        _dbContext.SaveChanges();

        // Act
        var result = _manufactorerService.GetAllManufactorersDbModel();

        // Assert
        Assert.That(result.Count, Is.EqualTo(2), "Should return all manufactorers in the database.");
        Assert.That(result.Any(m => m.Name == "Manufacturer 1"), "Should include 'Manufacturer 1'.");
        Assert.That(result.Any(m => m.Name == "Manufacturer 2"), "Should include 'Manufacturer 2'.");
    }

    [Test]
    public async Task GetManufactorersNamesAsync_ShouldReturnAllNames()
    {
        // Arrange
        var manufactorers = new List<Manufactorer>
        {
            new Manufactorer { Id = 1, Name = "Manufacturer 1", Address="Test address 1" },
            new Manufactorer { Id = 2, Name = "Manufacturer 2", Address="Test address 2" }
        };

        _dbContext.Manufactorers.AddRange(manufactorers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _manufactorerService.GetManufactorersNamesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2), "Should return all manufactorer names.");
        Assert.That(result.Contains("Manufacturer 1"), "Should include 'Manufacturer 1'.");
        Assert.That(result.Contains("Manufacturer 2"), "Should include 'Manufacturer 2'.");
    }

    [Test]
    public async Task GetManufacturersAsync_ShouldReturnAllManufacturersAsViewModels()
    {
        // Arrange
        var manufactorers = new List<Manufactorer>
        {
            new Manufactorer { Id = 1, Name = "Manufacturer 1", Address="Test address 1"  },
            new Manufactorer { Id = 2, Name = "Manufacturer 2", Address="Test address 2"  }
        };

        _dbContext.Manufactorers.AddRange(manufactorers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _manufactorerService.GetManufacturersAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2), "Should return all manufactorers as view models.");
        Assert.That(result.Any(m => m.Name == "Manufacturer 1"), "Should include 'Manufacturer 1'.");
        Assert.That(result.Any(m => m.Name == "Manufacturer 2"), "Should include 'Manufacturer 2'.");
    }



    
}