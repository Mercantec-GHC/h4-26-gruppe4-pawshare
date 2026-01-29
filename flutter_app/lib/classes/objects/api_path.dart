
// Name all paths
// TODO: ADD ALL ENDPOINTS
enum ApiPath {
  animal,
}

// Specify the string needed for each path. Avoids accidental misspellings and ensures consistency
extension PathExtension on ApiPath {
  String get value {
    String name;
    switch (this) {
      case ApiPath.animal:
        name = 'Animal';
    }
    return name;
  }
}


class Animal extends Common {
  Animal({
    required this.Id,
    required this.Name,
    required this.Description,
    required this.Age,
    required this.TypeId,
    required this.animalType,
    required this.UserId,
    required this.user,
    required this.CreatedAt,
    required this.UpdatedAt,
    required this.Base64Image,
  });

  final String Name;

  final String Description;

  final String Base64Image;

  final int Age;

  final String TypeId;

  final AnimalType? animalType;

  final String UserId;

  final User? user;

  @override
  final String Id;

  @override
  final DateTime? CreatedAt;

  @override
  final DateTime? UpdatedAt;

  factory Animal.fromJson(Map<String, dynamic> json) {
    return Animal(
      Id: json['id'],
      Name: json['Name'],
      Description: json['Description'],
      Base64Image: json['Base64Image'],
      Age: json['Age'],
      TypeId: json['TypeId'],
      animalType: AnimalType.fromJson(json['AnimalType']),
      UserId: json['UserId'],
      user: User.fromJson(json['User']),
      CreatedAt: DateTime.tryParse(json['CreatedAt']),
      UpdatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}

class AnimalType extends Common {
  AnimalType({
    required this.Id,
    required this.Name,
    required this.Description,
    this.CreatedAt,
    this.UpdatedAt,
  });
  final String Name;
  final String Description;

  @override
  final String Id;

  @override
  final DateTime? CreatedAt;

  @override
  final DateTime? UpdatedAt;

  factory AnimalType.fromJson(Map<String, dynamic> json) {
    return AnimalType(
      Id: json['Id'],
      Name: json['Name'],
      Description: json['Description'],
      CreatedAt: DateTime.tryParse(json['CreatedAt']),
      UpdatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}

abstract class Common {
  String get Id;

  DateTime? get CreatedAt;

  DateTime? get UpdatedAt;
}

class User extends Common {
  User({
    required this.Id,
    required this.Name,
    required this.Email,
    required this.CreatedAt,
    required this.UpdatedAt,
  });

  final String Name;
  final String Email;

  @override
  // TODO: implement Id
  final String Id;

  @override
  final DateTime? CreatedAt;

  @override
  final DateTime? UpdatedAt;

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      Id: json['Id'],
      Name: json['Name'],
      Email: json['Email'],
      CreatedAt: DateTime.tryParse(json['CreatedAt']),
      UpdatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}
