import 'package:flutter/material.dart';
import 'default_appbar.dart';

class DefaultScaffold extends StatelessWidget  {
  const DefaultScaffold({super.key, this.title, required this.child, this.showTitle, this.additionalWidgets});
  final String? title;
  final List<Widget>? additionalWidgets;
  final Widget child;
  final bool? showTitle;

  @override
  Widget build(BuildContext context) => Scaffold(
      appBar: DefaultAppbar(
        title: title, 
        showTitle: showTitle,
        additionalWidgets: additionalWidgets,
      ),
      body: child,
  );
}