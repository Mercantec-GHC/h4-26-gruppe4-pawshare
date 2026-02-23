import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/objects/animal_type.dart';
import 'register_bloc.dart';
import 'register_events_states.dart';


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

  final TextEditingController _birthDateController = TextEditingController();
  DateTime? _selectedBirthDate;

  AnimalType? _selectedType;

  final _obscurePassword = true;
  final _obscureConfirmPassword = true;

  String? _passwordError;
  String? _emailError;
  

  bool get _isFormValid {
    return _nameController.text.isNotEmpty &&
        _emailController.text.isNotEmpty &&
        _passwordController.text.isNotEmpty &&
        _confirmPasswordController.text.isNotEmpty &&
        _cityController.text.isNotEmpty &&
        _animalNameController.text.isNotEmpty &&
        _selectedBirthDate != null &&
        _selectedType != null &&
        _passwordError == null &&
        _emailError == null;
  }

  @override
  void dispose() {
    _nameController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    _cityController.dispose();
    _animalNameController.dispose();
    _animalDescriptionController.dispose();
    _birthDateController.dispose();
    super.dispose();
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

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => RegisterBloc()..add(LoadAnimalTypes()),
      child: BlocListener<RegisterBloc, RegisterState>(
        listener: (context, state) {
          if (state.isSuccess) {
            Navigator.pushReplacementNamed(context, '/discover');
          }

          if (state.errorMessage != null) {
            ScaffoldMessenger.of(
              context,
            ).showSnackBar(SnackBar(content: Text(state.errorMessage!)));
          }
        },
        child: Scaffold(
          appBar: AppBar(title: const Text('Register Owner')),
          body: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: Column(
              children: [
                _buildField('Name', _nameController),

                const SizedBox(height: 16),

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

                const SizedBox(height: 16),

                TextField(
                  controller: _passwordController,
                  obscureText: _obscurePassword,
                  onChanged: (_) {
                    setState(() {
                      _validatePasswords();
                    });
                  },
                  decoration: const InputDecoration(
                    labelText: 'Password',
                    border: OutlineInputBorder(),
                  ),
                ),

                const SizedBox(height: 16),

                TextField(
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
                  ),
                ),

                const SizedBox(height: 16),

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

                const SizedBox(height: 16),

                TextField(
                  controller: _birthDateController,
                  readOnly: true,
                  onTap: _pickBirthDate,
                  decoration: const InputDecoration(
                    labelText: 'Date of Birth',
                    border: OutlineInputBorder(),
                    suffixIcon: Icon(Icons.calendar_today),
                  ),
                ),

                const SizedBox(height: 16),

                BlocBuilder<RegisterBloc, RegisterState>(
                  builder: (context, state) {
                    if (state.isLoadingTypes) {
                      return const Center(child: CircularProgressIndicator());
                    }

                    return DropdownButtonFormField<AnimalType>(
                      value: _selectedType,
                      decoration: const InputDecoration(
                        labelText: 'Animal type',
                        border: OutlineInputBorder(),
                      ),
                      items: state.animalTypes.map((type) {
                        return DropdownMenuItem(
                          value: type,
                          child: Text(type.name),
                        );
                      }).toList(),
                      onChanged: (value) {
                        setState(() {
                          _selectedType = value;
                        });
                      },
                    );
                  },
                ),

                const SizedBox(height: 24),

                BlocBuilder<RegisterBloc, RegisterState>(
                  builder: (context, state) {
                    return ElevatedButton(
                      onPressed: !_isFormValid || state.isLoading
                          ? null
                          : () {
                              context.read<RegisterBloc>().add(
                                RegisterOwnerSubmitted(
                                  body: {
                                    'Email': _emailController.text,
                                    'Name': _nameController.text,
                                    'Password': _passwordController.text,
                                    'City': _cityController.text,
                                    'ProfilePictureKey': '',
                                    'AnimalName': _animalNameController.text,
                                    'AnimalDescription':
                                        _animalDescriptionController.text,
                                    'DateOfBirth':
                                        "${_selectedBirthDate!.year.toString().padLeft(4, '0')}-"
                                        "${_selectedBirthDate!.month.toString().padLeft(2, '0')}-"
                                        "${_selectedBirthDate!.day.toString().padLeft(2, '0')}",
                                    'AnimalTypeId': _selectedType!.id,
                                  },
                                  email: _emailController.text,
                                  password: _passwordController.text,
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
