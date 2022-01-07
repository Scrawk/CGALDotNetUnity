using System;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet.Geometry;
using CGALDotNet.Marching;

namespace CGALDotNetUnity.Marching
{
    public class MarchingCubesExample : MonoBehaviour
    {

        public Material material;

        void Start()
        {
            PerformMarching();
        }

        private void PerformMarching()
        {
            int size = 20;
            int half = size / 2;

            var mc = new MarchingCubes();

            var vertices = new List<Point3d>();
            var indices = new List<int>();

            mc.Generate(UnionSDF, size + 1, size + 1, size + 1, vertices, indices);

            Mesh mesh = new Mesh();
            mesh.SetVertices(ToVector3(vertices));
            mesh.SetTriangles(indices, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            GameObject go = new GameObject("Mesh");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.GetComponent<Renderer>().material = material;
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.transform.localPosition = new Vector3(-half, -half, -half);

        }

        private double UnionSDF(Point3d point)
        {
            double sdf1 = SphereSDF(point);
            double sdf2 = BoxSDF(point);

            return Math.Min(sdf1, sdf2);
        }

        private double SphereSDF(Point3d point)
        {
            double radius = 5;
            Point3d Center = new Point3d(10);

            point = point - Center;
            return point.Magnitude - radius;
        }

        private double BoxSDF(Point3d point)
        {
            Point3d Min = new Point3d(2);
            Point3d Max = new Point3d(10);
            Point3d size = new Point3d(8);

            Point3d p = point - (Min + Max) * 0.5;
            p.x = Math.Abs(p.x);
            p.y = Math.Abs(p.y);
            p.z = Math.Abs(p.z);

            Point3d d = p - size * 0.5;
            Point3d max = Point3d.Max(d, 0);

            return max.Magnitude + Math.Min(Math.Max(Math.Max(d.x, d.y), d.z), 0.0);
        }

        private List<Vector3> ToVector3(IList<Point3d> points)
        {
            var list = new List<Vector3>(points.Count);

            for (int i = 0; i < points.Count; i++)
                list.Add(ToVector3(points[i]));

            return list;
        }

        private Vector3 ToVector3(Point3d p)
        {
            return new Vector3((float)p.x, (float)p.y, (float)p.z);
        }


    }

}
