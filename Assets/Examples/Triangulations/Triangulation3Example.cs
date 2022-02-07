using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Triangulations;
using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;
using CGALDotNet.Polyhedra;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Triangulations
{

    public class Triangulation3Example : MonoBehaviour
    {

        public Material vertexMaterial;

        public Material edgeMaterial;

        public Material hullMaterial;

        private GameObject m_triangulation;

        private GameObject m_hull;

        void Start()
        {
            var box = new Box3d(-20, 20);
            var randomPoints = Point3d.RandomPoints(0, 20, box);

            var tri = new DelaunayTriangulation3<EEK>(randomPoints);
            //tri.Refine(0.1, 1);

            if(hullMaterial != null)
            {
                var hull = tri.ComputeHull();
                m_hull = hull.ToUnityMesh("hull", hullMaterial);
            }

            var points = new List<Point3d>();
            tri.GetPoints(points);

            var segments = new List<SegmentIndex>();
            tri.GetSegmentsIndices(segments);

            m_triangulation = new GameObject("Triangulation");

            if (vertexMaterial != null)
            {
                foreach (var p in points)
                {
                    if (!p.IsFinite) continue;

                    var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.parent = m_triangulation.transform;

                    var renderer = sphere.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.sharedMaterial = vertexMaterial;

                    sphere.transform.position = ToVector3(p);
                    sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }

            if (edgeMaterial != null)
            {
                foreach (var seg in segments)
                {
                    var a = points[seg.A];
                    var b = points[seg.B];

                    if (!a.IsFinite || !b.IsFinite) continue;

                    CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
                }
            }

        }

        private void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
        {
            var offset = end - start;
            var scale = new Vector3(width, offset.magnitude / 2.0f, width);
            var position = start + (offset / 2.0f);

            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = m_triangulation.transform;

            var renderer = cylinder.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = edgeMaterial;

            cylinder.transform.position = position;
            cylinder.transform.up = offset;
            cylinder.transform.localScale = scale;
        }

        private Vector3 ToVector3(Point3d point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }

    }
}
