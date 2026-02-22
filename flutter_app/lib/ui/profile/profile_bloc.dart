import 'dart:convert';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart';
import '../../classes/helpers/api.dart';
import '../../classes/helpers/secure_storage_helper.dart';
import '../../classes/objects/api_path.dart';
import '../../classes/objects/secure_storage_key.dart';
import '../../classes/objects/user_dto.dart';
import 'profile_events_states.dart';

class ProfileBloc extends Bloc<ProfileEvents, ProfileState> {
  ProfileBloc() : super(const LoadingProfileState()) {
    on<LoadProfileEvent>(_onLoad);
    on<LogoutEvent>(_onLogout);
    on<ShowAccountSettingsEvent>(_onShowAccountSettings);
    on<ShowNotificationsEvent>(_onShowNotifications);
    on<ShowChangePasswordEvent>(_onShowChangePassword);
    on<ShowConnectedAnimalsEvent>(_onShowConnectedAnimals);
    on<ChangePasswordEvent>(_onChangePassword);
  }


  // region Events

  // Runs to get profile data
  void _onLoad(LoadProfileEvent event, Emitter<ProfileState> emit) async {
    emit(const LoadingProfileState());

    // Attempt to get profile data from api
    var profileData = await _getProfileInformation();

    if (profileData == null) {
      // If API failed or didn't return information, force user to log in again
      add(LogoutEvent());
      return;
    }

    emit(ShowProfileState(profile: profileData));
  }

  void _onLogout(LogoutEvent event, Emitter<ProfileState> emit) async {
    SecureStorageHelper.clearSecureStorage();
    emit(LoggedOutState());
  }

  void _onShowAccountSettings(ShowAccountSettingsEvent event, Emitter<ProfileState> emit) {
    emit(const ShowAccountSettingsState());
  }

  void _onShowChangePassword(ShowChangePasswordEvent event, Emitter<ProfileState> emit) {
    emit(const ShowChangePasswordState());
  }
  
  void _onShowConnectedAnimals(ShowConnectedAnimalsEvent event, Emitter<ProfileState> emit) {
    emit(const ShowConnectedAnimalsState());
  }

  void _onShowNotifications(ShowNotificationsEvent event, Emitter<ProfileState> emit) {
    emit(const ShowNotificationsState());
  }

  void _onChangePassword(ChangePasswordEvent event, Emitter<ProfileState> emit) async {
    try {
      final resp = await API.postRequest(
        ApiPath.changePassword,
        {
          'currentPassword': event.currentPassword,
          'newPassword': event.newPassword,
        },
      );

      if (resp.statusCode == 200) {
        emit(PasswordChangedState(true));
      } else {
        emit(PasswordChangedState(false));
      }
    } catch (e) {
      emit(PasswordChangedState(false));
    }
  }

  // endregion

  // region seperate functions

  // Gets profile information from API using saved User Id, If nothing is found or error occurs returns null.
  Future<UserDTO?> _getProfileInformation() async {
    try {
      // Get user id from secure storage
      /*String? userId = await SecureStorageHelper.readFromStorage(SecureStorageKey.userId);
  
      if (userId != null) {
        // Sends get request to api/users/(userId)
        Response resp = await API.getRequest(ApiPath.user);

        if (resp.statusCode != 200) {
          return null;
        }*/

        // Creates dto object and returns
        UserDTO profileData = UserDTO(id: "id", name: "name", email: "email", profilePictureKey: "b08cbea4e2214c989ed004f23deda31e_8a0c29db581934d850f0ce62da563592.jpg");//UserDTO.fromJson(json.decode(resp.body));
        return profileData; 
      //}

      return null;
    } catch (e) {
      return null;
    } 
  }

  // endregion
}
