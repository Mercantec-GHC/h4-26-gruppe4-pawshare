using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Context;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Repositories.Interfaces;
using Services;
using Models.DTOs;
using Microsoft.Extensions.Configuration;
using Backend.Test.Utils;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using NuGet.Packaging;
using Microsoft.Data.Sqlite;

namespace Backend.Test.Unit_Tests.Repositories;


public class UserRepoTest
{
    private IUserRepo _userRepo;
    private JwtService _jwtService;
    private List<User> _users;
    private string _refreshToken;

    private AuthService _auth;

    private Mock<AppDBContext> _mockDbContext;

    [OneTimeSetUp]
    public void Setup()
    {

        var factory = new TestApplicationFactory();
        var config = factory.Services.GetRequiredService<IConfiguration>();



        _users = new List<User>();
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .Options;

        _mockDbContext = new Mock<AppDBContext>(options);

        _mockDbContext.Setup(x => x.Users).ReturnsDbSet(_users);
        // ensure Add() actually inserts into the backing list
        _mockDbContext.Setup(m => m.Users.Add(It.IsAny<User>()))
            .Callback<User>(u => _users.Add(u))
            .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>)null);


        // ensure FindAsync looks up from the backing list
        _mockDbContext.Setup(m => m.Users.FindAsync(It.IsAny<object[]>()))
            .Returns<object[]>(ids =>
            {
                var id = (string)ids[0];
                var user = _users.FirstOrDefault(u => u.Id == id);
                return new ValueTask<User?>(user);
            });
        _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // ensure Remove actually deletes from the backing list
        _mockDbContext.Setup(m => m.Users.Remove(It.IsAny<User>()))
            .Callback<User>(u => _users.RemoveAll(x => x.Id == u.Id))
            .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>)null);

        // ensure Update replaces the item in the backing list
        _mockDbContext.Setup(m => m.Users.Update(It.IsAny<User>()))
            .Callback<User>(u =>
            {
                _users.RemoveAll(x => x.Id == u.Id);
                _users.Add(u);
            })
            .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>)null);





        _userRepo = new UserRepo(_mockDbContext.Object);

        _jwtService = new JwtService(config);

        var _roleRepo = new Mock<IRoleRepo>();

        var _mockAnimalRepo = new Mock<IAnimalRepo>();

        _auth = new AuthService(_userRepo, _jwtService, _roleRepo.Object, _mockAnimalRepo.Object);

    }





    [Test]
    public async Task Create_New_User()
    {
        var user = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };


        var result = await _userRepo.PostUser(user);




        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("1"));

        Assert.That(_users.Count, Is.EqualTo(1));
        Assert.That(_users[0].Id, Is.EqualTo("1"));

        Assert.That(_users.Count, Is.EqualTo(1));
        Assert.That(_users[0].Email, Is.EqualTo(user.Email));

        // Verify SaveChangesAsync was called
        _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Verify password with BCrypt.Verify (don't re-hash and compare)
        Assert.That(BCrypt.Net.BCrypt.Verify("Password1", _users[0].HashedPassword), Is.True);
    }

    [Test]
    public async Task Update_User()
    {
        // First create a user
        var user = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };

        await _userRepo.PostUser(user);

        // Now update it
        var updatedUserData = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user2@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };

        var updatedUser = await _userRepo.UpdateUser(updatedUserData);

        Assert.That(updatedUser, Is.Not.Null);
        Assert.That(updatedUser.Email, Is.EqualTo("user2@email.com"));

    }

    [Test]
    public async Task Get_User_By_Id()
    {


        var getUpdatedUser = await _userRepo.GetUser("1");

        Assert.That(getUpdatedUser, Is.Not.Null);
        Assert.That(getUpdatedUser.Id, Is.EqualTo("1"));

    }

    [Test]
    public async Task Get_User_By_Email()
    {
        // Create a user to query
        var user = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };

        await _userRepo.PostUser(user);

        var getUpdatedUser = await _userRepo.GetByEmail("user1@email.com");

        Assert.That(getUpdatedUser, Is.Not.Null);
        Assert.That(getUpdatedUser.Id, Is.EqualTo("1"));
        Assert.That(getUpdatedUser.Email, Is.EqualTo("user1@email.com"));
    }

    [Test]
    public async Task Get_User_By_Refresh_Token()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new AppDBContext(options);
        context.Database.EnsureCreated();
        
        // Disable foreign key constraints for testing
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = OFF;";
            cmd.ExecuteNonQuery();
        }

        var userRepo = new UserRepo(context);

        // Create a user first
        var user = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };

        await userRepo.PostUser(user);

        // Create a new AuthService with the real context
        var factory = new TestApplicationFactory();
        var config = factory.Services.GetRequiredService<IConfiguration>();
        var jwtService = new JwtService(config);
        var roleRepoMock = new Mock<IRoleRepo>();
        var _mockAnimalRepo = new Mock<IAnimalRepo>();
        var authService = new AuthService(userRepo, jwtService, roleRepoMock.Object, _mockAnimalRepo.Object);

        var userLoginDTO = new LoginDto()
        {
            Email = "user1@email.com",
            Password = "Password1"
        };

        var userLogin = await authService.Login(userLoginDTO);

        _refreshToken = userLogin.RefreshToken;

        var retrievedUser = await userRepo.GetByRefreshTokenAsync(_refreshToken);

        Assert.That(retrievedUser, Is.Not.Null);
        Assert.That(retrievedUser.Email, Is.EqualTo("user1@email.com"));
        connection.Dispose();
    }

    [Test]
    public async Task Update_User_Refresh_Token()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new AppDBContext(options);
        context.Database.EnsureCreated();
        
        // Disable foreign key constraints for testing
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = OFF;";
            cmd.ExecuteNonQuery();
        }

        var userRepo = new UserRepo(context);

        // Create a user first
        var user = new User
        {
            Id = "1",
            Name = "user1",
            Email = "user1@email.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password1"),
            Salt = "BCrypt internal",
            RealPassword = "Password1",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            RoleId = 1,
            City = "Test City"
        };

        await userRepo.PostUser(user);

        var refreshToken = Guid.NewGuid().ToString();

        await userRepo.UpdateRefreshToken("1", refreshToken, DateTime.UtcNow.AddDays(7));

        // Clear the change tracker to force a fresh query from the database
        context.ChangeTracker.Clear();

        var updatedUser = await userRepo.GetUser("1");

        Assert.That(updatedUser, Is.Not.Null);
        Assert.That(updatedUser.RefreshToken, Is.EqualTo(refreshToken));

        connection.Dispose();
    }

    [Test]
    public async Task Delete_user_By_Id()
    {

        // Verify user was created
        Assert.That(_users.Count, Is.EqualTo(1));

        // Delete the user
        var result = await _userRepo.DeleteUser("1");

        Assert.That(result, Is.True);
        Assert.That(_users.Count, Is.EqualTo(0));

    }

    [Test]
    public async Task Get_All_Users()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new AppDBContext(options);
        context.Database.EnsureCreated();
        
        // Disable foreign key constraints for testing
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = OFF;";
            cmd.ExecuteNonQuery();
        }
        
        var userRepo = new UserRepo(context);

        var users = new List<User>();
        for (int i = 0; i < 10; i++)
        {
            users.Add(new User
            {
                Id = $"{i}",
                Name = $"user{i}",
                Email = $"user{i}@email.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword($"Password{i}"),
                Salt = "BCrypt internal",
                RealPassword = $"Password{i}",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RoleId = 1,
                City = "Test City"

            });
        }

        context.Users.AddRange(users);
        await context.SaveChangesAsync();




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

        connection.Dispose();
    }

    [TearDown]
    public async Task DisposeAsync()
    {

    }
}