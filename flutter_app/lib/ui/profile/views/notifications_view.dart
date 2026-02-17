import 'package:flutter/material.dart';
import '../../../classes/helpers/theme_manager.dart';
import '../../../widgets/empty_state_card.dart';

class NotificationsView extends StatefulWidget {
  const NotificationsView(this.context, {super.key});

  final BuildContext context;

  @override
  State<NotificationsView> createState() => _NotificationsViewState();
}

class _NotificationsViewState extends State<NotificationsView> {
  final List<String> _items = [];

  @override
  Widget build(BuildContext context) {
    final theme = getCurrentThemeData(context);

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 12),
      child: _items.isEmpty ? Column(
        children: [
          const SizedBox(height: 4),
          const EmptyStateCard(message: 'No notifications'),
        ],
      ) : ListView.separated(
        itemCount: _items.length,
        separatorBuilder: (_, __) => const SizedBox(height: 12),
        itemBuilder: (context, index) {
          final text = _items[index];
          return Material(
            color: theme.listTileTheme.tileColor,
            borderRadius: BorderRadius.circular(12),
            child: Padding(
              padding: const EdgeInsets.all(12),
              child: Text(text, style: theme.textTheme.bodyMedium),
            ),
          );
        },
      ),
    );
  }
}
