
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import '../classes/helpers/general_helper.dart';
import '../classes/helpers/theme_manager.dart';
import '../classes/objects/user_dto.dart';
import '../classes/helpers/api.dart';
import '../colors.dart';
import 'dart:convert';

class ProfileName extends StatefulWidget {
  const ProfileName({
    super.key,
    required this.profile,
    this.onProfileUpdated,
  });

  final UserDTO profile;
  final Function(UserDTO)? onProfileUpdated;

  @override
  State<ProfileName> createState() => _ProfileNameState();
}

class _ProfileNameState extends State<ProfileName> {
  bool _isUploading = false;

  Future<void> _pickAndUploadImage() async {
    final ImagePicker picker = ImagePicker();

    // Show dialog to get picture
    final ImageSource? source = await showDialog<ImageSource>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Select Image Source'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            ListTile(
              leading: const Icon(Icons.camera_alt),
              title: const Text('Take Photo'),
              onTap: () => Navigator.pop(context, ImageSource.camera),
            ),

            ListTile(
              leading: const Icon(Icons.photo_library),
              title: const Text('Choose from Gallery'),
              onTap: () => Navigator.pop(context, ImageSource.gallery),
            ),
          ],
        ),
      ),
    );

    if (source == null) return;

    try {
      final XFile? image = await picker.pickImage(
        source: source,
        maxWidth: 800,
        maxHeight: 800,
        imageQuality: 85,
      );

      if (image == null) return;

      setState(() { _isUploading = true; });

      final bytes = await image.readAsBytes();
      final response = await API.uploadFile(bytes, image.name);

      if (response.statusCode == 200) {
        var payload = json.decode(response.body);
        String? fileKey = payload['objectKey'];

        if (fileKey == null || fileKey.isEmpty) {
          fileKey = payload['fileKey'];
        }

        if (fileKey == null || fileKey.isEmpty) {
          GeneralUtil.showToast(
            'Upload succeeded, but no file key was returned',
          );
          return;
        }

        final updatedUser = UserDTO(
          id: widget.profile.id,
          name: widget.profile.name,
          email: widget.profile.email,
          profilePictureKey: fileKey,
        );

        if (widget.onProfileUpdated != null) {
          widget.onProfileUpdated!(updatedUser);
        }

        GeneralUtil.showToast('Profile picture updated successfully');
      } else {
        GeneralUtil.showToast('Failed to upload: ${response.statusCode}');
      }
    } catch (e) {
      GeneralUtil.showToast('Error: $e');
    } finally {
      setState(() {
        _isUploading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final bool hasAvatar =
        widget.profile.profilePictureKey != null &&
        widget.profile.profilePictureKey!.isNotEmpty;
    ThemeData theme = getCurrentThemeData(context);
    final imageUrl = hasAvatar
        ? API.mediaFileUrl(widget.profile.profilePictureKey!)
        : null;

    return Container(
      width: double.infinity,
      decoration: BoxDecoration(
        color: theme.cardColor,
        borderRadius: BorderRadius.circular(6),
        boxShadow: isLightMode(context) ? [AppColors.lightShadow] : [],
      ),
      padding: const EdgeInsets.all(16),
      child: Row(
        children: [
          // AVATAR WITH UPLOAD
          GestureDetector(
            onTap: _isUploading ? null : _pickAndUploadImage,
            child: Stack(
              children: [
                CircleAvatar(
                  radius: 43,
                  backgroundColor: AppColors.avatarPlaceholder,
                  backgroundImage: hasAvatar && imageUrl != null
                      ? NetworkImage(imageUrl)
                      : null,
                  child: _isUploading
                      ? const CircularProgressIndicator(color: Colors.white)
                      : hasAvatar && imageUrl != null
                      ? null
                      : const Icon(Icons.person, size: 40, color: Colors.white),
                ),
                if (!_isUploading)
                  Positioned(
                    bottom: 0,
                    right: 0,
                    child: Container(
                      padding: const EdgeInsets.all(4),
                      decoration: BoxDecoration(
                        color: theme.primaryColor,
                        shape: BoxShape.circle,
                      ),
                      child: const Icon(
                        Icons.camera_alt,
                        size: 16,
                        color: Colors.white,
                      ),
                    ),
                  ),
              ],
            ),
          ),
    
          const SizedBox(width: 12),
    
          // NAME AND ROLE
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                widget.profile.name,
                style: theme.textTheme.titleLarge?.copyWith(fontSize: 20),
              ),
              const SizedBox(height: 6),
              Text(
                widget.profile.email,
                style: theme.textTheme.bodyMedium?.copyWith(fontSize: 14),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
