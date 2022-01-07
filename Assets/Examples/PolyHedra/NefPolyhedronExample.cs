using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.PolyHedra;

namespace CGALDotNetUnity.Polyhedra
{

    public class NefPolyhedronExample : MonoBehaviour
    {
        public Material material;

        private void Start()
        {
            var box1 = PolyhedronFactory<EEK>.CreateCube();
            box1.Translate(new Point3d(0.5));

            var box2 = PolyhedronFactory<EEK>.CreateCube();

            var nef1 = new NefPolyhedron3<EEK>(box1);
            var nef2 = new NefPolyhedron3<EEK>(box2);

            var nef3 = nef1.Join(nef2);

            if (nef3.ConvertToPolyhedron(out Polyhedron3<EEK> poly))
                MeshPolyhedron(poly, new Vector3(0, 0.5f, 0));

        }

        private void MeshPolyhedron(Polyhedron3<EEK> poly, Vector3 translation)
        {
            var points = new Point3d[poly.VertexCount];
            var indices = new int[poly.FaceCount * 3];

            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
            CreateMesh(splitPoints, spliIndices, translation);
        }

        private void SplitFaces(Point3d[] points, int[] indices, out Point3d[] splitPoints, out int[] splitIndices)
        {
            int triangles = indices.Length / 3;

            splitPoints = new Point3d[triangles * 3];
            splitIndices = new int[triangles * 3];

            for (int i = 0; i < triangles; i++)
            {
                var a = points[indices[i * 3 + 0]];
                var b = points[indices[i * 3 + 1]];
                var c = points[indices[i * 3 + 2]];

                splitPoints[i * 3 + 0] = a;
                splitPoints[i * 3 + 1] = b;
                splitPoints[i * 3 + 2] = c;

                splitIndices[i * 3 + 0] = i * 3 + 0;
                splitIndices[i * 3 + 1] = i * 3 + 1;
                splitIndices[i * 3 + 2] = i * 3 + 2;
            }

        }

        private void CreateMesh(Point3d[] points, int[] indices, Vector3 translation)
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(ToVector3(points));
            mesh.SetTriangles(indices, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            GameObject go = new GameObject("Mesh");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.GetComponent<Renderer>().material = material;
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.transform.localPosition = translation;
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
