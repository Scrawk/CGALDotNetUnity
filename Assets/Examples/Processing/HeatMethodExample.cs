using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNetGeometry.Numerics;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;

namespace CGALDotNetUnity.Processing
{

    public class HeatMethodExample : MonoBehaviour
    {
        public Material material;

        public string file;

        private SurfaceMesh3<EEK> poly;

        private GameObject m_object;

        private void Start()
        {
            //Used for debuging.
            //dont recommend loading meshes like this.
            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            poly = new SurfaceMesh3<EEK>();
            poly.ReadOFF(filename);
            poly.Rotate(Quaternion3d.RotateY(180));

            var distances = GetDistances(poly, 0);
            var colors = DistancesToColors(distances);

            m_object = CreateGameobject(name, poly, colors);
        }

        private List<double> GetDistances(SurfaceMesh3<EEK> poly, int index)
        {
            var hm = HeatMethod<EEK>.Instance;

            var distances = new List<double>();
            var max = hm.EstimateGeodesicDistances(poly, index, distances);

            for (int i = 0; i < distances.Count; i++)
                distances[i] = distances[i] / max;

            return distances;
        }

        private Color[] DistancesToColors(List<double> distances)
        {
            var colors = new Color[distances.Count];

            for(int i = 0; i < distances.Count; i++)
            {
                var d = (float)distances[i];
                colors[i] = new Color(d, 0, 0, 1);
            }

            return colors;
        }

        private GameObject CreateGameobject(string name, SurfaceMesh3<EEK> poly, Color[] colors)
        {
            return poly.ToUnityMesh(name, material, colors, false);
        }

    }

}
