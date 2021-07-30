using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Arrangements;
using CGALDotNet.Polygons;
using CGALDotNet.Hulls;

namespace CGALDotNetUnity.Polygons
{
    public class Arrangement2Example : InputBehaviour
    {

        private Color lineColor = new Color32(20, 20, 20, 255);

        private Color pointColor = new Color32(80, 80, 200, 255);

        private Color inputColor = new Color32(80, 200, 80, 255);

        private Color circumColor = new Color32(200, 80, 80, 255);

        private Color constraintColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private Arrangement2<EEK> arrangement;

        protected override void Start()
        {
            base.Start();

            //SetInputMode(INPUT_MODE.SEGMENT);
            //SetPointSize(0.5f);
            //SetInputColor(lineColor, inputColor);
            //EnableInputPointOutline(true, lineColor);

            Point2d p1 = new Point2d(-5, -5);
            Point2d p2 = new Point2d(-5, 5);
            Point2d p3 = new Point2d(5, -5);

            var segments = new Segment2d[]
            {
                new Segment2d(p1, p2),
                new Segment2d(p2, p3),
                new Segment2d(p3, p1)
            };

            var points = new Point2d[]
            {
                new Point2d(-5, -5),
                new Point2d(-5, 5),
                new Point2d(5, -5)
            };

            int count = 100;
            int seed = 0;
            var rnd = RandomPoints(count, seed);
            var hull = ConvexHull2<EEK>.Instance.CreateHull(rnd, 0, count);

            arrangement = new Arrangement2<EEK>();
            arrangement.InsertPolygon(hull, true);

            AddArrangement();
        }

        protected override void OnInputComplete(List<Point2d> points)
        {
            if (points.Count == 1)
            {
                arrangement.InsertPoint(points[0]);
            }
            else if (points.Count == 2)
            {
                var seg = new Segment2d(points[0], points[1]);

                if(arrangement.IntersectsSegment(seg))
                {
                    arrangement.InsertSegment(seg, false);
                    Debug.Log("Is valid = " + arrangement.IsValid());
                }
                else
                {
                    arrangement.InsertSegment(seg, true);
                    Debug.Log("Is valid = " + arrangement.IsValid());
                }

            }

            AddArrangement();
        }

        private void AddArrangement()
        {
            
            //ClearShapeRenderers();

            var segments = new Segment2d[arrangement.EdgeCount];
            arrangement.GetSegments(segments);
            //AddSegments("", GetSegments(), lineColor);

            var points = new Point2d[arrangement.VertexCount];
            arrangement.GetPoints(points);
            //AddPoints("", points, PointSize, pointColor);

            ClearSnapTargets();
            AddSnapTargets(points);

            //EnableShapePointOutline(true, lineColor);
        }

        private void OnRenderObject()
        {
            DrawGrid();
            //DrawShapes();
            //DrawInput();
            //DrawPoint();
        }

        private List<Segment2d> GetSegments()
        {
            var segments = new Segment2d[arrangement.EdgeCount];
            arrangement.GetSegments(segments);

            var list = new List<Segment2d>();

            foreach(var seg in segments)
            {

                var n = (Vector2d)(seg.B - seg.A);
                n = n.Normalized * PointSize;

                var offset = (Point2d)(n * PointSize * 2);
                var perp = (Point2d)(n.PerpendicularCCW * PointSize * 0.5f);

                var ab = new Segment2d(seg.A + offset, seg.B - offset);

                var ab_plus = ab + perp;
                var ab_min = ab - perp;

                list.Add(ab_plus);
                list.Add(ab_min);

                if (seg.Length > PointSize * 2)
                {
                    n = (Vector2d)(seg.A - ab_plus.A);
                    n = n.Normalized * PointSize;
                    list.Add(new Segment2d(ab_plus.A, ab_plus.A - (Point2d)n));

                    n = (Vector2d)(seg.B - ab_min.B);
                    n = n.Normalized * PointSize;
                    list.Add(new Segment2d(ab_min.B, ab_min.B - (Point2d)n));
                }

            }

            return list;
        }

        private Point2d[] RandomPoints(int count, int seed)
        {
            var points = new Point2d[count];

            var rnd = new System.Random(seed);

            for(int i = 0; i < count; i++)
            {
                double x = rnd.NextDouble(-5, 5);
                double y = rnd.NextDouble(-5, 5);

                points[i] = new Point2d(x, y);
            }

            return points;
        }


    }

}
