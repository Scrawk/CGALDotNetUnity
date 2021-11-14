using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Meshing;
using CGALDotNet.CSG;

namespace CGALDotNetUnity.Meshing
{
    public class SurfaceMesherExample : MonoBehaviour
    {

        public Material material;

        private Node3 Root;

        void Start()
        {
            PerformMeshing();
        }

        private void PerformMeshing()
        {
            /*
            int size = 20;
            int half = size / 2;

            var sphere = new SphereNode3(new Point3d(half), 5);
            var box = new BoxNode3(new Point3d(2), new Point3d(10));
            var union = new UnionNode3(sphere, box);

            Root = union;
            */

            var vertices = new List<Point3d>();
            var indices = new List<int>();
            var bounds = new Box2d(0, 1);
            var param = SurfaceMesherParams.Default;

            SurfaceMesher3<EIK>.Instance.Generate(vertices, indices, bounds, param);
  
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
            //go.transform.localPosition = new Vector3(-half, -half, -half);

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
