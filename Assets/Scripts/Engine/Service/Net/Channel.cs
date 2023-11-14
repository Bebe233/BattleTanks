using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static BEBE.Framework.Logging.Debug;
using System.IO;
using System.Threading;

namespace BEBE.Framework.Service.Net
{
    ///网络连接
    public class Channel : IDisposable
    {
        private TcpClient m_client = new TcpClient();
        private NetService sender;
        private string ip;
        private int port;
        public Channel(NetService sender, string ip_address, int port)
        {
            this.ip = ip_address;
            this.port = port;
            this.sender = sender;
        }

        public Channel(NetService sender, TcpClient accpet)
        {
            m_client = accpet;
            var endPoint = accpet.Client.LocalEndPoint as IPEndPoint;
            this.ip = endPoint.Address.ToString();
            this.port = endPoint.Port;
            this.sender = sender;
        }

        public async Task ConnectAsync()
        {
            await m_client.ConnectAsync(ip, port);
            LogWarning("channel connected !");
        }
        BinaryReader m_binaryReader;
        public void Recv()
        {
            try
            {
                if (!m_client.Connected) return;
                if (m_client.Available <= 0) return;
                stream = m_client.GetStream();
                if (!stream.CanRead) return;
                m_binaryReader = new BinaryReader(stream);
                Packet packet = new Packet(m_binaryReader);
                on_recieve_packet(packet);
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }
        private void on_recieve_packet(Packet packet)
        {
            //前4位 int类型 存储消息长度
            //约定 第5位 为消息类型标记位
            //0 表示 EventCode
            //1 表示 字符串
            //2 表示 BinaryData
            switch (packet.MsgType)
            {
                case MsgType.EventCode:
                    packet.DecodeEventCode(sender);
                    break;
                case MsgType.String:
                    Log($"RPC MSG --> {packet.DecodeString()}");
                    break;
                case MsgType.Json: break;
            }
        }

        NetworkStream stream;
        BinaryWriter m_binaryWriter;
        public void Send(Packet packet)
        {
            if (!m_client.Connected) return;
            stream = m_client.GetStream();
            if (!stream.CanWrite) return;
            m_binaryWriter = new BinaryWriter(stream);
            m_binaryWriter.Write(packet.Buffer.Data);
            m_binaryWriter.Flush();
        }

        public void Dispose()
        {
            m_client.Close();
            m_client.Dispose();
            LogWarning("channel disconnected !");
        }
    }
}