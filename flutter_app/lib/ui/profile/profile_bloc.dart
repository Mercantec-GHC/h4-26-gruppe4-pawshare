import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/objects/profile.dart';
import 'profile_events_states.dart';

class ProfileBloc extends Bloc<ProfileEvents, ProfileState> {
  ProfileBloc() : super(const LoadingProfileState()) {
    on<LoadProfileEvent>(_onLoad);
  }

  void _onLoad(LoadProfileEvent event, Emitter<ProfileState> emit) async {
    emit(const LoadingProfileState());
    // TODO: GET PROFILE DATA
    await Future.delayed(const Duration(milliseconds: 200));
    emit(ShowProfileState(profile: Profile()));
  }
}
