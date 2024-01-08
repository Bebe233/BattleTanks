using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static BEBE.Engine.Logging.Debug;

namespace BEBE.Engine.Service.Net
{
    ///网络连接
    public abstract class Channel : IDisposable
    {
        public int Id { get; set; }
        private TcpClient m_client = new TcpClient();
        private string ip;
        private int port;
        public Channel(string ip_address, int port)
        {
            this.ip = ip_address;
            this.port = port;
        }

        public Channel(int id, TcpClient accpet)
        {
            Id = id;
            m_client = accpet;
            var endPoint = accpet.Client.LocalEndPoint as IPEndPoint;
            this.ip = endPoint.Address.ToString();
            this.port = endPoint.Port;
        }

        public async Task ConnectAsync()
        {
            await m_client.ConnectAsync(ip, port);
            LogWarning("channel connected !");
        }
        private NetworkStream stream;
        private BinaryWriter m_binaryWriter;
        private BinaryReader m_binaryReader;
        public void RecieveMsg()
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
        protected abstract void on_recieve_packet(Packet packet);

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
            if (m_client.Connected)
                m_client.Close();
            m_client.Dispose();
            m_client = null;
            if (m_binaryReader != null)
            {
                m_binaryReader.Close();
                m_binaryReader.Dispose();
                m_binaryReader = null;
            }
            if (m_binaryWriter != null)
            {
                m_binaryWriter.Close();
                m_binaryWriter.Dispose();
                m_binaryWriter = null;
            }
            stream = null;
            LogWarning("channel disconnected !");
        }

    }
}