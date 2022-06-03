namespace AppPipe
{
    using System;
    using System.Data;
    using System.Runtime.Serialization;

    [DataContract]
    public class MessageData
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DataTable Data { get; set; }
    }
}
