using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Triangulations;

using Common.Unity.Drawing;

namespace CGALDotNetUnity.Triangulations
{

    public class Triangulation3Example : InputBehaviour
    {

        private Dictionary<string, CompositeRenderer> Renderers;

        private SegmentRenderer m_segmentRenderer;

        protected override void Start()
        {
            base.Start();

            Renderers = new Dictionary<string, CompositeRenderer>();

            var box = new Box3d(-5, 5);
            var corners = box.GetCorners();

            var tri = new Triangulation3<EEK>(corners);

            var points = new Point3d[tri.VertexCount];
            tri.GetPoints(points, points.Length);

            Renderers["Points"] = Draw().
            Points(points, Color.yellow, 0.25f).
            PopRenderer();

            var segments = ToTetrahedronSegmentIndices(tri);

            //Debug.Log("Segments = " + segments.Count);
            segments = GetUniqueSegments(segments);
            //Debug.Log("Segments = " + segments.Count);

            DrawSegments(segments, points);

        }

        private List<SegmentIndex> GetUniqueSegments(List<SegmentIndex> segments)
        {
            var set = new HashSet<SegmentIndex>();
            var list = new List<SegmentIndex>();

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                var opp = seg.Reversed;

                if (!set.Contains(seg) && !set.Contains(opp))
                {
                    set.Add(seg);
                    set.Add(opp);
                    list.Add(seg);
                }
                    
            }

            return list;
        }

        private void DrawSegments(List<SegmentIndex> segments, Point3d[] points)
        {
            m_segmentRenderer = new SegmentRenderer();
            m_segmentRenderer.DefaultColor = Color.red;

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                m_segmentRenderer.Load(ToVector3(points[seg.A]), ToVector3(points[seg.B]));
            }

        }

        private List<SegmentIndex> ToTetrahedronSegmentIndices(Triangulation3<EEK> tri)
        {

            int count = tri.TetrahdronIndiceCount;
            var indices = new int[count];

            tri.GetTetrahedronIndices(indices, count);

            var segments = new List<SegmentIndex>();

            for (int i = 0; i < indices.Length / 4; i++)
            {
                int i0 = indices[i * 4 + 0];
                int i1 = indices[i * 4 + 1];
                int i2 = indices[i * 4 + 2];
                int i3 = indices[i * 4 + 3];

                var i01 = new SegmentIndex(i0, i1);
                var i02 = new SegmentIndex(i0, i2);
                var i03 = new SegmentIndex(i0, i3);

                var i12 = new SegmentIndex(i1, i2);
                var i23 = new SegmentIndex(i2, i3);
                var i31 = new SegmentIndex(i3, i1);

                if (!i01.HasNullIndex) segments.Add(i01);
                if (!i02.HasNullIndex) segments.Add(i02);
                if (!i03.HasNullIndex) segments.Add(i03);

                if (!i12.HasNullIndex) segments.Add(i12);
                if (!i23.HasNullIndex) segments.Add(i23);
                if (!i31.HasNullIndex) segments.Add(i31);

            }

            return segments;
        }

        private Vector3 ToVector3(Point3d point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }

        private void OnPostRender()
        {
            m_segmentRenderer.Draw();

            foreach (var renderer in Renderers.Values)
                renderer.Draw();
        }


    }
}
