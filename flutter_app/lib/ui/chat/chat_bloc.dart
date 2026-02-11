import 'dart:convert';

import 'package:flutter_bloc/flutter_bloc.dart';

import '../../classes/helpers/api.dart';
import '../../classes/objects/api_path.dart';
import '../../classes/objects/message_dto.dart';
import 'chat_events_states.dart';

class ChatBloc extends Bloc<ChatEvents, ChatState> {
  ChatBloc() : super(const ChatInitialState()) {
    on<ChatLoadEvent>(_onChatLoad);
    on<ChatOpenEvent>(_onChatOpen);
    on<ChatBackToListEvent>(_onChatBackToList);
    on<ChatSendMessageEvent>(_onChatSendMessage);
  }

  static const String currentUserId = '331d511d-2ed3-4392-99e9-f31caf9097d4';

  final List<ChatDto> _chats = [];
  final Map<String, List<MessageDTO>> _messages = {};

  Future<void> _onChatLoad(ChatLoadEvent event, Emitter<ChatState> emit) async {
    final chats = await _fetchChats();
    _chats
      ..clear()
      ..addAll(chats);
    emit(ChatListState(List.unmodifiable(_chats)));
  }

  Future<void> _onChatOpen(ChatOpenEvent event, Emitter<ChatState> emit) async {
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
    final messages = await _fetchMessages(event.chatId);
    _messages[event.chatId] = messages;
    emit(ChatDetailState(chat: updatedChat, messages: messages));
  }

  void _onChatBackToList(ChatBackToListEvent event, Emitter<ChatState> emit) {
    emit(ChatListState(List.unmodifiable(_chats)));
  }

  Future<void> _onChatSendMessage(
    ChatSendMessageEvent event,
    Emitter<ChatState> emit,
  ) async {
    final trimmedText = event.text.trim();
    if (trimmedText.isEmpty) {
      return;
    }
    final sent = await _sendMessage(event.chatId, trimmedText);
    if (!sent) {
      return;
    }

    final refreshedMessages = await _fetchMessages(event.chatId);
    _messages[event.chatId] = refreshedMessages;

    if (refreshedMessages.isNotEmpty) {
      _updateChatPreview(event.chatId, refreshedMessages.last);
    }

    if (state is ChatDetailState) {
      final chat = _chats.firstWhere(
        (item) => item.id == event.chatId,
        orElse: () => (state as ChatDetailState).chat,
      );
      emit(
        ChatDetailState(
          chat: chat,
          messages: List.unmodifiable(refreshedMessages),
        ),
      );
    }
  }

  Future<List<ChatDto>> _fetchChats() async {
    final response = await API.getRequestWithId(
      ApiPath.chat,
      'user/$currentUserId',
    );

    if (response.statusCode != 200) {
      return [];
    }

    final decoded = json.decode(response.body);
    if (decoded is! List) {
      return [];
    }

    return decoded
        .whereType<Map<String, dynamic>>()
        .map(ChatDto.fromJson)
        .toList();
  }

  Future<List<MessageDTO>> _fetchMessages(String chatId) async {
    final response = await API.getRequestWithId(
      ApiPath.chat,
      '$chatId/messages',
    );

    if (response.statusCode != 200) {
      return [];
    }

    final decoded = json.decode(response.body);
    if (decoded is! List) {
      return [];
    }

    final messages = decoded
        .whereType<Map<String, dynamic>>()
        .map((json) => MessageDTO.fromJson(json, chatId: chatId))
        .toList();

    messages.sort((a, b) => a.createdTimestamp.compareTo(b.createdTimestamp));
    return messages;
  }

  // TODO: This should be changed when the api is ready to accept message content in the body instead of query parameters, and to not require userId as a parameter (it should be taken from the auth token instead)
  Future<bool> _sendMessage(String chatId, String content) async {
    final response = await API.postRequestWithId(
      ApiPath.chat,
      '$chatId/messages?userId=$currentUserId&content=${Uri.encodeComponent(content)}',
      null,
    );

    return response.statusCode == 204;
  }

  void _updateChatPreview(String chatId, MessageDTO latestMessage) {
    final chatIndex = _chats.indexWhere((chat) => chat.id == chatId);
    if (chatIndex == -1) {
      return;
    }

    final chat = _chats[chatIndex];
    _chats[chatIndex] = ChatDto(
      id: chat.id,
      title: chat.title,
      lastMessage: latestMessage.content,
      lastUpdated: latestMessage.createdTimestamp,
      unreadCount: chat.unreadCount,
    );
  }
}
