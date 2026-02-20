import 'package:flutter_bloc/flutter_bloc.dart';
import '../../classes/helpers/auth.dart';
import '../../services/animal_type_service.dart';
import '../../classes/services/chat_service.dart';
import 'register_events_states.dart';

class RegisterBloc extends Bloc<RegisterEvent, RegisterState> {

  final AnimalTypeService _animalTypeService = AnimalTypeService();

  RegisterBloc() : super(const RegisterState()) {
    on<RegisterInstitutionSubmitted>(_onInstitutionRegister);
    on<RegisterOwnerSubmitted>(_onOwnerRegister);
    on<LoadAnimalTypes>(_onLoadAnimalTypes);
    on<RegisterSubmitted>(_onRegisterSubmitted);
  }

  // 🔹 SIMPLE REGISTER (from other branch)
  Future<void> _onRegisterSubmitted(
    RegisterSubmitted event,
    Emitter<RegisterState> emit,
  ) async {
    emit(state.copyWith(isLoading: true, errorMessage: null));

    try {
      bool success = await Auth.register(event.email, event.password);

      if (!success) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Registration failed',
        ));
        return;
      }

      final loginSuccess = await Auth.login(event.email, event.password);

      if (!loginSuccess) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Login failed',
        ));
        return;
      }

      await ChatService.instance.connect();

      emit(state.copyWith(isLoading: false, isSuccess: true));

    } catch (_) {
      emit(state.copyWith(
        isLoading: false,
        errorMessage: 'Something went wrong',
      ));
    }
  }

  // 🔹 LOAD ANIMAL TYPES
  Future<void> _onLoadAnimalTypes(
    LoadAnimalTypes event,
    Emitter<RegisterState> emit,
  ) async {
    emit(state.copyWith(isLoadingTypes: true));

    try {
      final types = await _animalTypeService.getAllAnimalTypes();

      emit(state.copyWith(
        isLoadingTypes: false,
        animalTypes: types,
      ));
    } catch (_) {
      emit(state.copyWith(
        isLoadingTypes: false,
        errorMessage: 'Failed to load animal types',
      ));
    }
  }

  // 🔹 INSTITUTION REGISTER
  Future<void> _onInstitutionRegister(
    RegisterInstitutionSubmitted event,
    Emitter<RegisterState> emit,
  ) async {
    emit(state.copyWith(isLoading: true, errorMessage: null));

    try {
      bool success = await Auth.registerInstitution({
        'Email': event.email,
        'Name': event.name,
        'Password': event.password,
        'City': event.city,
        'Base64Pfp': '',
      });

      if (!success) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Registration failed',
        ));
        return;
      }

      final loginSuccess = await Auth.login(event.email, event.password);

      if (!loginSuccess) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Login failed',
        ));
        return;
      }

      await ChatService.instance.connect();

      emit(state.copyWith(isLoading: false, isSuccess: true));

    } catch (_) {
      emit(state.copyWith(
        isLoading: false,
        errorMessage: 'Something went wrong',
      ));
    }
  }

  // 🔹 OWNER REGISTER
  Future<void> _onOwnerRegister(
    RegisterOwnerSubmitted event,
    Emitter<RegisterState> emit,
  ) async {
    emit(state.copyWith(isLoading: true, errorMessage: null));

    try {
      bool success = await Auth.registerOwner(event.body);

      if (!success) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Registration failed',
        ));
        return;
      }

      final loginSuccess = await Auth.login(event.email, event.password);

      if (!loginSuccess) {
        emit(state.copyWith(
          isLoading: false,
          errorMessage: 'Login failed',
        ));
        return;
      }

      await ChatService.instance.connect();

      emit(state.copyWith(isLoading: false, isSuccess: true));

    } catch (_) {
      emit(state.copyWith(
        isLoading: false,
        errorMessage: 'Something went wrong',
      ));
    }
  }
}