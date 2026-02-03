using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Context;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Backend.Test.Unit_Tests.Repositories;


public class UserRepoTest
{
    [SetUp]
    public void Setup() {}

    [Test]
    public async Task Get_All_Users()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase("RepoUnitDB")
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
        
        db.Users.AddRange(users);
        
        await db.SaveChangesAsync();

        var userRepo = new UserRepo(db);

        var usersList = await userRepo.GetAllUsers();

        for (int i = 0; i < 10; i++)
        {
            Assert.That(users[i].Id, Is.EqualTo(usersList[i].Id));
            Assert.That(users[i].Name, Is.EqualTo(usersList[i].Name));
            Assert.That(users[i].HashedPassword, Is.EqualTo(usersList[i].HashedPassword));
            Assert.That(users[i].Salt, Is.EqualTo(usersList[i].Salt));
            Assert.That(users[i].CreatedAt, Is.EqualTo(usersList[i].CreatedAt));
            Assert.That(users[i].UpdatedAt, Is.EqualTo(usersList[i].UpdatedAt));
        }


    }
}