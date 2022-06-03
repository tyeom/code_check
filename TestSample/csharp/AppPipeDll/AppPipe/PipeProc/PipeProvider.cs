namespace AppPipe.PipeProc
{
    using System;

    public abstract class PipeProvider
    {
        protected PipeFactory.PipeType _pipeType = PipeFactory.PipeType.NONE;

        public PipeProvider(string pipeName)
        {
            PipeName = pipeName;
        }

        public string PipeName
        {
            private set;
            get;
        }

        protected MessagingService MessagingService
        {
            get;
            private set;
        }

        internal void SetMessagingServiceClass(MessagingService messagingService)
        {
            MessagingService = messagingService;
        }

        public virtual void Connect()
        {
            if (_pipeType == PipeFactory.PipeType.Client)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception("Client 타입에서만 사용할 수 있습니다.");
            }
        }

        public abstract void Close();

        public virtual void Start()
        {
            if (_pipeType == PipeFactory.PipeType.Server)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception("Server 타입에서만 사용할 수 있습니다.");
            }
        }

        public abstract void SendMessage(MessageData messageData);
    }
}
