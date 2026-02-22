import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'classes/helpers/theme_manager.dart';
import 'ui/auth/auth_gate.dart';
import 'ui/discover/discover_page.dart';
import 'ui/login/login_page.dart';
import 'ui/register/register_institution_page.dart';
import 'ui/register/register_owner_page.dart';

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
          home: const AuthGate(),
          routes: {
            '/login': (context) => const LoginPage(),
            '/discover': (context) => const DiscoverPage(),
            '/register-owner': (context) => const RegisterOwnerPage(),
            '/register-institution': (context) =>
                const RegisterInstitutionPage(),
          },
        );
      },
    );
  }
}
