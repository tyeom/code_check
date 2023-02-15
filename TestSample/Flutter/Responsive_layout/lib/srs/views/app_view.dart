import 'package:flutter/material.dart';

class AppView extends StatelessWidget {
  final Widget mobileView;
  final Widget desktopView;
  static const int _maxWidth = 900;

  const AppView({Key? key, required this.mobileView, required this.desktopView})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(builder: (context, constraints) {
      if (constraints.maxWidth < _maxWidth) {
        return mobileView;
      } else {
        return desktopView;
      }
    });
  }
}
