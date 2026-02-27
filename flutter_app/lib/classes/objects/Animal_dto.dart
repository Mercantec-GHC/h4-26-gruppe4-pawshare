import 'dart:convert' as JSON;

import 'animal_type.dart';
import 'appointment_animal_booking.dart';
import 'common.dart';
import 'user.dart';

class AnimalDTO {
  AnimalDTO({
    required this.id,
    required this.name,
    required this.description,
    required this.animalPictureKey,

  });

  final String name;

  final String description;

  final String? animalPictureKey;
  final String id;



  List<AnimalDTO> listOfAnimals(String body) =>
      List<AnimalDTO>.from(JSON.json.decode(body).map((x) => AnimalDTO.fromJson(x)));

  factory AnimalDTO.fromJson(Map<String, dynamic> json) {
    return AnimalDTO(
      id: json['id'] as String,
      name: (json['name'] as String).replaceAll('"', ''),
      description: (json['description'] as String).replaceAll('"', ''),
      animalPictureKey: (json['animalPictureKey'] as String? ?? '').replaceAll('"', ''),
    );
  }
}
