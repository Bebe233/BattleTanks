using System;

namespace BEBE.Engine.Math
{
    ///<summary>
    ///定点数浮点类型
    ///</summary>
    [Serializable]
    public struct LFloat : IEquatable<LFloat>, IComparable<LFloat>
    {
        public const int Precision = 1000; //精度
        public const int HalfPercision = Precision / 2;
        public const float PrecisionFactor = 0.001f;
        public const int size = sizeof(int);
        public int val;

        public LFloat(int val)
        {
            this.val = val * Precision;
        }

        public LFloat(long val)
        {
            this.val = (int)(val * Precision);
        }

        ///<summary>
        ///传入的是正常值放大1000倍后的数值
        ////<summary>
        public LFloat(bool isUseRawVal, int rawVal)
        {
            this.val = rawVal;
        }

        ///<summary>
        ///传入的是正常值放大1000倍后的数值
        ////<summary>
        public LFloat(bool isUseRawVal, long rawVal)
        {
            this.val = ((int)rawVal);
        }

        /*********************重载运算符***********************/
        #region override operator
        public static bool operator <(LFloat a, LFloat b)
        {
            return a.val < b.val;
        }

        public static bool operator >(LFloat a, LFloat b)
        {
            return a.val > b.val;
        }

        public static bool operator <=(LFloat a, LFloat b)
        {
            return a.val <= b.val;
        }

        public static bool operator >=(LFloat a, LFloat b)
        {
            return a.val >= b.val;
        }

        public static bool operator ==(LFloat a, LFloat b)
        {
            return a.val == b.val;
        }

        public static bool operator !=(LFloat a, LFloat b)
        {
            return a.val != b.val;
        }

        public static LFloat operator +(LFloat a, LFloat b)
        {
            return new LFloat(true, a.val + b.val);
        }

        public static LFloat operator -(LFloat a, LFloat b)
        {
            return new LFloat(true, a.val - b.val);
        }

        public static LFloat operator *(LFloat a, LFloat b)
        {
            long val = ((long)a.val) * b.val;
            return new LFloat(true, ((int)(val / 1000)));
        }

        public static LFloat operator /(LFloat a, LFloat b)
        {
            long val = ((long)(a.val * 1000)) / b.val;
            return new LFloat(true, ((int)val));
        }

        public static LFloat operator -(LFloat a)
        {
            return new LFloat(true, -a.val);
        }

        #endregion

        #region adapt for int

        public static LFloat operator +(LFloat a, int b)
        {
            return new LFloat(true, a.val + b * Precision);
        }

        public static LFloat operator -(LFloat a, int b)
        {
            return new LFloat(true, a.val - b * Precision);
        }

        public static LFloat operator *(LFloat a, int b)
        {
            return new LFloat(true, a.val * b);
        }

        public static LFloat operator /(LFloat a, int b)
        {
            return new LFloat(true, a.val / b);
        }

        public static LFloat operator +(int a, LFloat b)
        {
            return new LFloat(true, b.val + a * Precision);
        }

        public static LFloat operator -(int a, LFloat b)
        {
            return new LFloat(true, a * Precision - b.val);
        }

        public static LFloat operator *(int a, LFloat b)
        {
            return new LFloat(true, a * b.val);
        }

        public static LFloat operator /(int a, LFloat b)
        {
            return new LFloat(true, ((int)(((long)(a * Precision * Precision)) / b.val)));
        }

        public static bool operator <(LFloat a, int b)
        {
            return a.val < (b * Precision);
        }

        public static bool operator >(LFloat a, int b)
        {
            return a.val > (b * Precision);
        }

        public static bool operator <=(LFloat a, int b)
        {
            return a.val <= (b * Precision);
        }

        public static bool operator >=(LFloat a, int b)
        {
            return a.val >= (b * Precision);
        }

        public static bool operator ==(LFloat a, int b)
        {
            return a.val == (b * Precision);
        }

        public static bool operator !=(LFloat a, int b)
        {
            return a.val != (b * Precision);
        }


        public static bool operator <(int a, LFloat b)
        {
            return (a * Precision) < (b.val);
        }

        public static bool operator >(int a, LFloat b)
        {
            return (a * Precision) > (b.val);
        }

        public static bool operator <=(int a, LFloat b)
        {
            return (a * Precision) <= (b.val);
        }

        public static bool operator >=(int a, LFloat b)
        {
            return (a * Precision) >= (b.val);
        }

        public static bool operator ==(int a, LFloat b)
        {
            return (a * Precision) == (b.val);
        }

        public static bool operator !=(int a, LFloat b)
        {
            return (a * Precision) != (b.val);
        }

        bool IEquatable<LFloat>.Equals(LFloat other)
        {
            return this.val == other.val;
        }

        int IComparable<LFloat>.CompareTo(LFloat other)
        {
            return val.CompareTo(other.val);
        }

        public override int GetHashCode()
        {
            return val;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is LFloat && ((LFloat)obj).val == val;
        }

        public override string ToString()
        {
            return (val * PrecisionFactor).ToString();
        }

        #endregion

        #region override type convert
        public static implicit operator LFloat(short value)
        {
            return new LFloat(true, value * Precision);
        }

        public static explicit operator short(LFloat value)
        {
            return ((short)(value.val / Precision));
        }

        public static implicit operator LFloat(int value)
        {
            return new LFloat(true, value * Precision);
        }

        public static explicit operator int(LFloat value)
        {
            return value.val / Precision;
        }

        public static explicit operator LFloat(long value)
        {
            return new LFloat(true, value * Precision);
        }

        public static implicit operator long(LFloat value)
        {
            return value.val / Precision;
        }

        public static explicit operator LFloat(float value)
        {
            return new LFloat(true, ((long)(value * Precision)));
        }

        public static implicit operator float(LFloat value)
        {
            return value.val * PrecisionFactor;
        }

        public static explicit operator LFloat(double value)
        {
            return new LFloat(true, ((long)(value * Precision)));
        }

        public static implicit operator double(LFloat value)
        {
            return value.val * 0.001d;
        }

        #endregion

        #region some methods

        public int ToInt()
        {
            return val / Precision;
        }

        public long ToLong()
        {
            return val / Precision;
        }

        public float ToFloat()
        {
            return val * PrecisionFactor;
        }

        public double ToDouble()
        {
            return val * 0.001d;
        }

        public int Floor()
        {
            int x = val;
            if (x > 0)
            {
                x /= Precision;
            }
            else
            {
                if (x % Precision == 0) x /= Precision;
                else x = x / Precision - 1;
            }
            return x;
        }

        public int Ceil()
        {
            int x = val;
            if (x < 0)
            {
                x /= Precision;
            }
            else
            {
                if (x % Precision == 0) x /= Precision;
                else x = x / Precision + 1;
            }
            return x;
        }

        #endregion

        #region const value

        public static readonly LFloat Zero = new LFloat(true, 0);
        public static readonly LFloat One = new LFloat(true, Precision);
        public static readonly LFloat NagOne = new LFloat(true, -Precision);
        public static readonly LFloat Half = new LFloat(true, HalfPercision);
        public static readonly LFloat MaxValue = new LFloat(true, int.MaxValue);
        public static readonly LFloat MinValue = new LFloat(true, int.MinValue);

        #endregion

    }
}