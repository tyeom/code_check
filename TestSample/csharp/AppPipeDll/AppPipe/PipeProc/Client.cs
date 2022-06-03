namespace AppPipe.PipeProc
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Pipes;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    class Client : PipeProvider
    {
        private Stream _stream;
        private SafePipeHandle _handle;
        private bool _connected = false;

        public Client(string pipeName) :
            base(pipeName)
        {

        }

        public bool Connected
        {
            get { return _connected; }
        }

        public override void Connect()
        {
            try
            {
                NamedPipeClientStream pipeClient =
                        new NamedPipeClientStream(".", base.PipeName,
                            PipeDirection.InOut, PipeOptions.Asynchronous,
                            TokenImpersonationLevel.Impersonation);

                pipeClient.Connect(300);

                // 서버가 실행되지 않음, 핸들 생성 오류
                if (pipeClient.SafePipeHandle.IsInvalid)
                {
                    Debug.WriteLine("could not create handle - server probably not running");
                    //throw new Exception("could not create handle - server probably not running");
                    return;
                }

                _connected = true;
                _stream = pipeClient;
                _handle = pipeClient.SafePipeHandle;

                Task.Factory.StartNew(() =>
                {
                    this.Read();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Connect() - {0}", ex.Message));
            }
        }

        private void Read()
        {
            while (_connected)
            {
                int bytesRead = 0;

                try
                {
                    bytesRead = _stream.ReadByte() * 256;
                    bytesRead += _stream.ReadByte();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(string.Format("AppPipe Error : [Client -> Read()]"));
                    Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
                    break;
                }

                // 서버 연결 해제됨.
                if (bytesRead <= 0)
                {
                    _connected = false;
                    Debug.WriteLine(string.Format("AppPipe Debug : [Client -> Read()]"));
                    Debug.WriteLine(string.Format("AppPipe Debug : server has disconnected"));
                    break;
                }

                // 수신 메세지 처리
                StreamString streamString = new StreamString(_stream);
                string message = streamString.ReadString(bytesRead);
                MessageData messageData = SerializeHelper.DeserializeByDataContractSerializer<MessageData>(message);

                RemoteMessage remoteMessage = new RemoteMessage(messageData);
                base.MessagingService.MessageReceived(this, remoteMessage);
            }

            _stream.Close();
            _handle.Close();
        }

        /*
        public override void SendMessage(string message)
        {
            if (_connected == false)
            {
                this.Connect();
            }

            if (_connected == false) return;

            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                byte[] messageBuffer = encoder.GetBytes(message);

                _stream.Write(messageBuffer, 0, messageBuffer.Length);
                _stream.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("AppPipe Error : [Client -> SendMessage()]"));
                Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
            }
        }
        */

        public override void SendMessage(MessageData messageData)
        {
            if (_connected == false)
            {
                this.Connect();
            }

            if (_connected == false) return;

            try
            {
                string strData = SerializeHelper.SerializeByDataContractSerializer<MessageData>(messageData);

                StreamString streamString = new StreamString(_stream);
                streamString.WriteString(strData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("AppPipe Error : [Client -> SendMessage()]"));
                Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
            }
        }

        public override void Close()
        {
            _stream.Close();
        }
    }
}
