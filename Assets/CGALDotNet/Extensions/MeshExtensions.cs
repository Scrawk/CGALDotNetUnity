using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNetGeometry.Numerics;
using CGALDotNet.Polyhedra;

public static class CGALMeshExtensions
{
    public static Polyhedron3<K> ToCGALPolyhedron3<K>(this Mesh mesh) where K : CGALKernel, new()
    {
        int[] triangles = mesh.triangles;
        var points = mesh.vertices.ToCGALPoint3d();

        var poly = new Polyhedron3<K>();

        if (points.Length > 0 && triangles.Length > 0)
            poly.CreateTriangleMesh(points, points.Length, triangles, triangles.Length);

        return poly;
    }

    public static SurfaceMesh3<K> ToCGALSurfaceMesh3<K>(this Mesh mesh) where K : CGALKernel, new()
    {
        int[] triangles = mesh.triangles;
        var points = mesh.vertices.ToCGALPoint3d();

        var poly = new SurfaceMesh3<K>();

        if (points.Length > 0 && triangles.Length > 0)
            poly.CreateTriangleMesh(points, points.Length, triangles, triangles.Length);

        return poly;
    }

}


