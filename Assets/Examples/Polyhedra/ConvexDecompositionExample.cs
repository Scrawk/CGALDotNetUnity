using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;

namespace CGALDotNetUnity.Polyhedra
{

    public class ConvexDecompositionExample : MonoBehaviour
    {
        public Material material;

        private void Start()
        {
            var box1 = PolyhedronFactory<EEK>.CreateCube();
            box1.Translate(new Point3d(0.5));

            var box2 = PolyhedronFactory<EEK>.CreateCube();

            var nef1 = new NefPolyhedron3<EEK>(box1);
            var nef2 = new NefPolyhedron3<EEK>(box2);

            var nef3 = nef1.Join(nef2);

            var volumes = new List<Polyhedron3<EEK>>();
            nef3.GetVolumes(volumes);

            foreach (var poly in volumes)
            {
                if(!poly.IsPureTriangle)
                    poly.ConvertQuadsToTriangles();

                poly.ToUnityMesh("Convex", Vector3.zero, material);
            }

        }

    }

}
