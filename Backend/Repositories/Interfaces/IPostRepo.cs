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

    /// <summary>
    /// Updates given Post
    /// </summary>
    /// <param name="Post">The new version of the Post</param>
    /// <returns>The Post that was updated, returns null if not succesfull</returns>
    public Task<Post?> UpdatePost(Post Post);

    /// <summary>
    /// Delets Post from table
    /// </summary>
    /// <param name="PostId">Id of the Post needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeletePost(string PostId);
}