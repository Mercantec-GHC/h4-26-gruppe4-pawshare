import 'dart:convert';

import 'package:flutter/material.dart';

class ImagePlaceholder extends StatelessWidget {
  const ImagePlaceholder({
    super.key,
    required this.base64,
    this.width = 72,
    this.height = 72,
    this.fit = BoxFit.cover,
    this.borderRadius = 8.0,
    this.iconSize = 40.0,
  });

  final String base64;
  final double width;
  final double height;
  final BoxFit fit;
  final double borderRadius;
  final double iconSize;

  @override
  Widget build(BuildContext context) {
    final bytes = base64Decode(base64);
    return ClipRRect(
      borderRadius: BorderRadius.circular(borderRadius),
      child: Image.memory(
        bytes,
        width: width,
        height: height,
        fit: fit,
      ),
      );
  }
}
