using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Repositories;
using Repositories.Context;
using Services;

namespace Backend.Test.Unit_Tests.Services;

public class AnimalServiceTests
{
    private AppDBContext _dbContext = null!;
    private AnimalRepo _animalRepo = null!;
    private AnimalService _animalService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _dbContext = new AppDBContext(options);

        // Creates test role in db
        _dbContext.Roles.Add(new Role { Id = 2, Name = "role" });

        // Creates test users in db
        var userIds = new[] { "user_1", "userA", "userB" };
        foreach (var id in userIds)
        {
            _dbContext.Users.Add(new User
            {
                Id = id,
                Name = id + "_name",
                Email = id + "@mail.com",
                HashedPassword = "abc",
                Salt = "abc",
                RealPassword = "abc",
                Base64Pfp = "abc",
                RoleId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                City = "Test City"
            });
        }

        // Creates test animaltypes in db
        for(int i = 0; i < 3; i++)
        {
            _dbContext.AnimalTypes.Add(new AnimalType
            {
                Id = "type_" + i,
                Name = "type_" + i + "_name",
                Description = "desc",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        _dbContext.SaveChanges();

        _animalRepo = new AnimalRepo(_dbContext);
        _animalService = new AnimalService(_animalRepo);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetAnimalAsync_ReturnsAnimal_WhenFound()
    {
        var animal = new Animal
        {
            Id = "1",
            Name = "animal_1",
            Description = "description_1",
            Base64Image = "image_1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            TypeId = "type_1",
            UserId = "user_1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Animals.Add(animal);
        await _dbContext.SaveChangesAsync();

        var result = await _animalService.GetAnimalAsync("1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo("1"));
    }

    [Test]
    public async Task GetAllAnimalsAsync_ReturnsAllAnimals()
    {
        var animals = Enumerable.Range(0, 10).Select(i => new Animal
        {
            Id = $"{i}",
            Name = $"animal_{i}",
            Description = $"description_{i}",
            Base64Image = $"image_{i}",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(i)),
            TypeId = "type_1",
            UserId = "user_1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _dbContext.Animals.AddRange(animals);
        await _dbContext.SaveChangesAsync();

        var result = await _animalService.GetAllAnimalsAsync();

        Assert.That(result.Count, Is.EqualTo(animals.Count));
    }

    [Test]
    public async Task GetAnimalsByTypeAsync_ReturnsFilteredAnimals()
    {
        var animals = new List<Animal>();

        for (int i = 0; i < 3; i++)
        {
            animals.Add(new Animal
            {
                Id = (i + 1).ToString(),
                Name = $"a{i + 1}",
                Description = $"d{i + 1}",
                Base64Image = $"i{i + 1}",
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(i + 1)),
                TypeId = "type_" + i,
                UserId = "user_1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        _dbContext.Animals.AddRange(animals);
        await _dbContext.SaveChangesAsync();

        var result = await _animalService.GetAnimalsByTypeAsync("type_1");

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.All(a => a.TypeId == "type_1"));
    }


    [Test]
    public async Task GetAnimalsByUserAsync_ReturnsOnlyUserAnimals()
    {
        var animals = new List<Animal>();

        for (int i = 0; i < 3; i++)
        {
            
            animals.Add(new Animal
            {
                Id = (i + 1).ToString(),
                Name = $"a{i + 1}",
                Description = $"d{i + 1}",
                Base64Image = $"i{i + 1}",
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(i + 1)),
                TypeId = "type_" + i,
                UserId = i % 2 != 0 ? "userB" : "userA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        _dbContext.Animals.AddRange(animals);
        await _dbContext.SaveChangesAsync();

        var result = await _animalService.GetAnimalsByUserAsync("userA");

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(a => a.UserId == "userA"));
    }


    [Test]
    public async Task CreateAnimalAsync_CreatesAnimal()
    {
        var input = new Animal
        {
            Id = "placeholder",
            Name = "test_animal",
            Description = "test_description",
            Base64Image = "test_image",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2)),
            TypeId = "type_1",
            UserId = "user_1",
            CreatedAt = DateTime.MinValue,
            UpdatedAt = DateTime.MinValue
        };

        var result = await _animalService.CreateAnimalAsync(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(Guid.TryParse(result!.Id, out _), Is.True);
        Assert.That(result.CreatedAt, Is.Not.EqualTo(DateTime.MinValue));
        Assert.That(result.UpdatedAt, Is.Not.EqualTo(DateTime.MinValue));
    }

    [Test]
    public async Task DeleteAnimalAsync_ReturnsTrue_WhenDeleted()
    {
        var animal = new Animal
        {
            Id = "1",
            Name = "a",
            Description = "d",
            Base64Image = "i",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            TypeId = "type_1",
            UserId = "user_1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Animals.Add(animal);
        await _dbContext.SaveChangesAsync();

        var result = await _animalService.DeleteAnimalAsync("1");

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAnimalAsync_ReturnsFalse_WhenNotFound()
    {
        var result = await _animalService.DeleteAnimalAsync("missing");

        Assert.That(result, Is.False);
    }
}
