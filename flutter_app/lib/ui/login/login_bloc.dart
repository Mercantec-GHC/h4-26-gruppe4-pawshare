import 'package:flutter_bloc/flutter_bloc.dart';
import 'dart:async';
import '../../classes/helpers/auth.dart';
import '../../classes/services/chat_service.dart';
import 'login_events_states.dart';

class LoginBloc extends Bloc<LoginEvents, LoginState> {
  LoginBloc() : super(const LoginFormState()) {
    on<LoginSubmitted>(_onLoginSubmitted);
  }

  Future<void> _onLoginSubmitted(
    LoginSubmitted event,
    Emitter<LoginState> emit,
  ) async {
    emit(const LoginFormState(isLoading: true));

    try {
      bool success = await Auth.login(event.email, event.password);

      if (success) {
        await ChatService.instance.connect();
        emit(const LoginFormState(isSuccess: true));
      } else {
        emit(const LoginFormState(errorMessage: 'Invalid email or password'));
      }
    } catch (_) {
      emit(const LoginFormState(errorMessage: 'Something went wrong'));
    }
  }
}
