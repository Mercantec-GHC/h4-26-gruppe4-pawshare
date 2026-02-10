import 'package:flutter_bloc/flutter_bloc.dart';
import 'dart:async';
import '../../services/auth_service.dart';
import 'login_events_states.dart';


class LoginBloc extends Bloc<LoginEvents, LoginState> {
  final AuthService _authService;
  
  LoginBloc(this._authService) 
      : super(const LoginFormState()) {
    on<LoginSubmitted>(_onLoginSubmitted);
  }

  Future<void> _onLoginSubmitted(
    LoginSubmitted event,
    Emitter<LoginState> emit,
  ) async {
    emit(const LoginFormState(isLoading: true));

    try {
      await _authService.login(email: event.email, password: event.password);

      emit(const LoginFormState(isSuccess: true));
    } on AuthException catch (e) {
      emit(LoginFormState(errorMessage: e.message));
    } catch (_) {
      emit(const LoginFormState(errorMessage: 'Something went wrong'));
    }
  }
}