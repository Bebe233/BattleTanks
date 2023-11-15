using System;

namespace BEBE.Engine.Math.BaseType
{
    ///<summary>
    ///定点数二维向量
    ///</summary>
    [Serializable]
    public struct LVector2
    {
        public LFloat x, y;

        public LVector2(LFloat x, LFloat y)
        {
            this.x = x;
            this.y = y;
        }

        public LVector2(int x, int y)
        {
            this.x = new LFloat(x);
            this.y = new LFloat(y);
        }

        ///<summary>
        ///传入的是正常值放大1000倍后的数值
        ////<summary>
        public LVector2(bool isUseRawVal, int x, int y)
        {
            this.x = new LFloat(true, x);
            this.y = new LFloat(true, y);
        }

        #region override operator

        public static bool operator ==(LVector2 a, LVector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(LVector2 a, LVector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }


        public static LVector2 operator +(LVector2 a, LVector2 b)
        {
            return new LVector2(a.x + b.x, a.y + b.y);
        }

        public static LVector2 operator -(LVector2 a, LVector2 b)
        {
            return new LVector2(a.x - b.x, a.y - b.y);
        }

        public static LVector2 operator -(LVector2 a)
        {
            return new LVector2(-a.x, -a.y);
        }

        public static LVector2 operator *(LVector2 a, LFloat b)
        {
            return new LVector2(a.x * b, a.y * b);
        }

        public static LVector2 operator *(LFloat b, LVector2 a)
        {
            return new LVector2(a.x * b, a.y * b);
        }

        public static LVector2 operator *(LVector2 a, int b)
        {
            return new LVector2(a.x * b, a.y * b);
        }

        public static LVector2 operator *(int b, LVector2 a)
        {
            return new LVector2(a.x * b, a.y * b);
        }

        public static LVector2 operator /(LVector2 a, LFloat b)
        {
            return new LVector2(a.x / b, a.y / b);
        }

        public static LVector2 operator /(LVector2 a, int b)
        {
            return new LVector2(a.x / b, a.y / b);
        }


        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is LVector2)
            {
                LVector2 v = (LVector2)obj;
                return this.x == v.x && this.y == v.y;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.x.val * 49157 + this.y.val * 98317;
        }

        public override string ToString()
        {
            return $"({x.ToFloat()},{y.ToFloat()})";
        }

        #endregion

        #region  override type convert

        // LVector3 --> LVector2 implicit
        public static implicit operator LVector2(LVector3 v)
        {
            return new LVector2(true, v.x.val, v.y.val);
        }

        // LVector2 --> LVector3 implicit
        public static implicit operator LVector3(LVector2 v)
        {
            return new LVector3(true, v.x.val, v.y.val, 0);
        }

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
                    default: throw new IndexOutOfRangeException("vector idx invalid " + index);
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new IndexOutOfRangeException("vector idx invalid " + index);
                }
            }
        }

        public static LFloat Dot(LVector2 a, LVector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static LFloat Cross(LVector2 a, LVector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static LVector2 Lerp(LVector2 a, LVector2 b, LFloat f)
        {
            return new LVector2((b.x - a.x) * f + a.x, (b.y - a.y) * f + a.y);
        }

        #endregion

        #region const value
        public static readonly LVector2 Zero = new LVector2(true, 0, 0);
        public static readonly LVector2 One = new LVector2(true, LFloat.Precision, LFloat.Precision);
        public static readonly LVector2 Half = new LVector2(true, LFloat.HalfPercision, LFloat.HalfPercision);
        public static readonly LVector2 Up = new LVector2(true, 0, LFloat.Precision);
        public static readonly LVector2 Down = new LVector2(true, 0, -LFloat.Precision);
        public static readonly LVector2 Right = new LVector2(true, LFloat.Precision, 0);
        public static readonly LVector2 Left = new LVector2(true, -LFloat.Precision, 0);

        #endregion

        #region transform about


        ///<summary>
        ///二维向量的旋转
        ///OA-->(rcosα,rsinα),OB-->(rcosβ,rsinβ),β=α+θ,则
        ///rcosβ = rcos(α+θ)=r(cosαcosθ-sinαsinθ)=rcosαcosθ-rsinαsinθ
        ///rsinβ = rsin(α+θ)=r(sinαcosθ+cosαsinθ)=rsinαcosθ+rcosαsinθ
        ///又因 xa = rcosα , ya = rsinα , xb = rcosβ , yb = rsinβ , 代入上式，得
        ///xb = xa * cosθ - ya * sinθ
        ///yb = xa * sinθ + ya * cosθ
        ///</summary>
        public LVector2 Rotate(LFloat deg)
        {
            var rad = LMath.Deg2Rad * deg;
            LFloat sin, cos;
            LMath.SinCos(out sin, out cos, rad);
            return new LVector2(x * cos - y * sin, x * sin + y * cos);
        }

        public LVector2 Normalize()
        {
            var b = LMath.Sqrt(LMath.Sqr(x) + LMath.Sqr(y));
            x /= b;
            y /= b;
            return this;
        }

        public LVector2 normalized
        {
            get
            {
                LVector2 res = new LVector2(x, y);
                return res.Normalize();
            }
        }

        public LFloat magnitude
        {
            get
            {
                long x = (long)this.x.val;
                long y = (long)this.y.val;
                return new LFloat(true, LMath.Sqrt(x * x + y * y));
            }
        }

        public LFloat sqrMagnitude
        {
            get
            {
                long x = (long)this.x.val;
                long y = (long)this.y.val;
                return new LFloat(true, (x * x + y * y) / LFloat.Precision);
            }
        }

        #endregion

    }
}