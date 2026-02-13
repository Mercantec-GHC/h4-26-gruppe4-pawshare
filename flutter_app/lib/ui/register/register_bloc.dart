import 'package:flutter_bloc/flutter_bloc.dart';
import '../../services/auth_service.dart';
import 'register_events_states.dart';

class RegisterBloc extends Bloc<RegisterEvent, RegisterState> {
  final AuthService _authService;

  RegisterBloc(this._authService) : super(const RegisterFormState()) {
    on<RegisterSubmitted>(_onRegisterSubmitted);
  }

  Future<void> _onRegisterSubmitted(
    RegisterSubmitted event,
    Emitter<RegisterState> emit,
  ) async {
    emit(const RegisterFormState(isLoading: true));

    try {
      await _authService.register(email: event.email, password: event.password);

      emit(const RegisterFormState(isSuccess: true));
    } on AuthException catch (e) {
      emit(RegisterFormState(errorMessage: e.message));
    } catch (_) {
      emit(const RegisterFormState(errorMessage: 'Something went wrong'));
    }
  }
}
