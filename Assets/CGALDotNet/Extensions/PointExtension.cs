using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CGALDotNetGeometry.Numerics
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

        public static Point2d ToCGALPoint2d(this Vector2 vec)
        {
            return new Point2d(vec.x, vec.y);
        }

        public static Point3d ToCGALPoint3d(this Vector2 vec)
        {
            return new Point3d(vec.x, vec.y, 0);
        }

        public static Point3d ToCGALPoint3dXZ(this Vector3 vec)
        {
            return new Point3d(vec.x, 0, vec.y);
        }

        public static Point3d ToCGALPoint3d(this Vector3 vec)
        {
            return new Point3d(vec.x, vec.y, vec.z);
        }

        public static Point4d ToCGALPoint4d(this Vector4 vec)
        {
            return new Point4d(vec.x, vec.y, vec.w, vec.z);
        }

        public static Vector2[] ToUnityVector2(this IList<Point2d> points)
        {
            var vectors = new Vector2[points.Count];
            for (int i = 0; i < points.Count; i++)
                vectors[i] = points[i].ToUnityVector2();

            return vectors;
        }

        public static Vector3[] ToUnityVector3(this IList<Point2d> points)
        {
            var vectors = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
                vectors[i] = points[i].ToUnityVector3();

            return vectors;
        }

        public static Vector3[] ToUnityVector3XZ(this IList<Point2d> points)
        {
            var vectors = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
                vectors[i] = points[i].ToUnityVector3XZ();

            return vectors;
        }

        public static Vector3[] ToUnityVector3(this IList<Point3d> points)
        {
            var vectors = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
                vectors[i] = points[i].ToUnityVector3();

            return vectors;
        }

        public static Point2d[] ToCGALPoint2d(this IList<Vector2> vectors)
        {
            var points = new Point2d[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                points[i] = vectors[i].ToCGALPoint2d();

            return points;
        }

        public static Point3d[] ToCGALPoint3d(this IList<Vector3> vectors)
        {
            var points = new Point3d[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                points[i] = vectors[i].ToCGALPoint3d();

            return points;
        }

        public static Point4d[] ToCGALPoint4d(this IList<Vector4> vectors)
        {
            var points = new Point4d[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                points[i] = vectors[i].ToCGALPoint4d();

            return points;
        }

        public static Vector3[] ToUnityVector3(this Point3d[,] points)
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
