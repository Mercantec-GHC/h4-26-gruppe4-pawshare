using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;
using Repositories.Interfaces;
using Repositories;
using Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

/// <summary>
/// Controller for managing chat functionality including chat rooms, users, and messages.
/// </summary>
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
    /// <response code="404">Returns bad request if not successfull.</response>
    [HttpPost]
    public async Task<ActionResult<CreateChatDto>> CreateChat([Required] [FromBody] CreateChatDto dto)
    {
        var chat = await _chatService.CreateChatAsync(dto);
        if (chat is null)
            return BadRequest();

        return Ok(chat);
    }

    /// <summary>
    /// Gets all chats for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of chats the user belongs to.</returns>
    /// <response code="200">Returns the list of chats.</response>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ChatListItemDto>>> GetChats(string userId, [FromQuery] int? limit, [FromQuery] int? offset)
    {
        var chats = await _chatService.GetChatsForUserAsync(userId, limit, offset);
        return Ok(chats);
    }

    /// <summary>
    /// Gets messages from given chat
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="limit">The limit to amount of entries wanted.</param>
    /// <param name="offset">The offset for how many entries to skip.</param>
    /// <returns>The messages in the given chat.</returns>
    /// <response code="200">Returns the list even if empty.</response>
    /// <response code="404">If the messages are null.</response>
    [HttpGet("{chatId}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(string chatId, [FromQuery] int? limit, [FromQuery] int? offset)
    {
        List<MessageDto> messages = await _chatService.GetMessagesAsync(chatId, limit, offset);
        if (messages == null)
            return NotFound();

        return Ok(messages);
    }

    /// <summary>
    /// Sends a message to a given chat from a given user with a given message
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the sender.</param>
    /// <param name="content">The message to be sent.</param>
    /// <returns>Success or bad request</returns>
    /// <response code="204">Returns no content if successful.</response>
    /// <response code="400">Bad request if not succesful sending.</response>
    [HttpPost("{chatId}/messages")]
    public async Task<IActionResult> SendMessage(string chatId, [Required] [FromQuery] string userId, [Required] [FromQuery] string content)
    {
        var sent = await _chatService.SendMessageAsync(chatId, userId, content);
        if (!sent)
            return BadRequest();

        return NoContent();
    }

    /// <summary>
    /// Marks message as read.
    /// </summary>
    /// <param name="messageId">The unique identifier of the message.</param>
    /// <param name="userId">The unique identifier of the user who read the message.</param>
    /// <returns>Success or bad request.</returns>
    /// <response code="204">Returns no content if successful.</response>
    /// <response code="400">Bad request if not succesful marking as read.</response>
    [HttpPost("messages/{messageId}/read")]
    public async Task<IActionResult> MarkRead(string messageId, [Required] [FromQuery] string userId)
    {
        var ok = await _messageService.MarkMessageReadAsync(messageId, userId);
        if (!ok)
            return BadRequest();

        return NoContent();
    }

    /// <summary>
    /// Gets a list of all unread messages for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>List of unread messages, empty if none is found.</returns>
    /// <response code="200">Returns the list of messages even if null or empty.</response>
    [HttpGet("unread/{userId}")]
    public async Task<ActionResult<List<UnreadChatDto>>> GetUnreadList(string userId)
    {
        List<UnreadChatDto> result = await _chatService.GetUnreadChatsAsync(userId);
        return Ok(result);
    }
}
