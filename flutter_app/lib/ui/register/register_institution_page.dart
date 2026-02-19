import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'register_bloc.dart';
import 'register_events_states.dart';
import '../../classes/services/chat_service.dart';

class RegisterInstitutionPage extends StatefulWidget {
  const RegisterInstitutionPage({super.key});

  @override
  State<RegisterInstitutionPage> createState() =>
      _RegisterInstitutionPageState();
}

class _RegisterInstitutionPageState
    extends State<RegisterInstitutionPage> {
  final _nameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _cityController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  String? _passwordError;
  String? _emailError;

  bool get _isFormValid =>
      _nameController.text.isNotEmpty &&
      _emailController.text.isNotEmpty &&
      _passwordController.text.isNotEmpty &&
      _confirmPasswordController.text.isNotEmpty &&
      _cityController.text.isNotEmpty &&
      _passwordError == null &&
      _emailError == null;

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => RegisterBloc(),
      child: BlocListener<RegisterBloc, RegisterState>(
        listener: (context, state) {
          if (state.isSuccess) {
            Navigator.pushReplacementNamed(context, '/discover');
          }

          if (state.errorMessage != null) {
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(content: Text(state.errorMessage!)),
            );
          }
        },
        child: Scaffold(
          appBar: AppBar(title: const Text('Register Institution')),
          body: Padding(
            padding: const EdgeInsets.all(24),
            child: Column(
              children: [
                TextField(
                  controller: _nameController,
                  decoration: const InputDecoration(
                    labelText: 'Institution Name',
                    border: OutlineInputBorder(),
                  ),
                  onChanged: (_) => setState(() {}),
                ),
                const SizedBox(height: 16),

                TextField(
                  controller: _emailController,
                  decoration: InputDecoration(
                    labelText: 'Email',
                    border: const OutlineInputBorder(),
                    errorText: _emailError,
                  ),
                  onChanged: (_) => setState(() {}),
                ),
                const SizedBox(height: 16),

                TextField(
                  controller: _passwordController,
                  obscureText: true,
                  decoration: const InputDecoration(
                    labelText: 'Password',
                    border: OutlineInputBorder(),
                  ),
                  onChanged: (_) => setState(() {}),
                ),
                const SizedBox(height: 16),

                TextField(
                  controller: _confirmPasswordController,
                  obscureText: true,
                  decoration: InputDecoration(
                    labelText: 'Confirm Password',
                    border: const OutlineInputBorder(),
                    errorText: _passwordError,
                  ),
                  onChanged: (_) => setState(() {
                    _passwordError =
                        _passwordController.text !=
                                _confirmPasswordController.text
                            ? 'Passwords do not match'
                            : null;
                  }),
                ),
                const SizedBox(height: 16),

                TextField(
                  controller: _cityController,
                  decoration: const InputDecoration(
                    labelText: 'City',
                    border: OutlineInputBorder(),
                  ),
                  onChanged: (_) => setState(() {}),
                ),

                const SizedBox(height: 24),

                BlocBuilder<RegisterBloc, RegisterState>(
                  builder: (context, state) {
                    return ElevatedButton(
                      onPressed: !_isFormValid || state.isLoading
                          ? null
                          : () {
                              context.read<RegisterBloc>().add(
                                    RegisterInstitutionSubmitted(
                                      name: _nameController.text,
                                      email: _emailController.text,
                                      password:
                                          _passwordController.text,
                                      city: _cityController.text,
                                    ),
                                  );
                            },
                      child: state.isLoading
                          ? const CircularProgressIndicator()
                          : const Text('Create Account'),
                    );
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
