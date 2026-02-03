using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Context;
using Services;

namespace Backend.Test.Unit_Tests.Services;

public class UserServiceTest
{
    [SetUp]
    public void Setup() {}

    [Test]
    public async Task Get_All_Users()
    {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase("ServiceUnitDB")
                .Options;
        
            var db = new AppDBContext(options);
        
            var users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User
                {
                    Id = $"{i}",
                    Name = $"user{i}",
                    Base64Pfp = $"profile_pic_{i}",
                    Email =  $"user{i}@email.com",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword($"Password{i}"),
                    Salt = "BCrypt internal",
                    RealPassword = $"Password{i}",
                    CreatedAt =  DateTime.Now,
                    UpdatedAt =  DateTime.Now,
                
                });
            }
        
            db.Users.Add(new User
            {
                Id = "1",
                Name = $"user1",
                Base64Pfp = $"profile_pic_1",
                Email =  $"user1@email.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword($"Password1"),
                Salt = "BCrypt internal",
                RealPassword = $"Password1",
                CreatedAt =  DateTime.Now,
                UpdatedAt =  DateTime.Now,
                
            });
        
            await db.SaveChangesAsync();

            var userRepo = new UserRepo(db);
            var userService = new UserService(userRepo);
            var user = await userService.GetUser("1");
            
            Assert.That(user.Id, Is.EqualTo("1"));

            
    }
}
