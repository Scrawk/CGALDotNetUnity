using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace UnityEngine
{

    public static class VectorExtension4
    {
        public static Vector2f ToVector2f(this Vector4 v)
        {
            return new Vector2f(v.x, v.y);
        }

        public static Vector2d ToVector2d(this Vector4 v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static Vector2i ToVector2i(this Vector4 v)
        {
            return new Vector2i((int)v.x, (int)v.y);
        }

        public static Vector3f ToVector3f(this Vector4 v)
        {
            return new Vector3f(v.x, v.y, v.z);
        }

        public static Vector3d ToVector3d(this Vector4 v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        public static Vector3i ToVector3i(this Vector4 v)
        {
            return new Vector3i((int)v.x, (int)v.y, (int)v.z);
        }

        public static Vector4f ToVector4f(this Vector4 v)
        {
            return new Vector4f(v.x, v.y, v.z, v.w);
        }

        public static Vector4d ToVector4d(this Vector4 v)
        {
            return new Vector4d(v.x, v.y, v.z, v.w);
        }

        public static Vector4i ToVector4i(this Vector4 v)
        {
            return new Vector4i((int)v.x, (int)v.y, (int)v.z, (int)v.w);
        }

        public static Vector2 ToVector2(this Vector4f v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector4f v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector4 ToVector4(this Vector4f v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static Vector2 ToVector2(this Vector4d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector3 ToVector3(this Vector4d v)
        {
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        public static Vector4 ToVector4(this Vector4d v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        public static Vector2 ToVector2(this Vector4i v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector4i v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector4 ToVector4(this Vector4i v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

    }

}
