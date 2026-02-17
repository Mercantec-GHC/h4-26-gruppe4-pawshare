import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../classes/helpers/theme_manager.dart';
import '../../../classes/helpers/general_helper.dart';
import '../../../colors.dart';
import '../../../widgets/skeleton_tile.dart';
import '../profile_bloc.dart';
import '../profile_events_states.dart';

class ChangePasswordView extends StatefulWidget {
  const ChangePasswordView(this.context, {super.key});

  final BuildContext context;

  @override
  State<ChangePasswordView> createState() => _ChangePasswordViewState();
}

class _ChangePasswordViewState extends State<ChangePasswordView> {
  bool _loading = true;
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _currentController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _confirmController = TextEditingController();

  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(milliseconds: 300), () {
      if (mounted) setState(() => _loading = false);
    });
  }

  @override
  void dispose() {
    _currentController.dispose();
    _passwordController.dispose();
    _confirmController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final theme = getCurrentThemeData(context);

    if (_loading) {
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
          child: Column(
            children: const [
              SkeletonTile(),
              SizedBox(height: 12),
              SkeletonTile(),
              SizedBox(height: 12),
              SkeletonTile(),
            ],
          ),
        ),
      );
    }

    return BlocListener<ProfileBloc, ProfileState>( 
      listener: (context, state) { 
        if (state is PasswordChangedState) { 
          if (state.success) { 
            GeneralUtil.showToast('Password changed successfully'); 
            Navigator.pop(context); 
          } else { 
            GeneralUtil.showToast('Failed to change password');
          }
        }
      }, 
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 12),
          child: Form(
            key: _formKey,
            child: Column(
              children: [
                TextFormField(
                  controller: _currentController,
                  obscureText: true,
                  decoration: const InputDecoration(labelText: 'Current password'),
                  validator: (v) => (v == null || v.isEmpty) ? 'Enter current password' : null,
                ),
                const SizedBox(height: 12),
                TextFormField(
                  controller: _passwordController,
                  obscureText: true,
                  decoration: const InputDecoration(labelText: 'New password'),
                  validator: (v) => (v == null || v.length < 8) ? 'Minimum 8 characters' : null,
                ),
                const SizedBox(height: 12),
                TextFormField(
                  controller: _confirmController,
                  obscureText: true,
                  decoration: const InputDecoration(labelText: 'Confirm new password'),
                  validator: (v) => v != _passwordController.text ? 'Passwords do not match' : null,
                ),
                const SizedBox(height: 24),
                SizedBox(
                  width: double.infinity,
                  height: 52,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: theme.cardColor,
                      elevation: isLightMode(context) ? 4 : 0,
                      shadowColor: isLightMode(context) ? AppColors.lightShadow.color : null,
                    ),
                    onPressed: () {
                      if (_formKey.currentState?.validate() ?? false) {
                        context.read<ProfileBloc>().add(
                          ChangePasswordEvent(_currentController.text, _passwordController.text),
                        );
                      }
                    },
                    child: Text('Change password', style: theme.textTheme.titleLarge?.copyWith(fontSize: 20)),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
