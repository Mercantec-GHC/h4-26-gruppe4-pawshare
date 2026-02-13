import 'dart:convert';
import 'package:http/http.dart' as http;
import '../classes/objects/animal_type.dart';
import '../classes/objects/api_path.dart';
import '../config/api_config.dart';

class AnimalTypeService {
  Future<List<AnimalType>> getAllAnimalTypes() async {
    final response = await http.get(
      Uri.parse(
        '${ApiConfig.baseUrl}/api/${ApiPath.animal.value}Type',
      ),
    );

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);
      return data.map((e) => AnimalType.fromJson(e)).toList();
    } else {
      throw Exception('Failed to load animal types');
    }
  }
}