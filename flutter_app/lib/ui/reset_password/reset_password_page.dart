import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../../widgets/default_scaffold.dart';
import 'reset_password_bloc.dart';

class ResetPasswordPage extends StatefulWidget {
  final String token;

  const ResetPasswordPage({super.key, required this.token});

  @override
  State<ResetPasswordPage> createState() => _ResetPasswordPageState();
}

class _ResetPasswordPageState extends State<ResetPasswordPage> {
  final TextEditingController _passwordController = TextEditingController();

  String? _passwordError;
  String? _statusMessage;
  bool _isSuccess = false;

  bool get _isFormValid => _passwordController.text.isNotEmpty;

  bool _isValidPassword(String password) {
    return password.length >= 8;
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => ResetPasswordBloc(),
      child: BlocListener<ResetPasswordBloc, ResetPasswordState>(
        listener: (context, state) {
          if (state is ResetPasswordSuccess) {
            setState(() {
              _isSuccess = true;
              _statusMessage = "Password successfully updated";
            });
          }

          if (state is ResetPasswordFailure) {
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

            const Text(
              'Reset password',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 24),

            TextField(
              controller: _passwordController,
              obscureText: true,
              onChanged: (_) => setState(() {}),
              decoration: InputDecoration(
                labelText: 'New password',
                border: const OutlineInputBorder(),
                errorText: _passwordError,
              ),
            ),

            const SizedBox(height: 24),

            _buildButton(context),

            if (_statusMessage != null) ...[
              const SizedBox(height: 16),
              Text(
                _statusMessage!,
                textAlign: TextAlign.center,
                style: TextStyle(
                  color: _isSuccess ? Colors.green : Colors.red,
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],

            const SizedBox(height: 16),

            if (_isSuccess)
              TextButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text("Back to login"),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildButton(BuildContext context) {
    return BlocBuilder<ResetPasswordBloc, ResetPasswordState>(
      builder: (context, state) {
        if (state is ResetPasswordLoading) {
          return const Center(child: CircularProgressIndicator());
        }
        TextButton(
          onPressed: () {
            Navigator.push(
              context,
              MaterialPageRoute(
                builder: (_) =>
                    ResetPasswordPage(token: "ВСТАВ_ТУТ_ТОКЕН_З_EMAIL"),
              ),
            );
          },
          child: const Text("TEST RESET"),
        );
        return ElevatedButton(
          onPressed: _isFormValid
              ? () {
                  setState(() {
                    _passwordError = null;
                    _statusMessage = null;
                  });

                  if (!_isValidPassword(_passwordController.text)) {
                    setState(() {
                      _passwordError = "Password must be at least 8 characters";
                    });
                    return;
                  }

                  context.read<ResetPasswordBloc>().add(
                    ResetPasswordSubmitted(
                      widget.token,
                      _passwordController.text.trim(),
                    ),
                  );
                }
              : null,
          child: const Text("Update password"),
        );
      },
    );
  }
}
