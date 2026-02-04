import 'package:flutter/material.dart';
import 'ui/login/login_page.dart';

final globalNavigatorKey = GlobalKey<NavigatorState>();

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'PawShare',
      navigatorKey: globalNavigatorKey,
      debugShowCheckedModeBanner: false,
      //should be LoginPage() by default
      home: const LoginPage(),
    );
  }
}
