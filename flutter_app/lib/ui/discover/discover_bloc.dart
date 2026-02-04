
import 'dart:convert';

import 'package:flutter/rendering.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart';

import '../../classes/helpers/api.dart';
import '../../classes/objects/api_path.dart';
import 'discover_events_states.dart';

class DiscoverBloc extends Bloc<DiscoverEvents, DiscoverState> {
    DiscoverBloc() : super(const DiscoverTestState()) {
    on<DiscoverAnimals>(onDiscoverPageLoad);
  }
Future<Animal?> onDiscoverPageLoad(DiscoverAnimals event, Emitter<DiscoverState> emit ) async {
  Response resp = await API.getRequest(ApiPath.animal);
  if(resp.statusCode == 200) {
    Map<String, dynamic> decodeResp = json.decode(resp.body);
    try {
      return Animal.fromJson(decodeResp);
    }
    catch (e){
      debugPrint(e.toString());
    }
    
  }
  return null;
  
}

  Future<void> _onCheckPin(TestEvent event, Emitter<DiscoverState> emit) async {
    emit(const DiscoverTestState());
  }
  


}