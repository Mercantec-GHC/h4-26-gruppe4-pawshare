class SendMessageDto {
  final String content;
  final String chatId;

  SendMessageDto({required this.content, required this.chatId});

  Map<String, dynamic> toJson() => {'content': content, 'chatId': chatId};
}

class MessageDTO {
  final String id;
  final String content;
  final String senderId;
  final String chatId;
  final DateTime createdTimestamp;

  MessageDTO({
    required this.id,
    required this.content,
    required this.senderId,
    required this.chatId,
    required this.createdTimestamp,
  });

  factory MessageDTO.fromJson(
    Map<String, dynamic> json, {
    required String chatId,
  }) {
    final createdAt = json['createdAt'] ?? json['createdTimestamp'];
    final createdTimestamp = createdAt == null
      ? DateTime.now()
      : DateTime.parse(createdAt.toString());

    return MessageDTO(
      id: (json['messageId'] ?? json['id'] ?? '').toString(),
      content: (json['content'] ?? '').toString(),
      senderId: (json['senderId'] ?? '').toString(),
      chatId: chatId,
      createdTimestamp: createdTimestamp,
    );
  }
}

class ChatDto {
  final String id;
  final String title;
  final String lastMessage;
  final DateTime lastUpdated;
  final int unreadCount;

  ChatDto({
    required this.id,
    required this.title,
    required this.lastMessage,
    required this.lastUpdated,
    required this.unreadCount,
  });

  factory ChatDto.fromJson(Map<String, dynamic> json) {
    final newestMessage = json['newestMessage'];
    final lastUpdatedRaw = json['lastUpdated'] ??
        (newestMessage is Map<String, dynamic>
            ? newestMessage['createdAt']
            : null);
    final lastMessageRaw = newestMessage is Map<String, dynamic>
        ? newestMessage['content']
        : json['lastMessage'];
    final unreadRaw = json['unreadCount'];
    final unreadCount = unreadRaw is num
        ? unreadRaw.toInt()
        : int.tryParse(unreadRaw?.toString() ?? '') ?? 0;

    return ChatDto(
      id: (json['chatId'] ?? json['id']).toString(),
      title: (json['title'] ?? '').toString(),
      lastMessage: (lastMessageRaw ?? '').toString(),
      lastUpdated: lastUpdatedRaw == null
          ? DateTime.now()
          : DateTime.parse(lastUpdatedRaw.toString()),
      unreadCount: unreadCount,
    );
  }
}
