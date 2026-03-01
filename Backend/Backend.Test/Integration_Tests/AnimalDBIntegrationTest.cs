using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Repositories.Context;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Backend.Test.Integration_Tests;

public class AnimalDBIntegrationTest
{
    private readonly PostgreSqlContainer postgressContainer = new PostgreSqlBuilder("postgres:latest").Build();
    private string _connectionString = string.Empty;
    private WebApplicationFactory<Program> _factory;
    public AppDBContext db { get; private set; } = default!;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        await postgressContainer.StartAsync();
        _connectionString = postgressContainer.GetConnectionString();
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<AppDBContext>>();
                    services.AddDbContext<AppDBContext>(options =>
                    {
                        options.UseNpgsql(_connectionString);
                    });
                });
            });

        db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDBContext>();
        await db.Database.EnsureCreatedAsync();
    }

    [Test]
    public async Task Create_Animal_And_Check_If_It_Exists()
    {
        Console.WriteLine(_connectionString);

        var user = new User()
        {
            Id = "animal-user-1",
            Name = "Animal Owner",
            Email = "animalowner@test.com",
            RealPassword = "password123",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
            Salt = "BCrypt internal",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoleId = 1,
            City = "Test City"
        };

        var animalType = new AnimalType()
        {
            Id = "animal-type-1",
            Name = "Dog",
            Description = "Dog type",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        db.Users.Add(user);
        db.AnimalTypes.Add(animalType);
        await db.SaveChangesAsync();

        var animal = new Animal()
        {
            Id = "animal-1",
            Name = "Buddy",
            Description = "Friendly dog",
            AnimalPictureKey = "animals/buddy.jpg",
            DateOfBirth = new DateOnly(2020, 1, 1),
            TypeId = animalType.Id,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        db.Animals.Add(animal);
        await db.SaveChangesAsync();

        var foundAnimal = await db.Animals.FindAsync(animal.Id);

        Assert.That(foundAnimal, Is.Not.Null);
        Assert.That(foundAnimal.Name, Is.EqualTo("Buddy"));
        Assert.That(foundAnimal.Name, Is.Not.EqualTo("Buddy1"));
        Assert.That(foundAnimal.Name, Is.Not.TypeOf<int>());
        Assert.That(foundAnimal.Name, Is.TypeOf<string>());
        Assert.That(foundAnimal.Name, Is.Not.TypeOf<bool>());

        Assert.That(foundAnimal.Id, Is.EqualTo("animal-1"));
        Assert.That(foundAnimal.Id, Is.Not.EqualTo("animal-2"));
        Assert.That(foundAnimal.Id, Is.Not.TypeOf<int>());
        Assert.That(foundAnimal.Id, Is.TypeOf<string>());
        Assert.That(foundAnimal.Id, Is.Not.TypeOf<bool>());
    }

    [Test]
    public async Task Check_If_Animal_Exists_And_If_It_Does_Delete_It()
    {
        var user = new User()
        {
            Id = "animal-user-2",
            Name = "Animal Owner 2",
            Email = "animalowner2@test.com",
            RealPassword = "password123",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
            Salt = "BCrypt internal",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoleId = 1,
            City = "Test City"
        };

        var animalType = new AnimalType()
        {
            Id = "animal-type-2",
            Name = "Cat",
            Description = "Cat type",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        db.Users.Add(user);
        db.AnimalTypes.Add(animalType);
        await db.SaveChangesAsync();

        var animal = new Animal()
        {
            Id = "animal-2",
            Name = "Milo",
            Description = "Playful cat",
            AnimalPictureKey = "animals/milo.jpg",
            DateOfBirth = new DateOnly(2021, 6, 15),
            TypeId = animalType.Id,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        db.Animals.Add(animal);
        await db.SaveChangesAsync();

        Console.WriteLine(_connectionString);

        var foundAnimal = await db.Animals.SingleOrDefaultAsync<Animal>(e => e.Id == "animal-2");

        Assert.That(foundAnimal, Is.Not.Null);

        db.Animals.Remove(foundAnimal);
        await db.SaveChangesAsync();

        foundAnimal = await db.Animals.FindAsync("animal-2");

        Assert.That(foundAnimal, Is.Null);
    }

    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await postgressContainer.DisposeAsync();
        await _factory.DisposeAsync();
        if (db != null)
        {
            await db.DisposeAsync();
        }
    }
}
