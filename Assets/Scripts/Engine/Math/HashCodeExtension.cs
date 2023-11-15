using BEBE.Engine.Math.BaseType;
using BEBE.Engine.Math.LUT;

namespace BEBE.Engine.Math
{
    public static class HashCodeExtension
    {
        public static int GetHash(this byte val, ref int idx)
        {
            return val;
        }

        public static int GetHash(this short val, ref int idx)
        {
            return val;
        }

        public static int GetHash(this int val, ref int idx)
        {
            return val;
        }
        public static int GetHash(this long val, ref int idx)
        {
            return (int)val;
        }

        public static int GetHash(this sbyte val, ref int idx)
        {
            return val;
        }

        public static int GetHash(this ushort val, ref int idx)
        {
            return val;
        }

        public static int GetHash(this uint val, ref int idx)
        {
            return (int)val;
        }

        public static int GetHash(this ulong val, ref int idx)
        {
            return (int)val;
        }

        public static int GetHash(this bool val, ref int idx)
        {
            return val ? 1 : 0;
        }
        public static int GetHash(this string val, ref int idx)
        {
            return val?.GetHashCode() ?? 0;
        }
        public static int GetHash(this LFloat val, ref int idx)
        {
            return PrimerLUT.GetPrimer(val.val);
        }

        public static int GetHash(this LVector2 val, ref int idx)
        {
            return PrimerLUT.GetPrimer(val.x.val) + PrimerLUT.GetPrimer(val.y.val) * 17;
        }

        public static int GetHash(this LVector3 val, ref int idx)
        {
            return PrimerLUT.GetPrimer(val.x.val)
                   + PrimerLUT.GetPrimer(val.y.val) * 31
                   + PrimerLUT.GetPrimer(val.z.val) * 37;
        }
    }
}