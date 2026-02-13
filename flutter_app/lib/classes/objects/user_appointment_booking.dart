import 'appointment.dart';
import 'common.dart';
import 'user.dart';

class UserAppointmentBooking extends Common {
  UserAppointmentBooking({
    required this.id,
    required this.UserId,
    this.user,
    required this.AppointmentId,
    this.appointment,
    this.createdAt,
    this.updatedAt,
  });

  final String UserId;
  final User? user;

  final String AppointmentId;
  final Appointment? appointment;

  @override
  final String id;

  @override
  final DateTime? createdAt;

  @override
  final DateTime? updatedAt;

      factory UserAppointmentBooking.fromJson(Map<String, dynamic> json) {

    return UserAppointmentBooking(
      id: json['id'] as String,
      UserId: json['userId'] as String,
      user: json['user'] == null ? null :  User.fromJson(json['user']),
      AppointmentId: json['appointmentId'] as String,
      appointment: json['appointment'] == null ? null : Appointment.fromJson(json['appointment']),
      createdAt: DateTime.tryParse(json['createdAt']),
      updatedAt: DateTime.tryParse(json['updatedAt']),
    );
  }
}
