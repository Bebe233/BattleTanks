using BEBE.Engine.Math;
using UnityEngine;
namespace BEBE.Framework.ULMath
{
    public static class ULMath
    {
        public static LVector2 ToLVector2(this Vector2 vector3)
        {
            LFloat x = vector3.x.ToLFloat();
            LFloat y = vector3.y.ToLFloat();
            return new LVector2(x, y);
        }

        public static Vector2 ToVector2(this LVector2 lVector2)
        {
            float x = lVector2.x.ToFloat();
            float y = lVector2.y.ToFloat();
            return new Vector2(x, y);
        }


        public static LVector3 ToLVector3(this Vector3 vector3)
        {
            LFloat x = vector3.x.ToLFloat();
            LFloat y = vector3.y.ToLFloat();
            LFloat z = vector3.z.ToLFloat();
            return new LVector3(x, y, z);
        }

        public static Vector3 ToVector3(this LVector3 lVector3)
        {
            float x = lVector3.x.ToFloat();
            float y = lVector3.y.ToFloat();
            float z = lVector3.z.ToFloat();
            return new Vector3(x, y, z);
        }
    }
}