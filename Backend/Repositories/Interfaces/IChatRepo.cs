namespace Repositories.Interfaces;
using Models;

public interface IChatRepo
{
    public Task<List<Chat>> GetChatsWithUser(string userId);
    public Task<Chat> PostChat(Chat newChat);
}