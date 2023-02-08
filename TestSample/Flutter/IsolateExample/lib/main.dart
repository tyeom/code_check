import 'package:flutter/material.dart';
import 'package:isolate_example/srs/compute_test.dart';
import 'package:isolate_example/srs/isolate_test.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(title: 'Flutter Isolate example'),
    );
  }
}

class MyHomePage extends StatelessWidget {
  final String title;
  const MyHomePage({required this.title, super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            ElevatedButton(
                child: const Text(
                  'Isolate Test',
                  style: TextStyle(fontSize: 20),
                ),
                onPressed: () => Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: ((context) => const IsolateTest())))),
            const SizedBox(
              height: 20,
            ),
            ElevatedButton(
                child: const Text(
                  'Compute Test',
                  style: TextStyle(fontSize: 20),
                ),
                onPressed: () => Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: ((context) => const ComputeTest()))))
          ],
        ),
      ),
    );
  }
}
