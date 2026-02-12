using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Context;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Repositories.Interfaces;

namespace Backend.Test.Unit_Tests.Repositories;


public class UserRepoTest
{

    private AppDBContext _db;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
    .UseInMemoryDatabase("RepoUnitDB")
    .Options;

        _db = new AppDBContext(options);
    }





    [Test]
    public async Task Create_New_User()
    {
        var user = new User
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
        };

        IUserRepo userRepo = new UserRepo(_db);

        await userRepo.PostUser(user);


        var foundUser = await _db.Users.FindAsync(user.Id);

        Assert.That(foundUser, Is.Not.Null);
        Assert.That(foundUser.Id, Is.EqualTo("1"));
        Assert.That(foundUser.Id, Is.Not.EqualTo("2"));
        Assert.That(foundUser.Name, Is.EqualTo("user1"));
        Assert.That(foundUser.Name, Is.Not.EqualTo("user2"));
        Assert.That(foundUser.HashedPassword, Is.EqualTo(BCrypt.Net.BCrypt.HashPassword(user.RealPassword)));
        Assert.That(foundUser.RealPassword, Is.EqualTo("Password1"));

    }

    [Test]
    public async Task Update_User()
    {

    }

    [Test]
    public async Task Get_User_By_Id()
    {

    }

    [Test]
    public async Task Get_User_By_Email()
    {

    }

    [Test]
    public async Task Get_User_By_Refresh_Token()
    {

    }

    [Test]
    public async Task Update_User_Refresh_Token()
    {

    }

    [Test]
    public async Task Delete_user_By_Id()
    {


    }

    [Test]
    public async Task Get_All_Users()
    {


        var users = new List<User>();
        for (int i = 0; i < 10; i++)
        {
            users.Add(new User
            {
                Id = $"{i}",
                Name = $"user{i}",
                Base64Pfp = $"profile_pic_{i}",
                Email = $"user{i}@email.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword($"Password{i}"),
                Salt = "BCrypt internal",
                RealPassword = $"Password{i}",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,

            });
        }

        _db.Users.AddRange(users);

        await _db.SaveChangesAsync();

        var userRepo = new UserRepo(_db);

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

    [TearDown]
    public async Task DisposeAsync()
    {
        if (_db != null)
        {
            await _db.DisposeAsync();
        }

    }
}