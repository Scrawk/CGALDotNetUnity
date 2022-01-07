using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Hulls;
using CGALDotNet.Polyhedra;

namespace CGALDotNetUnity.Hulls
{
    public class ConvexHull3Example : MonoBehaviour
    {
        public Material material;

        private GameObject m_hull;

        private void Start()
        {

            var box = new Box3d(-5, 5);
            var points = Point3d.RandomPoints(0, 20, box);

            var poly = ConvexHull3<EEK>.Instance.CreateHullAsPolyhedron(points, points.Length);

            m_hull = poly.ToUnityMesh("Hull", Vector3.zero, material);
        }


    }
}
