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

        public Color lineColor = Color.black;

        public string file;

        private SegmentRenderer m_triangleRenderer, m_quadRenderer;

        private bool createdSegments;

        private Polyhedron3<EEK> poly;

        private Vector3 translation = Vector3.zero;

        private GameObject m_object;

        private void Start()
        {
            //Used for debuging.
            //dont recommend loading meshes like this.

            m_triangleRenderer = new SegmentRenderer();
            m_triangleRenderer.DefaultColor = lineColor;
            m_triangleRenderer.LineMode = LINE_MODE.TRIANGLES;

            m_quadRenderer = new SegmentRenderer();
            m_quadRenderer.DefaultColor = lineColor;
            m_quadRenderer.LineMode = LINE_MODE.QUADS;

            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            poly = new Polyhedron3<EEK>();
            poly.ReadOFF(filename);
            //poly.Triangulate();

            m_object = CreateGameobject(name, translation, poly);

            Print(poly);

            LookAt(m_object);
        }

        private void Print(Polyhedron3 poly)
        {
            var builder = new StringBuilder();
            poly.Print(builder);
            Debug.Log(builder.ToString());
        }

        private void OnRenderObject()
        {
            if (drawSegments)
            {
                if (!createdSegments)
                {
                    createdSegments = true;
                    CreateSegments(poly, translation);
                }

                m_triangleRenderer.Draw();
                m_quadRenderer.Draw();
            }
        }

        private GameObject CreateGameobject(string name, Vector3 translation, Polyhedron3<EEK> poly)
        {
            if (drawSegments)
            {
                createdSegments = true;
                CreateSegments(poly, translation);
            }

            return poly.ToUnityMesh(name, translation, material, false);
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

        private void CreateSegments(Polyhedron3 poly, Vector3 translation)
        {
            var primatives = poly.GetPrimativeCount();
            var points = new Point3d[poly.VertexCount];
            poly.GetPoints(points, points.Length);

            var vectors = ToVector3(points, translation);

            if (primatives.triangleCount > 0)
            {
                var triangles = new int[primatives.triangleCount * 3];
                poly.GetTriangleIndices(triangles, triangles.Length);

                m_triangleRenderer.Load(vectors, triangles);
            }

            if (primatives.quadCount > 0)
            {
                var quads = new int[primatives.quadCount * 4];
                poly.GetQuadIndices(quads, quads.Length);

                m_quadRenderer.Load(vectors, quads);
            }
        }

        private Vector3[] ToVector3(Point3d[] points, Vector3 translation)
        {
            var vectors = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                vectors[i] = new Vector3((float)p.x, (float)p.y, (float)p.z) + translation;
            }

            return vectors;
        }
    }

}
