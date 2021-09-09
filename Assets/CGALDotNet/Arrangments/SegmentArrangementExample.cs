using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Colors;
using Common.Unity.Utility;
using CGeom2D.Points;
using CGeom2D.Geometry;
using CGeom2D.Arrangements;

namespace CGALDotNetUnity.Points
{

    public class SegmentArrangementExample : InputBehaviour
    {
        /*
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color blueColor = new Color32(80, 80, 200, 255);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private List<Segment2d> segments;


        protected override void Start()
        {
            //ConsoleRedirect.Redirect();

            PointSize = 0.2f;

            base.Start();
            //DrawGridAxis(true);
            SetInputMode(INPUT_MODE.POINT_CLICK);
            CreatePalette(0);

            segments = SegmentArrangement.RandomSegments(10, 1, 10);

            var arr = new SegmentArrangement();
            segments = arr.Run(segments);

            Debug.Log("Segments " + segments.Count);
        }

        protected override void OnCleared()
        {
            Point = null;
            SetInputMode(INPUT_MODE.POINT_CLICK);
        }

        protected override void OnLeftClickDown(Point2d point)
        {
            Point = point;
        }

        private void OnPostRender()
        {
            DrawGrid();
            DrawSegments();
            DrawPoints();
            DrawClickPoint();
        }

        private void DrawSegments()
        {
            FromSegments(segments, lineColor).Draw();
        }

        private void DrawPoints()
        {
            var points = new List<Point2d>();

            foreach (var seg in segments)
                points.Add(seg.A, seg.B);

            FromPoints(points, blueColor, lineColor, 0.2f).Draw();
        }

        private void DrawClickPoint()
        {
            if (Point != null)
                FromPoints(new Point2d[] { Point.Value }, redColor, lineColor, 0.2f).Draw();
        }
        */
    }
}
