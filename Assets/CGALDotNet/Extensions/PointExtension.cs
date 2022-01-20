using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGALDotNet.Geometry
{

    public static class PointExtension
    {
        public static Vector2 ToUnityVector2(this Point2d point)
        {
            return new Vector2((float)point.x, (float)point.y);
        }

        public static Vector3 ToUnityVector3(this Point2d point)
        {
            return new Vector3((float)point.x, (float)point.y, 0);
        }

        public static Vector3 ToUnityVector3XZ(this Point2d point)
        {
            return new Vector3((float)point.x, 0, (float)point.y);
        }

        public static Vector3 ToUnityVector3(this Point3d point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }

        public static Vector4 ToUnityVector4(this Point4d point)
        {
            return new Vector4((float)point.x, (float)point.y, (float)point.w, (float)point.z);
        }

        public static Point2d ToCGALPoint2(this Vector2 vec)
        {
            return new Point2d(vec.x, vec.y);
        }

        public static Point3d ToCGALPoint3(this Vector2 vec)
        {
            return new Point3d(vec.x, vec.y, 0);
        }

        public static Point3d ToCGALPoint3XZ(this Vector3 vec)
        {
            return new Point3d(vec.x, 0, vec.y);
        }

        public static Point3d ToCGALPoint3(this Vector3 vec)
        {
            return new Point3d(vec.x, vec.y, vec.z);
        }

        public static Point4d ToCGALPoint4(this Vector4 vec)
        {
            return new Point4d(vec.x, vec.y, vec.w, vec.z);
        }

        public static Vector3[] ToUnityVector3(this Point2d[] points)
        {
            var vectors = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
                vectors[i] = points[i].ToUnityVector3();

            return vectors;
        }

        public static Vector3[] ToUnityVector3XZ(this Point2d[] points)
        {
            var vectors = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
                vectors[i] = points[i].ToUnityVector3XZ();

            return vectors;
        }

        public static Vector3[] ToUnityVector3(this  Point3d[] points)
        {
            var vectors = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
                vectors[i] = points[i].ToUnityVector3();

            return vectors;
        }

        public static Vector2[] ToUnityVector2(this Vector2d[] vectors)
        {
            var uvectors = new Vector2[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                uvectors[i] = vectors[i].ToUnityVector2();

            return uvectors;
        }

        public static Vector3[] ToUnityVector3(this Vector3d[] vectors)
        {
            var uvectors = new Vector3[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                uvectors[i] = vectors[i].ToUnityVector3();

            return uvectors;
        }

        public static List<Vector3> ToUnityVector3(IList<Point3d> points)
        {
            var list = new List<Vector3>(points.Count);

            for (int i = 0; i < points.Count; i++)
                list.Add((points[i].ToUnityVector3()));

            return list;
        }

        public static Point2d[] ToCGALVector2(this Vector2[] vectors)
        {
            var points = new Point2d[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                points[i] = vectors[i].ToCGALPoint2();

            return points;
        }

        public static Point3d[] ToCGALVector3(this Vector3[] vectors)
        {
            var points = new Point3d[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                points[i] = vectors[i].ToCGALPoint3();

            return points;
        }

        public static Point4d[] ToCGALVector4(this Vector4[] vectors)
        {
            var points = new Point4d[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                points[i] = vectors[i].ToCGALPoint4();

            return points;
        }


    }

}
