

class ChatId {
  ChatId({
    required this.chatId,

  });

final String chatId;


  factory ChatId.fromJson(Map<String, dynamic> json) {
    return ChatId(
      chatId: json['chatId'],
    );
  }
}