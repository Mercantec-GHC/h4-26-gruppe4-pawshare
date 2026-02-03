import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'discover_bloc.dart';
import 'discover_events_states.dart';

class DiscoverPage extends StatefulWidget {
  const DiscoverPage({super.key});

  @override
  State<DiscoverPage> createState() => _DiscoverPageState();
}

class _DiscoverPageState extends State<DiscoverPage> {


  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => DiscoverBloc(),
      child: BlocBuilder<DiscoverBloc, DiscoverState>(
        builder: (context, state) {
          context.read<DiscoverBloc>().add(DiscoverAnimals());
          return Scaffold(
            appBar: discoverAppBar(context),
            drawer: Drawer(
              child: ListView(
                children: [
                  DrawerHeader(
                    child: Center(
                      child: Image(
                        image: AssetImage('assets/pawshare_logo.png'),
                        height: 40,
                        width: 44,
                      ),
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.home),
                    title: Text('Discover'),
                    onTap: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(builder: (context) => DiscoverPage()),
                      );
                    },
                  ),
                ],
              ),
            ),
            body: Expanded(
              child: Center(
                child: Column(
                  //mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: [
                    //discoverAppBar(context),
                    SizedBox(height: 30),
                    DiscoverCard(),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  PreferredSizeWidget discoverAppBar(BuildContext context) {
    return AppBar(
      backgroundColor: Color(0xFFFFFCF5),
      elevation: 0,
      title: Center(
        child: Row(
          children: [
            Image(
              image: AssetImage('assets/pawshare_logo.png'),
              height: 40,
              width: 44,
              fit: BoxFit.fitWidth,
            ),
            Padding(padding: EdgeInsets.only(left: 40)),
            Text('Pawshare', overflow: TextOverflow.visible),
          ],
        ),
      ),
      actions: [IconButton(onPressed: () {}, icon: Icon(Icons.notifications))],
    );
  }

  
}



class DiscoverCard extends StatefulWidget {
  const DiscoverCard({super.key});

  @override
  State<DiscoverCard> createState() => _DiscoverCardState();
}

class _DiscoverCardState extends State<DiscoverCard> {

    bool isLiked = false;

  void toggleLike(){
    setState(() {
      isLiked = !isLiked;
    });
  }
  @override
  Widget build(BuildContext context) {
    // TODO: implement build

    return Container(
      width: 376,
      height: 134,
      padding: const EdgeInsets.all(16),
      decoration: ShapeDecoration(
        color: const Color(0xFFFFFCF5),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
        shadows: [
          BoxShadow(
            color: Color(0x3F000000),
            blurRadius: 4,
            offset: Offset(0, 4),
            spreadRadius: 10,
          ),
        ],
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        mainAxisAlignment: MainAxisAlignment.start,
        crossAxisAlignment: CrossAxisAlignment.start,
        spacing: 12,
        children: [
          Container(
            width: 67,
            height: 99,
            decoration: ShapeDecoration(
              color: const Color(0xFF2A3038),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(10),
              ),
            ),
          ),
          Container(
            width: 212,
            height: 99,
            padding: const EdgeInsets.all(16),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              spacing: 12,
              children: [
                SizedBox(
                  width: 45,
                  child: Text(
                    'Name',
                    style: TextStyle(
                      color: const Color(0xFF0C0C0C),
                      fontSize: 16,
                      fontFamily: 'Inter',
                      fontWeight: FontWeight.w400,
                    ),
                  ),
                ),
                SizedBox(
                  width: 65,
                  child: Text(
                    'Description',
                    style: TextStyle(
                      color: const Color(0xFF7F7F7F),
                      fontSize: 12,
                      fontFamily: 'Inter',
                      fontWeight: FontWeight.w400,
                    ),
                  ),
                ),
              ],
            ),
          ),
          SizedBox(
            width: 37,
            height: 37,
            child: LikeButton(isLiked: isLiked, onTap: toggleLike)
          ),
        ],
      ),
    );
  }
}


class LikeButton extends StatelessWidget {
  final onTap;
  final bool isLiked;

  LikeButton({super.key, this.onTap, required this.isLiked});

  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    return GestureDetector(
      onTap: onTap,
      child: Icon(isLiked ? Icons.favorite : Icons.favorite_outline, color: isLiked ? Colors.red : Colors.black87, size: 36)

    );
  }
}
