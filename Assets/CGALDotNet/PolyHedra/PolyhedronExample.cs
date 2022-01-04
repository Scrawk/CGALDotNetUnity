using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.PolyHedra;

namespace CGALDotNetUnity.Polyhedra
{

    public class PolyhedronExample : MonoBehaviour
    {
        public Material material;

        private void Start()
        {
            CreateCube(new Vector3(3, 0.5f, 0));

            CreateUVSphere(new Vector3(1, 0.5f, 0));

            CreateNormalizedCube(new Vector3(-1, 0.5f, 0));

            CreateIcosahedron(new Vector3(-3, 0.5f, 0));
        }

        private void CreateCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateCube();

            var points = new Point3d[poly.VertexCount];
            var indices = new int[poly.FaceCount * 3];

            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
            CreateMesh(splitPoints, spliIndices, translation);
        }

        private void CreateUVSphere(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateUVSphere(32, 32);

            var points = new Point3d[poly.VertexCount];
            var indices = new int[poly.FaceCount * 3];

            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
            CreateMesh(splitPoints, spliIndices, translation);
        }

        private void CreateNormalizedCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateNormalizedCube(16);

            var points = new Point3d[poly.VertexCount];
            var indices = new int[poly.FaceCount * 3];

            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
            CreateMesh(splitPoints, spliIndices, translation);
        }

        private void CreateIcosahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateIcosahedron();

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

            for(int i = 0; i < triangles; i++)
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
