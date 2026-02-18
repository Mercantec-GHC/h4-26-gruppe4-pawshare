import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/helpers/auth.dart';
import '../../classes/services/chat_service.dart';
import 'register_events_states.dart';

class RegisterBloc extends Bloc<RegisterEvent, RegisterState> {
  RegisterBloc() : super(const RegisterFormState()) {
    on<RegisterSubmitted>(_onRegisterSubmitted);
  }

  Future<void> _onRegisterSubmitted(
    RegisterSubmitted event,
    Emitter<RegisterState> emit,
  ) async {
    emit(const RegisterFormState(isLoading: true));

    try {
      bool success = await Auth.register(event.email, event.password);

      if (success) {
        final loginSuccess = await Auth.login(event.email, event.password);
        if (loginSuccess) {
          await ChatService.instance.connect();
        }

        emit(const RegisterFormState(isSuccess: true));
      } else {
        emit(const RegisterFormState(errorMessage: 'Registration failed'));
      }
    } catch (_) {
      emit(const RegisterFormState(errorMessage: 'Something went wrong'));
    }
  }
}
