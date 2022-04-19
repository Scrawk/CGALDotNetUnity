using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNetGeometry.Extensions;

namespace CGALDotNet.Polyhedra
{
    public static class IMeshExtension
    {
        public static GameObject ToUnityMesh<K>(this Polyhedron3<K> poly, string name, Material material, bool splitFaces = true)
                where K : CGALKernel, new()
        {
            return ToUnityMesh(poly, name, material, null, splitFaces);
        }

        public static GameObject ToUnityMesh<K>(this Polyhedron3<K> poly, string name, Material material, Color[] colors, bool splitFaces = true)
            where K : CGALKernel, new()
        {
            if (!poly.IsValid)
            {
                Debug.Log("Polyhedron3 is not valid");
                return new GameObject(name);
            }

            if (!poly.IsTriangle)
            {
                poly = poly.Copy();
                poly.Triangulate();
            }

            return IMeshToUnityMesh(poly, name, material, colors, splitFaces);
        }

        public static GameObject ToUnityMesh<K>(this SurfaceMesh3<K> poly, string name, Material material, bool splitFaces = true)
        where K : CGALKernel, new()
        {
            return ToUnityMesh(poly, name, material, null, splitFaces);
        }

        public static GameObject ToUnityMesh<K>(this SurfaceMesh3<K> poly, string name, Material material, Color[] colors, bool splitFaces = true)
            where K : CGALKernel, new()
        {
            if (!poly.IsValid)
            {
                Debug.Log("Polyhedron3 is not valid");
                return new GameObject(name);
            }

            if (!poly.IsTriangle)
            {
                poly = poly.Copy();
                poly.Triangulate();
            }

            return IMeshToUnityMesh(poly, name, material, colors, splitFaces);
        }

        private static GameObject IMeshToUnityMesh(this IMesh poly, string name, Material material, Color[] colors, bool splitFaces = true) 
        {
            int count = poly.VertexCount;
            if (count == 0)
            {
                Debug.Log("Polyhedron3 is empty");
                return new GameObject(name);
            }

            var points = new Point3d[count];
            var indices = new int[poly.FaceCount * 3];
            poly.GetPoints(points, points.Length);
            poly.GetTriangleIndices(indices, indices.Length);

            indices = indices.RemoveNullTriangles();

            Mesh mesh;

            if (splitFaces)
            {
                if(colors != null)
                {
                    ExtensionHelper.SplitFaces(points, colors, indices, out Point3d[] splitPoints, out Color[] splitColors, out int[] spliIndices);
                    mesh = ExtensionHelper.CreateMesh(splitPoints, splitColors, spliIndices);
                }
                else
                {
                    ExtensionHelper.SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
                    mesh = ExtensionHelper.CreateMesh(splitPoints, spliIndices);
                }
            }
            else
            {
                if(colors != null)
                    mesh = ExtensionHelper.CreateMesh(points, colors, indices);
                else
                    mesh = ExtensionHelper.CreateMesh(points, indices);
            }

            return ExtensionHelper.CreateGameobject(name, mesh, Vector3.zero, material);
        }

    }

}
