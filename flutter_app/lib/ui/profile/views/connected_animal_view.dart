import 'package:flutter/material.dart';

import '../../../classes/helpers/theme_manager.dart';
import '../../../colors.dart';
import '../../../widgets/skeleton_tile.dart';

class ConnectedAnimalView extends StatelessWidget {
  const ConnectedAnimalView(this.context, {super.key});

  final BuildContext context;

  @override
  Widget build(BuildContext context) {
    ThemeData theme = getCurrentThemeData(context);

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
                boxShadow: isLightMode(context) ? [AppColors.lightShadow] : [],
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
                          color: theme.dividerColor.withValues(alpha: 0.2),
                          borderRadius: BorderRadius.circular(4),
                        ),
                      ),
                      const SizedBox(height: 8),
                      Container(
                        width: 80,
                        height: 16,
                        decoration: BoxDecoration(
                          color: theme.dividerColor.withValues(alpha: 0.2),
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
                boxShadow: isLightMode(context) ? [AppColors.lightShadow] : [],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
