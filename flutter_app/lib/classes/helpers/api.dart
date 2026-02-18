import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:signalr_netcore/signalr_client.dart';
import '../objects/api_path.dart';
import 'auth.dart';

class API {
  static const String _url =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://pawshare-api.mercantec.tech')}/api/';
  static const String _testUrl =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://dev-pawshare-api.mercantec.tech')}/api/';

  static final Map<String, String> _headers = {};

  static String get _baseUrl => kReleaseMode ? _url : _testUrl;

  static Map<String, String> _jsonHeaders() {
    return {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      ..._headers,
    };
  }

  static Future<void> _applyAuthHeader() async {
    final accessToken = await Auth.getAccessToken();
    if (accessToken.isNotEmpty) {
      _headers['Authorization'] = 'Bearer $accessToken';
    }
  }

  static Uri _buildUri(ApiPath action, [String? id]) {
    final suffix = id == null ? action.value : '${action.value}/$id';
    return Uri.parse('$_baseUrl$suffix');
  }

  // Wraps API calls to automatically attempt token refresh on 401 responses
  static Future<http.Response> _attemptApiWithRefresh(
    Future<http.Response> Function() apiCall,
    Function(http.Response)? onSuccess,
  ) async {
    var response = await apiCall();

    if (response.statusCode == 401) {
      var refreshSuccess = await _tryToRefreshToken();

      if (refreshSuccess) {
        response = await apiCall();
      }
    }

    onSuccess?.call(response);
    return response;
  }

  // Attempts to refresh the JWT token using the refresh token. If successful, updates the Authorization header with the new token.
  // Returns: True if token refresh was successful, false otherwise
  static Future<bool> _tryToRefreshToken() async {
    var refreshSuccess = await Auth.refresh();

    if (refreshSuccess) {
      var newToken = await Auth.getAccessToken();
      _headers['Authorization'] = 'Bearer $newToken';
      return true;
    } else {
      await Auth.logout();
      _headers.remove('Authorization');
      return false;
    }
  }

  // Get Request
  static Future<http.Response> getRequest(ApiPath action) async {
    await _applyAuthHeader();

    // Get Request from url with header. To post change the get to post and add the body (the same way as you do the header).
    var temp = await _attemptApiWithRefresh(
      () => http.get(
        _buildUri(action),
        headers: {'Accept': 'application/json', ..._headers},
      ),
      null,
    );

    // Returns future response
    return temp;
  }

  // Get Request
  static Future<http.Response> getRequestWithId(
    ApiPath action,
    String id,
  ) async {
    await _applyAuthHeader();

    // Get Request from url with header and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.get(
        _buildUri(action, id),
        headers: {'Accept': 'application/json', ..._headers},
      ),
      null,
    );

    return temp;
  }

  // Post Request
  static Future<http.Response> postRequest(ApiPath action, Object? body) async {
    await _applyAuthHeader();

    // Post Request from url with header and body
    var temp = await _attemptApiWithRefresh(
      () => http.post(
        _buildUri(action),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );

    return temp;
  }

  // Post Request
  static Future<http.Response> postRequestWithId(
    ApiPath action,
    String id,
    Object? body,
  ) async {
    await _applyAuthHeader();

    // Post Request from url with header, body, and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.post(
        _buildUri(action, id),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );

    return temp;
  }

  // Put Request
  static Future<http.Response> putRequest(ApiPath action, Object? body) async {
    await _applyAuthHeader();

    // Put Request from url with header and body
    var temp = await _attemptApiWithRefresh(
      () => http.put(
        _buildUri(action),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );

    return temp;
  }

  // Put Request
  static Future<http.Response> putRequestWithId(
    ApiPath action,
    String id,
    Object? body,
  ) async {
    await _applyAuthHeader();

    // Put Request from url with header, body, and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.put(
        _buildUri(action, id),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );

    return temp;
  }

  // Delete Request
  static Future<http.Response> deleteRequest(ApiPath action) async {
    await _applyAuthHeader();

    // Delete Request from url with header
    var temp = await _attemptApiWithRefresh(
      () => http.delete(_buildUri(action), headers: _headers),
      null,
    );

    // Returns future response
    return temp;
  }

  // Delete Request
  static Future<http.Response> deleteRequestWithId(
    ApiPath action,
    String id,
  ) async {
    await _applyAuthHeader();

    // Delete Request from url with header and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.delete(_buildUri(action, id), headers: _headers),
      null,
    );

    // Returns future response
    return temp;
  }
}

class WebSocketAPI {
  static const String _hubUrl =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://pawshare-api.mercantec.tech')}/ws/chat';
  static const String _hubTestUrl =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://dev-pawshare-api.mercantec.tech')}/ws/chat';

  static String getHubUrl() => kReleaseMode ? _hubUrl : _hubTestUrl;

  static final Map<String, Function(String?)> _callbacks = {};

  static HubConnection? _connection;

  static Future<void> _initializeClient() async {
    _connection = HubConnectionBuilder()
        .withUrl(
          getHubUrl(),
          options: HttpConnectionOptions(
            accessTokenFactory: () async {
              final token = await Auth.getAccessToken();
              return token;
            },
            transport: HttpTransportType.WebSockets,
          ),
        )
        .withAutomaticReconnect()
        .build();

    for (final entry in _callbacks.entries) {
      _bindHandler(entry.key, entry.value);
    }
  }

  static void _bindHandler(String methodName, Function(String?) callback) {
    _connection!.off(methodName);
    _connection!.on(methodName, (arguments) {
      callback(_serializeSignalRPayload(arguments));
    });
  }

  static String? _serializeSignalRPayload(List<Object?>? arguments) {
    if (arguments == null || arguments.isEmpty) {
      return null;
    }

    final value = arguments.first;
    if (value == null) {
      return null;
    }

    if (value is String) {
      return value;
    }

    try {
      return jsonEncode(value);
    } catch (_) {
      return value.toString();
    }
  }

  Future<void> connect() async {
    if (_connection == null) {
      await _initializeClient();
    }

    if (_connection!.state != HubConnectionState.Connected &&
        _connection!.state != HubConnectionState.Connecting &&
        _connection!.state != HubConnectionState.Reconnecting) {
      await _connection!.start();
    }
  }

  Future<void> disconnect() async {
    if (_connection != null) {
      await _connection!.stop();
    }
  }

  Future<bool> isConnected() async {
    if (_connection == null) {
      return false;
    }

    return _connection!.state == HubConnectionState.Connected;
  }

  Future<dynamic> invoke(String methodName, {List<Object>? arguments}) async {
    await connect();
    final result = await _connection!.invoke(
      methodName,
      args: arguments ?? const [],
    );

    return result;
  }

  void registerHandler(String methodName, Function(String?) callback) {
    _callbacks[methodName] = callback;

    if (_connection != null) {
      _bindHandler(methodName, callback);
    }
  }

  void unregisterHandler(String methodName) {
    _callbacks.remove(methodName);

    if (_connection != null) {
      _connection!.off(methodName);
    }
  }
}
