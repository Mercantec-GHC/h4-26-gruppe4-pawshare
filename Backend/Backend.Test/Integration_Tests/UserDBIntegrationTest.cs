using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Models;
using Repositories.Context;

namespace Backend.Test.Integration_Tests;

public class UserDBIntegrationTest
{
    private readonly PostgreSqlContainer postgressContainer = new PostgreSqlBuilder().Build();
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
        await postgressContainer.DisposeAsync();
        await _factory.DisposeAsync();
        if (db != null)
        {
            await db.DisposeAsync();
        }

    }
}