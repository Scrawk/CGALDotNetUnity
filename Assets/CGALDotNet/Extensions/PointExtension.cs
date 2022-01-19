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

    }

}
