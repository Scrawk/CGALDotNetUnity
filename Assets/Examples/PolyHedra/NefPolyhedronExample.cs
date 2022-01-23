using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using Common.Unity.Drawing;

namespace CGALDotNetUnity.Polyhedra
{

    public class NefPolyhedronExample : MonoBehaviour
    {
        public Material material;

        private GameObject m_mesh;

        private CompositeRenderer m_wireframeRender;

        private void Start()
        {
            var box1 = PolyhedronFactory<EEK>.CreateCube();
            box1.Translate(new Point3d(0.5));

            var box2 = PolyhedronFactory<EEK>.CreateCube();

            var nef1 = new NefPolyhedron3<EEK>(box1);
            var nef2 = new NefPolyhedron3<EEK>(box2);

            var nef3 = nef1.Join(nef2);

            if (nef3.ConvertToPolyhedron(out Polyhedron3<EEK> poly))
            {
                m_mesh = poly.ToUnityMesh("Mesh", material, true);
                m_mesh.transform.position = new Vector3(0, 0.5f, 0);

                m_wireframeRender = RendererBuilder.CreateWireframeRenderer(poly, Color.black);
            }

        }

        private void OnRenderObject()
        {
            m_wireframeRender?.SetLocalToWorld(m_mesh.transform.localToWorldMatrix);
            m_wireframeRender?.Draw();
        }

    }

}
