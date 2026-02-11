
import 'animal_type.dart';
import 'appointment_animal_booking.dart';
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
    required this.booking
  });

  final String Name;

  final String Description;

  final String Base64Image;

  final int Age;

  final String TypeId;

  final AnimalType? animalType;

  final String UserId;

  final User? user;

  final AppointmentAnimalBooking? booking;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory Animal.fromJson(Map<String, dynamic> json) {
    return Animal(
      id: json['id'] as String,
      Name: (json['name'] as String).replaceAll('"', ''),
      Description: (json['description'] as String).replaceAll('"', ''),
      Base64Image: (json['base64Image'] as String).replaceAll('"', ''),
      Age: json['age'] as int,
      TypeId: (json['typeId'] as String).replaceAll('"', ''),
      animalType: json['animalType'] == null ? null : AnimalType.fromJson(json['animalType']),
      UserId: (json['userId'] as String).replaceAll('"', ''),
      user: json['user'] == null ? null :  User.fromJson(json['user']),
      createdAt: DateTime.tryParse(json['createdAt']),
      updatedAt: DateTime.tryParse(json['updatedAt']),
      booking: json['bookings'] == null ? null : AppointmentAnimalBooking.fromJson(json['bookings'])
    );
  }
}