using System;

namespace BEBE.Engine.Service.Net.Utils
{
    public class BytesHelpper
    {
        public static byte[] byte2bytes(byte val)
        {
            return new byte[] { val };
        }

        public static byte bytes2byte(byte[] val)
        {
            return val[0];
        }

        public static byte[] long2bytes(long val)
        {
            return BitConverter.GetBytes(val);
        }

        public static long bytes2long(byte[] val)
        {
            return BitConverter.ToInt64(val);
        }
    }
}