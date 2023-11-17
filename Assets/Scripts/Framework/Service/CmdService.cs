using System.Collections.Generic;
using BEBE.Engine.Event;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;

namespace BEBE.Framework.Service
{
    public class ClientCmdService : CmdService
    {
        Cmd[] cmds_local = new Cmd[NUM_CHARACTOR];
        Cmd[] cmds_server = new Cmd[NUM_CHARACTOR];
        public ClientCmdService(NetService netService) : base(netService)
        {
        }

        public override void SendCmd(Cmd binput)
        {
            m_netservice.Send(new Packet(new EventMsg(EventCode.ON_RECV_INPUT, ((BInput)binput).GetBytes(), m_netservice.Id)));
        }
    }

    public class ServerCmdService : CmdService
    {
        private PlayerInputs m_inputs;
        public ServerCmdService(NetService netService) : base(netService)
        {
            m_inputs = new PlayerInputs(NUM_CHARACTOR);
        }

        public override void SendCmd(Cmd binput)
        {

        }

        protected void EVENT_ON_RECV_INPUT(object param)
        {
            EventMsg msg = (EventMsg)param;
            int id = msg.Id;

            //如果收到所有的cmds ,分发给client
            m_inputs.put(id, msg.Content);
            //并清空dict


        }
    }
}