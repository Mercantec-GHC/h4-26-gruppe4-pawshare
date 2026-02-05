import 'common.dart';

class User extends Common {
  User({
    required this.id,
    required this.Name,
    required this.Email,
    required this.createdAt,
    required this.updatedAt,
  });

  final String Name;
  final String Email;

  @override
  // TODO: implement Id
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      id: json['Id'],
      Name: json['Name'],
      Email: json['Email'],
      createdAt: DateTime.tryParse(json['CreatedAt']),
      updatedAt: DateTime.tryParse(json['UpdatedAt']),
    );
  }
}