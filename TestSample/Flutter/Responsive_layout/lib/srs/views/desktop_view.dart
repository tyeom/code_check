import 'package:flutter/material.dart';

class DesktopView extends StatefulWidget {
  const DesktopView({super.key});

  @override
  State<DesktopView> createState() => _DesktopViewState();
}

class _DesktopViewState extends State<DesktopView> {
  // 경계선
  Widget _line() {
    return Container(
      height: 1,
      margin: const EdgeInsets.symmetric(horizontal: 15),
      color: Colors.grey.withOpacity(0.3),
    );
  }

  Widget _bodyWidget() {
    return Row(
      children: [
        // left
        SizedBox(
          width: 350,
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                Container(
                    alignment: Alignment.topRight,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        CircleAvatar(
                          radius: 100,
                          backgroundImage:
                              Image.asset("assets/images/13028129.jpg").image,
                        ),
                        const SizedBox(
                          height: 20,
                        ),
                        const Text(
                          "tyeom",
                          style: TextStyle(
                              fontSize: 17, fontWeight: FontWeight.bold),
                        ),
                        const SizedBox(
                          height: 20,
                        ),
                        ElevatedButton(
                            onPressed: () {},
                            style: ElevatedButton.styleFrom(
                                minimumSize: const Size(300, 50)),
                            child: const Text(
                              'Edit profile',
                              style: TextStyle(
                                  fontSize: 12, fontWeight: FontWeight.bold),
                            )),
                      ],
                    )),
                const SizedBox(
                  height: 20,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: const [
                    Icon(Icons.people),
                    SizedBox(width: 5),
                    Text(
                      '30',
                      style: TextStyle(fontWeight: FontWeight.bold),
                    ),
                    SizedBox(width: 5),
                    Text('followers'),
                    SizedBox(width: 5),
                    Text('·'),
                    SizedBox(width: 5),
                    Text(
                      '9',
                      style: TextStyle(fontWeight: FontWeight.bold),
                    ),
                    SizedBox(width: 5),
                    Text('following'),
                  ],
                ),
                const SizedBox(
                  height: 20,
                ),
                _line(),
                const SizedBox(
                  height: 20,
                ),
                Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Container(
                      color: Colors.deepPurple[300],
                      height: 120,
                    )),
                const SizedBox(
                  height: 20,
                ),
                _line(),
                const SizedBox(
                  height: 20,
                ),
                Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Container(
                      color: Colors.deepPurple[300],
                      height: 120,
                    )),
              ],
            ),
          ),
        ),

        // right
        Expanded(
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.all(16.0),
                child: SizedBox(
                  height: 70,
                  child: Row(
                    children: [
                      TextButton(
                          onPressed: () {},
                          child: const Text(
                            'Overview',
                            style: TextStyle(fontSize: 14),
                          )),
                      TextButton(
                          onPressed: () {},
                          child: const Text(
                            'Repositories',
                            style: TextStyle(fontSize: 14),
                          )),
                      TextButton(
                          onPressed: () {},
                          child: const Text(
                            'Projects',
                            style: TextStyle(fontSize: 14),
                          )),
                      TextButton(
                          onPressed: () {},
                          child: const Text(
                            'Packages',
                            style: TextStyle(fontSize: 14),
                          )),
                      TextButton(
                          onPressed: () {},
                          child: const Text(
                            'Stars',
                            style: TextStyle(fontSize: 14),
                          )),
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
          children: const [
            Icon(Icons.hub),
            Padding(
              padding: EdgeInsets.only(left: 10),
              child: Text(
                "Git Hub",
                style: TextStyle(fontSize: 17, fontWeight: FontWeight.bold),
              ),
            ),
          ],
        ),
        actions: [
          IconButton(onPressed: () {}, icon: const Icon(Icons.notifications)),
          IconButton(onPressed: () {}, icon: const Icon(Icons.add)),
          IconButton(onPressed: () {}, icon: const Icon(Icons.person)),
        ],
      ),
      body: _bodyWidget(),
    );
  }
}
