using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using Repositories;
using Repositories.Context;
using Repositories.Interfaces;
using Services;

namespace Backend.Test.Unit_Tests.Services;

public class UserServiceTest
{
    [SetUp]
    public void Setup() {}

    [Test]
    public async Task Get_User_By_Id()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "ServiceUnitDB_" + Guid.NewGuid().ToString())
            .Options;
    
        var db = new AppDBContext(options);
        
        // Ensure the database is created
        await db.Database.EnsureCreatedAsync();
    
        var newUser = new User
        {
            Id = "1",
            Name = "user1",
            Base64Pfp = "profile_pic_1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };
        
        // Add user to the database
        db.Users.Add(newUser);
        await db.SaveChangesAsync();

        var userRepo = new UserRepo(db);
        var mockRoleRepo = new Mock<IRoleRepo>();
        var userService = new UserService(userRepo, mockRoleRepo.Object);
        var user = await userService.GetUser("1");
        
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo("1"));
    }
}
