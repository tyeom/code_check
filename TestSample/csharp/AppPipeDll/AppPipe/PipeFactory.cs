namespace AppPipe
{
    using AppPipe.PipeProc;
    using System;

    public class PipeFactory
    {
        public enum PipeType
        {
            NONE,
            Client,
            Server,
        }

        public static PipeProvider CreatePipe(string pipeName, PipeType pipeType, MessagingService messagingService)
        {
            PipeProvider pipeProvider = null;
            switch (pipeType)
            {
                case PipeType.Client:
                    pipeProvider = new Client(pipeName);
                    pipeProvider.SetMessagingServiceClass(messagingService);
                    return pipeProvider;
                case PipeType.Server:
                    pipeProvider = new Server(pipeName);
                    pipeProvider.SetMessagingServiceClass(messagingService);
                    return pipeProvider;
                default:
                    throw new Exception("Unknown pipe type.");
            }
        }
    }
}
