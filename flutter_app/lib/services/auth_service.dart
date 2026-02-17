import 'dart:convert';
import 'package:http/http.dart' as http;
import '../classes/helpers/secure_storage_helper.dart';
import '../classes/objects/secure_storage_key.dart';
import '../config/api_config.dart';

class AuthService {
  final String baseUrl =
      'https://localhost:7258'; 

  Future<void> login({
    required String email,
    required String password,
  }) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/auth/login'),
      headers: {
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'email': email,
        'password': password,
      }),
    );

    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);

      final accessToken = data['accessToken'];
      final refreshToken = data['refreshToken'];

      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.jwtToken,
        accessToken,
      );
      
      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.refreshToken,
        refreshToken,
      );

      await SecureStorageHelper.saveToStorage(
        SecureStorageKey.userId, 
        data['userId'].toString(),
      );

    } else {
      throw AuthException('Invalid email or password');
    }
  }

Future<void> register({
  required String email,
  required String password,
}) async {
  final response = await http.post(
    Uri.parse('$baseUrl/api/auth/register'),
    headers: {
      'Content-Type': 'application/json',
    },
    body: jsonEncode({
      'email': email,
      'name': email,       // 
      'password': password,
      'base64Pfp': ''     // 
    }),
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
  final response = await http.post(
    Uri.parse('${ApiConfig.baseUrl}/api/auth/register-owner'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({
      'email': email,
      'name': name,
      'password': password,
      'city': city,
      'base64Pfp': base64Pfp,
      'animalName': animalName,
      'animalDescription': animalDescription,
      'animalAge': animalAge,
      'animalTypeId': animalTypeId,
    }),
  );

  if (response.statusCode != 200) {
    throw Exception('Register failed');
  }
}

  Future<bool> isLoggedIn() async {
    return await SecureStorageHelper.readFromStorage(SecureStorageKey.jwtToken) != null;
  }

  Future<void> logout() async {
    await SecureStorageHelper.clearSecureStorage();
  }
}

class AuthException implements Exception {
  final String message;
  AuthException(this.message);
}