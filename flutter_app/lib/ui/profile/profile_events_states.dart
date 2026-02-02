// EVENTS
import '../../classes/objects/profile.dart';

abstract class ProfileEvents {
  const ProfileEvents();
}

class LoadProfileEvent extends ProfileEvents {
  const LoadProfileEvent();
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
  final Profile profile;
  const ShowProfileState({required this.profile});
}
