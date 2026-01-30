using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

/// <summary>
/// Controller for managing posts in Pawshare.
/// Posts are created by institutions requesting animal visits.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// Gets all posts.
    /// </summary>
    /// <returns>A list of all posts in the system.</returns>
    /// <response code="200">Returns the list of posts.</response>
    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAllPosts()
    {
        var posts = await _postService.GetAllPostsAsync();
        return Ok(posts);
    }

    /// <summary>
    /// Gets a post by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>The post with the specified ID.</returns>
    /// <response code="200">Returns the post.</response>
    /// <response code="404">If the post is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(string id)
    {
        var post = await _postService.GetPostAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    /// <summary>
    /// Gets all posts created by a specific owner.
    /// </summary>
    /// <param name="ownerId">The unique identifier of the post owner.</param>
    /// <returns>A list of posts created by the specified owner.</returns>
    /// <response code="200">Returns the list of posts.</response>
    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<List<Post>>> GetPostsByOwner(string ownerId)
    {
        var posts = await _postService.GetPostsByOwnerAsync(ownerId);
        return Ok(posts);
    }

    /// <summary>
    /// Gets all posts that are requests for animal visits.
    /// </summary>
    /// <returns>A list of all request posts.</returns>
    /// <response code="200">Returns the list of request posts.</response>
    [HttpGet("requests")]
    public async Task<ActionResult<List<Post>>> GetRequestPosts()
    {
        var posts = await _postService.GetRequestPostsAsync();
        return Ok(posts);
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post data to create.</param>
    /// <returns>The newly created post.</returns>
    /// <response code="201">Returns the newly created post.</response>
    /// <response code="400">If the post data is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
    {
        var created = await _postService.CreatePostAsync(post);
        if (created == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetPost), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="id">The unique identifier of the post to update.</param>
    /// <param name="post">The updated post data.</param>
    /// <returns>The updated post.</returns>
    /// <response code="200">Returns the updated post.</response>
    /// <response code="404">If the post is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> UpdatePost(string id, [FromBody] Post post)
    {
        var updated = await _postService.UpdatePostAsync(id, post);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="id">The unique identifier of the post to delete.</param>
    /// <response code="204">If the post was successfully deleted.</response>
    /// <response code="404">If the post is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(string id)
    {
        var deleted = await _postService.DeletePostAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Allows a user to show interest in a post.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <param name="userId">The unique identifier of the user showing interest.</param>
    /// <response code="200">If the user successfully showed interest in the post.</response>
    /// <response code="404">If the post or user is not found.</response>
    [HttpPost("{postId}/accept/{userId}")]
    public async Task<ActionResult> AcceptPost(string postId, string userId)
    {
        var result = await _postService.AcceptPostAsync(postId, userId);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
}
