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
            var ponts = new HPoint3d[]
            {
                new HPoint3d(1, -1, -1, 1.25),
                new HPoint3d(1, 1, 1, 1.25),
                new HPoint3d(-1, 1, -1, 1.25),
                new HPoint3d(-1, -1, 1, 1.25),
            };

            var poly = SkinSurfaceMeshing<EEK>.Instance.CreateSkinPolyhedra(0.5, false, ponts, ponts.Length);

            poly.Print();

            poly.ToUnityMesh("Mesh", Vector3.zero, material, false);
        }

    }
}
