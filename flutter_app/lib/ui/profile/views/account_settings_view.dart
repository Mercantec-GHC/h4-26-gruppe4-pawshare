import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../classes/helpers/theme_manager.dart';
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
        return SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
            child: Column(
              children: [
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
                  icon: Icons.color_lens_outlined,
                  title: 'Theme Mode',
                  trailing: themeDropdown(),
                ),
  
                const SizedBox(height: 24),
              ],
            ),
          ),
        );
      },
    );
  }
}
