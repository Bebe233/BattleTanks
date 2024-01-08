namespace BEBE.Engine.Service.Net
{
    public abstract class BaseMsg
    {
        protected byte flag;
        public MsgType Flag => (MsgType)flag;
        protected int id = -1; //客户端id 省缺值为-1
        public int Id => id;
        protected int len_payload; //消息长度
    }
}