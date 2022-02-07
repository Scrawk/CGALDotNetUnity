using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNetGeometry.Numerics
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

        public static Vector2d ToCGALVector2d(this Vector2 vec)
        {
            return new Vector2d(vec.x, vec.y);
        }

        public static Vector3d ToCGALVector3d(this Vector2 vec)
        {
            return new Vector3d(vec.x, vec.y, 0);
        }

        public static Vector3d ToCGALVector3dXZ(this Vector3 vec)
        {
            return new Vector3d(vec.x, 0, vec.y);
        }

        public static Vector3d ToCGALVector3d(this Vector3 vec)
        {
            return new Vector3d(vec.x, vec.y, vec.z);
        }

        public static Vector4d ToCGALVector4d(this Vector4 vec)
        {
            return new Vector4d(vec.x, vec.y, vec.w, vec.z);
        }

        public static Vector2[] ToUnityVector2(this IList<Vector2d> vectors)
        {
            var uvectors = new Vector2[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                uvectors[i] = vectors[i].ToUnityVector2();

            return uvectors;
        }

        public static Vector3[] ToUnityVector3(this IList<Vector3d> vectors)
        {
            var uvectors = new Vector3[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                uvectors[i] = vectors[i].ToUnityVector3();

            return uvectors;
        }

        public static Vector3[] ToUnityVector3(this Vector3d[,] points)
        {
            var array = new Vector3[points.Length];

            int width = points.GetLength(0);
            int height = points.GetLength(1);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    array[i + j * width] = new Vector3((float)points[i, j].x, (float)points[i, j].y, (float)points[i, j].z);

            return array;
        }

    }

}
