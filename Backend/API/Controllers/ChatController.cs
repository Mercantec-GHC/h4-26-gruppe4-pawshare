using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;
using Repositories.Interfaces;
using Repositories;

namespace API.Controllers;

/// <summary>
/// Controller for managing chat functionality including chat rooms, users, and messages.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Gets a chat by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the chat.</param>
    /// <returns>The chat with the specified ID.</returns>
    /// <response code="200">Returns the chat.</response>
    /// <response code="404">If the chat is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Chat>> GetChat(string id)
    {
        var chat = await _chatService.GetChatAsync(id);
        if (chat == null)
        {
            return NotFound();
        }
        return Ok(chat);
    }

    /// <summary>
    /// Creates a new chat with the specified title and users.
    /// </summary>
    /// <param name="request">The request containing the chat title and initial users.</param>
    /// <returns>The newly created chat.</returns>
    /// <response code="201">Returns the newly created chat.</response>
    [HttpPost]
    public async Task<ActionResult<Chat>> CreateChat([FromBody] CreateChatRequest request)
    {
        var chat = await _chatService.CreateChatAsync(request.Title, request.Users);
        return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
    }

    /// <summary>
    /// Gets all chats for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of chats the user belongs to.</returns>
    /// <response code="200">Returns the list of chats.</response>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Chat>>> GetUserChats(string userId)
    {
        var chats = await _chatService.GetUserChatsAsync(userId);
        return Ok(chats);
    }

    /// <summary>
    /// Adds a user to an existing chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <returns>The updated chat with the new user.</returns>
    /// <response code="200">Returns the updated chat.</response>
    /// <response code="404">If the chat or user is not found.</response>
    [HttpPost("{chatId}/users/{userId}")]
    public async Task<ActionResult<Chat>> AddUserToChat(string chatId, string userId)
    {
        var chat = await _chatService.AddUserToChatAsync(chatId, userId);
        if (chat == null)
        {
            return NotFound();
        }
        return Ok(chat);
    }

    /// <summary>
    /// Removes a user from an existing chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <returns>The updated chat without the removed user.</returns>
    /// <response code="200">Returns the updated chat.</response>
    /// <response code="404">If the chat or user is not found.</response>
    [HttpDelete("{chatId}/users/{userId}")]
    public async Task<ActionResult<Chat>> RemoveUserFromChat(string chatId, string userId)
    {
        var chat = await _chatService.RemoveUserFromChatAsync(chatId, userId);
        if (chat == null)
        {
            return NotFound();
        }
        return Ok(chat);
    }

    /// <summary>
    /// Gets all messages from a specific chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <returns>A list of messages in the chat.</returns>
    /// <response code="200">Returns the list of messages.</response>
    [HttpGet("{chatId}/messages")]
    public async Task<ActionResult<List<Message>>> GetChatMessages(string chatId)
    {
        var messages = await _chatService.GetChatMessagesAsync(chatId);
        return Ok(messages);
    }

    /// <summary>
    /// Sends a new message to a chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="request">The request containing the sender ID and message content.</param>
    /// <returns>The updated chat with the new message.</returns>
    /// <response code="200">Returns the updated chat.</response>
    /// <response code="404">If the chat is not found.</response>
    [HttpPost("{chatId}/messages")]
    public async Task<ActionResult<Chat>> SendMessage(string chatId, [FromBody] SendMessageRequest request)
    {
        var chat = await _chatService.SendMessageAsync(chatId, request.SenderId, request.Content);
        if (chat == null)
        {
            return NotFound();
        }
        return Ok(chat);
    }
}

/// <summary>
/// Request model for creating a new chat.
/// </summary>
public class CreateChatRequest
{
    /// <summary>
    /// The title of the chat.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// The list of users to include in the chat.
    /// </summary>
    public required List<User> Users { get; set; }
}

/// <summary>
/// Request model for sending a message.
/// </summary>
public class SendMessageRequest
{
    /// <summary>
    /// The unique identifier of the user sending the message.
    /// </summary>
    public required string SenderId { get; set; }

    /// <summary>
    /// The content of the message.
    /// </summary>
    public required string Content { get; set; }
}
