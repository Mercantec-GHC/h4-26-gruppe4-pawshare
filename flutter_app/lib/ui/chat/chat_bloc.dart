import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';

import '../../classes/helpers/auth.dart';
import '../../classes/objects/message_dto.dart';
import '../../classes/services/chat_service.dart';
import 'chat_events_states.dart';

class ChatBloc extends Bloc<ChatEvents, ChatState> {
  ChatBloc() : super(const ChatInitialState()) {
    on<ChatLoadEvent>(_onChatLoad);
    on<ChatOpenEvent>(_onChatOpen);
    on<ChatBackToListEvent>(_onChatBackToList);
    on<ChatSendMessageEvent>(_onChatSendMessage);
    on<ChatRealtimeMessageReceivedEvent>(_onRealtimeMessageReceived);
  }

  static String currentUserId = '';

  final ChatService _chatService = ChatService.instance;
  final List<ChatDto> _chats = [];
  final Map<String, List<MessageDTO>> _messages = {};
  String? _activeChatId;
  StreamSubscription<MessageDTO>? _messageSubscription;

  Future<void> _onChatLoad(ChatLoadEvent event, Emitter<ChatState> emit) async {
    currentUserId = await Auth.getCurrentUserId();

    await _chatService.connect();

    _messageSubscription?.cancel();
    _messageSubscription = _chatService.messageStream.listen((message) {
      add(ChatRealtimeMessageReceivedEvent(message));
    });

    final chats = await _chatService.getInitialChats();
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

    if (_activeChatId != null && _activeChatId != event.chatId) {
      await _chatService.leaveChat(_activeChatId!);
    }

    _activeChatId = event.chatId;
    await _chatService.joinChat(event.chatId);

    final messages = await _chatService.getMessages(event.chatId);
    _messages[event.chatId] = messages;
    final unreadByChat = await _chatService.getUnreadCounts();
    final unreadCount = unreadByChat[event.chatId] ?? 0;
    final unreadMessages = _getNewestUnreadMessages(messages, unreadCount);
    await _chatService.markMessagesAsRead(unreadMessages);
    _setUnreadCount(event.chatId, 0);

    final updatedChat = _chats.firstWhere(
      (item) => item.id == event.chatId,
      orElse: () => currentChat,
    );

    emit(ChatDetailState(chat: updatedChat, messages: messages));
  }

  Future<void> _onChatBackToList(
    ChatBackToListEvent event,
    Emitter<ChatState> emit,
  ) async {
    if (_activeChatId != null) {
      await _chatService.leaveChat(_activeChatId!);
      _activeChatId = null;
    }

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
    final sent = await _chatService.sendMessage(event.chatId, trimmedText);
    if (!sent) {
      return;
    }
  }

  Future<void> _onRealtimeMessageReceived(
    ChatRealtimeMessageReceivedEvent event,
    Emitter<ChatState> emit,
  ) async {
    final message = event.message;
    final chatMessages = _addMessageIfMissing(message);

    _updateChatPreview(message.chatId, message);

    if (message.senderId != currentUserId) {
      if (_activeChatId == message.chatId) {
        await _chatService.markRead(message.id);
      } else {
        _incrementUnreadCount(message.chatId);
      }
    }

    if (state is ChatDetailState && _activeChatId == message.chatId) {
      final detailState = state as ChatDetailState;
      final chat = _chats.firstWhere(
        (item) => item.id == message.chatId,
        orElse: () => detailState.chat,
      );
      emit(
        ChatDetailState(chat: chat, messages: List.unmodifiable(chatMessages)),
      );
      return;
    }

    if (state is ChatListState) {
      emit(ChatListState(List.unmodifiable(_chats)));
    }
  }

  List<MessageDTO> _addMessageIfMissing(MessageDTO message) {
    final chatMessages = _messages.putIfAbsent(message.chatId, () => []);
    final alreadyExists = chatMessages.any((item) => item.id == message.id);

    if (!alreadyExists) {
      chatMessages.add(message);
      chatMessages.sort(
        (a, b) => a.createdTimestamp.compareTo(b.createdTimestamp),
      );
    }

    return chatMessages;
  }

  List<MessageDTO> _getNewestUnreadMessages(
    List<MessageDTO> messages,
    int unreadCount,
  ) {
    if (unreadCount <= 0) {
      return [];
    }

    final unreadCandidates =
        messages.where((message) => message.senderId != currentUserId).toList()
          ..sort((a, b) => b.createdTimestamp.compareTo(a.createdTimestamp));

    return unreadCandidates.take(unreadCount).toList();
  }

  void _incrementUnreadCount(String chatId) {
    final chatIndex = _chats.indexWhere((chat) => chat.id == chatId);
    if (chatIndex == -1) {
      return;
    }

    final chat = _chats[chatIndex];
    _chats[chatIndex] = _copyChatWith(chat, unreadCount: chat.unreadCount + 1);
  }

  void _setUnreadCount(String chatId, int unreadCount) {
    final chatIndex = _chats.indexWhere((chat) => chat.id == chatId);
    if (chatIndex == -1) {
      return;
    }

    final chat = _chats[chatIndex];
    _chats[chatIndex] = _copyChatWith(chat, unreadCount: unreadCount);
  }

  void _updateChatPreview(String chatId, MessageDTO latestMessage) {
    final chatIndex = _chats.indexWhere((chat) => chat.id == chatId);
    if (chatIndex == -1) {
      return;
    }

    final chat = _chats[chatIndex];
    _chats[chatIndex] = _copyChatWith(
      chat,
      lastMessage: latestMessage.content,
      lastUpdated: latestMessage.createdTimestamp,
    );
  }

  ChatDto _copyChatWith(
    ChatDto chat, {
    String? lastMessage,
    DateTime? lastUpdated,
    int? unreadCount,
  }) {
    return ChatDto(
      id: chat.id,
      title: chat.title,
      lastMessage: lastMessage ?? chat.lastMessage,
      lastUpdated: lastUpdated ?? chat.lastUpdated,
      unreadCount: unreadCount ?? chat.unreadCount,
    );
  }

  @override
  Future<void> close() async {
    if (_activeChatId != null) {
      await _chatService.leaveChat(_activeChatId!);
      _activeChatId = null;
    }

    await _messageSubscription?.cancel();
    _messageSubscription = null;

    return super.close();
  }
}
