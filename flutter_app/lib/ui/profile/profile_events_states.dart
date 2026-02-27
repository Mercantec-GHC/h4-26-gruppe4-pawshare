// EVENTS
import '../../classes/objects/user_dto.dart';

abstract class ProfileEvents {
  const ProfileEvents();
}

class LoadProfileEvent extends ProfileEvents {
  const LoadProfileEvent();
}

class LogoutEvent extends ProfileEvents {
  const LogoutEvent();
}

class ShowAccountSettingsEvent extends ProfileEvents {
  const ShowAccountSettingsEvent();
}

class ShowNotificationsEvent extends ProfileEvents {
  const ShowNotificationsEvent();
}

class ShowChangePasswordEvent extends ProfileEvents {
  const ShowChangePasswordEvent();
}

class ShowConnectedAnimalsEvent extends ProfileEvents {
  const ShowConnectedAnimalsEvent();
}

class ChangePasswordEvent extends ProfileEvents {
  final String currentPassword;
  final String newPassword;

  ChangePasswordEvent(this.currentPassword, this.newPassword);
}

class ProfileUpdatedEvent extends ProfileEvents {
  final dynamic updatedProfile;

  const ProfileUpdatedEvent(this.updatedProfile);
}


// STATES
abstract class ProfileState {
  const ProfileState();
}

class LoadingProfileState extends ProfileState {
  const LoadingProfileState();
}

class ShowProfileState extends ProfileState {
  final UserDTO profile;
  const ShowProfileState({required this.profile});
}

class ShowAccountSettingsState extends ProfileState {
  const ShowAccountSettingsState();
}

class ShowChangePasswordState extends ProfileState {
  const ShowChangePasswordState();
}

class ShowConnectedAnimalsState extends ProfileState {
  const ShowConnectedAnimalsState();
}

class ShowNotificationsState extends ProfileState {
  const ShowNotificationsState();
}

class LoggedOutState extends ProfileState {
  const LoggedOutState();
}

class PasswordChangedState extends ProfileState {
  final bool success;
  PasswordChangedState(this.success);
}
