using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BEBE.Framework.Event;
using BEBE.Framework.Utils;
using UnityEngine;

namespace BEBE.Framework.Service
{
    public abstract class NetService : BaseService
    {
        protected string ip_address;
        protected int port;

        public virtual void Init(string ip_address, int port)
        {
            register_events();
        }

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void Send(byte[] buffer);

        protected abstract void Recv();
    }

    public class TCPClientService : NetService
    {
        TcpClient m_client;
        BinaryWriter m_binaryWriter;
        BinaryReader m_binaryReader;
        int id;
        public int ID => id;
        public override void Init(string ip_address, int port)
        {
            base.Init(ip_address, port);
            this.ip_address = ip_address;
            this.port = port;
            m_client = new TcpClient();

        }

        public override async void Connect()
        {
            await m_client.ConnectAsync(ip_address, port);
            Debug.LogWarning("tcp client connected !");

            toggle = true;
            ThreadPool.QueueUserWorkItem(state => Recv());
        }

        public override void Disconnect()
        {
            toggle = false;
            m_client.Close();
            Debug.LogWarning("tcp client disconnected !");
        }

        bool toggle = false;
        static readonly int BUFFER_SIZE = 1024;
        byte[] buffer = new byte[BUFFER_SIZE];

        protected override void Recv()
        {
            Send(MsgHelpper.EncodeMsgBuffer("Hello Server !"));
            while (toggle)
            {
                try
                {
                    if (!m_client.Connected) continue;
                    NetworkStream stream = m_client.GetStream();
                    m_binaryReader = new BinaryReader(stream);
                    int index = 0;
                    index = m_binaryReader.Read(buffer, index, sizeof(int));
                    int length = BitConverter.ToInt32(buffer.AsMemory(0, sizeof(int)).ToArray());
                    index += m_binaryReader.Read(buffer, index, length);
                    if (index > 0) TCPCLIENT_ON_RECEIVE_MSG(buffer, index);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    toggle = false;
                }
            }
        }

        public override void Send(byte[] buffer)
        {
            if (!m_client.Connected) return;
            NetworkStream stream = m_client.GetStream();
            m_binaryWriter = new BinaryWriter(stream);
            m_binaryWriter.Write(buffer);
            m_binaryWriter.Flush();
        }

        protected void TCPCLIENT_ON_RECEIVE_MSG(byte[] buffer, int length)
        {
            MsgType mark = (MsgType)buffer[4];
            //前4位 int类型 存储消息长度
            //约定 第5位 为消息类型标记位
            //0 表示 EventCode
            //1 表示 字符串
            //2 表示 BinaryData
            switch (mark)
            {
                case MsgType.EventCode:
                    // Debug.Log($"TCPCLIENT RECV EVENTCODE ! {length}");
                    MsgHelpper.DecodeEventCodeBuffer(buffer, length);
                    break;
                case MsgType.String:
                    String msg = MsgHelpper.DecodeMsgBuffer(buffer, length);
                    Debug.Log($"TCPCLIENT RECV MSG : {msg}");
                    break;
                case MsgType.Json: break;
            }
        }

        protected void EVENT_ON_CONNECTED_SERVER(object param)
        {
            id = (byte)param;
            Debug.Log($"YOUR CLIENT ID : {id}");
        }
    }

    public class TCPServerService : NetService
    {
        TcpListener m_listenr;
        BinaryWriter m_binaryWriter;

        public override void Init(string ip_address, int port)
        {
            base.Init(ip_address, port);
            this.ip_address = ip_address;
            this.port = port;
            m_listenr = new TcpListener(IPAddress.Parse(ip_address), port);
        }

        ConcurrentDictionary<int, TcpClient> m_clients = new ConcurrentDictionary<int, TcpClient>();
        bool toggle = false;
        static readonly int BUFFER_SIZE = 1024;
        byte[] buffer = new byte[BUFFER_SIZE];
        public override void Connect()
        {
            toggle = true;
            m_listenr.Start();
            ThreadPool.QueueUserWorkItem(async state =>
            {
                byte id = 0;
                while (toggle)
                {
                    TcpClient accept = await m_listenr.AcceptTcpClientAsync();
                    m_clients.AddOrUpdate(id, accept, (id, accept) => accept);
                    Debug.LogWarning($"SERVER : new client {id} connected !");
                    //将id返回给client
                    SendOne(accept, MsgHelpper.EncodeEventCodeBuffer(Event.EventCode.ON_CONNECTED_SERVER, id));
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        int i = id++;
                        Recv(i);
                    }
                    );
                }

            });


        }

        public override void Disconnect()
        {
            toggle = false;
            m_clients.Clear();
            m_listenr.Stop();
            Debug.LogWarning("tcp server disconnected !");
        }


        public override void Send(byte[] buffer)
        {
            if (m_clients.Count < 1) return;
            foreach (var client in m_clients.Values)
            {
                SendOne(client, buffer);
            }
        }

        public void SendOne(TcpClient client, byte[] buffer)
        {
            if (!client.Connected) return;
            NetworkStream clientStream = client.GetStream();
            m_binaryWriter = new BinaryWriter(clientStream);
            m_binaryWriter.Write(buffer);
            m_binaryWriter.Flush();
        }

        protected void Recv(int id)
        {
            while (toggle)
            {
                try
                {
                    if (m_clients.TryGetValue(id, out TcpClient client))
                    {
                        if (!client.Connected) continue;
                        NetworkStream clientStream = client.GetStream();
                        BinaryReader m_binaryReader = new BinaryReader(clientStream);
                        int index = 0;
                        index = m_binaryReader.Read(buffer, index, sizeof(int));
                        int length = BitConverter.ToInt32(buffer.AsMemory(0, sizeof(int)).ToArray());
                        // Debug.Log($"C {id} index {index} length {length}");
                        index += m_binaryReader.Read(buffer, index, length);
                        if (index > 0) TCPSERVER_ON_RECEIVE_MSG(id, buffer, index);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    toggle = false;
                }
            }
        }

        protected void TCPSERVER_ON_RECEIVE_MSG(int id, byte[] buffer, int length)
        {
            MsgType mark = (MsgType)buffer[4];
            //约定 第4位 为消息类型标记位
            //0 表示 EventCode
            //1 表示 字符串
            //2 表示 BinaryData
            switch (mark)
            {
                case MsgType.EventCode: break;
                case MsgType.String:
                    String msg = MsgHelpper.DecodeMsgBuffer(buffer, length);
                    Debug.Log($"TCPSERVER RECV MSG : {msg}");
                    SendOne(m_clients[id], MsgHelpper.EncodeMsgBuffer($"Hello Client {id} !!"));
                    break;
                case MsgType.Json: break;
            }
        }

        protected override void Recv()
        {

        }
    }
}