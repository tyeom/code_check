import 'package:flutter/material.dart';
import 'package:responsive_layout/srs/views/desktop_view.dart';
import 'package:responsive_layout/srs/views/app_view.dart';
import 'package:responsive_layout/srs/views/mobile_view.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData.dark(useMaterial3:  true),
      home: const AppView(
        desktopView: DesktopView(),
        mobileView: MobileView(),
      ),
    );
  }
}