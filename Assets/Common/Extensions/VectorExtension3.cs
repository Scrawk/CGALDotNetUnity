using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace UnityEngine
{

    public static class VectorExtension3
    {

        public static Vector2f ToVector2f(this Vector3 v)
        {
            return new Vector2f(v.x, v.y);
        }

        public static Vector2d ToVector2d(this Vector3 v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static Vector2i ToVector2i(this Vector3 v)
        {
            return new Vector2i((int)v.x, (int)v.y);
        }

        public static Vector3f ToVector3f(this Vector3 v)
        {
            return new Vector3f(v.x, v.y, v.z);
        }

        public static Vector3d ToVector3d(this Vector3 v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        public static Vector3i ToVector3i(this Vector3 v)
        {
            return new Vector3i((int)v.x, (int)v.y, (int)v.z);
        }

        public static Vector2 ToVector2(this Vector3f v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector3f v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector4 ToVector4(this Vector3f v)
        {
            return new Vector4(v.x, v.y, v.z, 1);
        }

        public static Vector2 ToVector2(this Vector3d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector3 ToVector3(this Vector3d v)
        {
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        public static Vector4 ToVector4(this Vector3d v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, 1);
        }

        public static Vector2 ToVector2(this Vector3i v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector3i v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3Int ToVector3Int(this Vector3i v)
        {
            return new Vector3Int(v.x, v.y, v.z);
        }

        public static Vector4 ToVector4(this Vector3i v)
        {
            return new Vector4(v.x, v.y, v.z, 1);
        }

        public static Vector4 ToVector4(this Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z, 1);
        }

    }

}
