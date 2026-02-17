import 'package:flutter/material.dart';

class EmptyStateCard extends StatelessWidget {
  const EmptyStateCard({
    super.key,
    this.icon = Icons.pets_outlined,
    required this.message,
  });

  final IconData icon;
  final String message;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 96,
            height: 96,
            decoration: BoxDecoration(
              color: theme.cardColor,
              borderRadius: BorderRadius.circular(16),
                boxShadow: theme.brightness == Brightness.light
                  ? [BoxShadow(color: theme.shadowColor.withValues(alpha: 0.06), blurRadius: 6, offset: const Offset(0, 2))]
                  : [],
            ),
            alignment: Alignment.center,
            child: Icon(icon, size: 40, color: theme.iconTheme.color),
          ),
          const SizedBox(height: 12),
          Text(message, style: theme.textTheme.bodyMedium),
        ],
      ),
    );
  }
}
