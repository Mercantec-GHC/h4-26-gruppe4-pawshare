import 'dart:convert';
import '../classes/helpers/api.dart';
import '../classes/objects/api_path.dart';
import 'auth_storage.dart';

class AuthService {
  final AuthStorage _storage = AuthStorage();

  Future<void> login({
    required String email,
    required String password,
  }) async {
    final response = await API.postRequest(
      ApiPath.login,
      {
        'email': email,
        'password': password,
      }
    );

    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);

      final accessToken = data['accessToken'];
      final refreshToken = data['refreshToken'];

      await _storage.saveTokens(
        accessToken: accessToken,
        refreshToken: refreshToken,
      );
    } else {
      throw AuthException('Invalid email or password');
    }
  }

Future<void> register({
  required String email,
  required String password,
}) async {
  final response = await API.postRequest(
    ApiPath.register,
    {
      'email': email,
      'name': email,
      'password': password,
      'base64Pfp': ''
    }
  );

  print('REGISTER STATUS: ${response.statusCode}');
  print('REGISTER BODY: ${response.body}');

  if (response.statusCode == 200) {
    await login(email: email, password: password);
  } else {
    throw AuthException('Registration failed');
  }
}

Future<void> registerOwner({
  required String email,
  required String name,
  required String password,
  required String city,
  required String base64Pfp,
  required String animalName,
  required String animalDescription,
  required int animalAge,
  required String animalTypeId,
}) async {
  final response = await API.postRequest(
    ApiPath.registerOwner,
    {
      'email': email,
      'name': name,
      'password': password,
      'city': city,
      'base64Pfp': base64Pfp,
      'animalName': animalName,
      'animalDescription': animalDescription,
      'animalAge': animalAge,
      'animalTypeId': animalTypeId,
    }
  );

  if (response.statusCode != 200) {
    throw Exception('Register failed');
  }
}

  Future<bool> isLoggedIn() {
    return _storage.isLoggedIn();
  }

  Future<void> logout() {
    return _storage.logout();
  }
}

class AuthException implements Exception {
  final String message;
  AuthException(this.message);
}