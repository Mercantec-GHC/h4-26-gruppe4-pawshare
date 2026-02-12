import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../objects/api_path.dart';

class API {
  static const String _url = String.fromEnvironment(
    'API_URL_HTTPS',
    defaultValue: 'https://pawshare-api.mercantec.tech/',
  );
  static const String _testUrl = String.fromEnvironment(
    'API_URL_HTTPS',
    defaultValue: 'https://dev-pawshare-api.mercantec.tech/api/',
  );

  // Get Request
  static Future<http.Response> getRequest(ApiPath action) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Get Request from url with header. To post change the get to post and add the body (the same way as you do the header).
    var temp = await http.get(
      // Checks if it is release mode or debug mode. Uses Test URL on debug mode
      Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
      headers: header,
    );

    // Returns future response
    return temp;
  }

  // Get Request
  static Future<http.Response> getRequestWithId(
    ApiPath action,
    String id,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Get Request from url with header and "/(id)"
    var temp = await http.get(
      Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
      headers: header,
    );

    // Returns future response
    return temp;
  }

  // Post Request
  static Future<http.Response> postRequest(
    ApiPath action,
    Object? body,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Post Request from url with header and body
    var temp = await http.post(
      Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
      headers: header,
      body: body,
    );

    // Returns future response
    return temp;
  }

  // Post Request
  static Future<http.Response> postRequestWithId(
    ApiPath action,
    String id,
    Object? body,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Post Request from url with header, body, and "/(id)"
    var temp = await http.post(
      Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
      headers: header,
      body: body,
    );

    // Returns future response
    return temp;
  }

  // Put Request
  static Future<http.Response> putRequest(
    ApiPath action,
    Object? body,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Put Request from url with header and body
    var temp = await http.put(
      Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
      headers: header,
      body: body,
    );

    // Returns future response
    return temp;
  }

  // Put Request
  static Future<http.Response> putRequestWithId(
    ApiPath action,
    String id,
    Object? body,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Put Request from url with header, body, and "/(id)"
    var temp = await http.put(
      Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
      headers: header,
      body: body,
    );

    // Returns future response
    return temp;
  }

  // Delete Request
  static Future<http.Response> deleteRequest(
    ApiPath action,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Delete Request from url with header
    var temp = await http.delete(
      Uri.parse((kReleaseMode ? _url : _testUrl) + action.value),
      headers: header,
    );

    // Returns future response
    return temp;
  }

  // Delete Request
  static Future<http.Response> deleteRequestWithId(
    ApiPath action,
    String id,
  ) async {
    // Create header with action
    final header = {'Accept': 'application/json'};

    // Delete Request from url with header and "/(id)"
    var temp = await http.delete(
      Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
      headers: header,
    );

    // Returns future response
    return temp;
  }
}
