using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Polyhedra
{

    public class PolyhedronIOExample : MonoBehaviour
    {
        public Material material;

        public bool drawSegments = false;

        public bool drawVertexNormals = false;

        public bool drawFaceNormals = false;

        public Color lineColor = Color.black;

        public Color vertexNormalColor = Color.red;

        public Color faceNormalColor = Color.blue;

        public string file;

        private CompositeRenderer m_wirefameRender;

        private NormalRenderer m_vertNormalRenderer, m_faceNormalRenderer;

        private Polyhedron3<EEK> poly;

        private GameObject m_object;

        private void Start()
        {
            //Used for debuging.
            //dont recommend loading meshes like this.

            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            poly = new Polyhedron3<EEK>();
            poly.ReadOFF(filename);
            //poly.Triangulate();

            m_object = CreateGameobject(name, poly);

            CreateSegments(poly);
            LookAt(m_object);

            poly.PrintToUnity();
        }

        private void OnRenderObject()
        {
            if (drawSegments)
            {
                m_wirefameRender.SetColor(lineColor);
                m_wirefameRender.Draw();
            }

            if (drawVertexNormals)
            {
                m_vertNormalRenderer.SetColor(vertexNormalColor);
                m_vertNormalRenderer.Draw();
            }
                
            if (drawFaceNormals)
            {
                m_faceNormalRenderer.SetColor(faceNormalColor);
                m_faceNormalRenderer.Draw();
            }
                
        }

        private GameObject CreateGameobject(string name, Polyhedron3<EEK> poly)
        {
            return poly.ToUnityMesh(name, material, false);
        }

        private void LookAt(GameObject go)
        {
            var filter = go.GetComponent<MeshFilter>();
            if (filter == null) return;

            var bounds = filter.sharedMesh.bounds;
            var size = bounds.size;
            var center = bounds.center;

            Camera.main.transform.position = center + new Vector3(0, 0, size.z * 5);
            Camera.main.transform.LookAt(center, Vector3.up);
        }

        private void CreateSegments(Polyhedron3 poly)
        {
            m_vertNormalRenderer = RendererBuilder.CreateVertexNormalRenderer(poly, vertexNormalColor, 0.01f);
            m_faceNormalRenderer = RendererBuilder.CreateFaceNormalRenderer(poly, faceNormalColor, 0.01f);
            m_wirefameRender = RendererBuilder.CreateWireframeRenderer(poly, lineColor);
        }
    }

}
