namespace Repositories.Interfaces;
using Models;

public interface IMessageRepo
{
    public Task<List<Message>> GetMessagesFromChat(string chatId);
    public Task<Message> SendMessage(Message message);
}