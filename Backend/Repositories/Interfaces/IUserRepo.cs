using Models;
using System.Linq.Expressions;

namespace Repositories.Interfaces;

public interface IUserRepo
{
    /// <summary>
    /// Gets user by email from table
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <returns>User with given email, if not found returns null</returns>
    Task<User?> GetByEmail(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Gets user with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted user</param>
    /// <returns>User with given id, if not found returns null</returns>
    public Task<User?> GetUser(string id);
    
    /// <summary>
    /// Posts a new user to the table
    /// </summary>
    /// <param name="newUser">The new user that needs to be posted</param>
    /// <returns>Post that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<User?> PostUser(User newUser);

    /// <summary>
    /// Gets all users in the table
    /// </summary>
    /// <returns>List of Users in the table or empty list if none is found</returns>
    public Task<List<User>> GetAllUsers(Expression<Func<User, bool>>? filter = null);

    /// <summary>
    /// Updates given User
    /// </summary>
    /// <param name="User">The new version of the User</param>
    /// <returns>The User that was updated, returns null if not succesfull</returns>
    public Task<User?> UpdateUser(User User);

    Task UpdateRefreshToken(
    string userId,
    string refreshToken,
    DateTime expiresAt
);

    /// <summary>
    /// Delets User from table
    /// </summary>
    /// <param name="UserId">Id of the User needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeleteUser(string UserId);
}