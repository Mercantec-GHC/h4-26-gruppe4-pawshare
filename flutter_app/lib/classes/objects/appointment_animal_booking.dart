import 'animal.dart';
import 'appointment.dart';
import 'common.dart';

class AppointmentAnimalBooking extends Common {
  AppointmentAnimalBooking({
    required this.id,
    required this.AppointmentId,
    this.appointment,
    required this.AnimalId,
    this.animal,
    this.createdAt,
    this.updatedAt,
  });
  final String AppointmentId;
  final Appointment? appointment;

  final String AnimalId;
  final Animal? animal;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

  factory AppointmentAnimalBooking.fromJson(Map<String, dynamic> json) {
    return AppointmentAnimalBooking(
      id: json['id'] as String,
      AppointmentId: json['appointmentId'] as String,
      appointment: json['appointment'] == null
          ? null
          : Appointment.fromJson(json['appointment']),
      AnimalId: json['animalId'] as String,
      animal: json['animal'] == null ? null : Animal.fromJson(json['animal']),
      createdAt: DateTime.tryParse(json['createdAt']),
      updatedAt: DateTime.tryParse(json['updatedAt']),
    );
  }
}
