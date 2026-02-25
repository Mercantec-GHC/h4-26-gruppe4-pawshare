using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;
using Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

/// <summary>
/// Controller for managing chat functionality including chat rooms, users, and messages.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public ChatController(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }

    /// <summary>
    /// Creates a new chat with the specified title and users.
    /// </summary>
    /// <param name="dto">The DTO containing the chat title and initial users.</param>
    /// <returns>The newly created chat.</returns>
    /// <response code="200">Returns the newly created chat DTO.</response>
    /// <response code="400">Returns bad request if not successful.</response>
    [HttpPost]
    public async Task<ActionResult<CreateChatDto>> CreateChat([Required] [FromBody] CreateChatDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return BadRequest();

        dto.UserIds.Add(userId); // Ensure the creator is part of the chat

        var chat = await _chatService.CreateChatAsync(dto);
        if (chat is null)
            return BadRequest();

        return Ok(chat);
    }

    [HttpGet("{chatId}")]
    public async Task<ActionResult<bool>> GetChat(string chatId)
    {
        var exists = await _chatService.GetChat(chatId);
        if(!exists)
            return NotFound();
        return Ok(exists);
    }

    /// <summary>
    /// Gets all chats for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of chats the user belongs to.</returns>
    /// <response code="200">Returns the list of chats.</response>
    /// <response code="400">Returns bad request if userid can't be found.</response>
    // [HttpGet("me")]
    // public async Task<ActionResult<List<ChatListItemDto>>> GetChats([FromQuery] int? limit, [FromQuery] int? offset)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userId == null)
    //         return BadRequest();

    //     var chats = await _chatService.GetChatsForUserAsync(userId, limit, offset);
    //     return Ok(chats);
    // }

    /// <summary>
    /// Gets messages from given chat
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="limit">The limit to amount of entries wanted.</param>
    /// <param name="offset">The offset for how many entries to skip.</param>
    /// <returns>The messages in the given chat.</returns>
    /// <response code="200">Returns the list even if empty.</response>
    /// <response code="404">If the messages are null.</response>
    // [HttpGet("{chatId}/messages")]
    // public async Task<ActionResult<List<MessageDto>>> GetMessages(string chatId, [FromQuery] int? limit, [FromQuery] int? offset)
    // {
    //     List<MessageDto> messages = await _chatService.GetMessagesAsync(chatId, limit, offset);
    //     if (messages == null)
    //         return NotFound();

    //     return Ok(messages);
    // }

    /// <summary>
    /// Sends a message to a given chat from a given user with a given message
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="request">The request dto.</param>
    /// <returns>Success or bad request</returns>
    /// <response code="204">Returns no content if successful.</response>
    /// <response code="400">Bad request if not succesful sending or jwt not containing userid.</response>
    // [HttpPost("{chatId}/messages")]
    // public async Task<IActionResult> SendMessage(string chatId, [Required] [FromBody] string content)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    //     if (userId == null) 
    //         return BadRequest();

    //     var sent = await _chatService.SendMessageAsync(chatId, userId, content);
    //     if (!sent)
    //         return BadRequest();

    //     return NoContent();
    // }

    /// <summary>
    /// Marks message as read.
    /// </summary>
    /// <param name="messageId">The unique identifier of the message.</param>
    /// <param name="userId">The unique identifier of the user who read the message.</param>
    /// <returns>Success or bad request.</returns>
    /// <response code="204">Returns no content if successful.</response>
    /// <response code="400">Bad request if not succesful marking as read or if jwt not containing a userid.</response>
    // [HttpPost("messages/{messageId}/read")]
    // public async Task<IActionResult> MarkRead(string messageId)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    //     if (userId == null)
    //         return BadRequest();

    //     var ok = await _messageService.MarkMessageReadAsync(messageId, userId);
    //     if (!ok)
    //         return BadRequest();

    //     return NoContent();
    // }

    /// <summary>
    /// Gets a list of all unread messages for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>List of unread messages, empty if none is found.</returns>
    /// <response code="200">Returns the list of messages even if null or empty.</response>
    /// <response code="400">Returns bad request if jwt does not contain userid.</response>
    // [HttpGet("unread")]
    // public async Task<ActionResult<List<UnreadChatDto>>> GetUnreadList()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    //     if (userId == null)
    //         return BadRequest();

    //     List<UnreadChatDto> result = await _chatService.GetUnreadChatsAsync(userId);
    //     return Ok(result);
    // }
}
