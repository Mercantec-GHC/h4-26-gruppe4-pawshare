import 'common.dart';

class AnimalType extends Common {
  AnimalType({
    required this.id,
    required this.Name,
    required this.Description,
    this.createdAt,
    this.updatedAt,
  });
  final String Name;
  final String Description;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory AnimalType.fromJson(Map<String, dynamic> json) {
    return AnimalType(
      id: json['Id'],
      Name: json['Name'],
      Description: json['Description'],
      createdAt: DateTime.tryParse(json['CreatedAt']),
      updatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}