using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Marching;
using CGALDotNet.CSG;

namespace CGALDotNetUnity.Marching
{
    public class MarchingCubesExample : MonoBehaviour
    {

        public Material material;

        private Node3 Root;

        void Start()
        {
            PerformMarching();
        }

        private void PerformMarching()
        {
            int size = 20;
            int half = size / 2;

            var sphere = new SphereNode3(new Point3d(half), 5);
            var box = new BoxNode3(new Point3d(2), new Point3d(10));
            var union = new UnionNode3(sphere, box);

            Root = union;

            var ms = new MarchingCubes();

            var vertices = new List<Point3d>();
            var indices = new List<int>();

            ms.Generate(Root, size + 1, size + 1, size + 1, vertices, indices);

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
