import 'package:flutter/material.dart';
import 'default_appbar.dart';

class DefaultScaffold extends StatelessWidget  {
  const DefaultScaffold({super.key, this.title, required this.child, this.showTitle, this.additionalWidgets, this.leading});
  final String? title;
  final List<Widget>? additionalWidgets;
  final Widget child;
  final bool? showTitle;
  final Widget? leading;

  @override
  Widget build(BuildContext context) => Scaffold(
      appBar: DefaultAppbar(
        title: title, 
        showTitle: showTitle,
        additionalWidgets: additionalWidgets,
        leading: leading,
      ),
      body: child,
  );
}