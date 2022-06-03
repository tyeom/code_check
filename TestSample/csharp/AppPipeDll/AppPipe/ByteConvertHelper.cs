namespace AppPipe
{
    using System;
    using System.Runtime.InteropServices;

    public class ByteConvertHelper
    {
        public static byte[] StructureToByte(object obj)
        {
            int datasize = Marshal.SizeOf(obj);
            IntPtr buff = Marshal.AllocHGlobal(datasize);
            Marshal.StructureToPtr(obj, buff, false);

            byte[] data = new byte[datasize];
            Marshal.Copy(buff, data, 0, datasize);
            Marshal.FreeHGlobal(buff);

            return data;
        }

        public static object ByteToStructure(byte[] data, Type type)
        {
            IntPtr buff = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, buff, data.Length);
            object obj = Marshal.PtrToStructure(buff, type);
            Marshal.FreeHGlobal(buff);

            // 마샬링된 구조체와 원래 바이트 크기 비교 체크
            if (Marshal.SizeOf(obj) != data.Length)
            {
                return null;
            }

            return obj;
        }
    }
}
