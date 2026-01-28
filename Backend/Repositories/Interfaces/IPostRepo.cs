namespace Repositories.Interfaces;
using Models;

public interface IPostRepo
{
    /// <summary>
    /// Gets post with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted post</param>
    /// <returns>Post with given id, if not found returns null</returns>
    public Task<Post?> GetPost(string id);

    /// <summary>
    /// Posts a new post to the table
    /// </summary>
    /// <param name="newPost">The new post that needs to be posted</param>
    /// <returns>Post that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<Post?> PostPost(Post newPost);

    /// <summary>
    /// Gets all Posts in the table
    /// </summary>
    /// <returns>List of Posts in the table or empty list if none is found</returns>
    public Task<List<Post>> GetAllPosts();
}