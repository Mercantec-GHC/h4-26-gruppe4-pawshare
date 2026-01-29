using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public Task<User?> GetUser(string id)
    {
        return _userRepo.GetUser(id);
    }
}