import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'classes/helpers/theme_manager.dart';
import 'ui/auth/auth_gate.dart';
import 'ui/discover/discover_page.dart';
import 'ui/login/login_page.dart';
import 'ui/register/register_institution_page.dart';
import 'ui/register/register_owner_page.dart';
import 'ui/reset_password/reset_password_page.dart';

final globalNavigatorKey = GlobalKey<NavigatorState>();

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await SharedPreferences.getInstance();
  await loadThemeMode();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return ValueListenableBuilder<ThemeMode>(
      valueListenable: themeNotifier,
      builder: (_, mode, __) {
        return MaterialApp(
          title: 'PawShare',
          navigatorKey: globalNavigatorKey,
          themeMode: mode,
          theme: buildLightTheme(),
          darkTheme: buildDarkTheme(),

          onGenerateRoute: (settings) {
            final uri = Uri.parse(settings.name ?? '/');

            if (uri.path == '/reset-password') {
              final token = uri.queryParameters['token'] ?? '';
              return MaterialPageRoute(
                builder: (_) => ResetPasswordPage(token: token),
              );
            }
            switch (uri.path) {
              case '/login':
                return MaterialPageRoute(builder: (_) => const LoginPage());
              case '/discover':
                return MaterialPageRoute(builder: (_) => const DiscoverPage());
              case '/register-owner':
                return MaterialPageRoute(
                  builder: (_) => const RegisterOwnerPage(),
                );
              case '/register-institution':
                return MaterialPageRoute(
                  builder: (_) => const RegisterInstitutionPage(),
                );
              default:
                return MaterialPageRoute(builder: (_) => const AuthGate());
            }
          },
        );
      },
    );
  }
}
