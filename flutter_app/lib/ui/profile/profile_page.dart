import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../widgets/default_scaffold.dart';
import 'profile_bloc.dart';
import 'profile_events_states.dart';
import 'views/account_settings_view.dart';
import 'views/change_password_view.dart';
import 'views/connected_animal_view.dart';
import 'views/loading_profile_view.dart';
import 'views/show_profile_view.dart';

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
        builder: (context, state) => PopScope(
          canPop: state is ShowProfileState, 
          onPopInvokedWithResult: (didPop, result) async { 
            final bloc = context.read<ProfileBloc>(); 
            if (!didPop) { 
              bloc.add(const LoadProfileEvent()); 
            }          
          },
          child: DefaultScaffold(
            title: 'Profile',
            showTitle: true,
            child: Builder(
              builder: (context) {
                switch (state.runtimeType) {
                  case const (ShowProfileState):
                    return ShowProfileView(context, profile: (state as ShowProfileState).profile);
                  
                  case const (ShowAccountSettingsState):
                    return AccountSettingsView(context);
                  
                  case const (ShowChangePasswordState):
                    return ChangePasswordView(context);
                  
                  case const (ShowConnectedAnimalsState):
                    return ConnectedAnimalView(context);
                  
                  default:
                    return LoadingProfileView(context);
                }
              }
            ),
          ),
        ),
      ),
    );
  }
}
