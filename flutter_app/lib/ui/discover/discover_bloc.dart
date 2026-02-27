import 'dart:async';
import 'dart:convert';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart';

import '../../classes/helpers/api.dart';
import '../../classes/helpers/general_helper.dart';
import '../../classes/objects/animal.dart';
import '../../classes/objects/api_path.dart';
import 'discover_events_states.dart';

class DiscoverBloc extends Bloc<DiscoverEvents, DiscoverState> {
  DiscoverBloc() : super(DiscoverAnimalsInitial()) {
    on<DiscoverAnimals>(onDiscoverPageLoad);
    on<ContactButtonClicked>(onContactButtonClicked);
  }
String CreatedChatId = '';

  FutureOr<void> onDiscoverPageLoad(
    DiscoverAnimals event,
    Emitter<DiscoverState> emit,
  ) async {
    emit(DiscoverAnimalsLoading());
    Response resp = await API.getRequest(ApiPath.animal);
    if (resp.statusCode == 200) {
      List<dynamic> decodeResp = json.decode(resp.body);
      try {
        final animals = decodeResp.map((e) => Animal.fromJson(e)).toList();
        emit(DiscoverAnimalsSuccess(animals: animals));
      } catch (e) {
        emit(DiscoverAnimalsFailure(errorMessage: e.toString()));
      }
    }
  }

  FutureOr<void> onContactButtonClicked(
    ContactButtonClicked event,
    Emitter<DiscoverState> emit,
  ) async {
    var response = await API.postRequest(ApiPath.chat, {
      'userIds': [event.userId],
      'title': 'Chat about ${event.animalName}',
    });
    if (response.statusCode == 201 || response.statusCode == 200) {
      CreatedChatId = json.decode(response.body)['chatId'];
    }
    
  }


  FutureOr<void> onChatCreated(
    ChatCreated event,
    Emitter<ContactButtonState> emit,
  ) {
    // Handle chat creation logic here, e.g., navigate to chat page
    if(event.chatId == CreatedChatId) {
      // Navigate to chat page
      emit(ChatHasbeenCreated());
    }
  }
}
