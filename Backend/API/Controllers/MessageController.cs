using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

/// <summary>
/// Controller for managing messages in Pawshare chats.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    /// <summary>
    /// Gets all messages from a specific chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <returns>A list of messages in the chat.</returns>
    /// <response code="200">Returns the list of messages.</response>
    [HttpGet("chat/{chatId}")]
    public async Task<ActionResult<List<Message>>> GetMessagesByChat(string chatId)
    {
        var messages = await _messageService.GetMessagesByChatAsync(chatId);
        return Ok(messages);
    }

    /// <summary>
    /// Sends a new message to a chat.
    /// </summary>
    /// <param name="request">The request containing the chat ID, user ID, and message content.</param>
    /// <returns>The newly created message.</returns>
    /// <response code="200">Returns the created message.</response>
    /// <response code="400">If the request data is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Message>> SendMessage([FromBody] CreateMessageRequest request)
    {
        var message = await _messageService.SendMessageAsync(request.ChatId, request.UserId, request.Content);
        if (message == null)
        {
            return BadRequest();
        }
        return Ok(message);
    }

    /// <summary>
    /// Deletes a message.
    /// </summary>
    /// <param name="id">The unique identifier of the message to delete.</param>
    /// <response code="204">If the message was successfully deleted.</response>
    /// <response code="404">If the message is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var deleted = await _messageService.DeleteMessageAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}

/// <summary>
/// Request model for creating a message.
/// </summary>
public class CreateMessageRequest
{
    /// <summary>
    /// The unique identifier of the chat.
    /// </summary>
    public required string ChatId { get; set; }
    
    /// <summary>
    /// The unique identifier of the user sending the message.
    /// </summary>
    public required string UserId { get; set; }
    
    /// <summary>
    /// The content of the message.
    /// </summary>
    public required string Content { get; set; }
}
