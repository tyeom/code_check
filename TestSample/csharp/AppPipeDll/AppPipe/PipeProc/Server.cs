namespace AppPipe.PipeProc
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Pipes;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

    class Server : PipeProvider
    {
        private List<Client> _clients;
        private bool _running = false;
        private const int _MAX_CONNECT_CLIENT_COUNT_ = 2;

        public Server(string pipeName) :
            base(pipeName)
        {
            base._pipeType = PipeFactory.PipeType.Server;
            _clients = new List<Client>();
        }

        public override void Start()
        {
            Task.Factory.StartNew(() =>
            {
                this.ListenForClients();
            });
        }

        private void ListenForClients()
        {
            if (_clients.Count > _MAX_CONNECT_CLIENT_COUNT_) return;

            if (_clients.Count <= 0)
                _running = false;

            NamedPipeServerStream pipeServer =
                    new NamedPipeServerStream(base.PipeName, PipeDirection.InOut, _MAX_CONNECT_CLIENT_COUNT_, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            // 클라이언트 접속 대기
            pipeServer.WaitForConnection();

            _running = true;

            // pipe name 생성 오류
            if (pipeServer.SafePipeHandle.IsInvalid)
            {
                throw new Exception("could not create named pipe");
            }

            // 클라이언트 연결 오류
            if (pipeServer.IsConnected == false)
            {
                throw new Exception("could not connect client");
            }

            Client client = new Client();
            client.handle = pipeServer.SafePipeHandle;
            client.stream = pipeServer;

            lock (_clients)
                _clients.Add(client);

            Task.Factory.StartNew(() =>
            {
                this.Read(client);
            });
        }

        private void Read(object clientObj)
        {
            Client client = (Client)clientObj;

            while (_running)
            {
                int bytesRead = 0;

                try
                {
                    bytesRead = client.stream.ReadByte() * 256;
                    bytesRead += client.stream.ReadByte();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(string.Format("AppPipe Error : [Server -> Read()]"));
                    Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
                    break;
                }

                // 클라이언트 연결 해제됨.
                if (bytesRead <= 0)
                {
                    Debug.WriteLine(string.Format("AppPipe Debug : [Server -> Read()]"));
                    Debug.WriteLine(string.Format("AppPipe Debug : Client has disconnected"));
                    break;
                }

                // 수신 메세지 처리
                StreamString streamString = new StreamString(client.stream);
                string message = streamString.ReadString(bytesRead);
                MessageData messageData = SerializeHelper.DeserializeByDataContractSerializer<MessageData>(message);

                RemoteMessage remoteMessage = new RemoteMessage(messageData);
                base.MessagingService.MessageReceived(this, remoteMessage);
            }

            client.stream.Close();
            client.handle.Close();

            lock (_clients)
            {
                if (_clients.Contains(client))
                {
                    _clients.Remove(client);
                }
            }

            if (_clients.Count < _MAX_CONNECT_CLIENT_COUNT_)
            {
                this.ListenForClients();
            }
        }

        /*
        public override void SendMessage(object messageData)
        {
            try
            {
                lock (_clients)
                {
                    UTF8Encoding encoder = new UTF8Encoding();
                    byte[] messageBuffer = encoder.GetBytes(message);
                    foreach (Client client in _clients)
                    {
                        client.stream.Write(messageBuffer, 0, messageBuffer.Length);
                        client.stream.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("AppPipe Error : [Server -> SendMessage()]"));
                Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
            }
        }
        */

        public override void SendMessage(MessageData messageData)
        {
            try
            {
                lock (_clients)
                {
                    string strData = SerializeHelper.SerializeByDataContractSerializer<MessageData>(messageData);

                    foreach (Client client in _clients)
                    {
                        StreamString streamString = new StreamString(client.stream);
                        streamString.WriteString(strData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("AppPipe Error : [Server -> SendMessage()]"));
                Debug.WriteLine(string.Format("AppPipe Error : {0}", ex.ToString()));
            }
        }

        public override void Close()
        {
            foreach (Client client in _clients)
            {
                client.stream.Close();
            }
        }

        public class Client
        {
            public SafePipeHandle handle;
            public Stream stream;
        }
    }
}
