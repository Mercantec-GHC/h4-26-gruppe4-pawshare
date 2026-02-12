using Models;
using Models.DTOs;

namespace Mappers;

public static class MessageMapper
{
    public static MessageDto ToDto(Message m)
    {
        return new MessageDto
        {
            MessageId = m.Id,
            Content = m.Content,
            SenderId = m.UserId,
            CreatedAt = m.CreatedAt
        };
    }

    public static MessagePreviewDto? ToPreviewDto(Message? m)
    {
        if (m is null)
            return null;

        return new MessagePreviewDto
        {
            MessageId = m.Id,
            Content = m.Content,
            SenderId = m.UserId,
            CreatedAt = m.CreatedAt
        };
    }
}
