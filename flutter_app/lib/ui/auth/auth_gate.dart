import 'package:flutter/material.dart';
import '../../services/auth_service.dart';
import '../login/login_page.dart';
import '../discover/discover_page.dart';

class AuthGate extends StatelessWidget {
  const AuthGate({super.key});

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<bool>(
      future: AuthService().isLoggedIn(),
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