using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Polyhedra
{

    public class PolyhedronExample : MonoBehaviour
    {
        public Material material;

        public bool allowQuads = false;

        public bool drawSegments = false;

        public Color lineColor = Color.black;

        private GameObject m_cube;

        private GameObject m_uvsphere;

        private GameObject m_normalizedCube;

        private GameObject m_icosahedron;

        private GameObject m_plane;

        private GameObject m_torus;

        private GameObject m_tetrahedron;

        private GameObject m_octohedron;

        private GameObject m_dodecahedron;

        private GameObject m_cylinder;

        private GameObject m_cone;

        private SegmentRenderer m_triangleRenderer, m_quadRenderer;

        private void Start()
        {

            m_triangleRenderer = new SegmentRenderer();
            m_triangleRenderer.DefaultColor = lineColor;
            m_triangleRenderer.LineMode = LINE_MODE.TRIANGLES;

            m_quadRenderer = new SegmentRenderer();
            m_quadRenderer.DefaultColor = lineColor;
            m_quadRenderer.LineMode = LINE_MODE.QUADS;

            m_cube = CreateCube(new Vector3(3, 0.5f, 0));

            m_uvsphere = CreateUVSphere(new Vector3(1, 0.5f, 0));

            m_normalizedCube = CreateNormalizedCube(new Vector3(-1, 0.5f, 0));

            m_icosahedron = CreateIcosahedron(new Vector3(-3, 0.5f, 0));

            m_plane = CreatePlane(new Vector3(3, 0, 3));

            m_torus = CreateTorus(new Vector3(1, 0, 3));

            m_tetrahedron = CreateTetrahedron(new Vector3(-1, 0, 3));

            m_octohedron = CreateOctohedron(new Vector3(-3, 0, 3));

            m_dodecahedron = CreateDodecahedron(new Vector3(3, 0, 6));

            m_cylinder = CreateCylinder(new Vector3(1, 0, 6));

            m_cone = CreateCone(new Vector3(-1, 0, 6));
        }

        private void OnRenderObject()
        {
            if (drawSegments)
            {
                m_triangleRenderer.Draw();
                m_quadRenderer.Draw();
            }
        }

        private GameObject CreateCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateCube(1, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Cube", translation, material, true);
        }

        private GameObject CreateUVSphere(Vector3 translation)
        {
            var param = UVSphereParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateUVSphere(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("UV sphere", translation, material, true);
        }

        private GameObject CreateNormalizedCube(Vector3 translation)
        {
            var param = NormalizedCubeParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateNormalizedCube(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Normalized cube", translation, material, true);
        }

        private GameObject CreateTetrahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateTetrahedron();

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Tetrahedron", translation, material, true);
        }

        private GameObject CreateIcosahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateIcosahedron();

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Icosahedron", translation, material, true);
        }

        private GameObject CreateOctohedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateOctahedron();

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Octahedron", translation, material, true);
        }

        private GameObject CreateDodecahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateDodecahedron();

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Dodecahedron", translation, material, true);
        }

        private GameObject CreatePlane(Vector3 translation)
        {
            var param = PlaneParams.Default;
            var poly = PolyhedronFactory<EEK>.CreatePlane(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Plane", translation, material, true);
        }

        private GameObject CreateTorus(Vector3 translation)
        {
            var param = TorusParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateTorus(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Torus", translation, material, true);
        }

        private GameObject CreateCylinder(Vector3 translation)
        {
            var param = CylinderParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateCylinder(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Cylinder", translation, material, true);
        }

        private GameObject CreateCone(Vector3 translation)
        {
            var param = CylinderParams.Default;
            param.radiusTop = 0;
            var poly = PolyhedronFactory<EEK>.CreateCylinder(param, allowQuads);

            if (drawSegments)
                DrawSegments(poly, translation);

            return poly.ToUnityMesh("Cone", translation, material, true);
        }

        private void DrawSegments(Polyhedron3 poly, Vector3 translation)
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
            for(int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                vectors[i] = new Vector3((float)p.x, (float)p.y, (float)p.z) + translation;
            }

            return vectors;
        }
    }

}
