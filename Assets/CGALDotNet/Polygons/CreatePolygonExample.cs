using System;
using System.Collections.Generic;
using UnityEngine;

using CGALDotNet;
using Common.Core.Colors;

using CGeom2D.Geometry;
using CGeom2D.Numerics;
using CGeom2D.Points;

using POINT = CGALDotNet.Geometry.Point2d;
using POINT2 = CGeom2D.Points.Point2d;
using SEGMENT = CGeom2D.Geometry.Segment2d;

namespace CGALDotNetUnity.Polygons
{

    public class CreatePolygonExample : InputBehaviour
    {
        private Color pointColor = new Color32(200, 80, 80, 255);

        private Color faceColor = new Color32(80, 80, 200, 128);

        private Color lineColor = new Color32(0, 0, 0, 255);

        private POINT? Point;

        PointCollection collection;

        SweepLine line;

        SweepEvent currentEvent;

        ColorRGB[] palette;

        protected override void Start()
        {
            PointSize = 0.2f;

            base.Start();
            //DrawGridAxis(true);
            SetInputMode(INPUT_MODE.POINT_CLICK);

            collection = new PointCollection(1000000);

            collection.AddPoint(0, 0);
            collection.AddPoint(4, 0);
            collection.AddPoint(2, 2);
            collection.AddPoint(4, -4);
            collection.AddPoint(6, -6);
            collection.AddPoint(8, -4);

            collection.AddSegment(0, 1);
            collection.AddSegment(0, 2);
            collection.AddSegment(0, 3);
            collection.AddSegment(3, 4);
            collection.AddSegment(3, 5);

            line = collection.CreateSweepLine();
            currentEvent = line.PopEvent();

            palette = ColorRGB.Palatte();
            palette.Shuffle(0);

        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentEvent = line.PopEvent();
            }
        }

        protected override void OnInputComplete(List<POINT> points)
        {
            InputPoints.Clear();
            SetInputMode(INPUT_MODE.POINT_CLICK);
        }

        protected override void OnCleared()
        {
            collection.Clear();
            Point = null;
            SetInputMode(INPUT_MODE.POINT_CLICK);
            InputPoints.Clear();
        }

        protected override void OnLeftClickDown(POINT point)
        {
            Point = point;
        }

        private void OnDestroy()
        {
 
        }

        private void OnPostRender()
        {
            DrawGrid();

            CreatePoints(InputPoints.ToArray(), null, pointColor, lineColor, 0.2f).Draw();

            if (currentEvent != null)
            {
                var line = currentEvent.Line(100);
                CreateLine(line, lineColor).Draw();
            }

            var segments = new Dictionary<int, List<SEGMENT>>();
            collection.GetSegments(segments);

            foreach (var kvp in segments)
            {
                var color = RandomColor(kvp.Key);
                CreateSegments(kvp.Value, color).Draw();
            }

            var points = new List<POINT2>();
            collection.GetPoints(points);
            CreatePoints(points, pointColor, lineColor, 0.2f).Draw();

            if (Point != null)
                CreatePoints(new POINT[] { Point.Value }, Color.red, lineColor, 0.25f).Draw();
        }

        protected void OnGUI()
        {


        }

        private Color RandomColor(int i)
        {
            int len = palette.Length;
            return palette[i % len].ToColor();
        }
    }
}
