import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'classes/helpers/theme_manager.dart';
import 'ui/auth/auth_gate.dart';


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
          themeMode: ThemeMode.system, 
          theme: ThemeData(
            useMaterial3: true,
            brightness: Brightness.light,
            colorScheme: const ColorScheme.light(
              primary: Color(0xFFF5C84C), 
              onPrimary: Colors.black,
              secondary: Color(0xFFF5C84C),
            ),
          ),
          darkTheme: ThemeData(
            useMaterial3: true,
            brightness: Brightness.dark,
            colorScheme: const ColorScheme.dark(
              primary: Color(0xFFF5C84C),
              onPrimary: Colors.black,
              secondary: Color(0xFFF5C84C),
            ),
          ),
          home: const AuthGate(),
        );
      },
    );
  }
}
