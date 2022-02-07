using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNet.Polygons
{

    public static class PolygonWithHoles2Extension
    {
        public static GameObject ToUnityMesh(this PolygonWithHoles2 poly, string name, Vector3 position, Material material)
        {
            var indices = new List<int>();
            poly.Triangulate(indices);

            var points = new List<Point2d>();
            poly.GetAllPoints(points);

            Mesh mesh = ExtensionHelper.CreateMesh(points.ToArray(), ToArrayAndFlip(indices));

            return ExtensionHelper.CreateGameobject(name, mesh, position, material);
        }

        private static int[] ToArrayAndFlip(List<int> indices)
        {
            var array = new int[indices.Count];
            for (int i = 0; i < array.Length / 3; i++)
            {
                array[i * 3 + 2] = indices[i * 3 + 0];
                array[i * 3 + 1] = indices[i * 3 + 1];
                array[i * 3 + 0] = indices[i * 3 + 2];
            }

            return array;
        }


    }

}
