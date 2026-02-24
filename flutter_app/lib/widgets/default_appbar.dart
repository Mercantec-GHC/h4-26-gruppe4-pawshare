import 'package:flutter/material.dart';

class DefaultAppbar extends StatelessWidget implements PreferredSizeWidget{
  const DefaultAppbar({
    super.key,
    this.title,
    this.showTitle,
    this.additionalWidgets,
    this.leading,
    this.titleWidget,
  });
  final String? title;
  final List<Widget>? additionalWidgets;
  final bool? showTitle;
  final Widget? leading;
  final Widget? titleWidget;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return AppBar(
      leading: leading,
      iconTheme: theme.appBarTheme.iconTheme?.copyWith(
        color: theme.colorScheme.onSurface,
      ) ?? IconThemeData(color: theme.colorScheme.onSurface),
      actionsIconTheme: theme.appBarTheme.actionsIconTheme?.copyWith(
        color: theme.colorScheme.onSurface,
      ) ?? IconThemeData(color: theme.colorScheme.onSurface),
      title: titleWidget ??
          (title != null
              ? Padding(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 41, vertical: 24),
                  child: Text(
                    title!,
                    style: Theme.of(context).textTheme.headlineMedium,
                    textAlign: TextAlign.center,
                  ),
                )
              : showTitle == true
                  ? Padding(
                      padding: const EdgeInsets.symmetric(
                        horizontal: 41,
                        vertical: 24,
                      ),
                      child: Image(
                        image: const AssetImage('assets/logo.png'),
                        height: 40,
                      ),
                    )
                  : null),
      centerTitle: true,
      elevation: 0,
      actions: additionalWidgets,
    );
  }
  
  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}