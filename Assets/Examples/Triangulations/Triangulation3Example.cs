using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Triangulations;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Polyhedra;
using CGALDotNet.Extensions;
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

            m_triangulation.PrintObjectToUnity();

            var points = new Point3d[m_triangulation.VertexCount];
            m_triangulation.GetPoints(points, points.Length);
            points.Round(2);

            foreach (var p in points)
            {
                //Debug.Log(p);
            }

            var verts = new TriVertex3[m_triangulation.VertexCount];
            m_triangulation.GetVertices(verts, verts.Length);
            verts.Round(2);

            foreach (var v in verts)
            {
                //Debug.Log(v);
            }

            var segments = new Segment3d[m_triangulation.EdgeCount];
            m_triangulation.GetSegments(segments, segments.Length);

            Debug.Log("Segments = " + segments.Length);

            //var triangles = new int[m_triangulation.TriangleCount * 3];
            //m_triangulation.GetTriangleIndices(triangles, triangles.Length);

            //var tetrahedrons = new int[m_triangulation.TetrahedronCount * 4];
            //m_triangulation.GetTetrahedronIndices(tetrahedrons, tetrahedrons.Length);

            //var cells = new TriCell3[m_triangulation.TetrahedronCount];
            //m_triangulation.GetCells(cells, cells.Length);

            //var segments2 = segments.RemoveDuplicateSegments();
            //var triangles2 = triangles.RemoveDuplicateTriangles();

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
                int edges = 0;
                for (int i = 0; i < segments.Length; i++)
                {
                    var seg = segments[i];
                    var a = seg.A;
                    var b = seg.B;

                    //Debug.Log("Create edge " + A + " " + B);

                    if (!a.IsFinite)
                    {
                        //Debug.Log("a is not finite");
                        //Debug.Log(a);
                        continue;
                    }

                    if (!b.IsFinite)
                    {
                        //Debug.Log("b is not finite");
                        //Debug.Log(b);
                        continue;
                    }

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);

                    edges++;
                    //Debug.Log("Create edge " + a + " " + b);
                }

                //Debug.Log("Created " + edges + " edges)");

                /*
                for (int i = 0; i < segments2.Count; i++)
                {
                    var s = segments2[i];

                    if (s.A < 0 || s.A >= points.Length) continue;
                    if (s.B < 0 || s.B >= points.Length) continue;

                    var a = points[s.A];
                    var b = points[s.B];
        
                    if (!a.IsFinite || !b.IsFinite) continue;

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                }
                */

                /*
                for (int i = 0; i < segments.Length/ 2; i++)
                {
                    int A = segments[i * 2 + 0];
                    int B = segments[i * 2 + 1];

                    if (A < 0 || A >= points.Length) continue;
                    if (B < 0 || B >= points.Length) continue;

                    var a = points[A];
                    var b = points[B];

                    if (!a.IsFinite || !b.IsFinite) continue;

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                }

                /*
                for (int i = 0; i < triangles2.Count; i++)
                {
                    var t = triangles2[i];

                    //if (t.A < 0 || t.A >= points.Length) continue;
                    //if (t.B < 0 || t.B >= points.Length) continue;
                    //if (t.C < 0 || t.C >= points.Length) continue;

                    var a = points[t.A];
                    var b = points[t.B];
                    var c = points[t.C];

                    if (!a.IsFinite || !b.IsFinite || !c.IsFinite) continue;

                    //CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                    //CreateCylinderBetweenPoints(ToVector3(a), ToVector3(c), 0.1f);
                    //CreateCylinderBetweenPoints(ToVector3(c), ToVector3(b), 0.1f);
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

            Debug.Log(scale);

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

                    Debug.Log(i);

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
