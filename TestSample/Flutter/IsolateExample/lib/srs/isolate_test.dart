import 'dart:io';
import 'dart:isolate';

import 'package:flutter/material.dart';

class IsolateTest extends StatefulWidget {
  const IsolateTest({super.key});

  @override
  State<IsolateTest> createState() => _IsolateTestState();
}

// 오래걸리는 작업 함수 [Isolate의 endpoint 함수는 최상위(Top-level) 함수이거나 static함수여야 한다., Compute도 동일]
void expensiveTask(List<Object> param) {
  int sec = param[0] as int;
  SendPort sendPort = param[1] as SendPort;

  sleep(Duration(seconds: sec));
  sendPort.send("Completion");
}

class _IsolateTestState extends State<IsolateTest> {
  String? _taskResult;
  late Isolate _isolate;
  final _receivePort = ReceivePort();

  @override
  void initState() {
    super.initState();

    _receivePort.listen((message) {
      setState(() {
        _taskResult = message.toString();
      });
    });
  }

  String _expensiveTask_Sync(int sec) {
    sleep(Duration(seconds: sec));
    return "Completion";
  }

  Widget _taskResultWidget() {
    if (_taskResult == null) {
      return Container();
    } else if (_taskResult != null && _taskResult.toString().isEmpty) {
      return const CircularProgressIndicator();
    } else {
      return Text(
        '결과 : $_taskResult',
        style: const TextStyle(fontSize: 25),
      );
    }
  }

  @override
  void dispose() {
    _receivePort.close();
    _isolate.kill(priority: Isolate.immediate);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: const Text(
          'IsolateTest',
          style: TextStyle(fontSize: 20),
        )),
        body: Center(
          child: Column(children: [
            const SizedBox(height: 10),

            _taskResultWidget(),

            const SizedBox(height: 10),

            // 일반 동기로 작업 시작
            ElevatedButton(
                onPressed: () async {
                  setState(() {
                    _taskResult = '';
                    _taskResult = _expensiveTask_Sync(5);
                  });
                },
                child: const Text(
                  'Start task with Sync',
                  style: TextStyle(fontSize: 17),
                )),

            const SizedBox(height: 10),

            // Isolate로 작업 시작
            ElevatedButton(
                onPressed: () async {
                  setState(() {
                    _taskResult = '';
                  });

                  _isolate = await Isolate.spawn<List<Object>>(
                      expensiveTask, [5, _receivePort.sendPort]);
                },
                child: const Text(
                  'Start task with Isolate',
                  style: TextStyle(fontSize: 17),
                )),

            const SizedBox(height: 10),

            // 작업 취소 (Isolate로)
            ElevatedButton(
                onPressed: () {
                  _isolate.kill(priority: Isolate.immediate);

                  setState(() {
                    _taskResult = 'Task canceled';
                  });
                },
                child: const Text(
                  'Cancel task',
                  style: TextStyle(fontSize: 17),
                ))
          ]),
        ));
  }
}
