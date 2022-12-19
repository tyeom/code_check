import 'dart:io';
import 'dart:isolate';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';

class ComputeTest extends StatefulWidget {
  const ComputeTest({super.key});

  @override
  State<ComputeTest> createState() => _ComputeTestState();
}

class _ComputeTestState extends State<ComputeTest> {
  String? _taskResult;

  // 오래걸리는 작업 함수 [Isolate의 endpoint 함수는 최상위(Top-level) 함수이거나 static함수여야 한다., Compute도 동일]
  static String _expensiveTask(int sec) {
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
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: const Text(
          'Compute Test',
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
                    _taskResult = _expensiveTask(5);
                  });
                },
                child: const Text(
                  'Start task with Sync',
                  style: TextStyle(fontSize: 17),
                )),

            const SizedBox(height: 10),

            // Compute로 작업 시작
            ElevatedButton(
                onPressed: () async {
                  setState(() {
                    _taskResult = '';
                  });

                  final taskResult = await compute(_expensiveTask, 5);

                  setState(() {
                    _taskResult = taskResult;
                  });
                },
                child: const Text(
                  'Start task with Compute',
                  style: TextStyle(fontSize: 17),
                )),

            const SizedBox(height: 10),

            // 작업 취소 - Compute는 작업 취소를 지원하지 않는다.

            // ElevatedButton(
            //     onPressed: () {
            //       _isolate.kill(priority: Isolate.immediate);

            //       setState(() {
            //         _taskResult = 'Task canceled';
            //       });
            //     },
            //     child: const Text(
            //       'Cancel task',
            //       style: TextStyle(fontSize: 17),
            //     ))
          ]),
        ));
  }
}
