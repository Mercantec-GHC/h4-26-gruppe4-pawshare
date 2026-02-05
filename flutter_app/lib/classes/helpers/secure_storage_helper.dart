
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

import '../objects/secure_storage_key.dart';

class SecureStorageHelper {
  static final _secureStorage = const FlutterSecureStorage();

  // Saves to secure storage
  static Future<void> saveToStorage(SecureStorageKey key, String value) async {
    try {
      await _secureStorage.write(key: key.value, value: value);
    } catch (e) {
      debugPrint('Error saving to secure storage: $e');
    }
  }

  // Reads from secure storage
  static Future<String?> readFromStorage(SecureStorageKey key) async {
    try {
      return await _secureStorage.read(key: key.value);
    } catch (e) {
      debugPrint('Error saving to secure storage: $e');
      return null;
    }
  }

  // Clears all of the secure storage
  static Future<void> clearSecureStorage() async {
    try {
      SecureStorageKey.values.forEach((key) async {
        await _secureStorage.delete(key: key.value);
      });
    } catch (e) {
      debugPrint('Error clearing secure storage: $e');
    }
  }

}