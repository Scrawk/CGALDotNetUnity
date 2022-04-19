using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;
using Common.Unity.Drawing;

namespace CGALDotNetUnity.Processing
{

    public class HeatMethodExample : MonoBehaviour
    {
        public Material material;

        public string file;

        public Color wireframeColor = Color.black;

        private SurfaceMesh3<EEK> m_mesh;

        private GameObject m_object;

        private Texture2D m_gradient;

        private SegmentRenderer m_wireframe;

        private void Start()
        {
            //Used for debuging.
            //dont recommend loading meshes like this.
            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            m_mesh = new SurfaceMesh3<EEK>();
            m_mesh.ReadOFF(filename);
            m_mesh.Rotate(Quaternion3d.RotateY(180));

            var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RayCast(ray);

            m_wireframe = RendererBuilder.CreateWireframeRenderer(m_mesh, wireframeColor, 0.001f);
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
            var gradient = CreateGradient();
            var colors = new Color[distances.Count];

            for(int i = 0; i < distances.Count; i++)
            {
                var a = (float)distances[i];
                colors[i] = gradient.GetPixelBilinear(a, 0);
            }

            return colors;
        }

        private GameObject CreateGameobject(string name, SurfaceMesh3<EEK> poly, Color[] colors)
        {
            return poly.ToUnityMesh(name, material, colors, false);
        }

        private Texture2D CreateGradient()
        {
            if (m_gradient != null)
                return m_gradient;

            var colors = new Color[]
            {
                Color.red,
                Color.yellow,
                Color.white
            };

            m_gradient = new Texture2D(colors.Length, 1);
            m_gradient.wrapMode = TextureWrapMode.Clamp;
            m_gradient.SetPixels(colors);
            m_gradient.Apply();

            return m_gradient;
        }

        private bool RayCast(Ray ray)
        {
   
            if (m_mesh.LocateVertex(ray.ToCGALRay3d(), 0.01, out MeshVertex3 vertex))
            {
                Debug.Log(vertex);

                if (m_object != null)
                    Destroy(m_object);

                var distances = GetDistances(m_mesh, vertex.Index);
                var colors = DistancesToColors(distances);

                m_object = CreateGameobject(name, m_mesh, colors);

                return true;
            }
            else
            {
                if (m_object != null)
                    Destroy(m_object);

                return false;
            }
        }

        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RayCast(ray);
            }

            if(Input.GetKeyDown(KeyCode.Tab))
            {
                m_wireframe.Enabled = !m_wireframe.Enabled;
            }
        }

        private void OnRenderObject()
        {
            if(m_object != null)
            {
                m_wireframe.SetColor(wireframeColor);
                m_wireframe.Draw();
            }

        }

    }

}
