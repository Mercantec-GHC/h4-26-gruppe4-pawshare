import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

import '../objects/secure_storage_key.dart';

class SecureStorageHelper {
  static const _secureStorage = FlutterSecureStorage();

  // Saves a value to secure storage under the given key
  static Future<void> saveToStorage(SecureStorageKey key, String value) async {
    try {
      await _secureStorage.write(key: key.value, value: value);
    } catch (e) {
      debugPrint('Error saving to secure storage: $e');
    }
  }

  // Reads a value from secure storage by key, returns null if not found or on error
  static Future<String?> readFromStorage(SecureStorageKey key) async {
    try {
      return await _secureStorage.read(key: key.value);
    } catch (e) {
      debugPrint('Error reading from secure storage: $e');
      return null;
    }
  }

  // Deletes a single value from secure storage by key
  static Future<void> deleteFromStorage(SecureStorageKey key) async {
    try {
      await _secureStorage.delete(key: key.value);
    } catch (e) {
      debugPrint('Error deleting from secure storage: $e');
    }
  }

  // Clears all values from secure storage
  static Future<void> clearSecureStorage() async {
    try {
      await _secureStorage.deleteAll();
    } catch (e) {
      debugPrint('Error clearing secure storage: $e');
    }
  }
}