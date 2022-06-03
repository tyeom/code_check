namespace AppPipe
{
    using System;

    public sealed class RemoteMessage
    {
        public RemoteMessage(MessageData messageData)
        {
            MessageData = messageData;
        }

        public MessageData MessageData { get; set; }
    }
}
