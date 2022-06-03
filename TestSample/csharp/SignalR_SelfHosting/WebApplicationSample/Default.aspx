<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplicationSample.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>>SignalR Simple Chat - Self Hosting</title>
  </head>
  <body>
    <form id="ServerFrm" runat="server">
      <div>
        <input type="text" id="TxtMessage" />
        <input type="button" id="BtnSend" value="Send" />
        <input type="hidden" id="UserName" />
        <ul id="Chats"></ul>
      </div>

      <script src="Scripts/jquery-1.6.4.min.js"></script>
      <script src="Scripts/jquery.signalR-2.0.0-rc1.min.js"></script>
      <!-- 뒤에 signalr/hubs은 자동으로 무조건 붙는다. -->
      <script src="http://localhost/signalrTest/signalr/hubs"></script>

      <script type="text/javascript">
          $(function () {
            // 뒤에 signalr은 자동으로 무조건 붙는다.
            // {맵 라우팅 지정 주소}/signalr
            $.connection.hub.url = "http://localhost/signalrTest/signalr";

            var chat = $.connection.chatHubTest;
            chat.client.NewMessage = function (Cl_Name, Cl_Message) {
                var User = $('<div />').text(Cl_Name).html();
                var UserChat = $('<div />').text(Cl_Message).html();
                $('#Chats').append('<li><strong>' + User + '</strong>: ' + UserChat + '</li>');
            };

            $('#UserName').val(prompt('이름을 입력하세요!:', ''));
            $('#TxtMessage').focus();
            $.connection.hub.start().done(function () {
                $('#BtnSend').click(function () {
                    chat.server.letsChat($('#UserName').val(), $('#TxtMessage').val());
                    $('#TxtMessage').val('').focus();
                });
            });
        });
      </script>
    </form>
  </body>
</html>
