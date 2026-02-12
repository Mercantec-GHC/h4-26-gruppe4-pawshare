import 'dart:convert';

import '../objects/api_path.dart';
import '../objects/secure_storage_key.dart';
import 'api.dart';
import 'secure_storage_helper.dart';

class Auth {
  static Future<bool> login(String email, String password) async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'login', {
      'Email': email,
      'Password': password,
    });

    if (resp.statusCode == 200) {
      var decoded = json.decode(resp.body);
      String token = decoded['AccessToken'];
      String refreshToken = decoded['RefreshToken'];
      SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, token);
      SecureStorageHelper.saveToStorage(
        SecureStorageKey.refreshToken,
        refreshToken,
      );

      var meResponse = await API.getRequestWithId(ApiPath.auth, 'me');

      if (meResponse.statusCode == 200) {
        var meDecoded = json.decode(meResponse.body);
        String userId = meDecoded['UserId'];
        SecureStorageHelper.saveToStorage(SecureStorageKey.userId, userId);
      }

      return true;
    } else {}
    return false;
  }

  static Future<void> logout() async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'logout', null);

    if (resp.statusCode == 204) {
      SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, '');
      SecureStorageHelper.saveToStorage(SecureStorageKey.refreshToken, '');
      SecureStorageHelper.saveToStorage(SecureStorageKey.userId, '');
    }
  }

  static Future<bool> register(String email, String password) async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'register', {
      'Email': email,
      'Password': password,
    });

    if (resp.statusCode == 201) {
      return true;
    }
    return false;
  }

  static Future<bool> refresh() async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'refresh', {
      'RefreshToken': await getRefreshToken(),
    });

    if (resp.statusCode == 200) {
      var decoded = json.decode(resp.body);
      String newToken = decoded['AccessToken'];
      String refreshToken = decoded['RefreshToken'];
      SecureStorageHelper.saveToStorage(
        SecureStorageKey.refreshToken,
        refreshToken,
      );
      SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, newToken);
      return true;
    }

    return false;
  }

  static Future<String> getCurrentUserId() async {
    return await SecureStorageHelper.readFromStorage(SecureStorageKey.userId) ??
        '';
  }

  static Future<String> getAccessToken() async {
    return await SecureStorageHelper.readFromStorage(
          SecureStorageKey.jwtToken,
        ) ??
        '';
  }

  static Future<String> getRefreshToken() async {
    return await SecureStorageHelper.readFromStorage(
          SecureStorageKey.refreshToken,
        ) ??
        '';
  }
}
