using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Models;
using Repositories.Context;

namespace Backend.Test.Integration_Tests;

public class UserDBIntegrationTest2
{
    private WebApplicationFactory<Program> _factory;
    public AppDBContext db { get; private set; } = default!;
    private string _connectionString = string.Empty;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        var config = builder.Build();

        _connectionString = config.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("MY_ENV_VAR");

        Console.WriteLine(_connectionString);
        
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.Single(a => a.ServiceType == typeof(DbContextOptions<AppDBContext>)));
                    services.AddDbContext<AppDBContext>(options =>
                    {
                        options.UseNpgsql(_connectionString);
                    });
                });
            });

        db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDBContext>();
        await db.Database.MigrateAsync();
    }

    [Test]
    public async Task Create_User_And_Check_If_It_Exists2()
    {
        var user = new User()
        {
            Id = "1",
            Name = "User",
            Email = "user@test.com",
            RealPassword = "password123",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
            Salt = "BCrypt internal",
            Base64Pfp = "profile_picture.png",
            CreatedAt =  DateTime.UtcNow,
            UpdatedAt =  DateTime.UtcNow,
            
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
        
        
        
    }
    
    
    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        if (db != null)
        {
            await db.DisposeAsync();
        }

    }
}