using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Shapes
{

    public static class GeometryExtension
    {

        public static Vector2[] ToUnityVector2(this IList<Segment2d> segments)
        {
            var array = new Vector2[segments.Count * 2];

            for (int i = 0; i < segments.Count; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;
                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }

            return array;
        }

        public static Vector3[] ToUnityVector3(this IList<Segment3d> segments)
        {
            var array = new Vector3[segments.Count * 2];

            for (int i = 0; i < segments.Count; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;
                array[i * 2 + 0] = new Vector3((float)a.x, (float)a.y, (float)a.z);
                array[i * 2 + 1] = new Vector3((float)b.x, (float)b.y, (float)b.z);
            }

            return array;
        }

        public static Vector2[] ToUnityVector2(this IList<Ray2d> rays)
        {
            var array = new Vector2[rays.Count * 2];

            for (int i = 0; i < rays.Count; i++)
            {
                var a = rays[i].Position;
                var b = a + rays[i].Direction;
                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }

            return array;
        }

        public static Vector2[] ToUnityVector2(this IList<Triangle2d> triangles)
        {
            var array = new Vector2[triangles.Count * 3];

            for (int i = 0; i < triangles.Count; i++)
            {
                var t = triangles[i];

                var a = new Vector2((float)t.A.x, (float)t.A.y);
                var b = new Vector2((float)t.B.x, (float)t.B.y);
                var c = new Vector2((float)t.C.x, (float)t.C.y);

                array[i * 3 + 0] = a;
                array[i * 3 + 1] = b;
                array[i * 3 + 2] = c;
            }

            return array;
        }

        public static Vector2[] ToUnityVector2(this IList<Circle2d> points)
        {
            var array = new Vector2[points.Count];

            for (int i = 0; i < points.Count; i++)
                array[i] = new Vector2((float)points[i].Center.x, (float)points[i].Center.y);

            return array;
        }

        public static float[] ToRadius(this IList<Circle2d> circles)
        {
            var array = new float[circles.Count];

            for (int i = 0; i < circles.Count; i++)
                array[i] = (float)circles[i].Radius;

            return array;
        }

        public static List<Vector2> ToUnityVector2(this List<Segment2d> segments)
        {
            var list = new List<Vector2>();
            foreach (var seg in segments)
            {
                var a = new Vector2((float)seg.A.x, (float)seg.A.y);
                var b = new Vector2((float)seg.B.x, (float)seg.B.y);
                list.Add(a);
                list.Add(b);
            }

            return list;
        }

    }

}
