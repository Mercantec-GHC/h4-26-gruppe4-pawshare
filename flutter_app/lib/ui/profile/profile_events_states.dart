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

// STATES
abstract class ProfileState {
  const ProfileState();
}

class LoadingProfileState extends ProfileState {
  const LoadingProfileState();
}

class ShowProfileState extends ProfileState {
  // TODO: REPLACE WITH REAL MODEL
  final UserDTO profile;
  const ShowProfileState({required this.profile});
}
