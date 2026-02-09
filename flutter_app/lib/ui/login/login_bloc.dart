import 'package:flutter_bloc/flutter_bloc.dart';
import 'dart:async';
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

    await Future.delayed(const Duration(seconds: 1));

    if (event.email == 'test@test.com' &&
        event.password == '123456') {
      emit(const LoginFormState(isSuccess: true));
    } else {
      emit(const LoginFormState(
        errorMessage: 'Invalid email or password',
      ));
    }
  }
}