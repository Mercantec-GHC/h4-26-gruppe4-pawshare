import 'dart:convert' as JSON;

import 'animal_type.dart';
import 'appointment_animal_booking.dart';
import 'common.dart';
import 'user.dart';

class Animal extends Common {
  Animal({
    required this.id,
    required this.Name,
    required this.Description,
    required this.dateOfBirth,
    required this.TypeId,
    required this.animalType,
    required this.TypeDescription,
    required this.TypeName,
    required this.UserId,
    required this.user,
    required this.UserName,
    required this.createdAt,
    required this.updatedAt,
    required this.animalPictureKey,
    required this.bookings,
  });

  final String Name;

  final String Description;

  final String animalPictureKey;

  final DateTime dateOfBirth;

  final String TypeId;

  final String? TypeName;

  final String? TypeDescription;

  final AnimalType? animalType;

  final String UserId;

  final String UserName;

  final User? user;

  final List<AppointmentAnimalBooking>? bookings;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  List<Animal> listOfAnimals(String body) =>
      List<Animal>.from(JSON.json.decode(body).map((x) => Animal.fromJson(x)));

  factory Animal.fromJson(Map<String, dynamic> json) {
    return Animal(
      id: json['id'] as String,
      Name: (json['name'] as String).replaceAll('"', ''),
      Description: (json['description'] as String).replaceAll('"', ''),
      animalPictureKey: (json['animalPictureKey'] as String? ?? '').replaceAll('"', ''),
      dateOfBirth: DateTime.tryParse(json['dateOfBirth']) ?? DateTime.now(),
      TypeId: (json['typeId'] as String).replaceAll('"', ''),
      TypeName: json['typeName'] == null ? null : (json['typeName'] as String).replaceAll('"', ''),
      TypeDescription: json['typeDescription'] == null ? null : (json['typeDescription'] as String).replaceAll('"', ''),
      animalType: json['animalType'] == null
          ? null
          : AnimalType.fromJson(json['animalType']),
      UserId: (json['userId'] as String).replaceAll('"', ''),
      UserName: (json['userName'] as String).replaceAll('"', ''),
      user: json['user'] == null ? null : User.fromJson(json['user']),
      createdAt: DateTime.tryParse(json['createdAt']),
      updatedAt: DateTime.tryParse(json['updatedAt']),
      bookings: json['bookings'] == null
          ? []
          : List<AppointmentAnimalBooking>.from(
              json['bookings'].map((x) => AppointmentAnimalBooking.fromJson(x)),
            ),
    );
  }
}
