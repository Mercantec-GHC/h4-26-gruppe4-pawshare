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

final class DiscoverAnimalsFailure extends DiscoverState {
  final String errorMessage;

  DiscoverAnimalsFailure({required this.errorMessage});
}

@immutable
sealed class DiscoverEvents {}

final class DiscoverAnimals extends DiscoverEvents {}
