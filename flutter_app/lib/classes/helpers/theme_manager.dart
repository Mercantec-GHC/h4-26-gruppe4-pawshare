import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../../colors.dart';
import '../objects/theme_value.dart';

final ValueNotifier<ThemeMode> themeNotifier = ValueNotifier(ThemeMode.system);

// Reusable values
String _fontFamily = 'Rubik';
double _appbarFontSize = 24;
FontWeight _appbarFontWeight = FontWeight.w600;

// Toggles the current theme
void toggleTheme() {
  themeNotifier.value = themeNotifier.value == ThemeMode.dark ? ThemeMode.light : ThemeMode.dark;
}

// Checks if the user is light mode
bool isLightMode(BuildContext context) {
  final mode = themeNotifier.value;

  switch (mode) {
    case ThemeMode.light:
      return true;

    case ThemeMode.dark:
      return false;

    case ThemeMode.system:
      final systemBrightness = MediaQuery.of(context).platformBrightness;
      return systemBrightness == Brightness.light;
  }
}

// Gets current theme data
ThemeData getCurrentThemeData(BuildContext context) {
  final mode = themeNotifier.value;

  switch (mode) {
    case ThemeMode.light:
      return buildLightTheme();

    case ThemeMode.dark:
      return buildDarkTheme();

    case ThemeMode.system:
      final systemBrightness = MediaQuery.of(context).platformBrightness;
      return systemBrightness == Brightness.light
          ? buildLightTheme()
          : buildDarkTheme();
  }
}

// Loads the theme string from shared preference and sets a ThemeMode 
Future<void> loadThemeMode() async {
  final prefs = await SharedPreferences.getInstance();
  final saved = prefs.getString('themeMode');

  if (saved != null) {
    themeNotifier.value = ThemeModeX.fromString(saved);
  } else {
    // Defaults to system
    themeNotifier.value = ThemeMode.system;
  }
}

// Saves theme as a string in shared preferences
Future<void> saveThemeMode(ThemeMode mode) async {
  final prefs = await SharedPreferences.getInstance();
  await prefs.setString('themeMode', mode.value);
}

// Dropdown widget
Widget themeDropdown() {
  return ValueListenableBuilder<ThemeMode>(
    valueListenable: themeNotifier,
    builder: (context, mode, _) {
      return DropdownButton<ThemeMode>(
        value: mode,
        items: ThemeMode.values.map((mode) {
          return DropdownMenuItem(
            value: mode,
            child: Text(mode.value),
          );
        }).toList(),
        onChanged: (newMode) {
          if (newMode != null) {
            themeNotifier.value = newMode;
            saveThemeMode(newMode);
          }
        },
      );
    },
  );
}

// Themes
ThemeData buildLightTheme() {
  return ThemeData(
    brightness: Brightness.light,
    scaffoldBackgroundColor: AppColors.lightBackground,
    fontFamily: _fontFamily,
    appBarTheme: AppBarTheme(
      backgroundColor: AppColors.lightHeader,
      elevation: 0,
      centerTitle: true,
      titleTextStyle: TextStyle(
        color: AppColors.primaryText,
        fontSize: _appbarFontSize,
        fontWeight: _appbarFontWeight,
      ),
    ),
    cardColor: AppColors.lightCard,
    listTileTheme: const ListTileThemeData(
      tileColor: AppColors.lightTile,
      iconColor: AppColors.primaryText,
      textColor: AppColors.primaryText,
    ),
  );
}

ThemeData buildDarkTheme() {
  return ThemeData(
    brightness: Brightness.dark,
    scaffoldBackgroundColor: AppColors.darkBackground,
    fontFamily: _fontFamily,
    appBarTheme: AppBarTheme(
      backgroundColor: AppColors.darkHeader,
      elevation: 0,
      centerTitle: true,
      titleTextStyle: TextStyle(
        color: Colors.white,
        fontSize: _appbarFontSize,
        fontWeight: _appbarFontWeight,
      ),
    ),
    cardColor: AppColors.darkCard,
    listTileTheme: const ListTileThemeData(
      tileColor: AppColors.darkTile,
      iconColor: Colors.white,
      textColor: Colors.white,
    ),
  );
}
