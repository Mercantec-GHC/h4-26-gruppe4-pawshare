import 'package:flutter/material.dart';
import '../../classes/objects/animal_type.dart';
import '../../services/animal_type_service.dart';
import '../../classes/helpers/auth.dart';

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
  final _animalTypeService = AnimalTypeService();
  DateTime? _selectedBirthDate;
  final TextEditingController _birthDateController = TextEditingController();

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
        _selectedBirthDate != null &&
        _passwordError == null &&
        _emailError == null;
  }

  @override
  void initState() {
    super.initState();
    _loadAnimalTypes();
  }

  Future<void> _pickBirthDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: DateTime(2020),
      firstDate: DateTime(2000),
      lastDate: DateTime.now(),
    );

    if (picked != null) {
      setState(() {
        _selectedBirthDate = picked;
        _birthDateController.text =
            "${picked.year}-${picked.month.toString().padLeft(2, '0')}-${picked.day.toString().padLeft(2, '0')}";
      });
    }
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
            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: TextField(
                controller: _birthDateController,
                readOnly: true,
                onTap: _pickBirthDate,
                decoration: InputDecoration(
                  labelText: 'Date of Birth',
                  border: const OutlineInputBorder(),
                  suffixIcon: const Icon(Icons.calendar_today),
                  hintText: _selectedBirthDate == null
                      ? 'Select date'
                      : '${_selectedBirthDate!.day}.${_selectedBirthDate!.month}.${_selectedBirthDate!.year}',
                ),
              ),
            ),
            const SizedBox(height: 16),

            if (_isLoadingTypes)
              const Center(child: CircularProgressIndicator())
            else
              DropdownButtonFormField<AnimalType>(
                initialValue: _selectedType,
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
                        bool success = await Auth.registerOwner({
                          'Email': _emailController.text,
                          'Name': _nameController.text,
                          'Password': _passwordController.text,
                          'City': _cityController.text,
                          'Base64Pfp': '',
                          'AnimalName': _animalNameController.text,
                          'AnimalDescription':
                              _animalDescriptionController.text,
                          'DateOfBirth':
                              "${_selectedBirthDate!.year.toString().padLeft(4, '0')}-"
                              "${_selectedBirthDate!.month.toString().padLeft(2, '0')}-"
                              "${_selectedBirthDate!.day.toString().padLeft(2, '0')}",
                          'AnimalTypeId': _selectedType!.id,
                        });

                        if (success) {
                          await Auth.login(
                            _emailController.text,
                            _passwordController.text,
                          );

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
