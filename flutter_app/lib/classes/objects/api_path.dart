// Name all paths
enum ApiPath {
  animal,
  animalType,
  chat,
  auth,
  refresh,
  user,
  animalUser,
  changePassword,
}

// Specify the string needed for each path. Avoids accidental misspellings and ensures consistency
extension PathExtension on ApiPath {
  String get value {
    String name;
    switch (this) {
      case ApiPath.animal:
        name = 'Animal';
      case ApiPath.animalType:
        name = 'AnimalType';
      case ApiPath.user:
        name = 'Users';
      case ApiPath.chat:
        name = 'Chat';
      case ApiPath.auth:
        name = 'Auth';
        break;
      case ApiPath.refresh:
        name = 'Auth/Refresh';
        break;
      case ApiPath.animalUser:
        name = 'Animal/User';
        break;
      case ApiPath.changePassword:
        name = 'Users/change-password';
        break;
    }
    return name;
  }
}
