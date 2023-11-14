using BEBE.Framework.Math.BaseType;

namespace BEBE.Framework.Math
{
    public static partial class LMathExtension
    {
        public static LFloat ToLFloat(this float val)
        {
            return new LFloat(true, (int)(val * LFloat.Precision));
        }

        public static LFloat ToLFloat(this int val)
        {
            return new LFloat(val);
        }

        public static LFloat ToLFloat(this long val)
        {
            return new LFloat(val);
        }
    }
}