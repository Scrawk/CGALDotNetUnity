using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Arrangements;

namespace CGALDotNetUnity.Polygons
{
    public class Arrangement2Example : MonoBehaviour
    {

        private Arrangement2<EEK> arrangement;

        private List<BaseRenderer> Renderers { get; set; }

        void Start()
        {
            Point2d p1 = new Point2d(-2, -2);
            Point2d p2 = new Point2d(-2, 2);
            Point2d p3 = new Point2d(2, -2);

            var segments = new Segment2d[]
            {
                new Segment2d(p1, p2),
                new Segment2d(p2, p3),
                new Segment2d(p3, p1)
            };

            arrangement = new Arrangement2<EEK>(segments);

            Renderers = new List<BaseRenderer>();

            segments = new Segment2d[arrangement.EdgeCount];
            arrangement.GetSegments(segments);
            AddSegments(segments, Color.blue);

            var points = new Point2d[arrangement.VertexCount];
            arrangement.GetPoints(points);
            AddPoints(points, Color.yellow);
        }

        private void Update()
        {

        }

        private void OnRenderObject()
        {
            foreach (var renderer in Renderers)
                renderer.Draw();
        }

        protected void AddSegments(Segment2d[] segments, Color color)
        {
            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.Color = color;
            lines.Load(ToVector2(segments));
            Renderers.Add(lines);
        }

        protected void AddPoints(Point2d[] points, Color color)
        {
            var verts = new VertexRenderer(0.02f);
            verts.Orientation = DRAW_ORIENTATION.XY;
            verts.Color = color;
            verts.Load(ToVector2(points));
            Renderers.Add(verts);
        }

        private Vector2[] ToVector2(Segment2d[] segments)
        {
            var array = new Vector2[segments.Length * 2];

            for (int i = 0; i < segments.Length; i++)
            {
                var a = segments[i].A;
                var b = segments[i].B;

                array[i * 2 + 0] = new Vector2((float)a.x, (float)a.y);
                array[i * 2 + 1] = new Vector2((float)b.x, (float)b.y);
            }
                
            return array;
        }

        private Vector2[] ToVector2(Point2d[] points)
        {
            var array = new Vector2[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                array[i] = new Vector2((float)p.x, (float)p.y);
            }
                

            return array;
        }

    }

}
