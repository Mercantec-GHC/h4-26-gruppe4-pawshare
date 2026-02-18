import 'package:flutter/material.dart';
import '../../classes/helpers/auth.dart';
import '../login/login_page.dart';
import '../discover/discover_page.dart';

class AuthGate extends StatelessWidget {
  const AuthGate({super.key});

  Future<bool> _isLoggedIn() async {
    final token = await Auth.getAccessToken();
    return token.isNotEmpty;
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<bool>(
      future: _isLoggedIn(),
      builder: (context, snapshot) {
        if (!snapshot.hasData) {
          return const Scaffold(
            body: Center(child: CircularProgressIndicator()),
          );
        }

        return snapshot.data!
            ? const DiscoverPage()
            : const LoginPage();
      },
    );
  }
}