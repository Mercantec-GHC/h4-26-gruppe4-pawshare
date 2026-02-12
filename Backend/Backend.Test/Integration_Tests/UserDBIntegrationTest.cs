using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Models;
using Repositories.Context;
using Microsoft.Extensions.DependencyInjection.Extensions;
using API.Controllers;

namespace Backend.Test.Integration_Tests;

public class UserDBIntegrationTest
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
        // await db.Database.EnsureCreatedAsync();
        await db.Database.MigrateAsync();
        //var pending = await db.Database.GetPendingMigrationsAsync();
        //Console.WriteLine("Pending migrations: " + string.Join(", ", pending));

    }



    [Test]
    public async Task Create_User_And_Check_If_It_Exists()
    {

        Console.WriteLine(_connectionString);
        var user = new User()
        {
            Id = "1",
            Name = "User",
            Email = "user@test.com",
            RealPassword = "password123",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
            Salt = "BCrypt internal",
            Base64Pfp = "profile_picture.png",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

        };

        db.Users.Add(user);

        await db.SaveChangesAsync();

        var foundUser = await db.Users.FindAsync(user.Id);

        Assert.That(foundUser, Is.Not.Null);
        Assert.That(foundUser.Name, Is.EqualTo("User"));
        Assert.That(foundUser.Name, Is.Not.EqualTo("User1"));
        Assert.That(foundUser.Name, Is.Not.TypeOf<int>());
        Assert.That(foundUser.Name, Is.TypeOf<string>());
        Assert.That(foundUser.Name, Is.Not.TypeOf<bool>());

        Assert.That(foundUser.Id, Is.EqualTo("1"));
        Assert.That(foundUser.Id, Is.Not.EqualTo("2"));
        Assert.That(foundUser.Id, Is.Not.TypeOf<int>());
        Assert.That(foundUser.Id, Is.TypeOf<string>());
        Assert.That(foundUser.Id, Is.Not.TypeOf<bool>());

        // db.Users.Remove(user);
        // await db.SaveChangesAsync();

    }

    [Test]
    public async Task Check_If_It_Exists_And_If_It_Does_Delete_It()
    {
        /*
        var user = new User()
        {
            Id = "1",
            Name = "User",
            Email = "user@test.com",
            RealPassword = "password123",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
            Salt = "BCrypt internal",
            Base64Pfp = "profile_picture.png",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

        };

        db.Users.Add(user);

        await db.SaveChangesAsync();
        */

        Console.WriteLine(_connectionString);

        var foundUser = await db.Users.SingleOrDefaultAsync<User>(e => e.Id == "1");

        Assert.That(foundUser, Is.Not.Null);


        db.Users.Remove(foundUser);
        await db.SaveChangesAsync();

        foundUser = await db.Users.FindAsync("1");

        Assert.That(foundUser, Is.Null);


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