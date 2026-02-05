import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../objects/api_path.dart';

class API {
  static const String _url = 'https://api.com/';
  static const String _testUrl = 'https://dev-pawshare-api.mercantec.tech/api/';

  // Get Request
  static Future<http.Response> getRequest(ApiPath action) async {
    // Create header with action
    final header = {
      'Accept': 'application/json',
    };

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
  static Future<http.Response> getRequestWithId(ApiPath action, String id) async {
    // Create header with action
    final header = {
      'Accept': 'application/json',
    };

    // Get Request from url with header and "/(id)"
    var temp = await http.get(
      Uri.parse('${kReleaseMode ? _url : _testUrl}${action.value}/$id'),
      headers: header,
    );

    // Returns future response
    return temp;
  }
}