import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';
import 'package:listview_performance_test/data_model.dart';

class DataController extends GetxController {
  // ArticleModel 모델
  final RxList<DataModel> dataList = <DataModel>[].obs;

  // ListView ScrollController
  final Rx<ScrollController> scrollController = ScrollController().obs;

  // 데이터 요청중
  RxBool isDataFetching = false.obs;

  @override
  void onInit() {
    // TODO: implement onInit
    super.onInit();

    scrollController.value.addListener(() {
      if (scrollController.value.position.pixels ==
          scrollController.value.position.maxScrollExtent) {
        fetchDataList();

        print('data length : ${dataList.length}');
      }
    });
  }

  // 데이터 요청
  void fetchDataList() async {
    print('데이터 요청');

    //await Future.delayed(Duration(seconds: 3));

    Client client = http.Client();
    var url = Uri.http('arong.info:7003', '/posts', null);
    final response = await client.get(url);
    final jsonMap = jsonDecode(response.body);

    final fetchDataList =
        jsonMap.map<DataModel>((json) => DataModel.fromJson(json)).toList();
    dataList.addAll(fetchDataList);

    dataFetching(false);
  }

  void dataFetching(bool isFetching) {
    isDataFetching(isFetching);
  }
}
