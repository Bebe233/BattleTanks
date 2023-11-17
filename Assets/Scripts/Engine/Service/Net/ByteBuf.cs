using System;
using System.IO;
using BEBE.Engine.Math;

namespace BEBE.Engine.Service.Net
{
    public class ByteBuf
    {
        public int Length => data == null ? 0 : data.Length;
        protected byte[] data;
        public byte[] Data => data;
        public ByteBuf(byte[] buffer)
        {
            data = buffer;
        }

        public ByteBuf(int capacity)
        {
            data = new byte[capacity];
        }

        public ByteBuf()
        {
            data = new byte[4];
        }

        protected int srcReadIdx = 0, srcWriteIdx = 0;

        public void SetReadIndex(int idx)
        {
            srcReadIdx = idx;
        }
        public void ResetReadIndex() { srcReadIdx = 0; }
        public int GetReadIndex()
        {
            return srcReadIdx;
        }

        public void SetWriteIndex(int idx)
        {
            srcWriteIdx = idx;
        }
        public void ResetWriteIndex() { srcWriteIdx = 0; }
        public int GetWriteIndex()
        {
            return srcWriteIdx;
        }

        public byte[] ReadBytes()
        {
            int length = Length - srcReadIdx;
            if (length > 0)
                return ReadBytes(length);
            else
                return null;
        }

        public byte[] ReadBytes(int length)
        {
            if (length <= 0) return null;
            byte[] res = new byte[length];
            Buffer.BlockCopy(data, srcReadIdx, res, 0, length);
            srcReadIdx += length;
            return res;
        }

        public byte ReadByte()
        {
            return data[srcReadIdx++];
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4));
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8));
        }

        public LFloat ReadLFloat()
        {
            return ReadInt().ToLFloat();
        }

        public void WriteBytes(BinaryReader reader)
        {
            int index = reader.Read(data, 0, sizeof(int));
            if (index == 0) return;
            SetWriteIndex(index);
            int length = ReadInt();
            if (length == 0) return;
            resizeIfNeeded(length);
            index = reader.Read(data, index, length);
            SetWriteIndex(index);
        }

        public void WriteBytes(byte[] src)
        {
            if (src != null)
                WriteBytes(src, 0, src.Length);
        }

        public void WriteBytes(byte[] src, int srcIdx, int length)
        {
            resizeIfNeeded(length);
            Buffer.BlockCopy(src, srcIdx, data, srcWriteIdx, length);
            srcWriteIdx += length;
        }

        public void ResizeIfNeeded(int length) { resizeIfNeeded(length); }
        private void resizeIfNeeded(int length)
        {
            int remain = Length - srcWriteIdx;
            if (remain < length)
            {
                int need = length - remain;
                Array.Resize(ref data, Length + need);
            }
        }

        public void WriteByte(byte val)
        {
            resizeIfNeeded(sizeof(byte));
            data[srcWriteIdx++] = val;
        }

        public void WriteInt(int val)
        {
            resizeIfNeeded(sizeof(int));
            WriteBytes(BitConverter.GetBytes(val), 0, sizeof(int));
        }

        public void WriteLong(long val)
        {
            resizeIfNeeded(sizeof(long));
            WriteBytes(BitConverter.GetBytes(val), 0, sizeof(long));
        }

        public void WriteString(string msg)
        {
            resizeIfNeeded(msg.Length);
            WriteBytes(System.Text.Encoding.UTF8.GetBytes(msg));
        }

        public void WriteLFloat(LFloat val)
        {
            resizeIfNeeded(LFloat.size);
            WriteInt(val.val);
        }
    }
}