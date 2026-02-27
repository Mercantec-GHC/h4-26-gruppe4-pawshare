import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/helpers/auth.dart';

abstract class ResetPasswordEvent {}

class ResetPasswordSubmitted extends ResetPasswordEvent {
  final String token;
  final String newPassword;

  ResetPasswordSubmitted(this.token, this.newPassword);
}

abstract class ResetPasswordState {}

class ResetPasswordInitial extends ResetPasswordState {}

class ResetPasswordLoading extends ResetPasswordState {}

class ResetPasswordSuccess extends ResetPasswordState {}

class ResetPasswordFailure extends ResetPasswordState {
  final String message;
  ResetPasswordFailure(this.message);
}

class ResetPasswordBloc
    extends Bloc<ResetPasswordEvent, ResetPasswordState> {
  ResetPasswordBloc() : super(ResetPasswordInitial()) {
    on<ResetPasswordSubmitted>(_onSubmitted);
  }

  Future<void> _onSubmitted(
    ResetPasswordSubmitted event,
    Emitter<ResetPasswordState> emit,
  ) async {
    emit(ResetPasswordLoading());

    try {
      final success = await Auth.resetPassword(
        event.token,
        event.newPassword,
      );

      if (success) {
        emit(ResetPasswordSuccess());
      } else {
        emit(ResetPasswordFailure("Invalid or expired token"));
      }
    } catch (_) {
      emit(ResetPasswordFailure("Something went wrong"));
    }
  }
}