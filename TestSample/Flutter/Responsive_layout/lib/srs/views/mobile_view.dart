import 'package:flutter/material.dart';
import 'package:flutter_speed_dial/flutter_speed_dial.dart';

class MobileView extends StatefulWidget {
  const MobileView({super.key});

  @override
  State<MobileView> createState() => _MobileViewState();
}

class _MobileViewState extends State<MobileView> {
  // 경계선
  Widget _line() {
    return Container(
      height: 1,
      margin: const EdgeInsets.symmetric(horizontal: 15),
      color: Colors.grey.withOpacity(0.3),
    );
  }

  Widget _bodyWidget() {
    return Column(
      children: [
        // left
        SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: Padding(
            padding: const EdgeInsets.only(left: 20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const SizedBox(
                  height: 20,
                ),
                Row(
                  children: [
                    CircleAvatar(
                      radius: 30,
                      backgroundImage:
                          Image.asset("assets/images/13028129.jpg").image,
                    ),
                    const Padding(
                      padding: EdgeInsets.only(left: 20),
                      child: Text(
                        "tyeom",
                        style: TextStyle(
                            fontSize: 17, fontWeight: FontWeight.bold),
                      ),
                    ),
                  ],
                ),
                const SizedBox(
                  height: 25,
                ),
                const Text(
                  ".Net developer (C# / WPF,ASP.NET Core) Node.js developer",
                  style: TextStyle(fontSize: 13),
                ),
                const SizedBox(
                  height: 20,
                ),
                ElevatedButton(
                    onPressed: () {},
                    style: ElevatedButton.styleFrom(
                        minimumSize: const Size.fromHeight(40)),
                    child: const Text(
                      'Edit profile',
                      style:
                          TextStyle(fontSize: 12, fontWeight: FontWeight.bold),
                    )),
                const SizedBox(
                  height: 20,
                ),
                _line(),
                const SizedBox(
                  height: 20,
                ),
                Wrap(
                  spacing: 10,
                  runSpacing: 10,
                  children: [
                    'YOLO',
                    'PullShark',
                    'QuickDraw',
                    'Pair',
                    'Starstruck',
                    'ArcticCode'
                  ]
                      .map(
                        (item) => CircleAvatar(
                          radius: 30,
                          backgroundColor: Colors.lightGreen,
                          child: Text(
                            item,
                            style: const TextStyle(fontSize: 12),
                            textAlign: TextAlign.center,
                          ),
                        ),
                      )
                      .toList(),
                ),
                const SizedBox(
                  height: 20,
                ),
                _line(),
                const SizedBox(
                  height: 20,
                ),
              ],
            ),
          ),
        ),

        Expanded(
          child: ListView.separated(
            itemCount: 10,
            padding: const EdgeInsets.symmetric(horizontal: 10),
            physics: const ClampingScrollPhysics(), // bounce 효과 제거
            itemBuilder: (context, index) {
              return Padding(
                padding: const EdgeInsets.all(8.0),
                child: Container(
                  color: Colors.deepPurple[300],
                  height: 120,
                ),
              );
            },
            separatorBuilder: (_, __) {
              return _line();
            },
          ),
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Row(
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            SpeedDial(
              switchLabelPosition: true,
              icon: Icons.menu,
              activeIcon: Icons.close,
              spacing: 3,
              direction: SpeedDialDirection.down,
              children: [
                SpeedDialChild(
                    onTap: () {
                      print('Dashboard');
                    },
                    label: 'Dashboard'),
                SpeedDialChild(
                    onTap: () {
                      print('Pull requests');
                    },
                    label: 'Pull requests'),
                SpeedDialChild(
                    onTap: () {
                      print('Issues');
                    },
                    label: 'Issues'),
                SpeedDialChild(
                    onTap: () {
                      print('Codespace');
                    },
                    label: 'Codespace'),
                SpeedDialChild(
                    onTap: () {
                      print('Marketplace');
                    },
                    label: 'Marketplace'),
                SpeedDialChild(
                    onTap: () {
                      print('Explore');
                    },
                    label: 'Explore'),
                SpeedDialChild(
                    onTap: () {
                      print('Sponsors');
                    },
                    label: 'Sponsors'),
                SpeedDialChild(
                    onTap: () {
                      print('Settings');
                    },
                    label: 'Settings'),
              ],
            ),
            Expanded(
              child: Container(
                  alignment: Alignment.center, child: const Icon(Icons.hub)),
            ),
          ],
        ),
        actions: [
          IconButton(onPressed: () {}, icon: const Icon(Icons.notifications)),
        ],
      ),
      body: _bodyWidget(),
      //floatingActionButtonLocation: FloatingActionButtonLocation.startTop,
    );
  }
}
