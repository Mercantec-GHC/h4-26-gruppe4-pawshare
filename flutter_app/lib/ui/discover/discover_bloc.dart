import 'package:flutter_bloc/flutter_bloc.dart';

import 'discover_events_states.dart';

class DiscoverBloc extends Bloc<DiscoverEvents, DiscoverState> {
    DiscoverBloc() : super(const DiscoverTestState()) {
    on<TestEvent>(_onCheckPin);
  }

  Future<void> _onCheckPin(TestEvent event, Emitter<DiscoverState> emit) async {
    emit(const DiscoverTestState());
  }

  @override
  Future<void> close() {
    return super.close();
  }
}