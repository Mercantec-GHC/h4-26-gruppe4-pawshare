// TODO: ADD ALL KEYS
enum SecureStorageKey { userId, jwtToken, refreshToken }

extension PathExtension on SecureStorageKey {
  String get value {
    String name;
    switch (this) {
      case SecureStorageKey.userId:
        name = 'Pawshare-UserId';
      case SecureStorageKey.jwtToken:
        name = 'Pawshare-JwtToken';
      case SecureStorageKey.refreshToken:
        name = 'Pawshare-RefreshToken';
    }
    return name;
  }
}
