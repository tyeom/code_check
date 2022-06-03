namespace AppPipe
{
    using System;
    using System.IO;
    using System.Text;

    internal class StreamString
    {
        private Stream _ioStream;
        private UTF8Encoding _streamEncoding;

        public StreamString(Stream ioStream)
        {
            _ioStream = ioStream;
            _streamEncoding = new UTF8Encoding();
        }

        public string ReadString(int bytesRead)
        {
            byte[] inBuffer = new byte[bytesRead];
            _ioStream.Read(inBuffer, 0, bytesRead);

            return _streamEncoding.GetString(inBuffer);
        }

        public string ReadString()
        {
            int len = 0;

            len = _ioStream.ReadByte() * 256;
            len += _ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            _ioStream.Read(inBuffer, 0, len);

            return _streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = _streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            _ioStream.WriteByte((byte)(len / 256));
            _ioStream.WriteByte((byte)(len & 255));
            _ioStream.Write(outBuffer, 0, len);
            _ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
