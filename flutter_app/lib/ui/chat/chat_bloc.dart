import 'package:flutter_bloc/flutter_bloc.dart';

import '../../classes/objects/message_dto.dart';
import 'chat_events_states.dart';

class ChatBloc extends Bloc<ChatEvents, ChatState> {
  ChatBloc() : super(const ChatInitialState()) {
    _seedData();
    on<ChatLoadEvent>(_onChatLoad);
    on<ChatOpenEvent>(_onChatOpen);
    on<ChatBackToListEvent>(_onChatBackToList);
    on<ChatSendMessageEvent>(_onChatSendMessage);
  }

  static const String _currentUserId = 'me';

  final List<ChatDto> _chats = [];
  final Map<String, List<MessageDTO>> _messages = {};

  void _seedData() {
    final now = DateTime.now();
    _chats.addAll([
      ChatDto(
        id: 'chat-1',
        title: 'Maja Nielsen',
        lastMessage: 'See you at 17:30.',
        lastUpdated: now.subtract(const Duration(minutes: 6)),
        unreadCount: 1,
      ),
      ChatDto(
        id: 'chat-2',
        title: 'Pawshare Support',
        lastMessage: 'We can help with that.',
        lastUpdated: now.subtract(const Duration(hours: 2)),
        unreadCount: 0,
      ),
      ChatDto(
        id: 'chat-3',
        title: 'Jonas Holm',
        lastMessage: 'Thanks for the update!',
        lastUpdated: now.subtract(const Duration(days: 1, hours: 3)),
        unreadCount: 2,
      ),
    ]);

    _messages['chat-1'] = [
      MessageDTO(
        id: 'm-1',
        chatId: 'chat-1',
        content: 'Hi! Are we still on for later?',
        senderId: 'user-1',
        createdTimestamp: now.subtract(const Duration(minutes: 40)),
      ),
      MessageDTO(
        id: 'm-2',
        chatId: 'chat-1',
        content: 'Yes, see you at 17:30.',
        senderId: _currentUserId,
        createdTimestamp: now.subtract(const Duration(minutes: 35)),
      ),
    ];

    _messages['chat-2'] = [
      MessageDTO(
        id: 'm-3',
        chatId: 'chat-2',
        content: 'Hi, I need help with my booking.',
        senderId: _currentUserId,
        createdTimestamp: now.subtract(const Duration(hours: 3)),
      ),
      MessageDTO(
        id: 'm-4',
        chatId: 'chat-2',
        content: 'We can help with that. What seems to be the issue?',
        senderId: 'support',
        createdTimestamp: now.subtract(const Duration(hours: 2, minutes: 50)),
      ),
    ];

    _messages['chat-3'] = [
      MessageDTO(
        id: 'm-5',
        chatId: 'chat-3',
        content: 'I updated the visit details.',
        senderId: _currentUserId,
        createdTimestamp: now.subtract(const Duration(days: 1, hours: 4)),
      ),
      MessageDTO(
        id: 'm-6',
        chatId: 'chat-3',
        content: 'Thanks for the update!',
        senderId: 'user-2',
        createdTimestamp: now.subtract(
          const Duration(days: 1, hours: 3, minutes: 50),
        ),
      ),
    ];
  }

  Future<void> _onChatLoad(ChatLoadEvent event, Emitter<ChatState> emit) async {
    emit(ChatListState(List.unmodifiable(_chats)));
  }

  void _onChatOpen(ChatOpenEvent event, Emitter<ChatState> emit) {
    final chatIndex = _chats.indexWhere((chat) => chat.id == event.chatId);
    if (chatIndex == -1) {
      emit(ChatListState(List.unmodifiable(_chats)));
      return;
    }

    final currentChat = _chats[chatIndex];
    final updatedChat = ChatDto(
      id: currentChat.id,
      title: currentChat.title,
      lastMessage: currentChat.lastMessage,
      lastUpdated: currentChat.lastUpdated,
      unreadCount: 0,
    );
    _chats[chatIndex] = updatedChat;
    final messages = List<MessageDTO>.from(_messages[event.chatId] ?? const []);
    emit(ChatDetailState(chat: updatedChat, messages: messages));
  }

  void _onChatBackToList(ChatBackToListEvent event, Emitter<ChatState> emit) {
    emit(ChatListState(List.unmodifiable(_chats)));
  }

  void _onChatSendMessage(ChatSendMessageEvent event, Emitter<ChatState> emit) {
    final trimmedText = event.text.trim();
    if (trimmedText.isEmpty) {
      return;
    }

    final now = DateTime.now();
    final updatedMessage = MessageDTO(
      id: 'm-${now.microsecondsSinceEpoch}',
      chatId: event.chatId,
      content: trimmedText,
      senderId: _currentUserId,
      createdTimestamp: now,
    );

    final existingMessages = List<MessageDTO>.from(
      _messages[event.chatId] ?? const [],
    );
    existingMessages.add(updatedMessage);
    _messages[event.chatId] = existingMessages;

    final chatIndex = _chats.indexWhere(
      (chat) => chat.id == event.chatId,
    );
    if (chatIndex != -1) {
      final chat = _chats[chatIndex];
      _chats[chatIndex] = ChatDto(
        id: chat.id,
        title: chat.title,
        lastMessage: trimmedText,
        lastUpdated: now,
        unreadCount: chat.unreadCount,
      );
    }

    if (state is ChatDetailState) {
      final chat = chatIndex == -1
          ? (state as ChatDetailState).chat
          : _chats[chatIndex];
      emit(
        ChatDetailState(
          chat: chat,
          messages: List.unmodifiable(existingMessages),
        ),
      );
    }
  }
}
