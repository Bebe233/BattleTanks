namespace BEBE.Engine.Service.Net.Utils
{
    public class IdGenerator
    {
        private static int id = -1;

        public static int Get()
        {
            return ++id;
        }
    }
}