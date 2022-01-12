using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNet.Processing;

namespace CGALDotNetUnity.Polyhedra
{

    public class PolyhedronExample : MonoBehaviour
    {
        public Material material;

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

        private void Start()
        {
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
        }

        private GameObject CreateCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateCube();
            return poly.ToUnityMesh("Cube", translation, material, true);
        }

        private GameObject CreateUVSphere(Vector3 translation)
        {
            var param = UVSphereParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateUVSphere(param);
            return poly.ToUnityMesh("UV sphere", translation, material, true);
        }

        private GameObject CreateNormalizedCube(Vector3 translation)
        {
            var param = NormalizedCubeParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateNormalizedCube(param);
            return poly.ToUnityMesh("Normalized cube", translation, material, true);
        }

        private GameObject CreateTetrahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateTetrahedron();
            return poly.ToUnityMesh("Tetrahedron", translation, material, true);
        }

        private GameObject CreateIcosahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateIcosahedron();
            return poly.ToUnityMesh("Icosahedron", translation, material, true);
        }

        private GameObject CreateOctohedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateOctahedron();
            return poly.ToUnityMesh("Octahedron", translation, material, true);
        }

        private GameObject CreateDodecahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateDodecahedron();
            return poly.ToUnityMesh("Dodecahedron", translation, material, true);
        }

        private GameObject CreatePlane(Vector3 translation)
        {
            var param = PlaneParams.Default;
            var poly = PolyhedronFactory<EEK>.CreatePlane(param);
            return poly.ToUnityMesh("Plane", translation, material, true);
        }

        private GameObject CreateTorus(Vector3 translation)
        {
            var param = TorusParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateTorus(param);
            return poly.ToUnityMesh("Torus", translation, material, true);
        }

        private GameObject CreateCylinder(Vector3 translation)
        {
            //TODO - missing the caps
            var param = CylinderParams.Default;
            var poly = PolyhedronFactory<EEK>.CreateCylinder(param);
            return poly.ToUnityMesh("Cylinder", translation, material, true);
        }
    }

}
