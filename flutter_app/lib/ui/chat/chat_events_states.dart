import '../../classes/objects/message_dto.dart';

abstract class ChatEvents {
  const ChatEvents();
}

class ChatLoadEvent extends ChatEvents {
  const ChatLoadEvent();
}

class ChatOpenEvent extends ChatEvents {
  const ChatOpenEvent(this.chatId);

  final String chatId;
}

class ChatBackToListEvent extends ChatEvents {
  const ChatBackToListEvent();
}

class ChatSendMessageEvent extends ChatEvents {
  const ChatSendMessageEvent(this.chatId, this.text);

  final String chatId;
  final String text;
}

abstract class ChatState {
  const ChatState();
}

class ChatInitialState extends ChatState {
  const ChatInitialState();
}

class ChatListState extends ChatState {
  const ChatListState(this.chats);

  final List<ChatDto> chats;
}

class ChatDetailState extends ChatState {
  const ChatDetailState({required this.chat, required this.messages});

  final ChatDto chat;
  final List<MessageDTO> messages;
}
