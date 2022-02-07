using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNet.Polygons
{

    public static class Polygon2Extension
    {
        public static GameObject ToUnityMesh(this Polygon2 poly, string name, Vector3 position, Material material)
        {
            if (!poly.IsSimple)
            {
                Debug.Log("Polygon is not simple");
                return new GameObject(name);
            }

            var indices = new List<int>();
            poly.Triangulate(indices);

            var points = new Point2d[poly.Count];
            poly.GetPoints(points, points.Length);

            Mesh mesh = ExtensionHelper.CreateMesh(points, ToArrayAndFlip(indices));
            
            return ExtensionHelper.CreateGameobject(name, mesh, position, material);
        }

        private static int[] ToArrayAndFlip(List<int> indices)
        {
            var array = new int[indices.Count];
            for(int i = 0; i < array.Length/3; i++)
            {
                array[i * 3 + 2] = indices[i * 3 + 0];
                array[i * 3 + 1] = indices[i * 3 + 1];
                array[i * 3 + 0] = indices[i * 3 + 2];
            }

            return array;
        }


    }

}
