import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../objects/api_path.dart';
import 'auth.dart';

class API {
  static const String _url =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://localhost:7258')}/api/';
  static const String _testUrl =
      '${String.fromEnvironment('API_URL_HTTPS', defaultValue: 'https://localhost:7258')}/api/';

  static final Map<String, String> _headers = {};

  static Map<String, String> _jsonHeaders() {
    return {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      ..._headers,
    };
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

    if (onSuccess != null) {
      onSuccess(response);
    }
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
      // TODO: Handle failed token refresh, e.g., by redirecting to login
    }
    return false;
  }

  // Get Request
  static Future<http.Response> getRequest(ApiPath action) async {
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Get Request from url with header. To post change the get to post and add the body (the same way as you do the header).
    var temp = await _attemptApiWithRefresh(
      () => http.get(
        // Checks if it is release mode or debug mode. Uses Test URL on debug mode
        Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
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
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Get Request from url with header and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.get(
        Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
        headers: {'Accept': 'application/json', ..._headers},
      ),
      null,
    );

    return temp;
  }

  // Post Request
  static Future<http.Response> postRequest(
    ApiPath action, 
    Object? body,
    {bool? isRefresh}
  ) async {
    if (isRefresh ?? false) {
      _headers.remove('Authorization');
    } else {
      final token = await Auth.getAccessToken();
      if (token.isNotEmpty) {
        _headers['Authorization'] = 'Bearer $token';
      }
    }

    // Post Request from url with header and body
    return await _attemptApiWithRefresh(
      () => http.post(
        Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );
  }

  // Post Request
  static Future<http.Response> postRequestWithId(
    ApiPath action,
    String id,
    Object? body,
  ) async {
    final token = await Auth.getAccessToken();
    if (token.isNotEmpty) {
      _headers['Authorization'] = 'Bearer $token';
    }

    // Post Request from url with header, body, and "/(id)"
    return await _attemptApiWithRefresh(
      () => http.post(
        Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );
  }

  // Put Request
  static Future<http.Response> putRequest(ApiPath action, Object? body) async {
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Put Request from url with header and body
    var temp = await _attemptApiWithRefresh(
      () => http.put(
        Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
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
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Put Request from url with header, body, and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.put(
        Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
        headers: _jsonHeaders(),
        body: body == null ? null : jsonEncode(body),
      ),
      null,
    );

    return temp;
  }

  // Delete Request
  static Future<http.Response> deleteRequest(ApiPath action) async {
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Delete Request from url with header
    var temp = await _attemptApiWithRefresh(
      () => http.delete(
        Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
        headers: _headers,
      ),
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
    if (await Auth.getAccessToken() != null &&
        await Auth.getAccessToken() != '') {
      _headers['Authorization'] = 'Bearer ${await Auth.getAccessToken()}';
    }

    // Delete Request from url with header and "/(id)"
    var temp = await _attemptApiWithRefresh(
      () => http.delete(
        Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
        headers: _headers,
      ),
      null,
    );

    // Returns future response
    return temp;
  }
}
