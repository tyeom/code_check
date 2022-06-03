namespace AppPipe
{
    using AppPipe.PipeProc;
    using System;

    public abstract class MessagingService
    {
        public abstract void MessageReceived(PipeProvider sender, RemoteMessage remoteMessage);
    }
}
