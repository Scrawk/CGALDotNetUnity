using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Arrangements;

namespace CGALDotNetUnity.Polygons
{
    public class Arrangement2Example : InputBehaviour
    {

        private Color lineColor = new Color32(20, 20, 20, 255);

        private Color pointColor = new Color32(200, 80, 80, 255);

        private Color circumColor = new Color32(200, 80, 80, 255);

        private Color constraintColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(120, 120, 120, 128);

        private Arrangement2<EEK> arrangement;

        protected override void Start()
        {
            base.Start();

            SetInputMode(INPUT_MODE.POINT);
            SetPointSize(0.5f);
            SetInputColor(lineColor, pointColor);
            EnableInputPointOutline(true, lineColor);

            Point2d p1 = new Point2d(-5, -5);
            Point2d p2 = new Point2d(-5, 5);
            Point2d p3 = new Point2d(5, -5);

            var segments = new Segment2d[]
            {
                new Segment2d(p1, p2),
                new Segment2d(p2, p3),
                new Segment2d(p3, p1)
            };

            arrangement = new Arrangement2<EEK>(segments);

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
                arrangement.InsertSegment(new Segment2d(points[0], points[1]), false);
            }

            AddArrangement();
        }

        private void AddArrangement()
        {
            ClearShapeRenderers();

            var segments = new Segment2d[arrangement.EdgeCount];
            arrangement.GetSegments(segments);
            AddSegments("", segments, lineColor);

            var points = new Point2d[arrangement.VertexCount];
            arrangement.GetPoints(points);
            AddPoints("", points, PointSize, pointColor);

            EnableShapePointOutline(true, lineColor);
        }

        private void OnRenderObject()
        {
            DrawGrid();
            DrawShapes();
            DrawInput();
            DrawPoint();
        }


    }

}
