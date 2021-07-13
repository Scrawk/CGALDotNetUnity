using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace UnityEngine
{

    public static class VectorExtension2
    {
        public static Vector2f ToVector2f(this Vector2 v)
        {
            return new Vector2f(v.x, v.y);
        }

        public static Vector2d ToVector2d(this Vector2 v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static Vector2i ToVector2i(this Vector2 v)
        {
            return new Vector2i((int)v.x, (int)v.y);
        }

        public static Vector3f ToVector3f(this Vector2 v)
        {
            return new Vector3f(v.x, v.y, 0);
        }

        public static Vector3d ToVector3d(this Vector2 v)
        {
            return new Vector3d(v.x, v.y, 0);
        }

        public static Vector3i ToVector3i(this Vector2 v)
        {
            return new Vector3i((int)v.x, (int)v.y, 0);
        }

        public static Vector4f ToVector4f(this Vector2 v)
        {
            return new Vector4f(v.x, v.y, 0, 1);
        }

        public static Vector4d ToVector4d(this Vector2 v)
        {
            return new Vector4d(v.x, v.y, 0, 1);
        }

        public static Vector4i ToVector4i(this Vector2 v)
        {
            return new Vector4i((int)v.x, (int)v.y, 0, 1);
        }

        public static Vector2 ToVector2(this Vector2f v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector2f v)
        {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector4 ToVector4(this Vector2f v)
        {
            return new Vector4(v.x, v.y, 0, 1);
        }

        public static Vector2 ToVector2(this Vector2d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector3 ToVector3(this Vector2d v)
        {
            return new Vector3((float)v.x, (float)v.y, 0);
        }

        public static Vector4 ToVector4(this Vector2d v)
        {
            return new Vector4((float)v.x, (float)v.y, 0, 1);
        }

        public static Vector2 ToVector2(this Vector2i v)
        {
            return new Vector2(v.x, v.y);
        }
        public static Vector2Int ToVector2Int(this Vector2i v)
        {
            return new Vector2Int(v.x, v.y);
        }

        public static Vector3Int ToVector3Int(this Vector2i v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }

        public static Vector3 ToVector3(this Vector2i v)
        {
            return new Vector3(v.x, v.y, 0);
        }

        public static Vector4 ToVector4(this Vector2i v)
        {
            return new Vector4(v.x, v.y, 0, 1);
        }

        public static Vector4 xy01(this Vector2 v)
        {
            return new Vector4(v.x, v.y, 0, 1);
        }

        public static Vector4 x0y1(this Vector2 v)
        {
            return new Vector4(v.x, 0, v.y, 1);
        }

    }

}
