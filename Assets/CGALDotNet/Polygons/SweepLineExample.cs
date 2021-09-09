using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Colors;
using Common.Unity.Utility;
using CGeom2D.Points;
using CGeom2D.Geometry;

namespace CGALDotNetUnity.Points
{

    public class SweepLineExample : InputBehaviour
    {
        /*
        private Color redColor = new Color32(200, 80, 80, 255);

        private Color blueColor = new Color32(80, 80, 200, 255);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private Point2d? Point;

        private PointCollection collection;

        private SweepLine line;

        private SweepEvent currentEvent;

        protected override void Start()
        {
            //ConsoleRedirect.Redirect();

            PointSize = 0.2f;

            base.Start();
            //DrawGridAxis(true);
            SetInputMode(INPUT_MODE.POINT_CLICK);
            CreatePalette(0);

            var comparer = new SweepComparer(SWEEP.X);
            collection = new PointCollection(comparer, 1000);

            collection.AddPoint(0, 0);
            collection.AddPoint(4, 0);
            collection.AddPoint(2, 2);
            collection.AddPoint(4, -4);
            collection.AddPoint(6, -6);
            collection.AddPoint(8, -4);

            collection.AddPoint(2, 0);
            collection.AddPoint(6, 0);

            //collection.AddPoint(-2, 0);

            collection.AddSegment(0, 1);
            collection.AddSegment(0, 2);
            collection.AddSegment(0, 3);
            collection.AddSegment(3, 4);
            collection.AddSegment(3, 5);

            collection.AddSegment(0, 6);
            collection.AddSegment(0, 7);

            line = collection.CreateSweepLine();
            currentEvent = line.PopEvent();
            
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentEvent = line.PopEvent();
            }
        }

        protected override void OnCleared()
        {
            collection.Clear();
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
            DrawLine();
            DrawSegments();
            DrawPoints();
            DrawStartPoints();
            DrawClickPoint();
        }

        private void DrawLine()
        {
            if (currentEvent != null)
            {
                var line = currentEvent.CreateLine();
                FromLine(line, lineColor).Draw();

                //var box = (Box2d)currentEvent.Bounds * collection.InvScale;
                //FromBox(box, lineColor).Draw();
            }
        }

        private void DrawSegments()
        {
            var segments = new Dictionary<int, List<Segment2d>>();
            collection.GetSegments(segments);

            foreach (var kvp in segments)
            {
                var color = SampleColor(kvp.Key);
                FromSegments(kvp.Value, color).Draw();
            }
        }

        private void DrawPoints()
        {
            var points = new List<Point2d>();
            collection.GetPoints(points);
            FromPoints(points, redColor, lineColor, 0.2f).Draw();
        }

        private void DrawStartPoints()
        {
            var segments = new Dictionary<int, List<Segment2d>>();
            collection.GetSegments(segments);

            var points = new List<Point2d>();
            foreach (var kvp in segments)
            {
                var p = collection.GetPoint2d(kvp.Key);
                points.Add(p);
            }

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
