using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;
using CGALDotNet.Meshing;
using Common.Unity.Drawing;

namespace CGALDotNetUnity.Meshing
{
    public class SkinSurfaceExample : MonoBehaviour
    {

        public Material material;

        void Start()
        {
            var points = Point3d.RandomPoints(0, 10, new Box3d(-10, 10));

            var poly = SkinSurfaceMeshing<EEK>.Instance.CreateSkinPolyhedra(0.5, true, points, points.Length);

            poly.Print();

            poly.ToUnityMesh("Mesh", Vector3.zero, material, false);
        }

    }
}
