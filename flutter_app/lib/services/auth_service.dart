class AuthService {
  Future<void> login({required String email, required String password}) async {
    // simulate API call
    await Future.delayed(const Duration(seconds: 1));

    if (email == 'test@test.com' && password == '123456') {
      return; // success
    } else {
      throw AuthException('Invalid email or password');
    }
  }

  Future<void> register({
    required String email,
    required String password,
  }) async {
    await Future.delayed(const Duration(seconds: 1));

    if (email.contains('@')) {
      return; // success
    } else {
      throw AuthException('Invalid email address');
    }
  }
}

class AuthException implements Exception {
  final String message;
  AuthException(this.message);
}
