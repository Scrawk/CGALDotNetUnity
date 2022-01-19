using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNet.Geometry
{

    public static class VectorExtension
    {
        public static Vector2 ToUnityVector2(this Vector2d vec)
        {
            return new Vector2((float)vec.x, (float)vec.y);
        }

        public static Vector3 ToUnityVector3(this Vector2d vec)
        {
            return new Vector3((float)vec.x, (float)vec.y, 0);
        }

        public static Vector3 ToUnityVector3(this Vector3d vec)
        {
            return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
        }

        public static Vector4 ToUnityVector4(this Vector4d vec)
        {
            return new Vector4((float)vec.x, (float)vec.y, (float)vec.w, (float)vec.z);
        }

        public static Vector2d ToCGALVector2(this Vector2 vec)
        {
            return new Vector2d(vec.x, vec.y);
        }

        public static Vector3d ToCGALVector3(this Vector2 vec)
        {
            return new Vector3d(vec.x, vec.y, 0);
        }

        public static Vector3d ToCGALVector3XZ(this Vector3 vec)
        {
            return new Vector3d(vec.x, 0, vec.y);
        }

        public static Vector3d ToCGALVector3(this Vector3 vec)
        {
            return new Vector3d(vec.x, vec.y, vec.z);
        }

        public static Vector4d ToCGALVector4(this Vector4 vec)
        {
            return new Vector4d(vec.x, vec.y, vec.w, vec.z);
        }

    }

}
