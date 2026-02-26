import 'package:flutter/widgets.dart';

import '../../classes/objects/animal.dart';

@immutable
sealed class DiscoverState {}

final class DiscoverAnimalsInitial extends DiscoverState {}

final class DiscoverAnimalsLoading extends DiscoverState {}

final class DiscoverAnimalsSuccess extends DiscoverState {
  final List<Animal> animals;
  DiscoverAnimalsSuccess({required this.animals});
}

@immutable
sealed class ContactButtonState {}

final class ChatHasbeenCreated extends ContactButtonState {
}

final class DiscoverAnimalsFailure extends DiscoverState {
  final String errorMessage;

  DiscoverAnimalsFailure({required this.errorMessage});
}

@immutable
sealed class DiscoverEvents {}

final class DiscoverAnimals extends DiscoverEvents {}

final class ContactButtonClicked extends DiscoverEvents {
  final String userId;
  final String animalName;

  ContactButtonClicked({required this.userId, required this.animalName});
}

final class ChatCreated extends DiscoverEvents {
  final BuildContext context;
  final String chatId;

  ChatCreated({required this.context, required this.chatId});
}
