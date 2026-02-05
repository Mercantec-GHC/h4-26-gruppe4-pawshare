import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/helpers/secure_storage_helper.dart';
import '../../classes/objects/secure_storage_key.dart';
import '../../classes/objects/user_dto.dart';
import '../../main.dart';
import '../login/login_page.dart';
import 'profile_events_states.dart';

class ProfileBloc extends Bloc<ProfileEvents, ProfileState> {
  ProfileBloc() : super(const LoadingProfileState()) {
    on<LoadProfileEvent>(_onLoad);
    on<LogoutEvent>(_onLogout);
    on<ShowAccountSettingsEvent>(_onShowAccountSettings);
    on<ShowChangePasswordEvent>(_onShowChangePassword);
    on<ShowConnectedAnimalsEvent>(_onShowConnectedAnimals);
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
    
    Navigator.pushAndRemoveUntil(
      globalNavigatorKey.currentContext!, 
      MaterialPageRoute(builder: (context) => LoginPage()),
      (route) => false,
    );
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

  // endregion

  // region seperate functions

  // Gets profile information from API using saved User Id, If nothing is found or error occurs returns null.
  Future<UserDTO?> _getProfileInformation() async {
    try {
      await SecureStorageHelper.saveToStorage(SecureStorageKey.userId, 'c8be36d7-18ef-4400-bf24-0ea3d60d7a34');

      // Get user id from secure storage
      String? userId = await SecureStorageHelper.readFromStorage(SecureStorageKey.userId);
  
      if (userId != null) {
        // Sends get request to api/users/(userId)
        // TODO: CORS ERROR
        //Response resp = await API.getRequestWithId(ApiPath.user, userId);
        var abc = '{"id": "c8be36d7-18ef-4400-bf24-0ea3d60d7a34", "name": "string", "email": "string", "base64Pfp": "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII="}';
  
        //var a = json.decode(resp.body);
        var a = json.decode(abc);

        // Creates dto object and returns
        UserDTO profileData = UserDTO.fromJson(a);
        return profileData; 
      }

      return null;
    } catch (e) {
      return null;
    } 
  }

  // endregion
}
