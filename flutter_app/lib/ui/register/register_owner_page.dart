import 'package:flutter/material.dart';
import '../../classes/objects/animal_type.dart';
import '../../services/animal_type_service.dart';
import '../../services/auth_service.dart';

class RegisterOwnerPage extends StatefulWidget {
  const RegisterOwnerPage({super.key});

  @override
  State<RegisterOwnerPage> createState() => _RegisterOwnerPageState();
}

class _RegisterOwnerPageState extends State<RegisterOwnerPage> {
  final _nameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();
  final _cityController = TextEditingController();

  final _animalNameController = TextEditingController();
  final _animalDescriptionController = TextEditingController();
  final _animalAgeController = TextEditingController();
  final _animalTypeService = AnimalTypeService();

  List<AnimalType> _animalTypes = [];
  AnimalType? _selectedType;
  bool _isLoadingTypes = true;

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
        _animalNameController.text.isNotEmpty &&
        _animalAgeController.text.isNotEmpty &&
        _passwordError == null &&
        _emailError == null;
  }

  @override
  void initState() {
    super.initState();
    _loadAnimalTypes();
  }

  Future<void> _loadAnimalTypes() async {
    try {
      final types = await _animalTypeService.getAllAnimalTypes();
      setState(() {
        _animalTypes = types;
        _isLoadingTypes = false;
      });
    } catch (e) {
      setState(() {
        _isLoadingTypes = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Register Owner')),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(24),
        child: Column(
          children: [
            _buildField('Name', _nameController),
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

            const Align(
              alignment: Alignment.centerLeft,
              child: Text(
                'Animal Information',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
            ),

            const SizedBox(height: 12),

            _buildField('Animal Name', _animalNameController),
            _buildField('Description', _animalDescriptionController),
            _buildField(
              'Age',
              _animalAgeController,
              keyboardType: TextInputType.number,
            ),
            const SizedBox(height: 16),

            if (_isLoadingTypes)
              const Center(child: CircularProgressIndicator())
            else
              DropdownButtonFormField<AnimalType>(
                value: _selectedType,
                decoration: const InputDecoration(
                  labelText: 'Animal type',
                  border: OutlineInputBorder(),
                ),
                items: _animalTypes.map((type) {
                  return DropdownMenuItem(value: type, child: Text(type.name));
                }).toList(),
                onChanged: (value) {
                  setState(() {
                    _selectedType = value;
                  });
                },
              ),

            const SizedBox(height: 24),

            ElevatedButton(
              onPressed: _isFormValid
                  ? () async {
                      if (_selectedType == null) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(content: Text('Select animal type')),
                        );
                        return;
                      }

                      try {
                        await AuthService().registerOwner(
                          email: _emailController.text,
                          name: _nameController.text,
                          password: _passwordController.text,
                          city: _cityController.text,
                          base64Pfp: '',
                          animalName: _animalNameController.text,
                          animalDescription: _animalDescriptionController.text,
                          animalAge: int.parse(_animalAgeController.text),
                          animalTypeId: _selectedType!.id,
                        );

                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(
                            content: Text('Registration successful'),
                          ),
                        );

                        Navigator.pop(context);
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
