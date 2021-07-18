using System;
using System.Collections.Generic;
using UnityEngine;

using Common.Unity.Drawing;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polygons;

namespace CGALDotNetUnity.Polygons
{

    public abstract class Polygon2Input : MonoBehaviour
    {

        private const float SNAP_DIST = 0.1f;

        private const int MIN_POINTS = 3;

        protected List<Point2d> Points { get; private set; }

        private List<int> Indices { get; set; }

        protected Color LineColor = Color.blue;

        protected bool MadePolygon { get; set; }

        protected float SnapPoint { get; set; }

        protected Color[] Colors = new Color[]
        {
            Color.red,
            Color.magenta,
            Color.blue,
            Color.cyan,
            Color.green,
            Color.yellow
        };

        private List<BaseRenderer> PolygonRenderers { get; set; }

        private VertexRenderer PointRenderer { get; set; }

        protected virtual void Start()
        {
            PolygonRenderers = new List<BaseRenderer>();
            PointRenderer = new VertexRenderer(0.02f);
            PointRenderer.Orientation = DRAW_ORIENTATION.XY;
        }

        protected virtual void OnPolygonComplete()
        {

        }

        protected virtual void OnPolygonCleared()
        {

        }

        protected virtual void OnLeftClick(Point2d point)
        {

        }

        protected virtual void Update()
        {
            bool leftMouseClicked = Input.GetMouseButtonDown(0);

            if (leftMouseClicked)
            {
                Point2d point = GetMousePosition();
                OnLeftClick(point);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetInput();
                ClearRenderers();
                OnPolygonCleared();
            }
            else if (!MadePolygon)
            {
                Point2d point = GetMousePosition();
                point = SnapToPolygon(point);

                if (leftMouseClicked)
                {
                    if (Points == null)
                    {
                        CreatePoints();
                        AddPoint(point);
                        AddPoint(point);
                    }
                    else
                    {
                        if (PolygonClosed())
                        {
                            ClosePolygon();
                            MadePolygon = true;
                            OnPolygonComplete();
                        }
                        else
                        {
                            AddPoint(point);
                        }
                    }
                }
                else
                {
                    MoveLastPoint(point);
                }
            }

        }

        protected void DrawInput()
        {
            if (Points == null) return;
            if (Indices == null) return;

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.Color = LineColor;
            lines.Load(Points.ConvertAll(p => new Vector2((float)p.x, (float)p.y)));

            var points = new VertexRenderer(0.02f);
            points.Orientation = DRAW_ORIENTATION.XY;
            points.Color = Color.yellow;
            points.Load(Points.ConvertAll(p => new Vector2((float)p.x, (float)p.y)));

            lines.Draw();
            points.Draw();
        }

        protected void DrawPolygons()
        {
            foreach (var renderer in PolygonRenderers)
                renderer.Draw();
        }

        protected void DrawPoint()
        {
            PointRenderer.Draw();
        }

        protected void AddPolygon(PolygonWithHoles2<EEK> polygon, Color lineColor, Color vertColor)
        {
            if (polygon == null) return;

            foreach (var poly in polygon.ToList())
                AddPolygon(poly, lineColor, vertColor);
        }

        protected void AddPolygon(Polygon2 polygon, Color lineColor, Color vertColor)
        {
            if (polygon == null) return;
            int count = polygon.Count;

            var lines = new SegmentRenderer();
            lines.LineMode = LINE_MODE.LINES;
            lines.Orientation = DRAW_ORIENTATION.XY;
            lines.Color = lineColor;
            lines.Load(ToVector2(polygon, count + 1));
            PolygonRenderers.Add(lines);

            var points = new VertexRenderer(0.02f);
            points.Orientation = DRAW_ORIENTATION.XY;
            points.Color = vertColor;
            points.Load(ToVector2(polygon, count));
            PolygonRenderers.Add(points);
        }

        protected void SetPoint(Point2d point, Color color)
        {
            PointRenderer.Clear();
            PointRenderer.Load(new Vector2((float)point.x, (float)point.y), color);
        }

        protected void ResetInput()
        {
            MadePolygon = false;
            Points = null;
            Indices = null;
        }

        protected void ClearRenderers()
        {
            PolygonRenderers.Clear();
            PointRenderer.Clear();
        }

        private Point2d GetMousePosition()
        {
            Vector3 p = Input.mousePosition;

            Camera cam = GetComponent<Camera>();
            p = cam.ScreenToWorldPoint(p);

            if (SnapPoint > 0.0f)
            {
                p.x = Mathf.Round(p.x * SnapPoint) / SnapPoint;
                p.y = Mathf.Round(p.y * SnapPoint) / SnapPoint;
            }

            return new Point2d(p.x, p.y);
        }

        private void CreatePoints()
        {
            Points = new List<Point2d>();
            Indices = new List<int>();
        }

        private void AddPoint(Point2d point)
        {
            if (Points == null) return;

            Points.Add(point);

            int count = Points.Count;
            if (count == 1) return;

            Indices.Add(count - 2);
            Indices.Add(count - 1);
        }

        private void MoveLastPoint(Point2d point)
        {
            if (Points == null) return;

            int count = Points.Count;
            if (count == 0) return;

            Points[count - 1] = point;
        }

        private Point2d SnapToPolygon(Point2d point)
        {
            if (Points == null) return point;

            int count = Points.Count;
            if (count < 4) return point;

            var x = Points[0].x - point.x;
            var y = Points[0].y - point.y;

            var dist = Math.Sqrt(x * x + y * y);

            if (dist <= SNAP_DIST)
            {
                point.x = Points[0].x;
                point.y = Points[0].y;
            }

            return point;
        }

        private bool PolygonClosed()
        {
            if (Points == null) return false;

            int count = Points.Count;
            if (count < MIN_POINTS) return false;

            var x = Points[0].x - Points[count - 1].x;
            var y = Points[0].y - Points[count - 1].y;

            return Math.Sqrt(x * x + y * y) <= SNAP_DIST;
        }

        private void ClosePolygon()
        {
            int count = Points.Count;
            if (count < MIN_POINTS) return;

            Points.RemoveAt(count - 1);

            Indices.Add(count - 2);
            Indices.Add(0);
        }

        private Vector2[] ToVector2(Polygon2 poylgon, int count)
        {
            var array = new Vector2[count];

            for(int i = 0; i < count; i++)
            {
                var p = poylgon[i];
                array[i] = new Vector2((float)p.x, (float)p.y);
            }

            return array;
        }

    }
}
