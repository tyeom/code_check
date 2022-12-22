import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:get/get.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';

import 'data_controller.dart';
import 'data_model.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  final DataController _dataController =
      Get.put<DataController>(DataController());

  Widget _makeDataList() {
    return ListView.builder(
        controller: _dataController.scrollController.value,
        padding: const EdgeInsets.symmetric(horizontal: 15),
        itemCount: _dataController.dataList.length + 1,
        itemBuilder: (context, index) {
          if (index < _dataController.dataList.length) {
            return Container(
                child: Card(
                    shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(16.0)),
                    elevation: 4.0,
                    child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: [
                          Text(
                            _dataController.dataList[index].userId.toString(),
                            style: const TextStyle(fontSize: 17),
                          ),
                          Text(
                            _dataController.dataList[index].id.toString(),
                            style: const TextStyle(fontSize: 17),
                          ),
                          Text(
                            _dataController.dataList[index].title,
                            style: const TextStyle(fontSize: 17),
                          ),
                          Text(
                            _dataController.dataList[index].body,
                            style: const TextStyle(fontSize: 17),
                          )
                        ])));
          } else if (_dataController.dataList.length <= 0) {
            return const Center(
              child: Text(
                '데이터 없음',
                style: TextStyle(fontSize: 20),
              ),
            );
          }

          return Center(child: CircularProgressIndicator());
        });
  }

  Widget _listViewWidget() {
    return Obx(() {
      if (_dataController.isDataFetching.isTrue) {
        return Center(child: RefreshProgressIndicator());
      } else {
        return _makeDataList();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'ListView Performance test',
              style: TextStyle(fontSize: 30, fontWeight: FontWeight.bold),
            ),
            Expanded(child: _listViewWidget()),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          _dataController.dataFetching(true);
          _dataController.fetchDataList();
        },
        tooltip: 'FetchData',
        child: const Icon(Icons.refresh),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
}
