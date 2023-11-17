using BEBE.Engine.Math.LUT;

namespace BEBE.Engine.Math
{
    public static partial class LMath
    {
        public static readonly LFloat PI = new LFloat(true, 3142);
        public static readonly LFloat PI_Half = new LFloat(true, 1571);
        public static readonly LFloat PIx2 = new LFloat(true, 6283);
        public static readonly LFloat Rad2Deg = 180 / PI;
        public static readonly LFloat Deg2Rad = PI / 180;

        #region base functions
        public static uint Sqrt32(uint a)
        {
            uint num = 0u;
            uint num2 = 0u;
            for (int i = 0; i < 16; i++)
            {
                num2 <<= 1;
                num <<= 2;
                num += a >> 30;
                a <<= 2;
                if (num2 < num)
                {
                    num2 += 1u;
                    num -= num2;
                    num2 += 1u;
                }
            }
            return num2 >> 1 & 65535u;
        }

        public static ulong Sqrt64(ulong a)
        {
            ulong num = 0uL;
            ulong num2 = 0uL;
            for (int i = 0; i < 32; i++)
            {
                num2 <<= 1;
                num <<= 2;
                num += a >> 62;
                a <<= 2;
                if (num2 < num)
                {
                    num2 += 1uL;
                    num -= num2;
                    num2 += 1uL;
                }
            }

            return num2 >> 1 & 0xffffffffu;
        }

        public static int Sqrt(int a)
        {
            if (a <= 0) return 0;
            return (int)Sqrt32((uint)a);
        }

        public static int Sqrt(long a)
        {
            if (a <= 0L) return 0;
            if (a <= (long)(0xffffffffu))
            {
                return (int)Sqrt32((uint)a);
            }
            return ((int)Sqrt64(((ulong)a)));
        }

        public static LFloat Sqrt(LFloat a)
        {
            if (a.val <= 0) return LFloat.Zero;
            return new LFloat(true, Sqrt(((long)a.val) * LFloat.Precision));
        }

        public static LFloat Sqr(LFloat a)
        {
            return a * a;
        }

        public static int Clamp(int a, int min, int max)
        {
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            if (a < min) return min;
            if (a > max) return max;
            return a;
        }

        public static LFloat Clamp(LFloat a, LFloat min, LFloat max)
        {
            if (min > max)
            {
                LFloat temp = min;
                min = max;
                max = temp;
            }
            if (a < min) return min;
            if (a > max) return max;
            return a;
        }

        public static LFloat Clamp01(LFloat a)
        {
            if (a < LFloat.Zero) return LFloat.Zero;
            if (a < LFloat.One) return LFloat.One;
            return a;
        }

        public static LFloat Abs(LFloat a)
        {
            if (a.val < 0) return new LFloat(true, -a.val);
            return a;
        }

        ///<summary>
        ///判断正负
        ///<param name="a">定点数浮点类型</param>
        ///<returns>返回-1，0，1</returns>
        ///</summary>
        public static int Sign(LFloat a)
        {
            return System.Math.Sign(a.val);
        }

        public static LFloat Round(LFloat a)
        {
            if (a <= 0)
            {
                var remainder = (-a.val) % LFloat.Precision;
                if (remainder > LFloat.HalfPercision)
                {
                    return new LFloat(true, a.val + remainder - LFloat.Precision);
                }
                else
                {
                    return new LFloat(true, a.val + remainder);
                }
            }
            else
            {
                var remainder = a.val % LFloat.Precision;
                if (remainder > LFloat.HalfPercision)
                {
                    return new LFloat(true, a.val - remainder + LFloat.Precision);
                }
                else
                {
                    return new LFloat(true, a.val - remainder);
                }
            }
        }


        public static LFloat Min(LFloat a, LFloat b)
        {
            return a < b ? a : b;
        }

        public static LFloat Max(LFloat a, LFloat b)
        {
            return a > b ? a : b;
        }

        public static LFloat Min(params LFloat[] values)
        {
            int length = values.Length;
            if (length == 0) return LFloat.Zero;
            LFloat num = values[0];
            for (int i = 0; i < length; i++)
            {
                if (values[i] < num) num = values[i];
            }
            return num;
        }

        public static LFloat Max(params LFloat[] values)
        {
            int length = values.Length;
            if (length == 0) return LFloat.Zero;
            LFloat num = values[0];
            for (int i = 0; i < length; i++)
            {
                if (values[i] > num) num = values[i];
            }
            return num;
        }

        public static LFloat Lerp(LFloat a, LFloat b, LFloat f)
        {
            return new LFloat(true, ((long)((b.val - a.val) * f.val)) / LFloat.Precision + a.val);
        }

        public static LFloat InverseLerp(LFloat a, LFloat b, LFloat value)
        {
            if (a != b) return Clamp01((value - a) / (b - a));
            return LFloat.Zero;
        }

        #endregion


        #region trigonometric functions 三角函数

        public static LFloat Atan2(int y, int x)
        {
            if (x == 0)
            {
                if (y > 0) return PI_Half;
                else if (y < 0) return -PI_Half;
                else return LFloat.Zero;
            }
            if (y == 0)
            {
                if (x > 0) return LFloat.Zero;
                else if (x < 0) return PI;
                else return LFloat.Zero;
            }
            int num, num2;
            if (x < 0)
            {
                if (y < 0)
                {
                    x = -x;
                    y = -y;
                    num = 1;
                }
                else
                {
                    x = -x;
                    num = -1;
                }
                num2 = -31416;
            }
            else
            {
                if (y < 0)
                {
                    y = -y;
                    num = -1;
                }
                else
                {
                    num = 1;
                }
                num2 = 0;
            }
            int dim = LUTAtan2.DIM;
            long num3 = (long)(dim - 1);
            long b = (long)((x >= y) ? x : y);
            int num4 = (int)((long)x * num3 / b);
            int num5 = (int)((long)y * num3 / b);
            int num6 = LUTAtan2.table[num5 * dim + num4];
            return new LFloat(true, (long)((num6 + num2) * num) / 10);
        }

        public static LFloat Atan2(LFloat y, LFloat x)
        {
            return Atan2(y.val, x.val);
        }

        public static LFloat Acos(LFloat val)
        {
            int num = (int)(val.val * (long)LUTAcos.HALF_COUNT / LFloat.Precision) + LUTAcos.HALF_COUNT;
            num = Clamp(num, 0, LUTAcos.COUNT);
            return new LFloat(true, (long)LUTAcos.table[num] / 10);
        }

        public static LFloat Asin(LFloat val)
        {
            int num = (int)(val.val * (long)LUTAsin.HALF_COUNT / LFloat.Precision) + LUTAsin.HALF_COUNT;
            num = Clamp(num, 0, LUTAsin.HALF_COUNT);
            return new LFloat(true, (long)LUTAsin.table[num] / 10);
        }

        public static LFloat Sin(LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            return new LFloat(true, (long)LUTSinCos.sin_table[index] / 10);
        }

        public static LFloat Cos(LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            return new LFloat(true, (long)LUTSinCos.cos_table[index] / 10);
        }

        public static void SinCos(out LFloat sin, out LFloat cos, LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            sin = new LFloat(true, (long)LUTSinCos.sin_table[index] / 10);
            cos = new LFloat(true, (long)LUTSinCos.cos_table[index] / 10);
        }

        #endregion

    }
}

