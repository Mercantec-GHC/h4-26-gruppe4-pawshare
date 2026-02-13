// EVENTS
abstract class RegisterEvent {
  const RegisterEvent();
}

class RegisterSubmitted extends RegisterEvent {
  final String email;
  final String password;

  const RegisterSubmitted({required this.email, required this.password});
}

// STATES
abstract class RegisterState {
  const RegisterState();
}

class RegisterFormState extends RegisterState {
  final bool isLoading;
  final bool isSuccess;
  final String? errorMessage;

  const RegisterFormState({
    this.isLoading = false,
    this.isSuccess = false,
    this.errorMessage,
  });
}
