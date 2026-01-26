// EVENTS
abstract class LoginEvents {
  const LoginEvents();
}

class TestEvent extends LoginEvents {
  const TestEvent();
}

// STATES
abstract class LoginState {
  const LoginState();
}

class LoginTestState extends LoginState {
  final bool isLoading;
  
  const LoginTestState({
    this.isLoading = false,
  });
}
