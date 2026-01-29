using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing posts in Pawshare.
/// Posts are created by institutions requesting animal visits.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Gets a post by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>The post if found, otherwise null.</returns>
    Task<Post?> GetPostAsync(string id);

    /// <summary>
    /// Gets all posts in the system.
    /// </summary>
    /// <returns>A list of all posts.</returns>
    Task<List<Post>> GetAllPostsAsync();

    /// <summary>
    /// Gets all posts created by a specific user/institution.
    /// </summary>
    /// <param name="ownerId">The unique identifier of the post owner.</param>
    /// <returns>A list of posts created by the specified owner.</returns>
    Task<List<Post>> GetPostsByOwnerAsync(string ownerId);

    /// <summary>
    /// Gets all posts that are requests for animal visits.
    /// </summary>
    /// <returns>A list of all request posts.</returns>
    Task<List<Post>> GetRequestPostsAsync();

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post data to create.</param>
    /// <returns>The newly created post if successful, otherwise null.</returns>
    Task<Post?> CreatePostAsync(Post post);

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="id">The unique identifier of the post to update.</param>
    /// <param name="post">The updated post data.</param>
    /// <returns>The updated post if found, otherwise null.</returns>
    Task<Post?> UpdatePostAsync(string id, Post post);

    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="id">The unique identifier of the post to delete.</param>
    /// <returns>True if the post was deleted successfully, otherwise false.</returns>
    Task<bool> DeletePostAsync(string id);

    /// <summary>
    /// Allows a user to show interest in a post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="userId">The unique identifier of the user showing interest.</param>
    /// <returns>True if the operation was successful, otherwise false.</returns>
    Task<bool> AcceptPostAsync(string postId, string userId);
}
