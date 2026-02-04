
class UserDTO {
  final String id;
  final String name;
  final String email;
  final String? base64Pfp;

  UserDTO({
    required this.id,
    required this.name,
    required this.email,
    required this.base64Pfp,
  });

  factory UserDTO.fromJson(Map<String, dynamic> json) {
    return UserDTO(
      id: json['id'] as String,
      name: json['name'] as String,
      email: json['email'] as String,
      base64Pfp: json['base64Pfp'] as String,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'email': email,
      'base64Pfp': base64Pfp,
    };
  }
}