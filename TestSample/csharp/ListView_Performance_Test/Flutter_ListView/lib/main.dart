import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';

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
  Future<List<DataModel>>? _dataList = null;

  Future<List<DataModel>> _fetchData() async {
    print('데이터 요청');

    Client client = http.Client();
    var url = Uri.http('arong.info:7003', '/posts', null);
    final response = await client.get(url);
    final jsonMap = jsonDecode(response.body);

    final fetchDataList =
        jsonMap.map<DataModel>((json) => DataModel.fromJson(json)).toList();

    List<DataModel> dataList = [];
    // 데이터 10만개
    for (int i = 0; i < 10000; i++) {
      dataList.addAll(fetchDataList);
    }

    return dataList;
  }

  Widget _makeDataList(List<DataModel> dataList) {
    return ListView.builder(
        padding: const EdgeInsets.symmetric(horizontal: 15),
        itemCount: dataList.length,
        itemBuilder: (context, index) => Container(
            child: Card(
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16.0)),
                elevation: 4.0,
                child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Text(
                        dataList[index].userId.toString(),
                        style: const TextStyle(fontSize: 17),
                      ),
                      Text(
                        dataList[index].id.toString(),
                        style: const TextStyle(fontSize: 17),
                      ),
                      Text(
                        dataList[index].title,
                        style: const TextStyle(fontSize: 17),
                      ),
                      Text(
                        dataList[index].body,
                        style: const TextStyle(fontSize: 17),
                      )
                    ]))));
  }

  Widget _listViewWidget() {
    return FutureBuilder(
        future: _dataList,
        builder: ((context, snapshot) {
          if (snapshot.connectionState == ConnectionState.none) {
            return Container();
          } else if (snapshot.connectionState != ConnectionState.done) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasData) {
            return _makeDataList(snapshot.data ?? []);
          } else {
            return const Center(
              child: Text(
                '데이터 없음',
                style: TextStyle(fontSize: 20),
              ),
            );
          }
        }));
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
          setState(() {
            _dataList = _fetchData();
          });
        },
        tooltip: 'FetchData',
        child: const Icon(Icons.refresh),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
}
