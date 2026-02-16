// Name all paths
// TODO: ADD ALL ENDPOINTS
enum ApiPath {
  animal,
  animalType,
  chat,
  auth,
  user,
  login,
  register,
  registerOwner
}

// Specify the string needed for each path. Avoids accidental misspellings and ensures consistency
extension PathExtension on ApiPath {
  String get value => switch (this) {
    ApiPath.animal => 'Animal',
    ApiPath.animalType => 'AnimalType',
    ApiPath.user => 'Users',
    ApiPath.chat => 'Chat',
    ApiPath.auth => 'Auth',
    ApiPath.login => 'auth/login',
    ApiPath.register => 'auth/register',
    ApiPath.registerOwner => 'auth/register-owner',
  };
}
