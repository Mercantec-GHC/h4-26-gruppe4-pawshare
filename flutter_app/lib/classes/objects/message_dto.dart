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

  factory MessageDTO.fromJson(Map<String, dynamic> json) {
    return MessageDTO(
      id: json['id'] as String,
      content: json['content'] as String,
      senderId: json['senderId'] as String,
      chatId: json['chatId'] as String,
      createdTimestamp: DateTime.parse(json['createdAt'] as String),
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
    return ChatDto(
      id: json['id'] as String,
      title: json['title'] as String,
      lastMessage: json['lastMessage'] as String,
      lastUpdated: DateTime.parse(json['lastUpdated'] as String),
      unreadCount: json['unreadCount'] as int,
    );
  }
}
