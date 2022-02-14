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

        public static GameObject ToUnityMesh(this IMesh poly, string name, Material material, bool splitFaces = true)
        {
            if (!poly.IsValid)
            {
                Debug.Log("Polyhedron3 is not valid");
                return new GameObject(name);
            }

            if (!poly.IsTriangle)
                poly.Triangulate();

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
                ExtensionHelper.SplitFaces(points, indices, out Point3d[] splitPoints, out int[] spliIndices);
                mesh = ExtensionHelper.CreateMesh(splitPoints, spliIndices);
            }
            else
            {
                mesh = ExtensionHelper.CreateMesh(points, indices);
            }

            return ExtensionHelper.CreateGameobject(name, mesh, Vector3.zero, material);
        }

    }

}
