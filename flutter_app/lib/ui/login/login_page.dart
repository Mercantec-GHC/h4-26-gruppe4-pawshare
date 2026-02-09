import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../widgets/default_scaffold.dart';
import 'login_bloc.dart';
import 'login_events_states.dart';
import '../register/register_page.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  
  bool _obscurePassword = true;
  
  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => LoginBloc(),
      child: BlocBuilder<LoginBloc, LoginState>(
        builder: (context, state) => DefaultScaffold(
          child: SingleChildScrollView(
           child: Column(
             crossAxisAlignment: CrossAxisAlignment.stretch,
             children: [
              const SizedBox(height: 30),

              if (state is LoginTestState) _buildTestState(context, state),
             ],
           ),
          ),

        ),
      ),
    );
  }

  Widget _buildTestState(BuildContext context, LoginTestState state) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 24),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          const SizedBox(height: 40),
            Image.asset('assets/pawshare_logo.png', height: 80),
            const SizedBox(height: 24),

          const Text(
            'Welcome to PawShare!',
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
          ),

          const SizedBox(height: 32),

          TextField(
            decoration: const InputDecoration(
              labelText: 'Email',
              border: OutlineInputBorder(),
            ),
          ),

          const SizedBox(height: 16),

         TextField(
            obscureText: _obscurePassword,
            decoration: InputDecoration(
              labelText: 'Password',
              border: const OutlineInputBorder(),
              suffixIcon: IconButton(
                splashRadius: 20,
                onPressed: () {
                  setState(() {
                    _obscurePassword = !_obscurePassword;
                  });
                },
                icon: Icon(
                  _obscurePassword ? Icons.visibility_off : Icons.visibility,
                  color: Theme.of(context).iconTheme.color,
                ),
              ),
            ),
          ),

          const SizedBox(height: 24),

          ElevatedButton(
           style: ElevatedButton.styleFrom(
              backgroundColor: Theme.of(context).colorScheme.primary,
              foregroundColor: Theme.of(context).colorScheme.onPrimary,
              padding: const EdgeInsets.symmetric(vertical: 14),
            ),
            onPressed: () {},
            child: const Text('Log in'),
          ),

          const SizedBox(height: 16),

          Row(
            children: [
              const Expanded(child: Divider()),
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 12),
                child: Text(
                  'or',
                  style: Theme.of(context).textTheme.bodyMedium,
                ),
              ),
              const Expanded(child: Divider()),
            ],
          ),

          const SizedBox(height: 12),

          TextButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const RegisterPage()),
              );
            },
            child: const Text('Create account'),
          ),

          Align(
            alignment: Alignment.centerRight,
            child: TextButton(
              onPressed: () {
                // later: forgot password
              },
              child: const Text('Forgot password?'),
            ),
          ),
        ],
      ),
    );
  }
}
