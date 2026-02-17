import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../classes/helpers/theme_manager.dart';
import '../../../classes/objects/user_dto.dart';
import '../../../colors.dart';
import '../../../widgets/profile_name.dart';
import '../../../widgets/profile_tile.dart';
import '../profile_bloc.dart';
import '../profile_events_states.dart';

class ShowProfileView extends StatelessWidget {
  const ShowProfileView(
    this.context, {
      super.key,
      required this.profile,
  });

  final BuildContext context;
  final UserDTO profile;

  @override
  Widget build(BuildContext context) {
    ThemeData theme = getCurrentThemeData(context);

    return SingleChildScrollView(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
        child: Column(
          children: [
            // PROFILE CARD
            ProfileName(profile: profile),
            const SizedBox(height: 18),

            ProfileTile(
              context: context, 
              icon: Icons.article_outlined, 
              title: 'My Posts', 
              onTap: () {
                debugPrint('my posts');
              },
            ),

            const SizedBox(height: 12),
            ProfileTile(
              context: context, 
              icon: Icons.place_outlined, 
              title: 'My Visits',
              onTap: () {
                debugPrint('my visits');
              },
            ),
            const SizedBox(height: 12),
              
            ProfileTile(
              context: context, 
              icon: Icons.notifications_none, 
              title: 'Notifications',
              onTap: () {
                context.read<ProfileBloc>().add(ShowNotificationsEvent());
              },
            ),
            const SizedBox(height: 12),

            // ACCOUNT SETTINGS WITH THEME SWITCH
            ProfileTile(
              context: context, 
              icon: Icons.settings_outlined, 
              title: 'Account Settings',
              onTap: () {
                context.read<ProfileBloc>().add(ShowAccountSettingsEvent());
              },
            ),

            const SizedBox(height: 24),

            // LOGOUT BUTTON
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
                  context.read<ProfileBloc>().add(LogoutEvent());
                },
                child: Text(
                  'Log Out',
                  style: theme.textTheme.titleLarge?.copyWith(fontSize: 20),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}