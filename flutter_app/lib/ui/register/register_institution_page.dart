import 'package:flutter/material.dart';
import '../../classes/helpers/auth.dart';
import '../../classes/services/chat_service.dart';

class RegisterInstitutionPage extends StatefulWidget {
  const RegisterInstitutionPage({super.key});

  @override
  State<RegisterInstitutionPage> createState() =>
      _RegisterInstitutionPageState();
}

class _RegisterInstitutionPageState extends State<RegisterInstitutionPage> {
  final _nameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _cityController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  bool _obscurePassword = true;
  bool _obscureConfirmPassword = true;
  String? _passwordError;
  String? _emailError;
  bool _isValidEmail(String email) {
    final regex = RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$');
    return regex.hasMatch(email);
  }

  bool get _isFormValid {
    return _nameController.text.isNotEmpty &&
        _emailController.text.isNotEmpty &&
        _passwordController.text.isNotEmpty &&
        _confirmPasswordController.text.isNotEmpty &&
        _cityController.text.isNotEmpty &&
        _passwordError == null;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Register Institution')),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(24),
        child: Column(
          children: [
            _buildField('Institution Name', _nameController),
            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: TextField(
                controller: _emailController,
                keyboardType: TextInputType.emailAddress,
                onChanged: (value) {
                  setState(() {
                    if (value.isEmpty) {
                      _emailError = null;
                    } else if (!_isValidEmail(value)) {
                      _emailError = 'Invalid email address';
                    } else {
                      _emailError = null;
                    }
                  });
                },
                decoration: InputDecoration(
                  labelText: 'Email',
                  border: const OutlineInputBorder(),
                  errorText: _emailError,
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: TextField(
                controller: _passwordController,
                obscureText: _obscurePassword,
                onChanged: (_) {
                  setState(() {
                    _validatePasswords();
                  });
                },
                decoration: InputDecoration(
                  labelText: 'Password',
                  border: const OutlineInputBorder(),
                  suffixIcon: IconButton(
                    icon: Icon(
                      _obscurePassword
                          ? Icons.visibility_off
                          : Icons.visibility,
                    ),
                    onPressed: () {
                      setState(() {
                        _obscurePassword = !_obscurePassword;
                      });
                    },
                  ),
                ),
              ),
            ),

            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: TextField(
                controller: _confirmPasswordController,
                obscureText: _obscureConfirmPassword,
                onChanged: (_) {
                  setState(() {
                    _validatePasswords();
                  });
                },
                decoration: InputDecoration(
                  labelText: 'Confirm Password',
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
            ),

            _buildField('City', _cityController),

            const SizedBox(height: 24),

            ElevatedButton(
              onPressed: _isFormValid
                  ? () async {
                      try {
                        bool success = await Auth.registerInstitution({
                          'Email': _emailController.text,
                          'Name': _nameController.text,
                          'Password': _passwordController.text,
                          'City': _cityController.text,
                          'Base64Pfp': '',
                        });

                        if (success) {
                          final loginSuccess = await Auth.login(
                            _emailController.text,
                            _passwordController.text,
                          );

                          if (loginSuccess) {
                            await ChatService.instance.connect();
                          }

                          Navigator.pushReplacementNamed(context, '/discover');
                        } else {
                          ScaffoldMessenger.of(context).showSnackBar(
                            const SnackBar(
                              content: Text('Registration failed'),
                            ),
                          );
                        }
                      } catch (e) {
                        ScaffoldMessenger.of(
                          context,
                        ).showSnackBar(SnackBar(content: Text('Error: $e')));
                      }
                    }
                  : null,
              child: const Text('Create Account'),
            ),
          ],
        ),
      ),
    );
  }

  void _validatePasswords() {
    if (_confirmPasswordController.text.isEmpty) {
      _passwordError = null;
      return;
    }

    if (_passwordController.text != _confirmPasswordController.text) {
      _passwordError = 'Passwords do not match';
    } else {
      _passwordError = null;
    }
  }

  Widget _buildField(
    String label,
    TextEditingController controller, {
    bool obscure = false,
    TextInputType keyboardType = TextInputType.text,
    void Function(String)? onChanged,
  }) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: TextField(
        controller: controller,
        obscureText: obscure,
        keyboardType: keyboardType,
        onChanged: onChanged ?? (_) => setState(() {}),
        decoration: InputDecoration(
          labelText: label,
          border: const OutlineInputBorder(),
        ),
      ),
    );
  }
}
