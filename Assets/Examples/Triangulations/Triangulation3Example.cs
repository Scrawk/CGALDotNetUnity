using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Triangulations
{

    public class Triangulation3Example : MonoBehaviour
    {

        public Material vertexMaterial;

        public Material edgeMaterial;

        private GameObject m_triangulation;

        void Start()
        {
            var box = new Box3d(-5, 5);
            var corners = box.GetCorners();

            var tri = new Triangulation3<EEK>(corners);

            var points = new List<Point3d>();
            tri.GetPoints(points);

            var segments = new List<SegmentIndex>();
            tri.GetSegmentsIndices(segments);

            m_triangulation = new GameObject("Triangulation");

            foreach(var p in points)
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = m_triangulation.transform;
                sphere.GetComponent<Renderer>().sharedMaterial = vertexMaterial;
                sphere.transform.position = ToVector3(p);
                sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            foreach(var seg in segments)
            {
                var a = points[seg.A];
                var b = points[seg.B];

                CreateCylinderBetweenPoints(ToVector3(a), ToVector3(b), 0.1f);
            }

        }

        private void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width)
        {
            var offset = end - start;
            var scale = new Vector3(width, offset.magnitude / 2.0f, width);
            var position = start + (offset / 2.0f);

            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = m_triangulation.transform;
            cylinder.GetComponent<Renderer>().sharedMaterial = edgeMaterial;
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
