import 'dart:async';
import 'dart:convert';

import '../helpers/api.dart';
import '../objects/message_dto.dart';

class ChatService {
  ChatService._();

  static final ChatService instance = ChatService._();

  static const String _initialChatsEvent = 'InitialChats';
  static const String _receiveMessageEvent = 'ReceiveMessage';

  final WebSocketAPI _webSocketAPI = WebSocketAPI();
  final StreamController<MessageDTO> _messageController =
      StreamController<MessageDTO>.broadcast();

  bool _handlersRegistered = false;
  Completer<List<ChatDto>>? _initialChatsCompleter;
  List<ChatDto> _initialChatsCache = [];

  Stream<MessageDTO> get messageStream => _messageController.stream;

  Future<void> connect() async {
    _registerHandlers();

    final alreadyConnected = await _webSocketAPI.isConnected();
    if (!alreadyConnected) {
      _initialChatsCompleter = Completer<List<ChatDto>>();
      _initialChatsCache = [];
      await _webSocketAPI.connect();
    }
  }

  Future<void> disconnect() async {
    await _webSocketAPI.disconnect();
    _initialChatsCache = [];
    _initialChatsCompleter = null;
  }

  Future<List<ChatDto>> getInitialChats() async {
    await connect();

    if (_initialChatsCache.isNotEmpty) {
      return List.unmodifiable(_initialChatsCache);
    }

    _initialChatsCompleter ??= Completer<List<ChatDto>>();

    try {
      return await _initialChatsCompleter!.future.timeout(
        const Duration(seconds: 5),
        onTimeout: () => List.unmodifiable(_initialChatsCache),
      );
    } catch (_) {
      return List.unmodifiable(_initialChatsCache);
    }
  }

  Future<List<MessageDTO>> getMessages(String chatId) async {
    final result = await _invoke('GetMessages', arguments: [chatId]);
    final decoded = _decodeHubResult(result);

    if (decoded is! List) {
      return [];
    }

    final messages = decoded
        .map((item) => _toMap(item))
        .whereType<Map<String, dynamic>>()
        .map((messageJson) => MessageDTO.fromJson(messageJson, chatId: chatId))
        .toList();

    messages.sort((a, b) => a.createdTimestamp.compareTo(b.createdTimestamp));
    return messages;
  }

  Future<Map<String, int>> getUnreadCounts() async {
    final result = await _invoke('GetUnreadList');
    final decoded = _decodeHubResult(result);

    if (decoded is! List) {
      return {};
    }

    final unreadByChat = <String, int>{};
    for (final item in decoded) {
      final map = _toMap(item);
      if (map == null) {
        continue;
      }

      final chatId = (map['chatId'] ?? '').toString();
      if (chatId.isEmpty) {
        continue;
      }

      unreadByChat[chatId] = _toInt(map['unreadCount']);
    }

    return unreadByChat;
  }

  Future<bool> sendMessage(String chatId, String content) async {
    return (await _invoke('SendMessage', arguments: [chatId, content])) != null;
  }

  Future<void> joinChat(String chatId) async {
    await _invoke('JoinChat', arguments: [chatId]);
  }

  Future<void> leaveChat(String chatId) async {
    await _invoke('LeaveChat', arguments: [chatId]);
  }

  Future<void> markRead(String messageId) async {
    await _invoke('MarkRead', arguments: [messageId]);
  }

  Future<void> markMessagesAsRead(List<MessageDTO> messages) async {
    for (final message in messages) {
      await markRead(message.id);
    }
  }

  void _registerHandlers() {
    if (_handlersRegistered) {
      return;
    }

    _webSocketAPI.registerHandler(_initialChatsEvent, _onInitialChatsRaw);
    _webSocketAPI.registerHandler(_receiveMessageEvent, _onReceiveMessageRaw);
    _handlersRegistered = true;
  }

  void _onInitialChatsRaw(String? payload) {
    final chats = _parseInitialChats(payload);
    _initialChatsCache = chats;

    final completer = _initialChatsCompleter;
    if (completer != null && !completer.isCompleted) {
      completer.complete(List.unmodifiable(chats));
    }
  }

  void _onReceiveMessageRaw(String? payload) {
    if (payload == null || payload.isEmpty) {
      return;
    }

    final parsed = _parseMessage(payload);
    if (parsed != null) {
      _messageController.add(parsed);
    }
  }

  List<ChatDto> _parseInitialChats(String? payload) {
    if (payload == null || payload.isEmpty) {
      return [];
    }

    try {
      final decoded = json.decode(payload);
      if (decoded is! List) {
        return [];
      }

      return decoded
          .map((item) => _toMap(item))
          .whereType<Map<String, dynamic>>()
          .map(ChatDto.fromJson)
          .toList();
    } catch (_) {
      return [];
    }
  }

  MessageDTO? _parseMessage(String payload) {
    try {
      final decoded = json.decode(payload);

      if (decoded is Map<String, dynamic>) {
        if (decoded['message'] is Map) {
          final messageMap = Map<String, dynamic>.from(
            decoded['message'] as Map,
          );
          final chatId =
              (decoded['chatId'] ??
                      decoded['ChatId'] ??
                      messageMap['chatId'] ??
                      '')
                  .toString();
          if (chatId.isEmpty) {
            return null;
          }

          return MessageDTO.fromJson(messageMap, chatId: chatId);
        }

        final chatId = (decoded['chatId'] ?? decoded['ChatId'] ?? '')
            .toString();
        if (chatId.isEmpty) {
          return null;
        }

        return MessageDTO.fromJson(decoded, chatId: chatId);
      }

      if (decoded is List && decoded.isNotEmpty && decoded.first is Map) {
        final map = Map<String, dynamic>.from(decoded.first as Map);
        final chatId = (map['chatId'] ?? '').toString();
        if (chatId.isEmpty) {
          return null;
        }

        return MessageDTO.fromJson(map, chatId: chatId);
      }
    } catch (_) {
      return null;
    }

    return null;
  }

  Future<dynamic> _invoke(String methodName, {List<Object>? arguments}) async {
    try {
      return await _webSocketAPI.invoke(methodName, arguments: arguments);
    } catch (_) {
      return null;
    }
  }

  dynamic _decodeHubResult(dynamic result) {
    if (result is String) {
      if (result.isEmpty) {
        return null;
      }

      try {
        return json.decode(result);
      } catch (_) {
        return null;
      }
    }

    return result;
  }

  Map<String, dynamic>? _toMap(dynamic value) {
    if (value is Map<String, dynamic>) {
      return value;
    }

    if (value is Map) {
      return Map<String, dynamic>.from(value);
    }

    return null;
  }

  int _toInt(dynamic value) {
    if (value is int) {
      return value;
    }

    if (value is num) {
      return value.toInt();
    }

    return int.tryParse(value?.toString() ?? '') ?? 0;
  }
}
