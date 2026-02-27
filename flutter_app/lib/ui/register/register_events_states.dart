// EVENTS
import '../../classes/objects/animal_type.dart';

abstract class RegisterEvent {}

class RegisterInstitutionSubmitted extends RegisterEvent {
  final String name;
  final String email;
  final String password;
  final String city;

  RegisterInstitutionSubmitted({
    required this.name,
    required this.email,
    required this.password,
    required this.city,
  });
}

class RegisterOwnerSubmitted extends RegisterEvent {
  final Map<String, dynamic> body;
  final String email;
  final String password;

  RegisterOwnerSubmitted({
    required this.body,
    required this.email,
    required this.password,
  });
}

class RegisterSubmitted extends RegisterEvent {
  final String email;
  final String password;

   RegisterSubmitted({required this.email, required this.password});
}
class LoadAnimalTypes extends RegisterEvent {}

// STATES
class RegisterState {
  final bool isLoading;
  final bool isSuccess;
  final String? errorMessage;
  final List<AnimalType> animalTypes;
  final bool isLoadingTypes;

  const RegisterState({
    this.isLoading = false,
    this.isSuccess = false,
    this.errorMessage,
    this.animalTypes = const [],
    this.isLoadingTypes = false,
  });

  RegisterState copyWith({
    bool? isLoading,
    bool? isSuccess,
    String? errorMessage,
    List<AnimalType>? animalTypes,
    bool? isLoadingTypes,
  }) {
    return RegisterState(
      isLoading: isLoading ?? this.isLoading,
      isSuccess: isSuccess ?? this.isSuccess,
      errorMessage: errorMessage,
      animalTypes: animalTypes ?? this.animalTypes,
      isLoadingTypes: isLoadingTypes ?? this.isLoadingTypes,
    );
  }
}
