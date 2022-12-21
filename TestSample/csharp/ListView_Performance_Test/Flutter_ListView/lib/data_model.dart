class DataModel {
  final int userId;
  final int id;
  final String title;
  final String body;

  DataModel(
      {required this.userId,
      required this.id,
      required this.title,
      required this.body});

  factory DataModel.fromJson(Map<String, dynamic> json) => DataModel(
      userId: json['userId'] as int,
      id: json['id'] as int,
      title: json['title'],
      body: json['body']);
}
