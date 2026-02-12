// Name all paths
// TODO: ADD ALL ENDPOINTS
enum ApiPath { animal, user, chat, auth }

// Specify the string needed for each path. Avoids accidental misspellings and ensures consistency
extension PathExtension on ApiPath {
  String get value {
    String name;
    switch (this) {
      case ApiPath.animal:
        name = 'Animal';
      case ApiPath.user:
        name = 'Users';
      case ApiPath.chat:
        name = 'Chat';
      case ApiPath.auth:
        name = 'Auth';
    }
    return name;
  }
}
