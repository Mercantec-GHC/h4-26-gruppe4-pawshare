

import 'appointment_animal_booking.dart';
import 'common.dart';
import 'user_appointment_booking.dart';

class Appointment extends Common {
  Appointment({
    required this.id,
    required this.Start,
    required this.End,
    required this.Address,
    required this.Description,
    this.Users,
    required this.Animals,
    this.createdAt,
    this.updatedAt
  });
  final DateTime Start;
  final DateTime End;
  final String Address;
  final String Description;
  final List<UserAppointmentBooking>? Users;
  final List<AppointmentAnimalBooking> Animals;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

    factory Appointment.fromJson(Map<String, dynamic> json) {

    return Appointment(
      id: json['id'] as String,
      Start: json['start'] as DateTime,
      End: json['end'] as DateTime,
      Address: json['address'] as String,
      Description: json['description'] as String,
      Users: json['users'] == null ? null : (json['users'] as List).map((e) => UserAppointmentBooking.fromJson(e)).toList(),
      Animals: (json['animals'] as List).map((e) => AppointmentAnimalBooking.fromJson(e)).toList(),
      createdAt: DateTime.tryParse(json['createdAt']),
      updatedAt: DateTime.tryParse(json['updatedAt']),
    );
  }
   
}
