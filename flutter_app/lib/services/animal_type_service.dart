import 'dart:convert';
import '../classes/objects/animal_type.dart';
import '../classes/objects/api_path.dart';
import '../classes/helpers/api.dart';

class AnimalTypeService {
  Future<List<AnimalType>> getAllAnimalTypes() async {
    final response = await API.getRequest(ApiPath.animalType);

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);
      return data.map((e) => AnimalType.fromJson(e)).toList();
    } else {
      throw Exception('Failed to load animal types');
    }
  }
}