
import 'dart:convert';

import 'package:flutter/material.dart';
import '../classes/objects/user_dto.dart';
import '../colors.dart';

class ProfileName extends StatelessWidget {
  const ProfileName({
    super.key,
    required this.theme,
    required this.isLightMode,
    required this.profile
  });

  final ThemeData theme;
  final bool isLightMode;
  final UserDTO profile;

  @override
  Widget build(BuildContext context) {
    final bool hasAvatar = profile.base64Pfp != null && profile.base64Pfp!.isNotEmpty;

    return Container(
      width: double.infinity,
      decoration: BoxDecoration(
        color: theme.cardColor,
        borderRadius: BorderRadius.circular(6),
        boxShadow: isLightMode ? [AppColors.lightShadow] : [],
      ),
      padding: const EdgeInsets.all(16),
      child: Row(
        children: [
    
          // AVATAR
          CircleAvatar(
            radius: 43,
            backgroundColor: AppColors.avatarPlaceholder,
            backgroundImage: hasAvatar
              ? MemoryImage(base64Decode(profile.base64Pfp!))
              : null,
            child: hasAvatar
              ? null
              : const Icon(Icons.person, size: 40, color: Colors.white),
          ),
    
          const SizedBox(width: 12),
    
          // NAME AND ROLE
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                profile.name,
                style: theme.textTheme.titleLarge?.copyWith(
                  fontSize: 20,
                ),
              ),
              const SizedBox(height: 6),
              Text(
                profile.email,
                style: theme.textTheme.bodyMedium?.copyWith(
                  fontSize: 14,
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
