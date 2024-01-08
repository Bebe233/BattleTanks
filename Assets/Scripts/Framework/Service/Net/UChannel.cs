using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using BEBE.Engine.Logging;
using System.Net.Sockets;

namespace BEBE.Framework.Service.Net
{
    public class UChannel : Channel
    {
        public UChannel(string ip_address, int port) : base(ip_address, port)
        {
        }
        public UChannel(int id, TcpClient accpet) : base(id, accpet)
        {

        }

        protected override void on_recieve_packet(Packet packet)
        {
            // //前4位 int类型 存储消息长度
            // //约定 第5位 为消息类型标记位
            // //0 表示 EventCode
            // //1 表示 字符串
            // //2 表示 BinaryData
            switch (packet.MsgType)
            {
                case MsgType.EventCode:
                    var event_msg = EventPacket.Wrap(packet).ParseEventMsg();
                    // Log($"RPC EVENTCODE --> {event_msg.EventCode}");
                    Dispatchor.Dispatch(event_msg.EventCode, event_msg);
                    break;
                case MsgType.String:
                    var string_msg = StringPacket.Wrap(packet).ParseStringMsg();
                    Debug.Log($"RPC MSG --> {string_msg.Id} {string_msg.Content}");
                    break;
            }
        }
    }
}