import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../widgets/default_scaffold.dart';
import 'forgot_password_bloc.dart';

class ForgotPasswordPage extends StatefulWidget {
  const ForgotPasswordPage({super.key});

  @override
  State<ForgotPasswordPage> createState() => _ForgotPasswordPageState();
}

class _ForgotPasswordPageState extends State<ForgotPasswordPage> {
  final TextEditingController _emailController = TextEditingController();

  String? _emailError;
  String? _statusMessage;
  bool _isSuccess = false;

  bool get _isFormValid => _emailController.text.isNotEmpty;

  bool _isValidEmail(String email) {
    return email.contains('@') && email.contains('.');
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => ForgotPasswordBloc(),
      child: BlocListener<ForgotPasswordBloc, ForgotPasswordState>(
        listener: (context, state) {
          if (state is ForgotPasswordSuccess) {
            setState(() {
              _isSuccess = true;
              _statusMessage = 'Reset link sent to your email';
            });
          }

          if (state is ForgotPasswordFailure) {
            setState(() {
              _isSuccess = false;
              _statusMessage = state.message;
            });
          }
        },
        child: DefaultScaffold(child: _buildContent(context)),
      ),
    );
  }

  Widget _buildContent(BuildContext context) {
    return SingleChildScrollView(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 24),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const SizedBox(height: 40),
            Image.asset('assets/pawshare_logo.png', height: 80),
            const SizedBox(height: 24),
            
            const Text(
              'Forgot password',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Enter your email and we will send you a password reset link.',
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 32),
            
            TextField(
              controller: _emailController,
              keyboardType: TextInputType.emailAddress,
              onChanged: (_) => setState(() {}),
              decoration: InputDecoration(
                labelText: 'Email',
                border: const OutlineInputBorder(),
                errorText: _emailError,
              ),
            ),
            const SizedBox(height: 24),
            
            _buildButton(context),
            if (_statusMessage != null) ...[
              const SizedBox(height: 12),
              Text(
                _statusMessage!,
                textAlign: TextAlign.center,
                style: TextStyle(
                  color: _isSuccess ? Colors.green : Colors.red,
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
            const SizedBox(height: 12),
            
            TextButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: const Text('Back to login'),
            ),
            
          ],
          
        ),
        
      ),
      
    );
    
  }

  Widget _buildButton(BuildContext context) {
    return BlocBuilder<ForgotPasswordBloc, ForgotPasswordState>(
      builder: (context, state) {
        if (state is ForgotPasswordLoading) {
          return const Center(child: CircularProgressIndicator());
        }

        return ElevatedButton(
          onPressed: _isFormValid
              ? () {
                  setState(() {
                    _emailError = null;
                    _statusMessage = null;
                  });

                  if (!_isValidEmail(_emailController.text)) {
                    setState(() {
                      _emailError = 'Invalid email address';
                    });
                    return;
                  }

                  context.read<ForgotPasswordBloc>().add(
                    ForgotPasswordSubmitted(_emailController.text.trim()),
                  );
                }
              : null,
          child: const Text('Send reset link'),
        );
      },
    );
  }
}
