import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/helpers/theme_manager.dart';
import '../../colors.dart';
import '../../widgets/default_scaffold.dart';
import '../../widgets/profile_tile.dart';
import '../../widgets/profile_name.dart';
import '../../widgets/skeleton_tile.dart';
import 'profile_bloc.dart';
import 'profile_events_states.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  @override
  Widget build(BuildContext context) {

    return BlocProvider(
      create: (_) => ProfileBloc()..add(const LoadProfileEvent()),
      child: BlocBuilder<ProfileBloc, ProfileState>(
        builder: (context, state) => DefaultScaffold(
          title: 'Profile',
          showTitle: true,
          child: Builder(
            builder: (context) {

              switch (state.runtimeType) {
                case const (ShowProfileState):
                  return _buildShowProfileState(context);

                default:
                  return _buildLoadingProfileState(context);
              }
            }
          ),
        ),
      ),
    );
  }



  Widget _buildShowProfileState(BuildContext context) {
    final theme = Theme.of(context);
    final isLightMode = theme.brightness == Brightness.light;
    
    return SingleChildScrollView(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
        child: Column(
          children: [
            // PROFILE CARD
            ProfileName(theme: theme, isLightMode: isLightMode),
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
                debugPrint('Notifications');
              },
            ),
            const SizedBox(height: 12),

            // ACCOUNT SETTINGS WITH THEME SWITCH
            ProfileTile(
              context: context, 
              icon: Icons.settings_outlined, 
              title: 'Account Settings',
              onTap: () {
                debugPrint('account settings');
              },
            ),

            // DARK/LIGHT MODE SWITCH
            themeSwitchWidget(),
            const SizedBox(height: 24),

            // LOGOUT BUTTON
            SizedBox(
              width: double.infinity,
              height: 52,
              child: ElevatedButton(
                style: ElevatedButton.styleFrom(
                  backgroundColor: theme.cardColor,
                  elevation: isLightMode ? 4 : 0,
                  shadowColor: isLightMode ? AppColors.lightShadow.color : null,
                ),
                onPressed: () {},
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

Widget _buildLoadingProfileState(BuildContext context) {
  final theme = Theme.of(context);
  final isLight = theme.brightness == Brightness.light;

  return SingleChildScrollView(
    child: Padding(
      padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
      child: Column(
        children: [
          // PROFILE CARD SKELETON
          Container(
            width: double.infinity,
            decoration: BoxDecoration(
              color: theme.cardColor,
              borderRadius: BorderRadius.circular(6),
              boxShadow: isLight ? [AppColors.lightShadow] : [],
            ),
            padding: const EdgeInsets.all(16),
            child: Row(
              children: [
                // Avatar placeholder
                Container(
                  width: 86,
                  height: 86,
                  decoration: const BoxDecoration(
                    color: AppColors.avatarPlaceholder,
                    shape: BoxShape.circle,
                  ),
                ),
                const SizedBox(width: 12),

                // Name + role placeholders
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Container(
                      width: 140,
                      height: 20,
                      decoration: BoxDecoration(
                        color: theme.dividerColor.withOpacity(0.2),
                        borderRadius: BorderRadius.circular(4),
                      ),
                    ),
                    const SizedBox(height: 8),
                    Container(
                      width: 80,
                      height: 16,
                      decoration: BoxDecoration(
                        color: theme.dividerColor.withOpacity(0.2),
                        borderRadius: BorderRadius.circular(4),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),

          const SizedBox(height: 18),

          // TILES
          SkeletonTile(),
          const SizedBox(height: 12),
          SkeletonTile(),
          const SizedBox(height: 12),
          SkeletonTile(),
          const SizedBox(height: 12),
          SkeletonTile(),

          const SizedBox(height: 24),

          // LOGOUT BUTTON SKELETON
          Container(
            width: double.infinity,
            height: 52,
            decoration: BoxDecoration(
              color: theme.cardColor,
              borderRadius: BorderRadius.circular(8),
              boxShadow: isLight ? [AppColors.lightShadow] : [],
            ),
          ),
        ],
      ),
    ),
  );
}


}
