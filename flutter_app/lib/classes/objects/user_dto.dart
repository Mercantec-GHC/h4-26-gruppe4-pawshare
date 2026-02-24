
class UserDTO {
  final String id;
  final String name;
  final String email;
  String? profilePictureKey;

  UserDTO({
    required this.id,
    required this.name,
    required this.email,
    required this.profilePictureKey,
  });


  factory UserDTO.fromJson(Map<String, dynamic> json) {
    return UserDTO(
      id: json['id'] as String,
      name: json['name'] as String,
      email: json['email'] as String,
      profilePictureKey: json['profilePictureKey'] as String?,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'email': email,
      'profilePictureKey': profilePictureKey,
    };
  }
}