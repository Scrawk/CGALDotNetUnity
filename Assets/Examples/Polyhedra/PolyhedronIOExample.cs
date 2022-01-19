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

        private SegmentRenderer m_triangleRenderer, m_quadRenderer;

        private NormalRenderer m_vertNormalRenderer, m_faceNormalRenderer;

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

            m_vertNormalRenderer = new NormalRenderer();
            m_vertNormalRenderer.DefaultColor = vertexNormalColor;

            m_faceNormalRenderer = new NormalRenderer();
            m_faceNormalRenderer.DefaultColor = faceNormalColor;

            string filename = Application.dataPath + "/Examples/Data/" + file;

            var split = filename.Split('/', '.');
            int i = split.Length - 2;
            var name = i > 0 ? split[i] : "Mesh";

            poly = new Polyhedron3<EEK>();
            poly.ReadOFF(filename);
            //poly.Triangulate();

            m_object = CreateGameobject(name, translation, poly);

            CreateSegments(poly, translation);
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
                m_triangleRenderer.Draw();
                m_quadRenderer.Draw();
            }

            if (drawVertexNormals)
                m_vertNormalRenderer.Draw();

            if (drawFaceNormals)
                m_faceNormalRenderer.Draw();
        }

        private GameObject CreateGameobject(string name, Vector3 translation, Polyhedron3<EEK> poly)
        {
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

            var centroids = new Point3d[poly.FaceCount];
            poly.GetCentroids(centroids, centroids.Length);

            var vertNormals = new Vector3d[poly.VertexCount];
            poly.ComputeVertexNormals();
            poly.GetVertexNormals(vertNormals, vertNormals.Length);

            var faceNormals = new Vector3d[poly.FaceCount];
            poly.ComputeFaceNormals();
            poly.GetFaceNormals(faceNormals, faceNormals.Length);

            var upoints = ToVector3(points, translation);
            var ucentroids = ToVector3(centroids, translation);
            var vnormals = ToVector3(vertNormals, 0.01f);
            var fnormals = ToVector3(faceNormals, 0.01f);

            m_vertNormalRenderer.Load(upoints, vnormals);
            m_faceNormalRenderer.Load(ucentroids, fnormals);

            if (primatives.triangleCount > 0)
            {
                var triangles = new int[primatives.triangleCount * 3];
                poly.GetTriangleIndices(triangles, triangles.Length);

                m_triangleRenderer.Load(upoints, triangles);
            }

            if (primatives.quadCount > 0)
            {
                var quads = new int[primatives.quadCount * 4];
                poly.GetQuadIndices(quads, quads.Length);

                m_quadRenderer.Load(upoints, quads);
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

        private Vector3[] ToVector3(Vector3d[] normals, float scale)
        {
            var vectors = new Vector3[normals.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                var p = normals[i];
                vectors[i] = new Vector3((float)p.x, (float)p.y, (float)p.z) * scale;
            }

            return vectors;
        }
    }

}
