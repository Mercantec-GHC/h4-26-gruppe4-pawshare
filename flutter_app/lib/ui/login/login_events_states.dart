// EVENTS
abstract class LoginEvents {
  const LoginEvents();
}

class LoginSubmitted extends LoginEvents {
  final String email;
  final String password;

  const LoginSubmitted({
    required this.email,
    required this.password,
  });
}

// STATES
abstract class LoginState {
  const LoginState();
}

class LoginFormState extends LoginState {
  final bool isLoading;
  final String? errorMessage;
  final bool isSuccess;

  const LoginFormState({
    this.isLoading = false,
    this.errorMessage,
    this.isSuccess = false,
  });
}


