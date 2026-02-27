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
      String token = decoded['accessToken'];
      String refreshToken = decoded['refreshToken'];
      await SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, token);
      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.refreshToken,
        refreshToken,
      );

      API.setAuthHeader(token);
      var meResponse = await API.getRequestWithId(ApiPath.auth, 'me');

      if (meResponse.statusCode == 200) {
        var meDecoded = json.decode(meResponse.body);
        String userId = meDecoded['userId'];
        await SecureStorageHelper.saveToStorage(
          SecureStorageKey.userId,
          userId,
        );
      }

      return true;
    }
    if (resp.statusCode == 401) return false;
    throw Exception('Login failed: ${resp.statusCode}');
  }

  static Future<void> logout() async {
    try {
      await API.postRequestWithId(ApiPath.auth, 'logout', null);
    } catch (_) {}

    try {
      await WebSocketAPI().disconnect();
    } catch (_) {}

    await SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, '');
    await SecureStorageHelper.saveToStorage(SecureStorageKey.refreshToken, '');
    await SecureStorageHelper.saveToStorage(SecureStorageKey.userId, '');

    API.clearAuthHeader();
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
    }, skipRefresh: true);

    if (resp.statusCode == 200) {
      var decoded = json.decode(resp.body);
      String newToken = decoded['accessToken'];
      String refreshToken = decoded['refreshToken'];
      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.refreshToken,
        refreshToken,
      );
      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.jwtToken,
        newToken,
      );
      API.setAuthHeader(newToken);
      return true;
    }
    await forceLogout();
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

  static Future<bool> registerOwner(Map<String, dynamic> body) async {
    var resp = await API.postRequestWithId(
      ApiPath.auth,
      'register-owner',
      body,
    );

    return resp.statusCode == 200 || resp.statusCode == 201;
  }

  static Future<bool> registerInstitution(Map<String, dynamic> body) async {
    var resp = await API.postRequestWithId(
      ApiPath.auth,
      'register-institution',
      body,
    );

    return resp.statusCode == 200 || resp.statusCode == 201;
  }

  static Future<void> forceLogout() async {
    try {
      await WebSocketAPI().disconnect();
    } catch (_) {}

    await SecureStorageHelper.saveToStorage(SecureStorageKey.jwtToken, '');
    await SecureStorageHelper.saveToStorage(SecureStorageKey.refreshToken, '');
    await SecureStorageHelper.saveToStorage(SecureStorageKey.userId, '');
  }

  static Future<bool> forgotPassword(String email) async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'forgot-password', {
      'Email': email,
    }, skipRefresh: true);

    return resp.statusCode == 200;
  }

  static Future<bool> resetPassword(String token, String newPassword) async {
    var resp = await API.postRequestWithId(ApiPath.auth, 'reset-password', {
      'Token': token,
      'NewPassword': newPassword,
    }, skipRefresh: true);

    return resp.statusCode == 200;
  }
}
