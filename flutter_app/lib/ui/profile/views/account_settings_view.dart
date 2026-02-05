import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../classes/helpers/theme_manager.dart';
import '../../../colors.dart';
import '../../../widgets/profile_tile.dart';
import '../profile_bloc.dart';
import '../profile_events_states.dart';

class AccountSettingsView extends StatelessWidget {
  const AccountSettingsView(this.context, {super.key});

  final BuildContext context;

  @override
  Widget build(BuildContext context) {
    return ValueListenableBuilder<ThemeMode>(
      valueListenable: themeNotifier,
      builder: (_, __, ___) {
        final theme = getCurrentThemeData(context);
  
        return SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
            child: Column(
              children: [
                // HEADER CARD
                Container(
                  width: double.infinity,
                  decoration: BoxDecoration(
                    color: theme.cardColor,
                    borderRadius: BorderRadius.circular(6),
                    boxShadow: isLightMode(context) ? [AppColors.lightShadow] : [],
                  ),
                  padding: const EdgeInsets.all(16),
                  child: Text(
                    'Account Settings',
                    style: theme.textTheme.titleLarge?.copyWith(fontSize: 22),
                  ),
                ),
  
                const SizedBox(height: 18),
  
                ProfileTile(
                  context: context,
                  icon: Icons.lock_outline,
                  title: 'Change Password',
                  onTap: () {
                    context.read<ProfileBloc>().add(ShowChangePasswordEvent());
                  },
                ),
  
                const SizedBox(height: 12),
  
                ProfileTile(
                  context: context,
                  icon: Icons.pets_outlined,
                  title: 'Connected Animals',
                  onTap: () {
                    context.read<ProfileBloc>().add(ShowConnectedAnimalsEvent());
                  },
                ),
  
                const SizedBox(height: 12),
  
                ProfileTile(
                  context: context,
                  icon: Icons.color_lens_outlined,
                  title: 'Theme Mode',
                  trailing: themeDropdown(),
                ),
  
                const SizedBox(height: 24),
  
                SizedBox(
                  width: double.infinity,
                  height: 52,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: theme.cardColor,
                      elevation: isLightMode(context) ? 4 : 0,
                      shadowColor: isLightMode(context) ? AppColors.lightShadow.color : null,
                    ),
                    onPressed: () {
                      context.read<ProfileBloc>().add(const LoadProfileEvent());
                    },
                    child: Text(
                      'Back',
                      style: theme.textTheme.titleLarge?.copyWith(fontSize: 20),
                    ),
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }
}
