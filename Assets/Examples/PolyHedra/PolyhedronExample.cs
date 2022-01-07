using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;

namespace CGALDotNetUnity.Polyhedra
{

    public class PolyhedronExample : MonoBehaviour
    {
        public Material material;

        private GameObject m_cube;

        private GameObject m_uvsphere;

        private GameObject m_normalizedCube;

        private GameObject m_icosahedron;

        private void Start()
        {
            m_cube = CreateCube(new Vector3(3, 0.5f, 0));

            m_uvsphere = CreateUVSphere(new Vector3(1, 0.5f, 0));

            m_normalizedCube = CreateNormalizedCube(new Vector3(-1, 0.5f, 0));

            m_icosahedron = CreateIcosahedron(new Vector3(-3, 0.5f, 0));
        }

        private GameObject CreateCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateCube();
            return poly.ToUnityMesh("Cube", translation, material, true);
        }

        private GameObject CreateUVSphere(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateUVSphere(32, 32);
            return poly.ToUnityMesh("UV sphere", translation, material, true);
        }

        private GameObject CreateNormalizedCube(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateNormalizedCube(16);
            return poly.ToUnityMesh("Normalized cube", translation, material, true);
        }

        private GameObject CreateIcosahedron(Vector3 translation)
        {
            var poly = PolyhedronFactory<EEK>.CreateIcosahedron();
            return poly.ToUnityMesh("Icosahedron", translation, material, true);
        }
    }

}
