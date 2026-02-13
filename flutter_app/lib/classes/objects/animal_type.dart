import 'common.dart';

class AnimalType extends Common {
  AnimalType({
    required this.id,
    required this.name,
    required this.description,
    this.createdAt,
    this.updatedAt,
  });

  final String name;
  final String description;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory AnimalType.fromJson(Map<String, dynamic> json) {
    return AnimalType(
      id: json['id'],
      name: json['name'],
      description: json['description'],
      createdAt: DateTime.tryParse(json['createdAt'] ?? ''),
      updatedAt: DateTime.tryParse(json['updatedAt'] ?? ''),
    );
  }
}