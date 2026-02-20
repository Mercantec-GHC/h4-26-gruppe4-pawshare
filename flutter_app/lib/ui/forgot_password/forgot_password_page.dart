import 'package:flutter/material.dart';
import '../../widgets/default_scaffold.dart';

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
    return DefaultScaffold(
      child: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 24),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              const SizedBox(height: 40),

              // Logo
              Image.asset(
                'assets/pawshare_logo.png',
                height: 80,
              ),

              const SizedBox(height: 24),

              const Text(
                'Forgot password',
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                ),
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

              ElevatedButton(
                style: ElevatedButton.styleFrom(
                  backgroundColor:
                      Theme.of(context).colorScheme.primary,
                  foregroundColor:
                      Theme.of(context).colorScheme.onPrimary,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                ),
                onPressed: _isFormValid
                    ? () {
                        setState(() {
                          _emailError = null;
                          _statusMessage = null;

                          if (!_isValidEmail(
                              _emailController.text)) {
                            _emailError =
                                'Invalid email address';
                            _isSuccess = false;
                            return;
                          }

                          // success (UI only)
                          _isSuccess = true;
                          _statusMessage =
                              'Reset link sent to your email';
                        });
                      }
                    : null,
                child: const Text('Send reset link'),
              ),

              if (_statusMessage != null) ...[
                const SizedBox(height: 12),
                Text(
                  _statusMessage!,
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    color: _isSuccess
                        ? Colors.green
                        : Colors.red,
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
      ),
    );
  }
}