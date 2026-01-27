import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'discover_bloc.dart';

class DiscoverPage extends StatefulWidget {
  const DiscoverPage({super.key});

  @override
  State<StatefulWidget> createState() => _DiscoverPageState();
}

class _DiscoverPageState extends State<DiscoverPage> {

  @override
  Widget build(BuildContext context) {
    return BlocProvider(create: (_) => DiscoverBloc(),
    child: Column(
      mainAxisAlignment: MainAxisAlignment.center,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        SizedBox(height: 30,)

      ],
    ),);
  }

  Widget discoverCard(BuildContext context) {
    return Card(
      child: Row(
        children: [
          Image(image: )
        ],
      ),
    );
  }
}