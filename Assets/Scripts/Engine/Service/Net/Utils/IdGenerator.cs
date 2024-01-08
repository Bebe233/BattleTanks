namespace BEBE.Engine.Service.Net.Utils
{
    public class IdGenerator
    {

        public IdGenerator(int start_num = 0)
        {
            id = start_num;
        }

        private int id;

        public int Get()
        {
            return id++;
        }
    }
}