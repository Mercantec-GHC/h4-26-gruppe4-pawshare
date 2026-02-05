
// TODO: ADD ALL KEYS
enum SecureStorageKey {
  userId
}

extension PathExtension on SecureStorageKey {
  String get value {
    String name;
    switch (this) {
      case SecureStorageKey.userId:
        name = 'UserId';
    }
    return name;
  }
}
