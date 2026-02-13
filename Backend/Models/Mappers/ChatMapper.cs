using Models;
using Models.DTOs;

namespace Mappers;

public static class ChatMapper
{
    public static ChatListItemDto ToListItemDto(Chat chat, Message? newestMessage, int unreadCount)
    {
        return new ChatListItemDto
        {
            ChatId = chat.Id,
            Title = chat.Title,
            NewestMessage = newestMessage is null ? null : MessageMapper.ToPreviewDto(newestMessage),
            Members = chat.ChatUsers.Select(cu => new ChatMemberDto {
                UserId = cu.UserId,
                UserName = cu.User?.Name ?? "Unknown"
            }).ToList(),
            UnreadCount = unreadCount
        };
    }
}
