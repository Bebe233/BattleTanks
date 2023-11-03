using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BEBE.Framework.Utils
{
    public class ByteHelpper
    {
        public static int Write(byte[] src, int srcIndex, byte[] dst, int dstIndex, int length)
        {
            System.Array.Copy(src, srcIndex, dst, dstIndex, length);
            return dstIndex + length;
        }

        public static int WriteByte(byte src, byte[] dst, int dstIndex)
        {
            dst[dstIndex++] = src;
            return dstIndex;
        }



        public static byte[] Read(byte[] src, ref int srcIndex, int length)
        {
            byte[] res = new byte[length];
            System.Array.Copy(src, srcIndex, res, 0, length);
            srcIndex += length;
            return res;
        }

        public static byte ReadByte(byte[] src, ref int srcIndex)
        {
            return Read(src, ref srcIndex, 1)[0];
        }

        public static int ReadInt(byte[] src, ref int srcIndex)
        {
            return BitConverter.ToInt32(Read(src, ref srcIndex, 4));
        }

    }
}