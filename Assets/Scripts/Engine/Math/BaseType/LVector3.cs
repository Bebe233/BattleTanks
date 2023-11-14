using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BEBE.Framework.Math.BaseType
{
    ///<summary>
    ///定点数三维向量
    ///</summary>
    [Serializable]
    public struct LVector3
    {
        public LFloat x, y, z;

        public LVector3(LFloat x, LFloat y, LFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public LVector3(int x, int y, int z)
        {
            this.x = new LFloat(x);
            this.y = new LFloat(y);
            this.z = new LFloat(z);
        }

        public LVector3(bool isUseRawVal, int x, int y, int z)
        {
            this.x = new LFloat(true, x);
            this.y = new LFloat(true, y);
            this.z = new LFloat(true, z);
        }

        #region override operator
        public static bool operator ==(LVector3 a, LVector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(LVector3 a, LVector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static LVector3 operator +(LVector3 a, LVector3 b)
        {
            return new LVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static LVector3 operator -(LVector3 a, LVector3 b)
        {
            return new LVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static LVector3 operator -(LVector3 a)
        {
            return new LVector3(-a.x, -a.y, -a.z);
        }

        public static LVector3 operator *(LVector3 a, LVector3 b)
        {
            return new LVector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static LVector3 operator *(LVector3 a, LFloat b)
        {
            return new LVector3(a.x * b, a.y * b, a.z * b);
        }
        public static LVector3 operator *(LVector3 a, int b)
        {
            return new LVector3(a.x * b, a.y * b, a.z * b);
        }
        public static LVector3 operator *(LFloat b, LVector3 a)
        {
            return new LVector3(a.x * b, a.y * b, a.z * b);
        }
        public static LVector3 operator *(int b, LVector3 a)
        {
            return new LVector3(a.x * b, a.y * b, a.z * b);
        }
        public static LVector3 operator /(LVector3 a, LFloat b)
        {
            return new LVector3(a.x / b, a.y / b, a.z / b);
        }
        public static LVector3 operator /(LVector3 a, int b)
        {
            return new LVector3(a.x / b, a.y / b, a.z / b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is LVector3)
            {
                LVector3 v = (LVector3)obj;
                return this.x == v.x && this.y == v.y && this.z == v.z;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.x.val * 73856093 ^ this.y.val * 19349663 ^ this.z.val * 83492791;
        }

        public override string ToString()
        {
            return $"({x.ToString()},{y.ToString()},{z.ToString()})";
        }
        #endregion

        #region override type convert

        #endregion

        #region some methods
        public LFloat this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException("vector idx invalid " + index);
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new IndexOutOfRangeException("vector idx invalid " + index);
                }
            }
        }

        public static LFloat Dot(LVector3 a, LVector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static LVector3 Cross(LVector3 a, LVector3 b)
        {
            return new LVector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static LVector3 Lerp(LVector3 a, LVector3 b, LFloat f)
        {
            return new LVector3((b.x - a.x) * f + a.x, (b.y - a.y) * f + a.y, (b.z - a.z) * f + a.z);
        }

        #endregion

        #region const value
        public static readonly LVector3 Zero = new LVector3(true, 0, 0, 0);
        public static readonly LVector3 One = new LVector3(true, LFloat.Precision, LFloat.Precision, LFloat.Precision);
        public static readonly LVector3 Half = new LVector3(true, LFloat.HalfPercision, LFloat.HalfPercision, LFloat.HalfPercision);
        public static readonly LVector3 Forward = new LVector3(true, 0, 0, LFloat.Precision);
        public static readonly LVector3 Back = new LVector3(true, 0, 0, -LFloat.Precision);
        public static readonly LVector3 Up = new LVector3(true, 0, LFloat.Precision, 0);
        public static readonly LVector3 Down = new LVector3(true, 0, -LFloat.Precision, 0);
        public static readonly LVector3 Right = new LVector3(true, LFloat.Precision, 0, 0);
        public static readonly LVector3 Left = new LVector3(true, -LFloat.Precision, 0, 0);
        #endregion

        #region transform about

        public LVector3 Normalize()
        {
            var b = LMath.Sqrt(LMath.Sqr(x) + LMath.Sqr(y) + LMath.Sqr(z));
            x /= b;
            y /= b;
            z /= b;
            return this;
        }

        public LVector3 normalized
        {
            get
            {
                LVector3 res = new LVector3(x, y, z);
                return res.Normalize();
            }
        }

        public LFloat magnitude
        {
            get
            {
                long x = (long)this.x.val;
                long y = (long)this.y.val;
                long z = (long)this.z.val;
                return new LFloat(true, LMath.Sqrt(x * x + y * y + z * z));
            }
        }

        public LFloat sqrMagnitude
        {
            get
            {
                long x = (long)this.x.val;
                long y = (long)this.y.val;
                long z = (long)this.z.val;
                return new LFloat(true, (x * x + y * y + z * z) / LFloat.Precision);
            }
        }

        #endregion
    }
}