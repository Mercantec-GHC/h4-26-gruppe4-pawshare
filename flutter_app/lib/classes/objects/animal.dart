
import 'animal_type.dart';
import 'common.dart';
import 'user.dart';

class Animal extends Common {
  Animal({
    required this.id,
    required this.Name,
    required this.Description,
    required this.Age,
    required this.TypeId,
    required this.animalType,
    required this.UserId,
    required this.user,
    required this.createdAt,
    required this.updatedAt,
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
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory Animal.fromJson(Map<String, dynamic> json) {
    return Animal(
      id: json['id'],
      Name: json['Name'],
      Description: json['Description'],
      Base64Image: json['Base64Image'],
      Age: json['Age'],
      TypeId: json['TypeId'],
      animalType: AnimalType.fromJson(json['AnimalType']),
      UserId: json['UserId'],
      user: User.fromJson(json['User']),
      createdAt: DateTime.tryParse(json['CreatedAt']),
      updatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}