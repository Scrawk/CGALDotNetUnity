using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet.Geometry;

public static class ExtensionHelper
{
    public static GameObject CreateGameobject(string name, Mesh mesh, Vector3 translation, Material material)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        go.GetComponent<Renderer>().material = material;
        go.GetComponent<MeshFilter>().mesh = mesh;
        go.transform.localPosition = translation;
        return go;
    }

    public static Mesh CreateMesh(Point3d[] points, int[] indices)
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(ToVector3(points));
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Mesh CreateMeshXZ(Point2d[] points, int[] indices)
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(ToVector3XZ(points));
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Mesh CreateMesh(Point2d[] points, int[] indices)
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(ToVector3(points));
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public static void SplitFaces(Point3d[] points, int[] indices, out Point3d[] splitPoints, out int[] splitIndices)
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

    public static List<Vector3> ToVector3(IList<Point3d> points)
    {
        var list = new List<Vector3>(points.Count);

        for (int i = 0; i < points.Count; i++)
            list.Add((points[i].ToUnityVector3()));

        return list;
    }

    public static List<Vector3> ToVector3(IList<Point2d> points)
    {
        var list = new List<Vector3>(points.Count);

        for (int i = 0; i < points.Count; i++)
            list.Add(points[i].ToUnityVector3());

        return list;
    }

    public static List<Vector3> ToVector3XZ(IList<Point2d> points)
    {
        var list = new List<Vector3>(points.Count);

        for (int i = 0; i < points.Count; i++)
            list.Add(points[i].ToUnityVector3XZ());

        return list;
    }

}
