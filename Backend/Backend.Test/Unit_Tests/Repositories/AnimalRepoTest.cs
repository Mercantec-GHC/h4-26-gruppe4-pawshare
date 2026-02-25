using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Context;
using NUnit.Framework;

namespace Backend.Test.Unit_Tests.Repositories;

public class AnimalRepoTest
{
    private AppDBContext _db = null!;
    private AnimalRepo _repo = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _db = new AppDBContext(options);

        // Create a test role in db
        _db.Roles.Add(new Role { Id = 2, Name = "AnimalUser" });

        // Create a test user in db
        _db.Users.Add(new User
        {
            Id = "user_default",
            Name = "Repo Test User",
            Email = "repo@test.com",
            HashedPassword = "x",
            Base64Pfp = "iVBORw0KGgoAAAANSUhEUgAAAAUA",
            RoleId = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            City = "Test City"
        });

        // Create a test animaltype in db
        _db.AnimalTypes.Add(new AnimalType
        {
            Id = "type_default",
            Name = "Default Type",
            Description = "Default Desc",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        _db.SaveChanges();

        _repo = new AnimalRepo(_db);
    }

    [TearDown]
    public void TearDown()
    {
        _db.Dispose();
    }

    [Test]
    public async Task Get_All_Animals()
    {
        var animals = Enumerable.Range(0, 10).Select(i => new Animal
        {
            Id = $"{i}",
            Name = $"animal_{i}",
            Description = $"description_{i}",
            Base64Image = $"image_{i}",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(i)),
            TypeId = "type_default",
            UserId = "user_default",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _db.Animals.AddRange(animals);
        await _db.SaveChangesAsync();

        var result = (await _repo.GetAllAnimals()).OrderBy(a => a.Id).ToList();
        var expected = animals.OrderBy(a => a.Id).ToList();

        Assert.That(result.Count, Is.EqualTo(expected.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            Assert.That(result[i].Id, Is.EqualTo(expected[i].Id));
            Assert.That(result[i].Name, Is.EqualTo(expected[i].Name));
            Assert.That(result[i].Description, Is.EqualTo(expected[i].Description));
            Assert.That(result[i].Base64Image, Is.EqualTo(expected[i].Base64Image));
            Assert.That(result[i].DateOfBirth, Is.EqualTo(expected[i].DateOfBirth));
            Assert.That(result[i].TypeId, Is.EqualTo(expected[i].TypeId));
            Assert.That(result[i].UserId, Is.EqualTo(expected[i].UserId));
        }
    }

    [Test]
    public async Task Get_Animal()
    {
        var animals = Enumerable.Range(0, 3).Select(i => new Animal
        {
            Id = $"{i}",
            Name = $"animal_{i}",
            Description = $"description_{i}",
            Base64Image = $"image_{i}",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(i)),
            TypeId = "type_default",
            UserId = "user_default",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _db.Animals.AddRange(animals);
        await _db.SaveChangesAsync();

        var first = await _repo.GetAnimal(animals.First().Id);
        Assert.That(first, Is.Not.Null);
        Assert.That(first!.Id, Is.EqualTo(animals.First().Id));

        var last = await _repo.GetAnimal(animals.Last().Id);
        Assert.That(last, Is.Not.Null);
        Assert.That(last!.Id, Is.EqualTo(animals.Last().Id));
        Assert.That(last.Id, Is.Not.EqualTo(first.Id));
    }

    [Test]
    public async Task Post_Animal()
    {
        var newAnimal = new Animal
        {
            Id = "1",
            Name = "animal_1",
            Description = "description_1",
            Base64Image = "image_1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            TypeId = "type_default",
            UserId = "user_default",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _repo.PostAnimal(newAnimal);

        Assert.That(result, Is.Not.Null);
        Assert.That(_db.Animals.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Update_Animal()
    {
        var animal = new Animal
        {
            Id = "1",
            Name = "old_animal_1",
            Description = "old_description_1",
            Base64Image = "old_image_1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            TypeId = "type_default",
            UserId = "user_default",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Animals.Add(animal);
        await _db.SaveChangesAsync();

        animal.Name = "new";
        animal.Description = "new_desc";

        var updated = await _repo.UpdateAnimal(animal);

        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Name, Is.EqualTo("new"));
        Assert.That(updated.Description, Is.EqualTo("new_desc"));
    }

    [Test]
    public async Task Delete_Animal()
    {
        var animal = new Animal
        {
            Id = "1",
            Name = "animal_1",
            Description = "description_1",
            Base64Image = "image_1",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            TypeId = "type_default",
            UserId = "user_default",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Animals.Add(animal);
        await _db.SaveChangesAsync();

        var result = await _repo.DeleteAnimal("1");

        Assert.That(result, Is.True);
        Assert.That(_db.Animals.Any(), Is.False);
    }
}
