namespace SimpleMVVMWpf.Models
{
    using SimpleMVVMWpf.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ChatModel : BaseModel
    {
        public enum EChatType
        {
            NONE,
            Normal,
            Photo,
            Video
        }

        public long idx { get; set; }
        public string user_idx { get; set; }
        public long room_idx { get; set; }
        public DateTime created_at { get; set; }
        public string msg { get; set; }
        public string read_user_idx { get; set; }
        public string chat_type { get; set; }
        public int total_user_count { get; set; }
        public byte[] photo { get; set; } = null;
        public string File { get; set; }
        public string note { get; set; }
        public string video { get; set; }
        public bool MINE { get; set; } = false;

        public EChatType ChatType { get; set; }
    }
}
