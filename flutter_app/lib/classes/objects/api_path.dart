
// Name all paths
// TODO: ADD ALL ENDPOINTS
enum ApiPath {
  animal,
  user
}

// Specify the string needed for each path. Avoids accidental misspellings and ensures consistency
extension PathExtension on ApiPath {
  String get value {
    String name;
    switch (this) {
      case ApiPath.animal:
        name = 'Animal';
      case ApiPath.user:
        name = 'Users';
    }
    return name;
  }
}
