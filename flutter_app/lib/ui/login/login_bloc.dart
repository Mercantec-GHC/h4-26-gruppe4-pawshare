import 'package:flutter_bloc/flutter_bloc.dart';
import 'dart:async';
import 'login_events_states.dart';

class LoginBloc extends Bloc<LoginEvents, LoginState> {
  Timer? _gameCheckTimer;

  LoginBloc() : super(const LoginTestState()) {
    on<TestEvent>(_onCheckPin);
  }

  Future<void> _onCheckPin(TestEvent event, Emitter<LoginState> emit) async {
    emit(const LoginTestState(isLoading: true));
  }

  @override
  Future<void> close() {
    _gameCheckTimer?.cancel();
    return super.close();
  }
}