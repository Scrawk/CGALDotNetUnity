using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Triangulations;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Polyhedra;
using CGALDotNetGeometry.Extensions;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Triangulations
{

    public class Triangulation3Example : MonoBehaviour
    {

        public Material vertexMaterial;

        public Material edgeMaterial;

        public Material hullMaterial;

        private GameObject m_triangulationGO;

        private GameObject m_hull;

        private List<GameObject> m_spheres = new List<GameObject>();

        private List<GameObject> m_edges = new List<GameObject>();

        private DelaunayTriangulation3<EEK> m_triangulation;

        void Start()
        {
            var box = new Box3d(-20, 20);
            var randomPoints = Point3d.RandomPoints(1, 10, box);

            m_triangulation = new DelaunayTriangulation3<EEK>(randomPoints);

            CreateTriangulation();

        }
        
        private void DestroyObjects()
        {
            Destroy(m_triangulationGO);
            Destroy(m_hull);

            foreach (var sphere in m_spheres)
                Destroy(sphere);

            m_spheres.Clear();

            foreach (var edge in m_edges)
                Destroy(edge);

            m_edges.Clear();
        }

        private void CreateTriangulation()
        {
            DestroyObjects();

            if (hullMaterial != null)
            {
                var hull = m_triangulation.ComputeHull();
                m_hull = hull.ToUnityMesh("hull", hullMaterial);
            }

            var points = new Point3d[m_triangulation.VertexCount];
            m_triangulation.GetPoints(points, points.Length);
 
            var verts = new TriVertex3[m_triangulation.VertexCount];
            m_triangulation.GetVertices(verts, verts.Length);

            var segments = new int[m_triangulation.EdgeCount];
            m_triangulation.GetSegmentIndices(segments, segments.Length);

            for (int i = 0; i < segments.Length / 2; i++)
            {
                int i0 = segments[i * 2 + 0];
                int i1 = segments[i * 2 + 1];

                Debug.Log(i0 + " " + i1);
            }

                //var segments = new List<SegmentIndex>();
                //m_triangulation.GetUniqueSegmentsIndices(segments);
                //m_triangulation.GetTetrahedronToSegmentIndices(segments);

                m_triangulationGO = new GameObject("Triangulation");

            if (vertexMaterial != null)
            {
                for(int i = 0; i < verts.Length; i++)
                {
                    var v = verts[i];
                    if (v.IsInfinite) continue;

                    var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.parent = m_triangulationGO.transform;

                    var renderer = sphere.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.sharedMaterial = vertexMaterial;

                    sphere.transform.position = ToVector3(v.Point);
                    sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    sphere.layer = LayerMask.NameToLayer("Geometry");
                    sphere.name = v.Index.ToString();

                    m_spheres.Add(sphere);
                }
            }

            if (edgeMaterial != null)
            {
                for (int i = 0; i < segments.Length/2; i++)
                {
                    int i0 = segments[i * 2 + 0];
                    int i1 = segments[i * 2 + 1];

                    if (i0 < 0 || i0 >= points.Length) continue;
                    if (i1 < 0 || i1 >= points.Length) continue;

                    var a = points[i0];
                    var b = points[i1];

                    if (!a.IsFinite || !b.IsFinite) continue;

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                }


                /*
                foreach (var seg in segments)
                {
                    if (seg.A < 0 || seg.A >= points.Length) continue;
                    if (seg.B < 0 || seg.B >= points.Length) continue;

                    var a = points[seg.A];
                    var b = points[seg.B];

                    if (!a.IsFinite || !b.IsFinite) continue;

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                }
                */
            }
        }

        private void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
        {
            var offset = end - start;
            var scale = new Vector3(width, offset.magnitude / 2.0f, width);
            var position = start + (offset / 2.0f);

            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = m_triangulationGO.transform;

            var renderer = cylinder.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = edgeMaterial;

            cylinder.transform.position = position;
            cylinder.transform.up = offset;
            cylinder.transform.localScale = scale;

            m_edges.Add(cylinder);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;

                    int i = int.Parse(objectHit.name);

                    if (m_triangulation.GetVertex(i, out TriVertex3 vert))
                    {
                        Debug.Log(vert);
                    }

                    
                }
            }

            
        }


        private Vector3 ToVector3(Point3d point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }

    }
}
