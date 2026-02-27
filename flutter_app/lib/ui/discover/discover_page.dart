import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart';

import '../../classes/helpers/api.dart';
import '../../classes/helpers/general_helper.dart';
import '../../classes/helpers/secure_storage_helper.dart';
import '../../classes/objects/animal.dart';
import '../../classes/objects/api_path.dart';
import '../../classes/objects/chat.dart';
import '../../classes/services/chat_service.dart';
import '../../colors.dart';
import '../chat/chat_page.dart';
import '../login/login_page.dart';
import '../profile/profile_page.dart';
import '../../widgets/default_appbar.dart';
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
    return Scaffold(
      appBar: DefaultAppbar(
        titleWidget: Row(
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
        additionalWidgets: [
          IconButton(onPressed: () {}, icon: Icon(Icons.notifications)),
        ],
      ),
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
                Navigator.of(
                  context,
                ).push(MaterialPageRoute(builder: (context) => DiscoverPage()));
              },
            ),
            ListTile(
              leading: Icon(Icons.account_circle),
              title: Text('Profile'),
              onTap: () {
                GeneralUtil.goToPage(context, ProfilePage());
              },
            ),
            ListTile(
              leading: Icon(Icons.chat_bubble),
              title: Text('Chat'),
              onTap: () {
                GeneralUtil.goToPage(context, ChatPage());
              },
            ),
            ListTile(
              leading: Icon(Icons.lock),
              title: Text('Log out'),
              onTap: () async {
                await SecureStorageHelper.clearSecureStorage();
                if (!context.mounted) return;
                Navigator.pushAndRemoveUntil(
                  context,
                  MaterialPageRoute(builder: (context) => LoginPage()),
                  (route) => false,
                );
              },
            ),
          ],
        ),
      ),
      body: BlocProvider(
        create: (_) => DiscoverBloc()..add(DiscoverAnimals()),
        child: BlocBuilder<DiscoverBloc, DiscoverState>(
          builder: (context, state) {
            switch (state) {
              case DiscoverAnimalsInitial():
              case DiscoverAnimalsLoading():
                return const Center(child: CircularProgressIndicator());
              case DiscoverAnimalsSuccess():
                var animals = (state as DiscoverAnimalsSuccess).animals;
                return _buildCards(animals);
              case DiscoverAnimalsFailure():
                var errorMessage =
                    (state as DiscoverAnimalsFailure).errorMessage;
                return Center(child: Text(errorMessage));
              default:
                return Container();
            }
          },
        ),
      ),
    );
  }

  Widget _buildCards(List<Animal> animals) => ListView.builder(
    padding: EdgeInsets.symmetric(horizontal: 12, vertical: 8),
    itemCount: animals.length,
    itemBuilder: (context, index) => DiscoverCard(
      name: animals[index].Name,
      age: animals[index].dateOfBirth!.year,
      description: animals[index].Description,
      userId: animals[index].UserId,
      imageKey: animals[index].animalPictureKey,
    ),
  );
}

class DiscoverCard extends StatefulWidget {
  final String name;
  final int age;
  final String description;
  final String? userId;
  final String? imageKey;
  const DiscoverCard({
    super.key,
    required this.name,
    required this.age,
    required this.description,
    this.userId,
    this.imageKey,
  });

  @override
  State<DiscoverCard> createState() => _DiscoverCardState();
}

class _DiscoverCardState extends State<DiscoverCard> {
  bool isLiked = false;

  void toggleLike() {
    setState(() {
      isLiked = !isLiked;
    });
  }

  @override
  Widget build(BuildContext context) {
    // TODO: implement build

    return GestureDetector(
      onTap: () => GeneralUtil.goToPage(context, ChatPage()),
      child: Container(
        margin: EdgeInsets.only(bottom: 12),
        width: 376,
        height: 134,
        padding: const EdgeInsets.all(16),
        decoration: ShapeDecoration(
          color: const Color(0xFFFFFCF5),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(14),
          ),
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
          spacing: 12,
          children: [
            CircleAvatar(
              radius: 30,
              backgroundColor: AppColors.avatarPlaceholder,
              backgroundImage: NetworkImage(
                API.mediaFileUrl(super.widget.imageKey!),
              ),
            ),
            Column(
              mainAxisSize: MainAxisSize.min,
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              spacing: 12,
              children: [
                Text(
                  super.widget.name,
                  style: TextStyle(
                    color: const Color(0xFF0C0C0C),
                    fontSize: 16,
                    fontFamily: 'Inter',
                    fontWeight: FontWeight.w400,
                  ),
                ),
                Text(
                  super.widget.description,
                  style: TextStyle(
                    color: const Color(0xFF7F7F7F),
                    fontSize: 12,
                    fontFamily: 'Inter',
                    fontWeight: FontWeight.w400,
                  ),
                ),
                Text(
                  super.widget.age.toString() + ' years old',
                  style: TextStyle(
                    color: const Color(0xFF7F7F7F),
                    fontSize: 12,
                    fontFamily: 'Inter',
                    fontWeight: FontWeight.w400,
                  ),
                ),
              ],
            ),
            TextButton(
              style: TextButton.styleFrom(
                backgroundColor: const Color(0xFF2A3038),
                minimumSize: Size(37, 37),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10),
                ),
              ),
              onPressed: () async {
                var response = await API.postRequest(ApiPath.chat, {
                  'userIds': [super.widget.userId],
                  'title': 'Chat about ${super.widget.name}',
                });
                Response chatIdResponse;
                bool chatCreated = false;
                if (response.statusCode == 200) {
                  var chatJson = json.decode(response.body);
                  var chatId = ChatId.fromJson(chatJson).chatId;

                  GeneralUtil.goToPage(context, ChatPage(chatId: chatId));
                } else {
                  // Handle error
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(content: Text('Failed to create chat')),
                  );
                }
              },
              child: Text('Contact ->'),
            ),
          ],
        ),
      ),
    );
  }
}

// keeping it for potential future task if we decide to add some sort of 'like' button
class LikeButton extends StatelessWidget {
  final onTap;
  final bool isLiked;

  LikeButton({super.key, this.onTap, required this.isLiked});

  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    return GestureDetector(
      onTap: onTap,
      child: Icon(
        isLiked ? Icons.favorite : Icons.favorite_outline,
        color: isLiked ? Colors.red : Colors.black87,
        size: 36,
      ),
    );
  }
}
