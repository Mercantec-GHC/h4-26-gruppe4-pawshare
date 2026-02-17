import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../widgets/default_scaffold.dart';
import '../login/login_page.dart';
import 'profile_bloc.dart';
import 'profile_events_states.dart';
import 'views/account_settings_view.dart';
import 'views/change_password_view.dart';
import 'views/connected_animal_view.dart';
import 'views/loading_profile_view.dart';
import 'views/show_profile_view.dart';
import 'views/notifications_view.dart';

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
      child: BlocListener<ProfileBloc, ProfileState>(
        listener: (context, state) {
          if (state is LoggedOutState) {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => const LoginPage()),
            );
          }
        },
        child: BlocBuilder<ProfileBloc, ProfileState>(
          builder: (context, state) => PopScope(
            canPop: state is ShowProfileState, 
            onPopInvokedWithResult: (didPop, result) async { 
              final bloc = context.read<ProfileBloc>(); 
              if (!didPop) { 
                bloc.add(const LoadProfileEvent()); 
              }          
            },
                child: Builder(
                  builder: (context) {
                    Widget? leading;
                    Widget childWidget;
                    String? title;
        
                    switch (state) {
                      case ShowProfileState(profile: final profile):
                        childWidget = ShowProfileView(context, profile: profile);
                        break;
        
                      case ShowAccountSettingsState():
                        leading = IconButton(
                          icon: const Icon(Icons.arrow_back),
                          onPressed: () => context.read<ProfileBloc>().add(const LoadProfileEvent()),
                        );
                        title = 'Account Settings';
                        childWidget = AccountSettingsView(context);
                        break;
        
                      case ShowChangePasswordState():
                        leading = IconButton(
                          icon: const Icon(Icons.arrow_back),
                          onPressed: () => context.read<ProfileBloc>().add(const LoadProfileEvent()),
                        );
                        title = 'Change Password';
                        childWidget = ChangePasswordView(context);
                        break;
        
                      case ShowConnectedAnimalsState():
                        leading = IconButton(
                          icon: const Icon(Icons.arrow_back),
                          onPressed: () => context.read<ProfileBloc>().add(const LoadProfileEvent()),
                        );
                        title = 'My animals';
                        childWidget = ConnectedAnimalView(context);
                        break;
        
                      case ShowNotificationsState():
                        leading = IconButton(
                          icon: const Icon(Icons.arrow_back),
                          onPressed: () => context.read<ProfileBloc>().add(const LoadProfileEvent()),
                        );
                        title = 'Notifications';
                        childWidget = NotificationsView(context);
                        break;
        
                      default:
                        childWidget = LoadingProfileView(context);
                    }
        
                    return DefaultScaffold(
                      title: title ?? 'Profile',
                      showTitle: true,
                      leading: leading,
                      child: childWidget,
                    );
                  },
            ),
          ),
        ),
      ),
    );
  }
}
