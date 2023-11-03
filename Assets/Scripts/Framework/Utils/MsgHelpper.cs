using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEBE.Framework.Event;
using BEBE.Framework.Managers;
using UnityEngine;

namespace BEBE.Framework.Utils
{
    //在发送消息时根据消息类型对数据进行封装
    //约定 第0位 为消息类型标记位
    //0 表示 EventCode
    //1 表示 字符串
    //2 表示 BinaryData
    public class MsgHelpper
    {
        public static byte[] EncodeMsgBuffer(string msg)
        {
            //前四位（Int）保存消息长度
            int length = msg.Length + 1;
            byte[] b_length = BitConverter.GetBytes(length);
            byte[] buf = new byte[b_length.Length + length];
            int writePos = ByteHelpper.Write(b_length, 0, buf, 0, b_length.Length);
            //消息类型
            buf[writePos++] = ((byte)MsgType.String);
            ByteHelpper.Write(Encoding.UTF8.GetBytes(msg), 0, buf, writePos, msg.Length);
            return buf;
        }

        public static byte[] EncodeEventCodeBuffer(EventCode eCode)
        {
            return bitpacker(eCode, ParamType.NONE, null);
        }

        public static byte[] EncodeEventCodeBuffer(EventCode eCode, int param)
        {
            return bitpacker(eCode, ParamType.INT, BitConverter.GetBytes(param));
        }

        public static byte[] EncodeEventCodeBuffer(EventCode eCode, byte param)
        {
            return bitpacker(eCode, ParamType.BYTE, new byte[1] { param });
        }

        private static byte[] bitpacker(EventCode eCode, ParamType pType, byte[] param)
        {
            if (param != null && param.Length > 0) // 有参
            {
                int length = param.Length + 3;
                byte[] b_length = BitConverter.GetBytes(length);
                byte[] buf = new byte[b_length.Length + length];
                int writePos = ByteHelpper.Write(b_length, 0, buf, 0, b_length.Length);
                writePos = ByteHelpper.WriteByte(((byte)MsgType.EventCode), buf, writePos);
                writePos = ByteHelpper.WriteByte(((byte)eCode), buf, writePos);
                writePos = ByteHelpper.WriteByte(((byte)pType), buf, writePos);//参数类型
                ByteHelpper.Write(param, 0, buf, writePos, param.Length);
                return buf;
            }
            else  //无参
            {
                int length = 3;
                byte[] b_length = BitConverter.GetBytes(length);
                byte[] buf = new byte[b_length.Length + length];
                int writePos = ByteHelpper.Write(b_length, 0, buf, 0, b_length.Length);
                writePos = ByteHelpper.WriteByte(((byte)MsgType.EventCode), buf, writePos);
                writePos = ByteHelpper.WriteByte(((byte)eCode), buf, writePos);
                writePos = ByteHelpper.WriteByte(((byte)pType), buf, writePos);//参数类型
                return buf;
            }
        }

        /***************************************************************/

        public static string DecodeMsgBuffer(byte[] buffer, int length)
        {
            return Encoding.UTF8.GetString(buffer, 5, length - 5);
        }

        public static void DecodeEventCodeBuffer(byte[] buffer, int length)
        {
            //EventCode
            int index = 5;
            EventCode eCode = (EventCode)ByteHelpper.ReadByte(buffer, ref index);
            ParamType pType = (ParamType)ByteHelpper.ReadByte(buffer, ref index);
            // Debug.Log($"EVENT {eCode.ToString()} {pType.ToString()}");
            if (length > 7) //有参
            {
                switch (pType)
                {
                    case ParamType.BYTE:
                        byte param = ByteHelpper.ReadByte(buffer, ref index);
                        DispatchMgr.Dispatch(eCode, param);
                        break;
                }
            }
            else
            {
                DispatchMgr.Dispatch(eCode, null);
            }
        }
    }
}