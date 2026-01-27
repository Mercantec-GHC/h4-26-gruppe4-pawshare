namespace Repositories.Interfaces;
using Models;

public interface IPostRepo
{
    public Task<Post> GetPost(string id);
    public Task<Post> PostPost(Post newPost);
    public Task<List<Post>> GetAllPosts();
}