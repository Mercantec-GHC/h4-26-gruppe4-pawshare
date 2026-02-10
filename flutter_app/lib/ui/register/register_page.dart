import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../services/auth_service.dart';
import '../../widgets/default_scaffold.dart';
import 'register_bloc.dart';
import 'register_events_states.dart';

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key});

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  bool _obscurePassword = true;
  bool _obscureConfirmPassword = true;
  String? _passwordError;
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _confirmPasswordController =
      TextEditingController();

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => RegisterBloc(AuthService()),
      child: BlocListener<RegisterBloc, RegisterState>(
        listener: (context, state) {
          if (state is RegisterFormState && state.isSuccess) {
            Navigator.pop(context); // назад на Login
          }
        },
        child: BlocBuilder<RegisterBloc, RegisterState>(
          builder: (context, state) => DefaultScaffold(
            child: SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 24),
                child: _buildRegisterForm(context, state),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildRegisterForm(BuildContext context, RegisterState state) {
    final formState = state is RegisterFormState
        ? state
        : const RegisterFormState();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        const SizedBox(height: 40),

        Image.asset('assets/pawshare_logo.png', height: 80),

        const SizedBox(height: 24),

        const Text(
          'Create account',
          textAlign: TextAlign.center,
          style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
        ),

        const SizedBox(height: 32),

        TextField(
          controller: _emailController,
          keyboardType: TextInputType.emailAddress,
          decoration: InputDecoration(
            labelText: 'Email',
            border: const OutlineInputBorder(),
            errorText: formState.errorMessage,
          ),
        ),

        const SizedBox(height: 16),

        TextField(
          controller: _passwordController,
          obscureText: _obscurePassword,
          decoration: InputDecoration(
            labelText: 'Password',
            border: const OutlineInputBorder(),
            suffixIcon: IconButton(
              icon: Icon(
                _obscurePassword ? Icons.visibility_off : Icons.visibility,
              ),
              onPressed: () {
                setState(() {
                  _obscurePassword = !_obscurePassword;
                });
              },
            ),
          ),
        ),

        const SizedBox(height: 16),

        TextField(
          controller: _confirmPasswordController,
          obscureText: _obscureConfirmPassword,
          decoration: InputDecoration(
            labelText: 'Confirm password',
            border: const OutlineInputBorder(),
            errorText: _passwordError,
            suffixIcon: IconButton(
              icon: Icon(
                _obscureConfirmPassword
                    ? Icons.visibility_off
                    : Icons.visibility,
              ),
              onPressed: () {
                setState(() {
                  _obscureConfirmPassword = !_obscureConfirmPassword;
                });
              },
            ),
          ),
        ),

        const SizedBox(height: 24),

        ElevatedButton(
          onPressed: formState.isLoading
              ? null
              : () {
                  setState(() {
                    _passwordError = null;
                  });

                  if (_passwordController.text !=
                      _confirmPasswordController.text) {
                    setState(() {
                      _passwordError = 'Passwords do not match';
                    });
                    return;
                  }

                  context.read<RegisterBloc>().add(
                    RegisterSubmitted(
                      email: _emailController.text,
                      password: _passwordController.text,
                    ),
                  );
                },
          child: formState.isLoading
              ? const SizedBox(
                  height: 20,
                  width: 20,
                  child: CircularProgressIndicator(strokeWidth: 2),
                )
              : const Text('Create account'),
        ),

        if (formState.errorMessage != null) ...[
          const SizedBox(height: 12),
          Text(
            formState.errorMessage!,
            textAlign: TextAlign.center,
            style: const TextStyle(color: Colors.red),
          ),
        ],

        const SizedBox(height: 12),

        TextButton(
          onPressed: () => Navigator.pop(context),
          child: const Text('Already have an account? Log in'),
        ),
      ],
    );
  }
}
