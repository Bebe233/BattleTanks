namespace BEBE.Framework.Service.Net.Msg
{
    public static class EventMsgExtension
    {
        public static EventMsg ToEventMsg(this object value)
        {
            return value as EventMsg;
        }
    }
}