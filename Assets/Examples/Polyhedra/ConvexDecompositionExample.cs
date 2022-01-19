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

            nef3.ConvexDecomposition();

            var volumes = new List<Polyhedron3<EEK>>();
            nef3.GetVolumes(volumes);

            for(int i = 0; i < volumes.Count; i++)
            {
                //first poly if the original so skip.
                if (i == 0) continue;

                var poly = volumes[i];

                var mat = new Material(material);
                mat.color = RandomColor();

                poly.ToUnityMesh("Convex", mat);
            }

        }

        private Color RandomColor()
        {
            Color col = new Color();
            col.r = Random.value;
            col.g = Random.value;
            col.b = Random.value;
            return col;
        }

    }

}
