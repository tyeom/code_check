namespace AppPipe
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PacketData
    {
        public int PacketType;
        public int DataPacketLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)] 
        public byte[] DataPacket;
    }
}
