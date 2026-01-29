using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class PostService : IPostService
{
    private readonly IPostRepo _postRepo;

    public PostService(IPostRepo postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task<Post?> GetPostAsync(string id)
    {
        return await _postRepo.GetPost(id);
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _postRepo.GetAllPosts();
    }

    public async Task<List<Post>> GetPostsByOwnerAsync(string ownerId)
    {
        var allPosts = await _postRepo.GetAllPosts();
        return allPosts.Where(p => p.OwnerId == ownerId).ToList();
    }

    public async Task<List<Post>> GetRequestPostsAsync()
    {
        var allPosts = await _postRepo.GetAllPosts();
        return allPosts.Where(p => p.IsRequest).ToList();
    }

    public async Task<Post?> CreatePostAsync(Post post)
    {
        post.Id = Guid.NewGuid().ToString();
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;
        return await _postRepo.PostPost(post);
    }

    public async Task<Post?> UpdatePostAsync(string id, Post post)
    {
        var existing = await _postRepo.GetPost(id);
        if (existing == null) return null;

        existing.Description = post.Description;
        existing.IsRequest = post.IsRequest;
        existing.Start = post.Start;
        existing.End = post.End;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public async Task<bool> DeletePostAsync(string id)
    {
        var post = await _postRepo.GetPost(id);
        return post != null;
    }

    public async Task<bool> AcceptPostAsync(string postId, string userId)
    {
        var post = await _postRepo.GetPost(postId);
        return post != null;
    }
}
