import 'dart:convert';
import 'package:flutter/material.dart';
import '../../../classes/helpers/api.dart';
import '../../../classes/helpers/theme_manager.dart';
import '../../../classes/helpers/general_helper.dart';
import '../../../classes/objects/api_path.dart';
import '../../../widgets/skeleton_tile.dart';
import '../../../widgets/empty_state_card.dart';
import '../../../classes/objects/animal.dart';

class ConnectedAnimalView extends StatefulWidget {
  const ConnectedAnimalView(this.context, {super.key});

  final BuildContext context;

  @override
  State<ConnectedAnimalView> createState() => _ConnectedAnimalViewState();
}

class _ConnectedAnimalViewState extends State<ConnectedAnimalView> {
  bool _loading = true;
  List<Animal> _animals = [];

  @override
  void initState() {
    super.initState();
    _fetchAnimals();
  }

  Future<void> _fetchAnimals() async {
    setState(() => _loading = true);
    
    try {
      final resp = await API.getRequest(ApiPath.animalUser);

      if (resp.statusCode == 200) {
        final List<dynamic> data = json.decode(resp.body);
        _animals = data.map((e) => Animal.fromJson(e as Map<String, dynamic>)).toList();
      } else {
        _animals = [];
        GeneralUtil.showToast('Failed to load animals (${resp.statusCode})');
      }

    } catch (e) {
      _animals = [];
      GeneralUtil.showToast('Failed to load animals');
    }
    setState(() => _loading = false);
   }  
   
   
   Future<void> _removeAnimal(String animalId) async {
    final resp = await API.deleteRequestWithId(ApiPath.animal, animalId);

    if (resp.statusCode == 204 || resp.statusCode == 200) {
      GeneralUtil.showToast('Animal removed');
      await _fetchAnimals();
      return;
    }

    GeneralUtil.showToast('Failed to remove animal (${resp.statusCode})');
  }

  @override
  Widget build(BuildContext context) {
    final theme = getCurrentThemeData(context);

    if (_loading) {
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 12.0),
          child: Column(
            children: const [
              SkeletonTile(),
              SizedBox(height: 12),
              SkeletonTile(),
              SizedBox(height: 12),
              SkeletonTile(),
            ],
          ),
        ),
      );
    }

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 12),
      child: _animals.isEmpty ? Column(
          children: [
            const SizedBox(height: 4),
            const EmptyStateCard(message: 'No animals found'),
          ],
        ) : ListView.separated(
          physics: const AlwaysScrollableScrollPhysics(),
          itemCount: _animals.length,
          separatorBuilder: (_, __) => const SizedBox(height: 12),
          itemBuilder: (context, index) {
            final a = _animals[index];
            
            return Material(
              color: theme.listTileTheme.tileColor,
              borderRadius: BorderRadius.circular(14),
              elevation: isLightMode(context) ? 4 : 0,
              child: InkWell(
                borderRadius: BorderRadius.circular(14),
                onTap: () {},
                child: SizedBox(
                  height: 96,
                  child: Padding(
                    padding: const EdgeInsets.all(12),
                    child: Row(
                      children: [
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Text(a.Name, style: theme.textTheme.bodyLarge?.copyWith(fontWeight: FontWeight.w600)),
                              const SizedBox(height: 6),
                              Text(a.Description, maxLines: 2, overflow: TextOverflow.ellipsis, style: theme.textTheme.bodySmall),
                            ],
                          ),
                        ),
                        IconButton(
                          icon: const Icon(Icons.delete_outline),
                        onPressed: () async {
                          await _removeAnimal(a.id);
                          },
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            );
          },
        ),
      );
  }
}
