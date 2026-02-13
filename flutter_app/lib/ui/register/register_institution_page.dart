import 'package:flutter/material.dart';

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
  String? _passwordError;

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
            _buildField('Email', _emailController),
            _buildField(
              'Password',
              _passwordController,
              obscure: true,
              onChanged: (_) {
                setState(() {
                  _validatePasswords();
                });
              },
            ),
            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: TextField(
                controller: _confirmPasswordController,
                obscureText: true,
                onChanged: (_) {
                  setState(() {
                    _validatePasswords();
                  });
                },
                decoration: InputDecoration(
                  labelText: 'Confirm Password',
                  border: const OutlineInputBorder(),
                  errorText: _passwordError,
                ),
              ),
            ),
            _buildField('City', _cityController),

            const SizedBox(height: 24),

            ElevatedButton(
              onPressed: _isFormValid
                  ? () {
                      // TODO: call API later
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
