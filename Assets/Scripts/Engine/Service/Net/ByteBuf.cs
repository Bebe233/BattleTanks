using System;
using System.IO;
using BEBE.Engine.Math;
using BEBE.Engine.Logging;
namespace BEBE.Engine.Service.Net
{
    public class ByteBuf
    {
        public int Capacity => data == null ? 0 : data.Length;
        protected byte[] data;
        public byte[] Data => data;
        public ByteBuf(byte[] buffer)
        {
            data = buffer;
            writer_idx = Capacity;
        }

        public ByteBuf(int capacity)
        {
            data = new byte[capacity];
        }

        public ByteBuf()
        {
            data = new byte[4];
        }

        protected int reader_idx = 0, writer_idx = 0;
        protected int mark_reader_idx, mark_writer_idx;

        public int Readablebytes => writer_idx - reader_idx;

        public int ReaderIndex()
        {
            return reader_idx;
        }
        public void ReaderIndex(int idx)
        {
            reader_idx = idx;
        }
        public void MarkReaderIndex() { mark_reader_idx = reader_idx; }
        public void ResetReaderIndex() { reader_idx = mark_reader_idx; }

        public int WriterIndex()
        {
            return writer_idx;
        }
        public void WriterIndex(int idx)
        {
            writer_idx = idx;
        }
        public void MarkWriterIndex() { mark_writer_idx = writer_idx; }
        public void ResetWriterIndex() { writer_idx = mark_writer_idx; }

        public byte[] ReadBytes()
        {
            return ReadBytes(Readablebytes);
        }

        public byte[] ReadBytes(int length)
        {
            if (length <= 0 || length > Readablebytes)
            {
                // Debug.LogError($"readbytes length is out of range ! length --> {length} , readablebytes --> {Readablebytes} , readerIdx --> {reader_idx} , writerIdx --> {writer_idx}");
                return null;
            }
            byte[] res = new byte[length];
            Buffer.BlockCopy(data, reader_idx, res, 0, length);
            reader_idx += length;
            return res;
        }

        public byte ReadByte()
        {
            return data[reader_idx++];
        }

        public bool ReadBool()
        {
            byte val = ReadByte();
            if (val == 0) return false;
            else if (val == 1) return true;
            else return false;
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
            return ReadInt().ToLFloat(true);
        }

        public string ReadString()
        {
            int len = ReadInt();
            return System.Text.Encoding.UTF8.GetString(ReadBytes(len));
        }

        public void WriteBytes(BinaryReader reader)
        {
            WriteBytes(reader, writer_idx, sizeof(int));
            int length = ReadInt();
            if (length == 0) return;
            resizeIfNeeded(length);
            WriteBytes(reader, writer_idx, length);
        }

        public void WriteBytes(BinaryReader reader, int srcIdx, int length)
        {
            int count = reader.Read(data, srcIdx, length);
            WriterIndex(srcIdx + count);
        }

        public void WriteBytes(byte[] src)
        {
            if (src != null)
                WriteBytes(src, 0, src.Length);
        }

        public void WriteBytes(byte[] src, int srcIdx, int length)
        {
            resizeIfNeeded(length);
            Buffer.BlockCopy(src, srcIdx, data, writer_idx, length);
            writer_idx += length;
        }

        private void resizeIfNeeded(int length)
        {
            int remain = Capacity - writer_idx;
            if (remain < length)
            {
                int need = length - remain;
                Array.Resize(ref data, Capacity + need);
            }
        }

        public void WriteByte(byte val)
        {
            resizeIfNeeded(sizeof(byte));
            data[writer_idx++] = val;
        }

        public void WriteBool(bool isTrue)
        {
            WriteByte(isTrue ? (byte)1 : (byte)0);
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
            byte[] chars = System.Text.Encoding.UTF8.GetBytes(msg);
            WriteInt(chars.Length);
            resizeIfNeeded(chars.Length + sizeof(int));
            WriteBytes(System.Text.Encoding.UTF8.GetBytes(msg));
        }

        public void WriteLFloat(LFloat val)
        {
            resizeIfNeeded(LFloat.size);
            WriteInt(val.val);
        }

        public void Clear()
        {
            reader_idx = writer_idx = 0;
            mark_reader_idx = mark_writer_idx = 0;
        }
    }
}